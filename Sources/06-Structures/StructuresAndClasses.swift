#!/usr/bin/env swift

// ============================================
// Swift 结构体和类
// ============================================

import Foundation

print("=== 结构体定义 ===\n")

struct Resolution {
    var width = 0
    var height = 0
}

struct VideoMode {
    var resolution = Resolution()
    var interlaced = false
    var frameRate = 0.0
    var name: String?
}

let someResolution = Resolution()
let someVideoMode = VideoMode()
print("分辨率：\(someResolution.width) x \(someResolution.height)")
print("帧率：\(someVideoMode.frameRate)")

print("\n=== 类定义 ===\n")

class Person {
    var name: String
    var age: Int

    init(name: String, age: Int) {
        self.name = name
        self.age = age
    }

    func introduce() {
        print("我叫 \(name)，今年 \(age) 岁")
    }
}

let person1 = Person(name: "张三", age: 25)
person1.introduce()

print("\n=== 值类型与引用类型 ===\n")

var cinema = someResolution
cinema.width = 2048
print("原始分辨率宽度：\(someResolution.width)")
print("修改后的副本宽度：\(cinema.width)")

let tenEighty = VideoMode()
tenEighty.resolution.width = 1920
tenEighty.resolution.height = 1080
tenEighty.name = "1080i"

let alsoTenEighty = tenEighty
alsoTenEighty.frameRate = 30.0

print("\ntenEighty 帧率：\(tenEighty.frameRate)")
print("alsoTenEighty 帧率：\(alsoTenEighty.frameRate)")

print("\n=== 恒等运算符 ===\n")

if tenEighty === alsoTenEighty {
    print("tenEighty 和 alsoTenEighty 引用同一个实例")
}

print("\n=== 实际案例 ===\n")

struct Point {
    var x: Double
    var y: Double

    func distance(to point: Point) -> Double {
        let dx = x - point.x
        let dy = y - point.y
        return sqrt(dx * dx + dy * dy)
    }
}

let pointA = Point(x: 0, y: 0)
let pointB = Point(x: 3, y: 4)
print("点 A 到点 B 的距离：\(pointA.distance(to: pointB))")

class BankAccount {
    var balance: Double
    let accountNumber: String

    init(accountNumber: String, initialBalance: Double = 0) {
        self.accountNumber = accountNumber
        self.balance = initialBalance
    }

    func deposit(_ amount: Double) {
        balance += amount
        print("存入 ¥\(amount)，余额：¥\(balance)")
    }

    func withdraw(_ amount: Double) -> Bool {
        if balance >= amount {
            balance -= amount
            print("取出 ¥\(amount)，余额：¥\(balance)")
            return true
        } else {
            print("余额不足")
            return false
        }
    }
}

print("\n银行账户示例：")
let account = BankAccount(accountNumber: "12345678", initialBalance: 1000)
account.deposit(500)
account.withdraw(300)
account.withdraw(2000)

print("\n=== 何时使用结构体，何时使用类 ===")
print("✅ 使用结构体：")
print("  - 数据模型简单")
print("  - 需要值语义（拷贝而非引用）")
print("  - 不需要继承")
print("  - 示例：Point, Size, Rect")

print("\n✅ 使用类：")
print("  - 需要继承")
print("  - 需要引用语义")
print("  - 需要析构器")
print("  - 示例：ViewController, Manager, Service")

print("\n>>> 结构体和类示例完成 <<<")
