#!/usr/bin/env swift

// ============================================
// Swift 闭包
// ============================================

import Foundation

print("=== 闭包表达式 ===\n")

// MARK: - sorted 方法

let names = ["Chris", "Alex", "Ewa", "Barry", "Daniella"]

func backward(_ s1: String, _ s2: String) -> Bool {
    return s1 > s2
}

var reversedNames = names.sorted(by: backward)
print("使用函数排序：\(reversedNames)")

// MARK: - 闭包表达式语法

reversedNames = names.sorted(by: { (s1: String, s2: String) -> Bool in
    return s1 > s2
})
print("完整闭包表达式：\(reversedNames)")

// MARK: - 根据上下文推断类型

reversedNames = names.sorted(by: { s1, s2 in return s1 > s2 })
print("类型推断：\(reversedNames)")

// MARK: - 单表达式闭包的隐式返回

reversedNames = names.sorted(by: { s1, s2 in s1 > s2 })
print("隐式返回：\(reversedNames)")

// MARK: - 参数名称缩写

reversedNames = names.sorted(by: { $0 > $1 })
print("参数名称缩写：\(reversedNames)")

// MARK: - 运算符方法

reversedNames = names.sorted(by: >)
print("运算符方法：\(reversedNames)")

print("\n=== 尾随闭包 ===\n")

func someFunctionThatTakesAClosure(closure: () -> Void) {
    closure()
}

someFunctionThatTakesAClosure(closure: {
    print("不使用尾随闭包")
})

someFunctionThatTakesAClosure() {
    print("使用尾随闭包")
}

someFunctionThatTakesAClosure {
    print("省略括号的尾随闭包")
}

reversedNames = names.sorted { $0 > $1 }
print("\n尾随闭包排序：\(reversedNames)")

// MARK: - 多重尾随闭包

func loadPicture(from server: String, completion: (String) -> Void, onFailure: () -> Void) {
    if server == "server1.example.com" {
        completion("图片数据")
    } else {
        onFailure()
    }
}

print("")
loadPicture(from: "server1.example.com") { picture in
    print("成功加载：\(picture)")
} onFailure: {
    print("无法加载图片")
}

print("\n=== 值捕获 ===\n")

func makeIncrementer(forIncrement amount: Int) -> () -> Int {
    var runningTotal = 0
    func incrementer() -> Int {
        runningTotal += amount
        return runningTotal
    }
    return incrementer
}

let incrementByTen = makeIncrementer(forIncrement: 10)
print(incrementByTen())
print(incrementByTen())
print(incrementByTen())

let incrementBySeven = makeIncrementer(forIncrement: 7)
print(incrementBySeven())

print(incrementByTen())

print("\n=== 闭包是引用类型 ===\n")

let alsoIncrementByTen = incrementByTen
print(alsoIncrementByTen())

print("\n=== 逃逸闭包 ===\n")

var completionHandlers: [() -> Void] = []

func someFunctionWithEscapingClosure(completionHandler: @escaping () -> Void) {
    completionHandlers.append(completionHandler)
}

func someFunctionWithNonescapingClosure(closure: () -> Void) {
    closure()
}

class SomeClass {
    var x = 10
    func doSomething() {
        someFunctionWithEscapingClosure { self.x = 100 }
        someFunctionWithNonescapingClosure { x = 200 }
    }
}

let instance = SomeClass()
instance.doSomething()
print("doSomething 后：\(instance.x)")

completionHandlers.first?()
print("执行逃逸闭包后：\(instance.x)")

print("\n=== 自动闭包 ===\n")

var customersInLine = ["Chris", "Alex", "Ewa", "Barry", "Daniella"]
print("队列中有 \(customersInLine.count) 位顾客")

let customerProvider = { customersInLine.remove(at: 0) }
print("队列中有 \(customersInLine.count) 位顾客")

print("现在服务 \(customerProvider())！")
print("队列中有 \(customersInLine.count) 位顾客")

func serve(customer customerProvider: @autoclosure () -> String) {
    print("现在服务 \(customerProvider())！")
}

print("")
serve(customer: customersInLine.remove(at: 0))

print("\n=== 常用高阶函数 ===\n")

let numbers = [1, 2, 3, 4, 5]

let doubled = numbers.map { $0 * 2 }
print("map 翻倍：\(doubled)")

let evens = numbers.filter { $0 % 2 == 0 }
print("filter 偶数：\(evens)")

let sum = numbers.reduce(0) { $0 + $1 }
print("reduce 求和：\(sum)")

let strings = ["apple", "banana", "cherry"]
let upperCased = strings.map { $0.uppercased() }
print("map 大写：\(upperCased)")

let longWords = strings.filter { $0.count > 5 }
print("filter 长单词：\(longWords)")

let concatenated = strings.reduce("") { $0 + $1 }
print("reduce 拼接：\(concatenated)")

print("\n=== 复杂示例 ===\n")

let scores = [85, 92, 78, 95, 88]
let passedScores = scores
    .filter { $0 >= 80 }
    .map { $0 + 5 }
    .sorted()

print("及格分数加 5 后排序：\(passedScores)")

let people = ["张三": 25, "李四": 30, "王五": 22]
let adults = people
    .filter { $0.value >= 18 }
    .map { $0.key }
    .sorted()

print("成年人列表：\(adults)")

print("\n>>> 闭包示例完成 <<<")
