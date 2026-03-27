#!/usr/bin/env swift

// ============================================
// Swift 函数
// ============================================

import Foundation

print("=== 函数定义与调用 ===\n")

// MARK: - 基础函数

func greet(person: String) -> String {
    let greeting = "你好, " + person + "!"
    return greeting
}

print(greet(person: "张三"))
print(greet(person: "李四"))

// MARK: - 无参数函数

func sayHello() -> String {
    return "你好，世界！"
}

print("\n" + sayHello())

// MARK: - 多参数函数

func greetAgain(person: String, alreadyGreeted: Bool) -> String {
    if alreadyGreeted {
        return "你好，" + person + "！又见面了！"
    } else {
        return "你好，" + person + "！"
    }
}

print(greetAgain(person: "王五", alreadyGreeted: true))
print(greetAgain(person: "赵六", alreadyGreeted: false))

// MARK: - 无返回值函数

func printGreeting(person: String) {
    print("你好，\(person)！")
}

print("")
printGreeting(person: "Anna")

// MARK: - 多返回值函数

func minMax(array: [Int]) -> (min: Int, max: Int) {
    var currentMin = array[0]
    var currentMax = array[0]

    for value in array[1..<array.count] {
        if value < currentMin {
            currentMin = value
        } else if value > currentMax {
            currentMax = value
        }
    }

    return (currentMin, currentMax)
}

let bounds = minMax(array: [8, -6, 2, 109, 3, 71])
print("\n最小值：\(bounds.min)，最大值：\(bounds.max)")

// MARK: - 可选元组返回类型

func minMaxOptional(array: [Int]) -> (min: Int, max: Int)? {
    if array.isEmpty { return nil }

    var currentMin = array[0]
    var currentMax = array[0]

    for value in array[1..<array.count] {
        if value < currentMin {
            currentMin = value
        } else if value > currentMax {
            currentMax = value
        }
    }

    return (currentMin, currentMax)
}

if let bounds = minMaxOptional(array: [8, -6, 2, 109, 3, 71]) {
    print("最小值：\(bounds.min)，最大值：\(bounds.max)")
}

if minMaxOptional(array: []) == nil {
    print("空数组返回 nil")
}

// MARK: - 隐式返回

func greeting(for person: String) -> String {
    "你好，" + person + "！"
}

print("\n" + greeting(for: "Dave"))

// MARK: - 参数标签和参数名称

func greet(person: String, from hometown: String) -> String {
    return "你好，\(person)！很高兴你来自 \(hometown)。"
}

print(greet(person: "Bill", from: "Cupertino"))

// MARK: - 省略参数标签

func someFunction(_ firstParameterName: Int, secondParameterName: Int) {
    print("第一个参数：\(firstParameterName)，第二个参数：\(secondParameterName)")
}

print("")
someFunction(1, secondParameterName: 2)

// MARK: - 默认参数值

func calculatePrice(price: Double, discount: Double = 0.1) -> Double {
    return price * (1 - discount)
}

print("\n原价 100，默认折扣：\(calculatePrice(price: 100))")
print("原价 100，自定义折扣：\(calculatePrice(price: 100, discount: 0.2))")

// MARK: - 可变参数

func arithmeticMean(_ numbers: Double...) -> Double {
    var total: Double = 0
    for number in numbers {
        total += number
    }
    return total / Double(numbers.count)
}

print("\n平均值：\(arithmeticMean(1, 2, 3, 4, 5))")
print("平均值：\(arithmeticMean(3, 8.25, 18.75))")

// MARK: - inout 参数

func swapTwoInts(_ a: inout Int, _ b: inout Int) {
    let temporaryA = a
    a = b
    b = temporaryA
}

var someInt = 3
var anotherInt = 107
print("\n交换前：someInt = \(someInt), anotherInt = \(anotherInt)")
swapTwoInts(&someInt, &anotherInt)
print("交换后：someInt = \(someInt), anotherInt = \(anotherInt)")

// MARK: - 函数类型

func addTwoInts(_ a: Int, _ b: Int) -> Int {
    return a + b
}

func multiplyTwoInts(_ a: Int, _ b: Int) -> Int {
    return a * b
}

var mathFunction: (Int, Int) -> Int = addTwoInts
print("\n结果：\(mathFunction(2, 3))")

mathFunction = multiplyTwoInts
print("结果：\(mathFunction(2, 3))")

// MARK: - 函数类型作为参数类型

func printMathResult(_ mathFunction: (Int, Int) -> Int, _ a: Int, _ b: Int) {
    print("结果：\(mathFunction(a, b))")
}

print("")
printMathResult(addTwoInts, 3, 5)

// MARK: - 函数类型作为返回类型

func stepForward(_ input: Int) -> Int {
    return input + 1
}

func stepBackward(_ input: Int) -> Int {
    return input - 1
}

func chooseStepFunction(backward: Bool) -> (Int) -> Int {
    return backward ? stepBackward : stepForward
}

var currentValue = 3
let moveNearerToZero = chooseStepFunction(backward: currentValue > 0)

print("\n倒数到零：")
while currentValue != 0 {
    print("\(currentValue)...")
    currentValue = moveNearerToZero(currentValue)
}
print("完成！")

// MARK: - 嵌套函数

func chooseStepFunctionNested(backward: Bool) -> (Int) -> Int {
    func stepForwardInner(input: Int) -> Int { return input + 1 }
    func stepBackwardInner(input: Int) -> Int { return input - 1 }

    return backward ? stepBackwardInner : stepForwardInner
}

var currentValueNested = -4
let moveNearerToZeroNested = chooseStepFunctionNested(backward: currentValueNested > 0)

print("\n使用嵌套函数，从负数数到零：")
while currentValueNested != 0 {
    print("\(currentValueNested)...")
    currentValueNested = moveNearerToZeroNested(currentValueNested)
}
print("完成！")

print("\n>>> 函数示例完成 <<<")
