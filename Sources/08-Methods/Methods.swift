#!/usr/bin/env swift

// ============================================
// Swift 方法
// ============================================

import Foundation

print("=== 实例方法 ===\n")

class Counter {
    var count = 0

    func increment() {
        count += 1
    }

    func increment(by amount: Int) {
        count += amount
    }

    func reset() {
        count = 0
    }
}

let counter = Counter()
counter.increment()
print("count = \(counter.count)")
counter.increment(by: 5)
print("count = \(counter.count)")
counter.reset()
print("count = \(counter.count)")

print("\n=== 结构体中的 mutating 方法 ===\n")

struct Point {
    var x = 0.0, y = 0.0

    mutating func moveBy(x deltaX: Double, y deltaY: Double) {
        x += deltaX
        y += deltaY
    }
}

var somePoint = Point(x: 1.0, y: 1.0)
somePoint.moveBy(x: 2.0, y: 3.0)
print("当前坐标：(\(somePoint.x), \(somePoint.y))")

print("\n枚举中的 mutating 方法：")

enum TriStateSwitch {
    case off, low, high
    mutating func next() {
        switch self {
        case .off:
            self = .low
        case .low:
            self = .high
        case .high:
            self = .off
        }
    }
}

var ovenLight = TriStateSwitch.low
ovenLight.next()
print("电灯状态：\(ovenLight)")
ovenLight.next()
print("电灯状态：\(ovenLight)")

print("\n=== 类型方法 ===\n")

struct LevelTracker {
    static var highestUnlockedLevel = 1
    var currentLevel = 1

    static func unlock(_ level: Int) {
        if level > highestUnlockedLevel {
            highestUnlockedLevel = level
        }
    }

    static func isUnlocked(_ level: Int) -> Bool {
        return level <= highestUnlockedLevel
    }

    @discardableResult
    mutating func advance(to level: Int) -> Bool {
        if LevelTracker.isUnlocked(level) {
            currentLevel = level
            return true
        } else {
            return false
        }
    }
}

class Player {
    var tracker = LevelTracker()
    let playerName: String

    init(name: String) {
        playerName = name
    }

    func complete(level: Int) {
        LevelTracker.unlock(level + 1)
        tracker.advance(to: level + 1)
    }
}

var player = Player(name: "阿勇")
player.complete(level: 1)
print("最高解锁关卡：\(LevelTracker.highestUnlockedLevel)")

print("\n=== 自定义描述方法 ===\n")

struct Book {
    var title: String
    var author: String

    func summary() -> String {
        return "《\(title)》作者：\(author)"
    }
}

let book = Book(title: "Swift 编程", author: "Apple")
print(book.summary())

print("\n=== 链式调用 ===\n")

class MusicPlayer {
    private(set) var playlist: [String] = []

    @discardableResult
    func add(song: String) -> MusicPlayer {
        playlist.append(song)
        return self
    }

    @discardableResult
    func remove(song: String) -> MusicPlayer {
        playlist.removeAll { $0 == song }
        return self
    }

    func play() {
        print("开始播放：\(playlist.joined(separator: ", "))")
    }
}

let playerInstance = MusicPlayer()
playerInstance
    .add(song: "夜空中最亮的星")
    .add(song: "平凡之路")
    .remove(song: "平凡之路")
    .play()

print("\n=== extension 中的方法 ===\n")

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

var number = 5
number.square()
print("平方后的值：\(number)")

print("\n>>> 方法示例完成 <<<")
