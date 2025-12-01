#!/usr/bin/env swift

// ============================================
// Swift 控制流
// ============================================

import Foundation

print("=== if 与 else ===")

let temperature = 28
if temperature <= 0 {
    print("穿羽绒服")
} else if temperature < 20 {
    print("穿外套")
} else {
    print("穿 T 恤")
}

print("\n=== switch ===")

let someCharacter: Character = "e"
switch someCharacter {
case "a", "e", "i", "o", "u":
    print("该字符是元音：\(someCharacter)")
case "b"..."z":
    print("该字符是辅音：\(someCharacter)")
default:
    print("不是英文字母")
}

let approximateCount = 62
let countedThings = "羊"
print("\n>>> switch 区间匹配")
let naturalCount: String
switch approximateCount {
case 0:
    naturalCount = "没有"
case 1..<5:
    naturalCount = "几个"
case 5..<12:
    naturalCount = "一打以内"
case 12..<100:
    naturalCount = "几十个"
case 100..<1000:
    naturalCount = "上百个"
default:
    naturalCount = "好多"
}
print("我们有 \(naturalCount) \(countedThings)")

print("\n>>> switch 值绑定")
let somePoint = (2, 0)
switch somePoint {
case (0, 0):
    print("原点")
case (_, 0):
    print("在 x 轴上：x = \(somePoint.0)")
case (0, _):
    print("在 y 轴上：y = \(somePoint.1)")
case (-2...2, -2...2):
    print("在蓝色方块内部")
default:
    print("在蓝色方块外部")
}

print("\n>>> switch where 条件")
let yetAnotherPoint = (1, -1)
switch yetAnotherPoint {
case let (x, y) where x == y:
    print("在 y = x 直线上")
case let (x, y) where x == -y:
    print("在 y = -x 直线上")
case let (x, y):
    print("普通点：\(x), \(y)")
}

print("\n>>> switch 穿透 (fallthrough)")
let integerToDescribe = 5
var description = "数字 \(integerToDescribe) 是"
switch integerToDescribe {
case 2, 3, 5, 7, 11:
    description += " 一个质数，并且"
    fallthrough
case 4, 6, 8, 9, 10:
    description += " 小于 12"
default:
    description += " 其他数字"
}
print(description)

print("\n=== 循环 ===")

for index in 1...5 {
    print("第 \(index) 次循环")
}

let names = ["张三", "李四", "王五"]
for name in names {
    print("你好，\(name)")
}

let factorySettings = ["音量": 5, "亮度": 3, "对比度": 7]
for (settingName, settingValue) in factorySettings {
    print("\(settingName): \(settingValue)")
}

print("\n>>> while 与 repeat-while")

var i = 0
while i < 3 {
    print("while: \(i)")
    i += 1
}

i = 0
repeat {
    print("repeat-while: \(i)")
    i += 1
} while i < 3

print("\n=== 控制转移 ===")

let puzzleInput = "great minds think alike"
var puzzleOutput = ""
for character in puzzleInput {
    switch character {
    case "a", "e", "i", "o", "u", " ":
        continue
    default:
        puzzleOutput.append(character)
    }
}
print("移除元音后的字符串：\(puzzleOutput)")

let numberSymbol: Character = "三"  // 简体中文数字三
var possibleIntegerValue: Int?
switch numberSymbol {
case "1", "١", "一", "๑":
    possibleIntegerValue = 1
case "2", "٢", "二", "๒":
    possibleIntegerValue = 2
case "3", "٣", "三", "๓":
    possibleIntegerValue = 3
case "4", "٤", "四", "๔":
    possibleIntegerValue = 4
default:
    break
}

if let integerValue = possibleIntegerValue {
    print("该符号表示数字 \(integerValue)")
} else {
    print("找不到对应数字")
}

print("\n=== guard 语句 ===")

func processPerson(_ person: [String: String]) {
    guard let name = person["name"] else {
        print("缺少姓名，无法处理")
        return
    }
    print("开始处理：\(name)")

    guard let city = person["city"] else {
        print("找不到城市信息")
        return
    }

    print("\(name) 来自 \(city)")
}

processPerson(["name": "Allen", "city": "上海"])
processPerson(["name": "Tom"])

print("\n=== API 可用性检查 ===")

if #available(iOS 15, macOS 12, *) {
    print("使用最新 API")
} else {
    print("使用旧版本降级方案")
}

print("\n>>> 控制流示例完成 <<<")
