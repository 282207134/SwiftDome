#!/usr/bin/env swift

// ============================================
// Swift 继承
// ============================================

import Foundation

print("=== 基类定义 ===\n")

class Vehicle {
    var currentSpeed = 0.0
    var description: String {
        return "以 \(currentSpeed) 公里/小时的速度行驶"
    }

    func makeNoise() {
        // 基类不实现，由子类重写
    }
}

let someVehicle = Vehicle()
print("Vehicle: \(someVehicle.description)")

print("\n=== 子类化 ===\n")

class Bicycle: Vehicle {
    var hasBasket = false
}

let bicycle = Bicycle()
bicycle.hasBasket = true
bicycle.currentSpeed = 15.0
print("Bicycle: \(bicycle.description)")
print("是否有篮子：\(bicycle.hasBasket)")

class Tandem: Bicycle {
    var currentNumberOfPassengers = 0
}

let tandem = Tandem()
tandem.hasBasket = true
tandem.currentNumberOfPassengers = 2
tandem.currentSpeed = 22.0
print("\nTandem: \(tandem.description)")
print("乘客数：\(tandem.currentNumberOfPassengers)")

print("\n=== 重写 ===\n")

class Train: Vehicle {
    override func makeNoise() {
        print("Choo Choo")
    }
}

let train = Train()
train.makeNoise()

class Car: Vehicle {
    var gear = 1

    override var description: String {
        return super.description + "，档位：\(gear)"
    }
}

let car = Car()
car.currentSpeed = 25.0
car.gear = 3
print("\nCar: \(car.description)")

print("\n=== 属性观察器重写 ===\n")

class AutomaticCar: Car {
    override var currentSpeed: Double {
        didSet {
            gear = Int(currentSpeed / 10.0) + 1
        }
    }
}

let automatic = AutomaticCar()
automatic.currentSpeed = 35.0
print("AutomaticCar: \(automatic.description)")

print("\n=== 防止重写 (final) ===\n")

class FinalVehicle {
    final func honk() {
        print("Beep beep!")
    }
}

class SportsCar: FinalVehicle {
    // 不能重写 honk() 方法
}

let sportsCar = SportsCar()
sportsCar.honk()

print("\n=== 实战案例：形状类层次 ===\n")

class Shape {
    var name: String

    init(name: String) {
        self.name = name
    }

    func area() -> Double {
        return 0
    }

    func perimeter() -> Double {
        return 0
    }

    func description() -> String {
        return "这是一个 \(name)"
    }
}

class Circle: Shape {
    var radius: Double

    init(radius: Double) {
        self.radius = radius
        super.init(name: "圆形")
    }

    override func area() -> Double {
        return Double.pi * radius * radius
    }

    override func perimeter() -> Double {
        return 2 * Double.pi * radius
    }

    override func description() -> String {
        return super.description() + "，半径：\(radius)"
    }
}

class Rectangle: Shape {
    var width: Double
    var height: Double

    init(width: Double, height: Double) {
        self.width = width
        self.height = height
        super.init(name: "矩形")
    }

    override func area() -> Double {
        return width * height
    }

    override func perimeter() -> Double {
        return 2 * (width + height)
    }

    override func description() -> String {
        return super.description() + "，宽：\(width)，高：\(height)"
    }
}

let circle = Circle(radius: 5)
print(circle.description())
print("面积：\(circle.area())")
print("周长：\(circle.perimeter())")

let rectangle = Rectangle(width: 4, height: 6)
print("\n\(rectangle.description())")
print("面积：\(rectangle.area())")
print("周长：\(rectangle.perimeter())")

print("\n=== 多态 ===\n")

let shapes: [Shape] = [
    Circle(radius: 3),
    Rectangle(width: 5, height: 4),
    Circle(radius: 7)
]

for shape in shapes {
    print("\(shape.description())，面积：\(shape.area())")
}

print("\n=== 实战案例：员工管理系统 ===\n")

class Employee {
    var name: String
    var baseSalary: Double

    init(name: String, baseSalary: Double) {
        self.name = name
        self.baseSalary = baseSalary
    }

    func calculateSalary() -> Double {
        return baseSalary
    }

    func introduce() {
        print("我是 \(name)，基本工资：\(baseSalary)")
    }
}

class Manager: Employee {
    var teamSize: Int

    init(name: String, baseSalary: Double, teamSize: Int) {
        self.teamSize = teamSize
        super.init(name: name, baseSalary: baseSalary)
    }

    override func calculateSalary() -> Double {
        return baseSalary + Double(teamSize) * 1000
    }

    override func introduce() {
        super.introduce()
        print("管理团队规模：\(teamSize) 人")
    }
}

class Developer: Employee {
    var programmingLanguages: [String]

    init(name: String, baseSalary: Double, languages: [String]) {
        self.programmingLanguages = languages
        super.init(name: name, baseSalary: baseSalary)
    }

    override func calculateSalary() -> Double {
        return baseSalary + Double(programmingLanguages.count) * 500
    }

    override func introduce() {
        super.introduce()
        print("掌握语言：\(programmingLanguages.joined(separator: ", "))")
    }
}

let employees: [Employee] = [
    Manager(name: "张经理", baseSalary: 10000, teamSize: 5),
    Developer(name: "李工程师", baseSalary: 8000, languages: ["Swift", "Python", "JavaScript"]),
    Developer(name: "王工程师", baseSalary: 9000, languages: ["Java", "Kotlin"])
]

for employee in employees {
    employee.introduce()
    print("实际工资：\(employee.calculateSalary())\n")
}

print(">>> 继承示例完成 <<<")
