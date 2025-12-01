#!/usr/bin/env swift

// ============================================
// Swift 基础 - 字符串和字符
// ============================================

import Foundation

print("=== 字符串和字符 ===\n")

// MARK: - 字符串字面量

let someString = "这是一个字符串"
print(someString)

let quotation = """
"想象力比知识更重要。"
    - 爱因斯坦
"""
print(quotation + "\n")

// MARK: - 特殊字符

let specialCharacters = "空字符: \\0, 反斜杠: \\\\, 制表符: \\t, 换行符: \\n"
print(specialCharacters)

let dollarSign = "\u{24}"
let blackHeart = "\u{2665}"
let sparklingHeart = "\u{1F496}"
print("Unicode 字符：\(dollarSign) \(blackHeart) \(sparklingHeart)\n")

// MARK: - 扩展字符串分隔符

let threeMoreDoubleQuotationMarks = #"""
这里有三个双引号： """
"""#
print(threeMoreDoubleQuotationMarks + "\n")

// MARK: - 初始化空字符串

var emptyString = ""
var anotherEmptyString = String()

if emptyString.isEmpty {
    print("字符串是空的")
}

// MARK: - 字符串可变性

var variableString = "马"
variableString += "和马车"
print(variableString)

let constantString = "荧光棒"
print(constantString + "\n")

// MARK: - 使用字符

for character in "Dog!🐶" {
    print(character)
}
print("")

let catCharacters: [Character] = ["C", "a", "t", "!", "🐱"]
let catString = String(catCharacters)
print(catString + "\n")

// MARK: - 连接字符串和字符

let string1 = "你好"
let string2 = ", 世界"
var welcome = string1 + string2
print(welcome)

var instruction = "看这边"
instruction += ", 然后看那边"
print(instruction)

let exclamationMark: Character = "!"
welcome.append(exclamationMark)
print(welcome + "\n")

// MARK: - 字符串插值

let multiplier = 3
let message = "\(multiplier) 乘以 2.5 等于 \(Double(multiplier) * 2.5)"
print(message + "\n")

// MARK: - 计算字符数量

let unusualMenagerie = "考拉 🐨, 蜗牛 🐌, 企鹅 🐧, 树袋熊 🐨"
print("unusualMenagerie 有 \(unusualMenagerie.count) 个字符\n")

// MARK: - 访问和修改字符串

let greeting = "你好，朋友！"
print(greeting[greeting.startIndex])

print(greeting[greeting.index(before: greeting.endIndex)])

let index = greeting.index(greeting.startIndex, offsetBy: 3)
print(greeting[index])

for index in greeting.indices {
    print("\(greeting[index]) ", terminator: "")
}
print("\n")

// MARK: - 插入和删除

var welcomeMsg = "你好"
welcomeMsg.insert("!", at: welcomeMsg.endIndex)
print(welcomeMsg)

welcomeMsg.insert(contentsOf: " 世界", at: welcomeMsg.index(before: welcomeMsg.endIndex))
print(welcomeMsg)

// MARK: - 子字符串

let greetingStr = "你好，世界！"
let indexEnd = greetingStr.firstIndex(of: "，") ?? greetingStr.endIndex
let beginning = greetingStr[..<indexEnd]
print("子字符串：\(beginning)")

let newString = String(beginning)
print("转换为 String：\(newString)\n")

// MARK: - 比较字符串

let quotationStr = "我们是一样一样的。"
let sameQuotation = "我们是一样一样的。"

if quotationStr == sameQuotation {
    print("这两个字符串相等")
}

// MARK: - 前缀和后缀

let romeoAndJuliet = [
    "第一幕 第一场 维罗纳。一条公共街道。",
    "第一幕 第二场 凯普莱特家的宴会厅。",
    "第二幕 第一场 维罗纳。一条公共街道。",
    "第二幕 第二场 凯普莱特家的果园。"
]

var act1SceneCount = 0
for scene in romeoAndJuliet {
    if scene.hasPrefix("第一幕") {
        act1SceneCount += 1
    }
}
print("\n第一幕共有 \(act1SceneCount) 场")

var mansionCount = 0
var streetCount = 0
for scene in romeoAndJuliet {
    if scene.hasSuffix("宴会厅。") {
        mansionCount += 1
    } else if scene.hasSuffix("街道。") {
        streetCount += 1
    }
}
print("\(mansionCount) 场在宴会厅，\(streetCount) 场在街道")

print("\n>>> 字符串和字符示例完成 <<<")
