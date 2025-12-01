#!/usr/bin/env swift

// ============================================
// Swift 基础 - 数据类型
// ============================================

import Foundation

print("=== Swift 数据类型 ===\n")

// MARK: - 整数类型

let minValue = Int8.min
let maxValue = Int8.max
print("Int8 范围：\(minValue) 到 \(maxValue)")

let unsignedMin = UInt8.min
let unsignedMax = UInt8.max
print("UInt8 范围：\(unsignedMin) 到 \(unsignedMax)\n")

// MARK: - 浮点数类型

let floatNumber: Float = 3.14
let doubleNumber: Double = 3.141592653589793
print("Float 精度：\(floatNumber)")
print("Double 精度：\(doubleNumber)\n")

// MARK: - 布尔类型

let orangesAreOrange = true
let turnipsAreDelicious = false
print("橙子是橙色的：\(orangesAreOrange)")
print("萝卜好吃：\(turnipsAreDelicious)\n")

// MARK: - 字符串类型

let greeting = "你好，世界！"
let multilineString = """
这是一个
多行字符串
可以包含多行内容
"""
print(greeting)
print(multilineString)

let name = "张三"
let age = 25
let introduction = "我叫 \(name)，今年 \(age) 岁"
print(introduction)

let emptyString = ""
let anotherEmptyString = String()
if emptyString.isEmpty {
    print("字符串为空\n")
}

// MARK: - 字符类型

let exclamationMark: Character = "!"
let catCharacter: Character = "🐱"
print("字符：\(exclamationMark) \(catCharacter)\n")

// MARK: - 类型别名

typealias AudioSample = UInt16
let maxAmplitude = AudioSample.max
print("音频样本最大值：\(maxAmplitude)\n")

// MARK: - 类型安全与类型推断

let three = 3
let pointOneFour = 0.14
let pi = Double(three) + pointOneFour
print("π ≈ \(pi)")

let integerPi = Int(pi)
print("整数 π = \(integerPi)\n")

// MARK: - 数字字面量

let decimalInteger = 17
let binaryInteger = 0b10001
let octalInteger = 0o21
let hexadecimalInteger = 0x11

print("十进制：\(decimalInteger)")
print("二进制：\(binaryInteger)")
print("八进制：\(octalInteger)")
print("十六进制：\(hexadecimalInteger)\n")

let decimalDouble = 12.1875
let exponentDouble = 1.21875e1
let hexadecimalDouble = 0xC.3p0

print("十进制小数：\(decimalDouble)")
print("指数形式：\(exponentDouble)")
print("十六进制小数：\(hexadecimalDouble)\n")

// MARK: - 数值字面量的格式化

let paddedDouble = 000123.456
let oneMillion = 1_000_000
let justOverOneMillion = 1_000_000.000_000_1

print("填充零：\(paddedDouble)")
print("一百万：\(oneMillion)")
print("稍大于一百万：\(justOverOneMillion)\n")

print(">>> 数据类型示例完成 <<<")
