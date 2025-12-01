#!/usr/bin/env swift

// ============================================
// TodoApp - 待办事项应用示例
// ============================================

import Foundation

struct TodoItem: Codable {
    let id: UUID
    var title: String
    var isCompleted: Bool
    let createdAt: Date

    init(title: String) {
        self.id = UUID()
        self.title = title
        self.isCompleted = false
        self.createdAt = Date()
    }
}

class TodoManager {
    private var items: [TodoItem] = []

    func add(title: String) {
        let item = TodoItem(title: title)
        items.append(item)
        print("✅ 已添加：\(title)")
    }

    func listAll() {
        guard !items.isEmpty else {
            print("📋 没有待办事项")
            return
        }

        print("\n📋 所有待办事项：")
        for (index, item) in items.enumerated() {
            let status = item.isCompleted ? "✅" : "⭕️"
            print("\(index + 1). \(status) \(item.title)")
        }
    }

    func complete(at index: Int) {
        guard index >= 0 && index < items.count else {
            print("❌ 无效的索引")
            return
        }
        items[index].isCompleted = true
        print("✅ 已完成：\(items[index].title)")
    }

    func remove(at index: Int) {
        guard index >= 0 && index < items.count else {
            print("❌ 无效的索引")
            return
        }
        let removed = items.remove(at: index)
        print("🗑️ 已删除：\(removed.title)")
    }

    func listPending() {
        let pending = items.filter { !$0.isCompleted }
        guard !pending.isEmpty else {
            print("🎉 所有任务已完成！")
            return
        }

        print("\n⏰ 待完成事项：")
        for (index, item) in pending.enumerated() {
            print("\(index + 1). \(item.title)")
        }
    }

    func statistics() {
        let total = items.count
        let completed = items.filter { $0.isCompleted }.count
        let pending = total - completed
        let completionRate = total > 0 ? Double(completed) / Double(total) * 100 : 0

        print("\n📊 统计信息：")
        print("总任务：\(total)")
        print("已完成：\(completed)")
        print("待完成：\(pending)")
        print("完成率：\(String(format: "%.1f", completionRate))%")
    }
}

print("=== TodoApp 待办事项应用 ===\n")

let manager = TodoManager()

manager.add(title: "学习 Swift 基础语法")
manager.add(title: "完成闭包练习")
manager.add(title: "阅读泛型章节")
manager.add(title: "实现一个小项目")

manager.listAll()

print("\n--- 完成第一个任务 ---")
manager.complete(at: 0)

print("\n--- 完成第二个任务 ---")
manager.complete(at: 1)

manager.listAll()

manager.listPending()

manager.statistics()

print("\n--- 删除已完成的任务 ---")
manager.remove(at: 0)

manager.listAll()

manager.statistics()

print("\n🎉 TodoApp 示例运行完成！")
