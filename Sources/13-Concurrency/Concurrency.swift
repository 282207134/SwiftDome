#!/usr/bin/env swift

// ============================================
// Swift 并发编程（async/await）
// ============================================

import Foundation

print("=== 异步函数 ===\n")

func fetchUserID(from server: String) async -> Int {
    print("正在从 \(server) 获取用户 ID...")
    try? await Task.sleep(nanoseconds: 1_000_000_000)
    return 42
}

func fetchUsername(for id: Int) async -> String {
    print("正在获取用户 \(id) 的用户名...")
    try? await Task.sleep(nanoseconds: 500_000_000)
    return "Alice"
}

print("=== 并发执行 ===\n")

func fetchUserInfo() async {
    async let userID = fetchUserID(from: "primary")
    async let username = fetchUsername(for: await userID)

    let user = await "用户 ID: \(userID), 用户名: \(username)"
    print(user)
}

Task {
    await fetchUserInfo()
}

print("\n=== Task Group ===\n")

func downloadPhotos(urls: [String]) async -> [String] {
    await withTaskGroup(of: String.self) { group in
        for url in urls {
            group.addTask {
                print("下载：\(url)")
                try? await Task.sleep(nanoseconds: 500_000_000)
                return "照片来自 \(url)"
            }
        }

        var results: [String] = []
        for await result in group {
            results.append(result)
        }
        return results
    }
}

Task {
    let urls = ["url1", "url2", "url3"]
    let photos = await downloadPhotos(urls: urls)
    print("下载完成：\(photos.count) 张照片")
}

print("\n=== Actor ===\n")

actor Counter {
    private var value = 0

    func increment() -> Int {
        value += 1
        return value
    }

    func getValue() -> Int {
        return value
    }
}

Task {
    let counter = Counter()

    await withTaskGroup(of: Int.self) { group in
        for _ in 0..<5 {
            group.addTask {
                await counter.increment()
            }
        }

        for await result in group {
            print("计数：\(result)")
        }
    }

    let finalValue = await counter.getValue()
    print("最终计数：\(finalValue)")
}

print("\n=== 异步序列 ===\n")

struct NumberSequence: AsyncSequence {
    typealias Element = Int

    struct AsyncIterator: AsyncIteratorProtocol {
        var current = 1

        mutating func next() async -> Int? {
            guard current <= 5 else { return nil }
            try? await Task.sleep(nanoseconds: 200_000_000)
            defer { current += 1 }
            return current
        }
    }

    func makeAsyncIterator() -> AsyncIterator {
        return AsyncIterator()
    }
}

Task {
    for await number in NumberSequence() {
        print("接收到数字：\(number)")
    }
}

print("\n=== MainActor ===\n")

@MainActor
class ViewModel {
    var title = "初始标题"

    func updateTitle() {
        title = "更新后的标题"
        print("标题已更新：\(title)")
    }
}

Task { @MainActor in
    let viewModel = ViewModel()
    viewModel.updateTitle()
}

print("\n=== 异步错误处理 ===\n")

enum NetworkError: Error {
    case badURL
    case requestFailed
}

func fetchData(from url: String) async throws -> String {
    guard !url.isEmpty else {
        throw NetworkError.badURL
    }

    try await Task.sleep(nanoseconds: 500_000_000)

    if url.contains("error") {
        throw NetworkError.requestFailed
    }

    return "来自 \(url) 的数据"
}

Task {
    do {
        let data = try await fetchData(from: "https://api.example.com")
        print("获取成功：\(data)")
    } catch NetworkError.badURL {
        print("URL 无效")
    } catch {
        print("请求失败：\(error)")
    }
}

print("\n=== 实战案例：批量下载 ===\n")

struct FileDownloader {
    func download(file: String) async throws -> String {
        print("开始下载：\(file)")
        try await Task.sleep(nanoseconds: UInt64.random(in: 500_000_000...1_000_000_000))
        return "✅ \(file)"
    }

    func downloadAll(files: [String]) async throws -> [String] {
        try await withThrowingTaskGroup(of: String.self) { group in
            for file in files {
                group.addTask {
                    try await self.download(file: file)
                }
            }

            var results: [String] = []
            for try await result in group {
                results.append(result)
            }
            return results
        }
    }
}

Task {
    let downloader = FileDownloader()
    let files = ["file1.zip", "file2.pdf", "file3.mp4"]

    do {
        let results = try await downloader.downloadAll(files: files)
        print("\n下载结果：")
        for result in results {
            print(result)
        }
    } catch {
        print("下载出错：\(error)")
    }
}

print("\n=== 等待所有任务完成 ===")

Task {
    try? await Task.sleep(nanoseconds: 5_000_000_000)
    print("\n>>> 并发编程示例完成 <<<")
}

RunLoop.main.run(until: Date(timeIntervalSinceNow: 6))
