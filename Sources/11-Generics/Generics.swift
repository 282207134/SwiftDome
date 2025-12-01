#!/usr/bin/env swift

// ============================================
// Swift 泛型
// ============================================

import Foundation

print("=== 泛型的问题 ===\n")

func swapTwoInts(_ a: inout Int, _ b: inout Int) {
    let temporaryA = a
    a = b
    b = temporaryA
}

var someInt = 3
var anotherInt = 107
print("交换前：\(someInt), \(anotherInt)")
swapTwoInts(&someInt, &anotherInt)
print("交换后：\(someInt), \(anotherInt)")

print("\n=== 泛型函数 ===\n")

func swapTwoValues<T>(_ a: inout T, _ b: inout T) {
    let temporaryA = a
    a = b
    b = temporaryA
}

var str1 = "Hello"
var str2 = "World"
print("交换前：\(str1), \(str2)")
swapTwoValues(&str1, &str2)
print("交换后：\(str1), \(str2)")

print("\n=== 泛型类型 ===\n")

struct Stack<Element> {
    var items: [Element] = []

    mutating func push(_ item: Element) {
        items.append(item)
    }

    mutating func pop() -> Element? {
        return items.isEmpty ? nil : items.removeLast()
    }

    func peek() -> Element? {
        return items.last
    }

    var count: Int {
        return items.count
    }
}

var stackOfStrings = Stack<String>()
stackOfStrings.push("uno")
stackOfStrings.push("dos")
stackOfStrings.push("tres")
stackOfStrings.push("cuatro")
print("栈中元素：\(stackOfStrings.items)")

if let topItem = stackOfStrings.pop() {
    print("弹出：\(topItem)")
}
print("栈顶元素：\(stackOfStrings.peek() ?? "空")")

print("\n=== 扩展泛型类型 ===\n")

extension Stack {
    var topItem: Element? {
        return items.isEmpty ? nil : items[items.count - 1]
    }
}

if let topItem = stackOfStrings.topItem {
    print("栈顶：\(topItem)")
}

print("\n=== 类型约束 ===\n")

func findIndex<T: Equatable>(of valueToFind: T, in array: [T]) -> Int? {
    for (index, value) in array.enumerated() {
        if value == valueToFind {
            return index
        }
    }
    return nil
}

let doubleIndex = findIndex(of: 9.3, in: [3.14159, 0.1, 0.25, 9.3])
print("索引：\(doubleIndex ?? -1)")

let stringIndex = findIndex(of: "Andrea", in: ["Mike", "Malcolm", "Andrea"])
print("索引：\(stringIndex ?? -1)")

print("\n=== 关联类型 ===\n")

protocol Container {
    associatedtype Item
    mutating func append(_ item: Item)
    var count: Int { get }
    subscript(i: Int) -> Item { get }
}

struct IntStack: Container {
    var items: [Int] = []

    mutating func push(_ item: Int) {
        items.append(item)
    }

    mutating func pop() -> Int {
        return items.removeLast()
    }

    typealias Item = Int

    mutating func append(_ item: Int) {
        self.push(item)
    }

    var count: Int {
        return items.count
    }

    subscript(i: Int) -> Int {
        return items[i]
    }
}

var intStack = IntStack()
intStack.append(10)
intStack.append(20)
intStack.append(30)
print("IntStack: \(intStack.items)")
print("索引 1 的值：\(intStack[1])")

print("\n=== 泛型 where 子句 ===\n")

func allItemsMatch<C1: Container, C2: Container>(
    _ someContainer: C1, _ anotherContainer: C2
) -> Bool where C1.Item == C2.Item, C1.Item: Equatable {
    if someContainer.count != anotherContainer.count {
        return false
    }

    for i in 0..<someContainer.count {
        if someContainer[i] != anotherContainer[i] {
            return false
        }
    }

    return true
}

extension Stack: Container {
    mutating func append(_ item: Element) {
        self.push(item)
    }

    subscript(i: Int) -> Element {
        return items[i]
    }
}

var stackOfInts1 = Stack<Int>()
stackOfInts1.push(10)
stackOfInts1.push(20)

var stackOfInts2 = Stack<Int>()
stackOfInts2.push(10)
stackOfInts2.push(20)

if allItemsMatch(stackOfInts1, stackOfInts2) {
    print("两个栈完全匹配")
} else {
    print("两个栈不匹配")
}

print("\n=== 泛型约束扩展 ===\n")

extension Stack where Element: Equatable {
    func isTop(_ item: Element) -> Bool {
        guard let topItem = items.last else {
            return false
        }
        return topItem == item
    }
}

var stackOfComparable = Stack<String>()
stackOfComparable.push("Hello")
stackOfComparable.push("World")
print("栈顶是否为 'World'：\(stackOfComparable.isTop("World"))")
print("栈顶是否为 'Hello'：\(stackOfComparable.isTop("Hello"))")

print("\n=== 实战案例：泛型队列 ===\n")

struct Queue<Element> {
    private var items: [Element] = []

    mutating func enqueue(_ item: Element) {
        items.append(item)
    }

    mutating func dequeue() -> Element? {
        return items.isEmpty ? nil : items.removeFirst()
    }

    func peek() -> Element? {
        return items.first
    }

    var isEmpty: Bool {
        return items.isEmpty
    }

    var count: Int {
        return items.count
    }
}

var queue = Queue<String>()
queue.enqueue("第一个")
queue.enqueue("第二个")
queue.enqueue("第三个")
print("队列大小：\(queue.count)")

if let first = queue.dequeue() {
    print("出队：\(first)")
}

print("队首：\(queue.peek() ?? "空")")

print("\n=== 实战案例：泛型结果类型 ===\n")

enum Result<Success, Failure: Error> {
    case success(Success)
    case failure(Failure)
}

enum NetworkError: Error {
    case badURL
    case noConnection
    case timeout
}

func fetchData(from url: String) -> Result<String, NetworkError> {
    if url.isEmpty {
        return .failure(.badURL)
    }
    return .success("获取的数据")
}

let result = fetchData(from: "https://api.example.com")
switch result {
case .success(let data):
    print("成功：\(data)")
case .failure(let error):
    print("失败：\(error)")
}

print("\n>>> 泛型示例完成 <<<")
