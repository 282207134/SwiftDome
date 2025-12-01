#!/usr/bin/env swift

// ============================================
// Swift 协议
// ============================================

import Foundation

print("=== 协议定义 ===\n")

protocol FullyNamed {
    var fullName: String { get }
}

struct Person: FullyNamed {
    var fullName: String
}

let john = Person(fullName: "张三")
print(john.fullName)

class Starship: FullyNamed {
    var prefix: String?
    var name: String

    init(name: String, prefix: String? = nil) {
        self.name = name
        self.prefix = prefix
    }

    var fullName: String {
        return (prefix != nil ? prefix! + " " : "") + name
    }
}

let ncc1701 = Starship(name: "Enterprise", prefix: "USS")
print(ncc1701.fullName)

print("\n=== 方法要求 ===\n")

protocol RandomNumberGenerator {
    func random() -> Double
}

class LinearCongruentialGenerator: RandomNumberGenerator {
    var lastRandom = 42.0
    let m = 139968.0
    let a = 3877.0
    let c = 29573.0

    func random() -> Double {
        lastRandom = (lastRandom * a + c).truncatingRemainder(dividingBy: m)
        return lastRandom / m
    }
}

let generator = LinearCongruentialGenerator()
print("随机数：\(generator.random())")
print("随机数：\(generator.random())")

print("\n=== 可变方法要求 ===\n")

protocol Togglable {
    mutating func toggle()
}

enum OnOffSwitch: Togglable {
    case off, on
    mutating func toggle() {
        self = (self == .off) ? .on : .off
    }
}

var lightSwitch = OnOffSwitch.off
lightSwitch.toggle()
print("开关状态：\(lightSwitch)")

print("\n=== 协议构造器要求 ===\n")

protocol SomeProtocol {
    init()
}

class SomeClass: SomeProtocol {
    required init() {
        // 必须使用 required 以确保子类也实现
    }
}

print("\n=== 作为类型使用 ===\n")

class Dice {
    let sides: Int
    let generator: RandomNumberGenerator

    init(sides: Int, generator: RandomNumberGenerator) {
        self.sides = sides
        self.generator = generator
    }

    func roll() -> Int {
        return Int(generator.random() * Double(sides)) + 1
    }
}

var d6 = Dice(sides: 6, generator: LinearCongruentialGenerator())
print("掷骰子：\(d6.roll())")

print("\n=== 委托模式 ===\n")

protocol DiceGame {
    var dice: Dice { get }
    func play()
}

protocol DiceGameDelegate: AnyObject {
    func gameDidStart(_ game: DiceGame)
    func game(_ game: DiceGame, didStartNewTurnWithDiceRoll diceRoll: Int)
    func gameDidEnd(_ game: DiceGame)
}

class SnakesAndLadders: DiceGame {
    let finalSquare = 25
    let dice = Dice(sides: 6, generator: LinearCongruentialGenerator())
    var square = 0
    weak var delegate: DiceGameDelegate?
    let board: [Int]

    init() {
        board = [
            0, 0, 0, +8, 0, 0, 0, +11, 0, 0,
            +9, 0, 0, 0, 0, -11, 0, 0, 0, -2,
            0, 0, 0, 0, 0, 0
        ]
    }

    func play() {
        square = 0
        delegate?.gameDidStart(self)
        gameLoop: while square != finalSquare {
            let diceRoll = dice.roll()
            delegate?.game(self, didStartNewTurnWithDiceRoll: diceRoll)
            switch square + diceRoll {
            case finalSquare:
                break gameLoop
            case let newSquare where newSquare > finalSquare:
                continue gameLoop
            default:
                square += diceRoll
                square += board[square]
            }
        }
        delegate?.gameDidEnd(self)
    }
}

class DiceGameTracker: DiceGameDelegate {
    var numberOfTurns = 0

    func gameDidStart(_ game: DiceGame) {
        numberOfTurns = 0
        print("游戏开始")
    }

    func game(_ game: DiceGame, didStartNewTurnWithDiceRoll diceRoll: Int) {
        numberOfTurns += 1
        print("第 \(numberOfTurns) 回合，掷出：\(diceRoll)")
    }

    func gameDidEnd(_ game: DiceGame) {
        print("游戏结束，共进行了 \(numberOfTurns) 回合")
    }
}

let tracker = DiceGameTracker()
let game = SnakesAndLadders()
game.delegate = tracker
// 为保证示例输出可控，这里不实际运行 game.play()

print("\n=== 协议扩展 ===\n")

extension RandomNumberGenerator {
    func randomBool() -> Bool {
        return random() > 0.5
    }
}

print("随机布尔值：\(generator.randomBool())")

print("\n=== 协议组合 ===\n")

protocol Named {
    var name: String { get }
}

protocol Aged {
    var age: Int { get }
}

struct PersonInfo: Named, Aged {
    var name: String
    var age: Int
}

func wishHappyBirthday(to celebrator: Named & Aged) {
    print("祝 \(celebrator.name) \(celebrator.age) 岁生日快乐！")
}

let birthdayPerson = PersonInfo(name: "小明", age: 20)
wishHappyBirthday(to: birthdayPerson)

print("\n=== 可选协议要求 ===\n")

@objc protocol CounterDataSource {
    @objc optional func increment(forCount count: Int) -> Int
    @objc optional var fixedIncrement: Int { get }
}

class Counter {
    var count = 0
    var dataSource: CounterDataSource?

    func increment() {
        if let amount = dataSource?.increment?(forCount: count) {
            count += amount
        } else if let amount = dataSource?.fixedIncrement {
            count += amount
        }
    }
}

class ThreeSource: NSObject, CounterDataSource {
    let fixedIncrement = 3
}

var counter = Counter()
counter.dataSource = ThreeSource()
for _ in 0..<4 {
    counter.increment()
    print("计数器：\(counter.count)")
}

print("\n=== 协议继承 ===\n")

protocol TextRepresentable {
    var textualDescription: String { get }
}

protocol PrettyTextRepresentable: TextRepresentable {
    var prettyTextualDescription: String { get }
}

struct Hamster: TextRepresentable {
    var name: String
    var textualDescription: String {
        return "仓鼠：\(name)"
    }
}

let simonTheHamster = Hamster(name: "Simon")
print(simonTheHamster.textualDescription)

print("\n>>> 协议示例完成 <<<")
