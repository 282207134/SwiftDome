#!/usr/bin/env swift

// ============================================
// Swift 基础 - 常量、变量与基本类型
// 参考：The Swift Programming Language - The Basics
// ============================================

import Foundation

print("=== 常量与变量 ===\n")

// MARK: - 常量 (let) 与变量 (var)
let maximumNumberOfLoginAttempts = 10 // 常量：不可修改
var currentLoginAttempt = 0            // 变量：可以修改

print("最大登录次数：\(maximumNumberOfLoginAttempts)")
print("当前尝试次数：\(currentLoginAttempt)")

currentLoginAttempt += 1
print("更新后的尝试次数：\(currentLoginAttempt)\n")

// MARK: - 类型推断与类型注解
let meaningOfLife = 42                     // 推断为 Int
let pi = 3.14159                           // 推断为 Double
var welcomeMessage: String = "你好，Swift"   // 显式注解为 String
print("生命、宇宙以及任何事情终极答案：\(meaningOfLife)")
print("π 的值约等于：\(pi)")
print("问候语：\(welcomeMessage)\n")

// MARK: - 数值型字面量与转换
let decimalInteger = 17
let binaryInteger = 0b10001
let octalInteger = 0o21
let hexadecimalInteger = 0x11

print("十进制：\(decimalInteger)，二进制：\(binaryInteger)，八进制：\(octalInteger)，十六进制：\(hexadecimalInteger)")

let three = 3
let pointOneFourOneFiveNine = 0.14159
let piNumber = Double(three) + pointOneFourOneFiveNine
print("合并为 Double：\(piNumber)\n")

// MARK: - 元组 (Tuple)
let http404Error = (statusCode: 404, description: "未找到")
print("状态码：\(http404Error.statusCode)，描述：\(http404Error.description)")

let coordinates = (x: 3, y: 4)
let (xValue, yValue) = coordinates
print("坐标：x=\(xValue)，y=\(yValue)\n")

// MARK: - 可选类型 (Optional)
var serverResponseCode: Int? = 404
print("服务器响应：\(String(describing: serverResponseCode))")

serverResponseCode = nil // 可以赋值为 nil 表示“没有值”
print("服务器响应：\(String(describing: serverResponseCode))\n")

let possibleNumber = "123"
let convertedNumber = Int(possibleNumber) // 转换结果为 Int? 类型
print("字符串 \(possibleNumber) 转换后的数值：\(String(describing: convertedNumber))\n")

// MARK: - 可选绑定与 guard 语句
if let actualNumber = convertedNumber {
    print("可选绑定成功，值为：\(actualNumber)")
} else {
    print("转换失败")
}

func greet(person: [String: String]) {
    guard let name = person["name"] else {
        print("缺少姓名")
        return
    }
    print("你好，\(name)！")

    guard let location = person["location"] else {
        print("希望你多多旅行")
        return
    }
    print("希望你在 \(location) 玩得开心！")
}

greet(person: ["name": "李雷"])
greet(person: ["name": "韩梅梅", "location": "上海"])

// MARK: - 空合并运算符
let defaultColor = "蓝色"
var userDefinedColorName: String? = nil
let colorToUse = userDefinedColorName ?? defaultColor
print("选择颜色：\(colorToUse)\n")

// MARK: - 区间运算符
for index in 1...5 {
    print("关闭第 \(index) 盏灯")
}
print("")

let names = ["Anna", "Alex", "Brian", "Jack"]
for name in names[2...] {
    print("后半段同学：\(name)")
}
print("")

// MARK: - 断言与先决条件
let age = 18
assert(age >= 0, "年龄不能为负数")
precondition(age >= 0, "年龄必须大于等于 0")

print("年龄合法：\(age)\n")

// MARK: - 综合示例
let person: (name: String, age: Int?, city: String?) = ("王小明", nil, "北京")
let description: String

if let age = person.age, let city = person.city {
    description = "\(person.name) \(age) 岁，居住在 \(city)"
} else {
    description = "\(person.name) 的信息不完整"
}

print(description)

// 使用 nil 合并提供默认值
let safeAge = person.age ?? 18
let safeCity = person.city ?? "未知城市"
print("补充信息：年龄 \(safeAge)，城市 \(safeCity)")

print("\n>>> 基础部分示例执行完毕 <<<")
