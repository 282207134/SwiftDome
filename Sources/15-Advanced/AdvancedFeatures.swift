#!/usr/bin/env swift

// ============================================
// Swift 高级特性
// ============================================

import Foundation

print("=== 类型转换 ===\n")

class MediaItem {
    var name: String
    init(name: String) {
        self.name = name
    }
}

class Movie: MediaItem {
    var director: String
    init(name: String, director: String) {
        self.director = director
        super.init(name: name)
    }
}

class Song: MediaItem {
    var artist: String
    init(name: String, artist: String) {
        self.artist = artist
        super.init(name: name)
    }
}

let library = [
    Movie(name: "盗梦空间", director: "克里斯托弗·诺兰"),
    Song(name: "夜空中最亮的星", artist: "逃跑计划"),
    Movie(name: "星际穿越", director: "克里斯托弗·诺兰"),
    Song(name: "平凡之路", artist: "朴树")
]

var movieCount = 0
var songCount = 0

for item in library {
    if item is Movie {
        movieCount += 1
    } else if item is Song {
        songCount += 1
    }
}

print("媒体库包含 \(movieCount) 部电影和 \(songCount) 首歌曲")

print("\n=== 向下转型 ===\n")

for item in library {
    if let movie = item as? Movie {
        print("电影：\(movie.name)，导演：\(movie.director)")
    } else if let song = item as? Song {
        print("歌曲：\(song.name)，艺术家：\(song.artist)")
    }
}

print("\n=== Any 和 AnyObject ===\n")

var things: [Any] = []
things.append(0)
things.append(0.0)
things.append(42)
things.append(3.14159)
things.append("hello")
things.append((3.0, 5.0))
things.append(Movie(name: "阿甘正传", director: "罗伯特·泽米吉斯"))

for thing in things {
    switch thing {
    case 0 as Int:
        print("整数零")
    case 0 as Double:
        print("浮点数零")
    case let someInt as Int:
        print("整数值：\(someInt)")
    case let someDouble as Double where someDouble > 0:
        print("正浮点数值：\(someDouble)")
    case is Double:
        print("某个其他双精度浮点数")
    case let someString as String:
        print("字符串值：\"\(someString)\"")
    case let (x, y) as (Double, Double):
        print("(x, y) 点：(\(x), \(y))")
    case let movie as Movie:
        print("电影：\(movie.name)，导演：\(movie.director)")
    default:
        print("其他类型")
    }
}

print("\n=== 内存管理：弱引用和无主引用 ===\n")

class Person {
    let name: String
    init(name: String) {
        self.name = name
        print("\(name) 初始化")
    }
    var apartment: Apartment?
    deinit {
        print("\(name) 被释放")
    }
}

class Apartment {
    let unit: String
    init(unit: String) {
        self.unit = unit
        print("公寓 \(unit) 初始化")
    }
    weak var tenant: Person?
    deinit {
        print("公寓 \(unit) 被释放")
    }
}

var john: Person? = Person(name: "John")
var unit4A: Apartment? = Apartment(unit: "4A")

john?.apartment = unit4A
unit4A?.tenant = john

print("\n打破循环引用：")
john = nil
unit4A = nil

print("\n=== 可选链 ===\n")

class Person2 {
    var residence: Residence?
}

class Residence {
    var rooms: [Room] = []
    var numberOfRooms: Int {
        return rooms.count
    }
    subscript(i: Int) -> Room {
        get {
            return rooms[i]
        }
        set {
            rooms[i] = newValue
        }
    }
    func printNumberOfRooms() {
        print("房间数量：\(numberOfRooms)")
    }
    var address: Address?
}

class Room {
    let name: String
    init(name: String) { self.name = name }
}

class Address {
    var buildingName: String?
    var buildingNumber: String?
    var street: String?
    func buildingIdentifier() -> String? {
        if let buildingNumber = buildingNumber, let street = street {
            return "\(buildingNumber) \(street)"
        } else if buildingName != nil {
            return buildingName
        } else {
            return nil
        }
    }
}

let person2 = Person2()
if let roomCount = person2.residence?.numberOfRooms {
    print("房间数量：\(roomCount)")
} else {
    print("无法获取房间数量")
}

print("\n=== 嵌套类型 ===\n")

struct BlackjackCard {
    enum Suit: Character {
        case spades = "♠", hearts = "♡", diamonds = "♢", clubs = "♣"
    }

    enum Rank: Int {
        case two = 2, three, four, five, six, seven, eight, nine, ten
        case jack, queen, king, ace

        struct Values {
            let first: Int, second: Int?
        }

        var values: Values {
            switch self {
            case .ace:
                return Values(first: 1, second: 11)
            case .jack, .queen, .king:
                return Values(first: 10, second: nil)
            default:
                return Values(first: self.rawValue, second: nil)
            }
        }
    }

    let rank: Rank, suit: Suit
    var description: String {
        var output = "\(suit.rawValue) "
        output += String(rank.values.first)
        if let second = rank.values.second {
            output += " 或 \(second)"
        }
        return output
    }
}

let theAceOfSpades = BlackjackCard(rank: .ace, suit: .spades)
print("黑桃 A：\(theAceOfSpades.description)")

print("\n=== 类型别名 ===\n")

typealias Coordinate = (x: Double, y: Double)

func distance(from point1: Coordinate, to point2: Coordinate) -> Double {
    let dx = point1.x - point2.x
    let dy = point1.y - point2.y
    return sqrt(dx * dx + dy * dy)
}

let start: Coordinate = (0, 0)
let end: Coordinate = (3, 4)
print("距离：\(distance(from: start, to: end))")

print("\n=== 访问控制 ===\n")

public class PublicClass {
    public var publicProperty = 0
    internal var internalProperty = 0
    fileprivate var fileprivateProperty = 0
    private var privateProperty = 0

    public init() {}
}

print("访问控制关键字：")
print("- open: 最开放，允许在模块外继承和重写")
print("- public: 公开，允许访问但不能继承")
print("- internal: 默认，模块内可见")
print("- fileprivate: 文件私有")
print("- private: 真正私有，作用域内可见")

print("\n>>> 高级特性示例完成 <<<")
