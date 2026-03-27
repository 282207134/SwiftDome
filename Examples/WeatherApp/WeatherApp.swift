#!/usr/bin/env swift

// ============================================
// WeatherApp - 天气应用示例（进阶）
// ============================================

import Foundation

enum WeatherError: Error {
    case cityNotFound
    case networkError
    case invalidData
}

struct Weather: Codable {
    let city: String
    let temperature: Double
    let condition: String
    let humidity: Int

    func display() {
        print("\n🌤️  天气信息 🌤️")
        print("城市：\(city)")
        print("温度：\(temperature)°C")
        print("天气：\(condition)")
        print("湿度：\(humidity)%")
    }
}

protocol WeatherService {
    func fetchWeather(for city: String) throws -> Weather
}

class MockWeatherService: WeatherService {
    private let database: [String: Weather] = [
        "北京": Weather(city: "北京", temperature: 25.0, condition: "晴朗", humidity: 45),
        "上海": Weather(city: "上海", temperature: 28.0, condition: "多云", humidity: 60),
        "广州": Weather(city: "广州", temperature: 32.0, condition: "雷雨", humidity: 80),
        "深圳": Weather(city: "深圳", temperature: 30.0, condition: "晴朗", humidity: 70)
    ]

    func fetchWeather(for city: String) throws -> Weather {
        guard let weather = database[city] else {
            throw WeatherError.cityNotFound
        }
        return weather
    }
}

class WeatherApp {
    private let service: WeatherService

    init(service: WeatherService) {
        self.service = service
    }

    func showWeather(for city: String) {
        do {
            let weather = try service.fetchWeather(for: city)
            weather.display()
        } catch WeatherError.cityNotFound {
            print("❌ 未找到城市：\(city)")
        } catch {
            print("❌ 获取天气失败：\(error)")
        }
    }

    func compareWeather(cities: [String]) {
        print("\n📊 城市天气对比：")
        print(String(repeating: "-", count: 50))

        var temperatures: [(city: String, temp: Double)] = []

        for city in cities {
            do {
                let weather = try service.fetchWeather(for: city)
                temperatures.append((city, weather.temperature))
                print("\(city): \(weather.temperature)°C (\(weather.condition))")
            } catch {
                print("\(city): 数据获取失败")
            }
        }

        if let hottest = temperatures.max(by: { $0.temp < $1.temp }) {
            print("\n🔥 最热城市：\(hottest.city) (\(hottest.temp)°C)")
        }

        if let coldest = temperatures.min(by: { $0.temp < $1.temp }) {
            print("❄️  最冷城市：\(coldest.city) (\(coldest.temp)°C)")
        }
    }
}

print("=== WeatherApp 天气应用 ===\n")

let service = MockWeatherService()
let app = WeatherApp(service: service)

print("查询北京天气：")
app.showWeather(for: "北京")

print("\n查询上海天气：")
app.showWeather(for: "上海")

print("\n查询不存在的城市：")
app.showWeather(for: "月球")

let cities = ["北京", "上海", "广州", "深圳"]
app.compareWeather(cities: cities)

print("\n🎉 WeatherApp 示例运行完成！")
