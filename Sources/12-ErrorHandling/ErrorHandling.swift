#!/usr/bin/env swift

// ============================================
// Swift 错误处理
// ============================================

import Foundation

print("=== 错误类型定义 ===\n")

enum PrinterError: Error {
    case outOfPaper
    case noToner
    case onFire
}

func send(job: Int, toPrinter printerName: String) throws -> String {
    if printerName == "从未存在" {
        throw PrinterError.noToner
    }

    if printerName.isEmpty {
        throw PrinterError.outOfPaper
    }

    return "工作 \(job) 已送达"
}

print("=== do-catch ===\n")

do {
    let printerResponse = try send(job: 1040, toPrinter: "OfficePrinter")
    print(printerResponse)
} catch PrinterError.outOfPaper {
    print("缺纸")
} catch PrinterError.noToner {
    print("缺少墨粉")
} catch {
    print("未知错误：\(error)")
}

print("\n=== try? ===")

let success = try? send(job: 1884, toPrinter: "OfficePrinter")
let failure = try? send(job: 1885, toPrinter: "从未存在")
print("成功结果：\(String(describing: success))")
print("失败结果：\(String(describing: failure))")

print("\n=== try! ===")

let guaranteedSuccess = try! send(job: 1440, toPrinter: "OfficePrinter")
print(guaranteedSuccess)

print("\n=== 传播错误 ===")

func makeASandwich() throws {
    throw PrinterError.outOfPaper
}

func eatSandwich() {
    do {
        try makeASandwich()
        print("吃到了三明治")
    } catch {
        print("没吃到三明治：\(error)")
    }
}

eatSandwich()

print("\n=== defer ===")

func processFile(filename: String) throws {
    // 模拟打开文件
    print("打开文件 -> \(filename)")
    defer {
        print("关闭文件 -> \(filename)")
    }

    // 模拟错误
    throw PrinterError.onFire
}

do {
    try processFile(filename: "input.txt")
} catch {
    print("处理文件时出错：\(error)")
}

print("\n=== 自定义错误信息 ===")

enum ValidationError: Error {
    case invalidUsername
    case weakPassword(reason: String)
}

func register(username: String, password: String) throws {
    guard username.count >= 3 else {
        throw ValidationError.invalidUsername
    }

    guard password.count >= 6 else {
        throw ValidationError.weakPassword(reason: "密码过短")
    }

    guard password.rangeOfCharacter(from: CharacterSet.decimalDigits) != nil else {
        throw ValidationError.weakPassword(reason: "必须包含数字")
    }

    print("注册成功")
}

func attemptRegistration(username: String, password: String) {
    do {
        try register(username: username, password: password)
    } catch ValidationError.invalidUsername {
        print("用户名无效")
    } catch ValidationError.weakPassword(let reason) {
        print("密码太弱：\(reason)")
    } catch {
        print("其他错误：\(error)")
    }
}

attemptRegistration(username: "ab", password: "123456")
attemptRegistration(username: "alice", password: "secret")
attemptRegistration(username: "alice", password: "secret1")

print("\n=== Result 类型 ===")

func fetchUser(id: Int) -> Result<String, Error> {
    if id == 0 {
        return .failure(ValidationError.invalidUsername)
    }
    return .success("用户：\(id)")
}

let userResult = fetchUser(id: 1)
switch userResult {
case .success(let user):
    print("获取成功：\(user)")
case .failure(let error):
    print("获取失败：\(error)")
}

print("\n=== 实战案例：文件读取 ===")

enum FileError: Error {
    case notFound
    case unreadable
}

func readFile(at path: String) throws -> String {
    let files = ["readme.txt": "欢迎使用", "config.json": "{ }"]
    guard let content = files[path] else {
        throw FileError.notFound
    }
    return content
}

do {
    let content = try readFile(at: "readme.txt")
    print("文件内容：\(content)")
} catch FileError.notFound {
    print("文件不存在")
} catch {
    print("读取文件失败：\(error)")
}

print("\n>>> 错误处理示例完成 <<<")
