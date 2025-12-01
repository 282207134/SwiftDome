#!/usr/bin/env swift

// ============================================
// Swift 属性
// ============================================

import Foundation

print("=== 存储属性 ===\n")

struct FixedLengthRange {
    var firstValue: Int
    let length: Int
}

var rangeOfThreeItems = FixedLengthRange(firstValue: 0, length: 3)
print("范围：\(rangeOfThreeItems.firstValue) 到 \(rangeOfThreeItems.firstValue + rangeOfThreeItems.length)")

rangeOfThreeItems.firstValue = 6
print("更新后的范围：\(rangeOfThreeItems.firstValue) 到 \(rangeOfThreeItems.firstValue + rangeOfThreeItems.length)")

print("\n=== 计算属性 ===\n")

struct Point {
    var x = 0.0, y = 0.0
}

struct Size {
    var width = 0.0, height = 0.0
}

struct Rect {
    var origin = Point()
    var size = Size()

    var center: Point {
        get {
            let centerX = origin.x + (size.width / 2)
            let centerY = origin.y + (size.height / 2)
            return Point(x: centerX, y: centerY)
        }
        set(newCenter) {
            origin.x = newCenter.x - (size.width / 2)
            origin.y = newCenter.y - (size.height / 2)
        }
    }
}

var square = Rect(origin: Point(x: 0.0, y: 0.0), size: Size(width: 10.0, height: 10.0))
print("初始中心点：(\(square.center.x), \(square.center.y))")

square.center = Point(x: 15.0, y: 15.0)
print("新的中心点：(\(square.center.x), \(square.center.y))")
print("新的原点：(\(square.origin.x), \(square.origin.y))")

print("\n=== 只读计算属性 ===\n")

struct Cuboid {
    var width = 0.0, height = 0.0, depth = 0.0

    var volume: Double {
        return width * height * depth
    }
}

let fourByFiveByTwo = Cuboid(width: 4.0, height: 5.0, depth: 2.0)
print("立方体体积：\(fourByFiveByTwo.volume)")

print("\n=== 属性观察器 ===\n")

class StepCounter {
    var totalSteps: Int = 0 {
        willSet(newTotalSteps) {
            print("即将设置为 \(newTotalSteps) 步")
        }
        didSet {
            if totalSteps > oldValue {
                print("增加了 \(totalSteps - oldValue) 步")
            }
        }
    }
}

let stepCounter = StepCounter()
stepCounter.totalSteps = 200
stepCounter.totalSteps = 360
stepCounter.totalSteps = 896

print("\n=== 属性包装器 ===\n")

@propertyWrapper
struct TwelveOrLess {
    private var number = 0

    var wrappedValue: Int {
        get { return number }
        set { number = min(newValue, 12) }
    }
}

struct SmallRectangle {
    @TwelveOrLess var height: Int
    @TwelveOrLess var width: Int
}

var rectangle = SmallRectangle()
print("初始高度：\(rectangle.height)")

rectangle.height = 10
print("设置为 10：\(rectangle.height)")

rectangle.height = 24
print("尝试设置为 24，实际：\(rectangle.height)")

print("\n=== 类型属性 ===\n")

struct SomeStructure {
    static var storedTypeProperty = "Some value."
    static var computedTypeProperty: Int {
        return 1
    }
}

class SomeClass {
    static var storedTypeProperty = "Some value."
    static var computedTypeProperty: Int {
        return 27
    }
    class var overrideableComputedTypeProperty: Int {
        return 107
    }
}

print(SomeStructure.storedTypeProperty)
print(SomeClass.computedTypeProperty)

print("\n=== 实战案例：温度转换 ===\n")

struct Temperature {
    var celsius: Double = 0

    var fahrenheit: Double {
        get {
            return celsius * 9 / 5 + 32
        }
        set {
            celsius = (newValue - 32) * 5 / 9
        }
    }

    var kelvin: Double {
        get {
            return celsius + 273.15
        }
        set {
            celsius = newValue - 273.15
        }
    }
}

var temp = Temperature()
temp.celsius = 25
print("摄氏度：\(temp.celsius)°C")
print("华氏度：\(temp.fahrenheit)°F")
print("开尔文：\(temp.kelvin)K")

temp.fahrenheit = 77
print("\n设置华氏度为 77°F")
print("摄氏度：\(temp.celsius)°C")

print("\n=== 实战案例：用户设置 ===\n")

@propertyWrapper
struct UserDefault<T> {
    let key: String
    let defaultValue: T

    var wrappedValue: T {
        get {
            return UserDefaults.standard.object(forKey: key) as? T ?? defaultValue
        }
        set {
            UserDefaults.standard.set(newValue, forKey: key)
        }
    }
}

struct AppSettings {
    @UserDefault(key: "isDarkMode", defaultValue: false)
    static var isDarkMode: Bool

    @UserDefault(key: "fontSize", defaultValue: 14)
    static var fontSize: Int
}

print("深色模式：\(AppSettings.isDarkMode)")
print("字体大小：\(AppSettings.fontSize)")

print("\n>>> 属性示例完成 <<<")
