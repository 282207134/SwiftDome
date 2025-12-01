#!/usr/bin/env swift

// ============================================
// Swift 扩展
// ============================================

import Foundation

print("=== 扩展 (Extensions) ===\n")

extension Double {
    var km: Double { return self * 1_000.0 }
    var m: Double { return self }
    var cm: Double { return self / 100.0 }
    var mm: Double { return self / 1_000.0 }
    var ft: Double { return self / 3.28084 }
}

let marathonDistance = 42.km
print("马拉松距离：\(marathonDistance) 米")

print("\n=== 初始化器扩展 ===\n")

struct Size {
    var width = 0.0
    var height = 0.0
}

struct Point {
    var x = 0.0
    var y = 0.0
}

struct Rect {
    var origin = Point()
    var size = Size()
}

extension Rect {
    init(center: Point, size: Size) {
        let originX = center.x - (size.width / 2)
        let originY = center.y - (size.height / 2)
        self.init(origin: Point(x: originX, y: originY), size: size)
    }
}

let centerRect = Rect(center: Point(x: 4.0, y: 4.0), size: Size(width: 3.0, height: 3.0))
print("中心矩形原点：(\(centerRect.origin.x), \(centerRect.origin.y))")

print("\n=== 方法扩展 ===\n")

extension Int {
    func repetitions(task: () -> Void) {
        for _ in 0..<self {
            task()
        }
    }

    mutating func square() {
        self *= self
    }
}

3.repetitions {
    print("Swift 很好玩！")
}

var number = 7
number.square()
print("平方后：\(number)")

print("\n=== 下标扩展 ===\n")

extension Int {
    subscript(digitIndex: Int) -> Int {
        var decimalBase = 1
        for _ in 0..<digitIndex {
            decimalBase *= 10
        }
        return (self / decimalBase) % 10
    }
}

print("987654321 的第 0 位：\(987654321[0])")
print("第 2 位：\(987654321[2])")

print("\n=== 嵌套类型 ===\n")

extension Character {
    enum Kind {
        case vowel, consonant, other
    }

    var kind: Kind {
        switch String(self).lowercased() {
        case "a", "e", "i", "o", "u":
            return .vowel
        case "b"..."z":
            return .consonant
        default:
            return .other
        }
    }
}

let letter: Character = "E"
print("字母 \(letter) 是 \(letter.kind == .vowel ? "元音" : "辅音或其他")")

print("\n=== 协议扩展 ===\n")

protocol TextRepresentable {
    var textualDescription: String { get }
}

extension Array: TextRepresentable where Element: TextRepresentable {
    var textualDescription: String {
        let descriptions = self.map { $0.textualDescription }
        return "[" + descriptions.joined(separator: ", ") + "]"
    }
}

struct Dice: TextRepresentable {
    var sides: Int
    var textualDescription: String {
        return "一个 \(sides) 面骰子"
    }
}

let diceArray: [Dice] = [Dice(sides: 6), Dice(sides: 12)]
print(diceArray.textualDescription)

print("\n=== 扩展现有类型 ===\n")

extension DateFormatter {
    static let yyyyMMdd: DateFormatter = {
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyy-MM-dd"
        return formatter
    }()
}

let today = Date()
print("今天日期：\(DateFormatter.yyyyMMdd.string(from: today))")

print("\n=== 约束扩展 ===\n")

extension Collection where Element: Numeric {
    func average() -> Element {
        let total = reduce(0, +)
        return total / Element(exactly: count)!  // 示例目的，假设可以整除
    }
}

let numericArray: [Double] = [1.0, 2.0, 3.0, 4.0]
print("平均值：\(numericArray.average())")

print("\n=== 实战案例：扩展 String ===\n")

extension String {
    var isEmail: Bool {
        let pattern = "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,64}"
        return range(of: pattern, options: .regularExpression) != nil
    }

    func trimmed() -> String {
        trimmingCharacters(in: .whitespacesAndNewlines)
    }
}

let email = "info@example.com"
print("是否为邮箱：\(email.isEmail)")
print("去除空白：\("  Hello  ".trimmed())")

print("\n>>> 扩展示例完成 <<<")
