#!/usr/bin/env swift

// ============================================
// Swift 集合类型：数组、集合、字典
// ============================================

import Foundation

print("=== 数组 (Array) ===\n")

var shoppingList: [String] = ["鸡蛋", "牛奶"]
shoppingList.append("面包")
shoppingList += ["苹果", "香蕉"]
print("购物清单：\(shoppingList)")

shoppingList[0] = "有机鸡蛋"
print("更新后：\(shoppingList)")

shoppingList[1...3] = ["杏仁奶", "全麦面包"]
print("替换片段：\(shoppingList)")

shoppingList.insert("燕麦", at: 0)
print("插入后：\(shoppingList)")

let removedItem = shoppingList.remove(at: 2)
print("移除 \(removedItem) 后：\(shoppingList)")

print("\n使用 enumerated 遍历：")
for (index, item) in shoppingList.enumerated() {
    print("第 \(index + 1) 项：\(item)")
}

print("\n数组常用操作：")
let numbers = [1, 2, 3, 4, 5]
print("原数组：\(numbers)")
print("映射成平方：\(numbers.map { $0 * $0 })")
print("筛选偶数：\(numbers.filter { $0 % 2 == 0 })")
print("累加求和：\(numbers.reduce(0, +))")

print("\n=== 集合 (Set) ===\n")

var favoriteGenres: Set<String> = ["Rock", "Classical", "Hip hop"]
favoriteGenres.insert("Jazz")
print("音乐类型：\(favoriteGenres)")

if favoriteGenres.contains("Rock") {
    print("包含摇滚")
}

print("\n集合操作：")
let oddDigits: Set = [1, 3, 5, 7, 9]
let evenDigits: Set = [0, 2, 4, 6, 8]
let singleDigitPrimeNumbers: Set = [2, 3, 5, 7]

print("并集：\(oddDigits.union(evenDigits).sorted())")
print("交集：\(oddDigits.intersection(singleDigitPrimeNumbers).sorted())")
print("差集：\(oddDigits.subtracting(singleDigitPrimeNumbers).sorted())")
print("对称差：\(oddDigits.symmetricDifference(singleDigitPrimeNumbers).sorted())")

print("\n集合关系：")
let houseAnimals: Set = ["🐶", "🐱"]
let farmAnimals: Set = ["🐮", "🐔", "🐑", "🐶", "🐱"]
let cityAnimals: Set = ["🐦", "🐭"]

print("家畜是否包含宠物：\(farmAnimals.isSuperset(of: houseAnimals))")
print("宠物是否是家畜的子集：\(houseAnimals.isSubset(of: farmAnimals))")
print("宠物与城市动物是否不相交：\(houseAnimals.isDisjoint(with: cityAnimals))")

print("\n=== 字典 (Dictionary) ===\n")

var airports: [String: String] = ["YYZ": "Toronto Pearson", "DUB": "Dublin"]
airports["LHR"] = "London"
print("机场字典：\(airports)")

if let oldValue = airports.updateValue("Dublin Airport", forKey: "DUB") {
    print("原值：\(oldValue)")
}

airports["APL"] = "Apple International"
airports["APL"] = nil

for (airportCode, airportName) in airports {
    print("代码：\(airportCode) 名称：\(airportName)")
}

print("\n=== 综合案例 ===\n")

struct Student {
    let name: String
    var scores: [String: Int]
}

var students: [Student] = [
    Student(name: "张三", scores: ["语文": 95, "数学": 88]),
    Student(name: "李四", scores: ["语文": 85, "数学": 92]),
    Student(name: "王五", scores: ["语文": 78, "数学": 80])
]

for student in students {
    let totalScore = student.scores.values.reduce(0, +)
    let averageScore = Double(totalScore) / Double(student.scores.count)
    print("学生：\(student.name)，总分：\(totalScore)，平均分：\(averageScore)")
}

print("\n=== 实战练习建议 ===")
print("1. 使用数组实现一个待办事项列表")
print("2. 使用集合去除重复联系人")
print("3. 使用字典做用户配置存储")

print("\n>>> 集合类型示例完成 <<<")
