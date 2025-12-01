# Swift 学习项目

欢迎来到 Swift 学习项目！这是一个全面的 Swift 语言学习资源库，包含了 Swift 编程语言的各种特性、示例代码和详细的中文文档。

## 📚 项目简介

本项目参考 [Swift 官方中文文档](https://doc.swiftgg.team/documentation/the-swift-programming-language/)，系统地整理了 Swift 编程语言的核心概念和实践示例。每个示例都包含详细的中文注释，帮助你快速理解和掌握 Swift。

## 🎯 学习目标

- 掌握 Swift 基础语法和核心概念
- 理解面向对象编程和协议导向编程
- 学习 Swift 的高级特性（泛型、闭包、错误处理等）
- 实践常用设计模式和最佳实践
- 能够独立开发 Swift 应用程序

## 📂 项目结构

```
SwiftLearning/
├── README.md                    # 项目说明文档
├── 使用文档.md                  # 详细使用指南
├── Sources/                     # 源代码目录
│   ├── 01-Basics/              # 基础知识
│   ├── 02-ControlFlow/         # 控制流
│   ├── 03-Functions/           # 函数
│   ├── 04-Closures/            # 闭包
│   ├── 05-Collections/         # 集合类型
│   ├── 06-Structures/          # 结构体和类
│   ├── 07-Properties/          # 属性
│   ├── 08-Methods/             # 方法
│   ├── 09-Inheritance/         # 继承
│   ├── 10-Protocols/           # 协议
│   ├── 11-Generics/            # 泛型
│   ├── 12-ErrorHandling/       # 错误处理
│   ├── 13-Concurrency/         # 并发编程
│   ├── 14-Extensions/          # 扩展
│   └── 15-Advanced/            # 高级特性
├── Docs/                       # 学习文档
│   ├── 01-基础知识.md
│   ├── 02-控制流.md
│   ├── 03-函数.md
│   ├── 04-闭包.md
│   ├── 05-集合类型.md
│   ├── 06-结构体和类.md
│   ├── 07-属性.md
│   ├── 08-方法.md
│   ├── 09-继承.md
│   ├── 10-协议.md
│   ├── 11-泛型.md
│   ├── 12-错误处理.md
│   ├── 13-并发编程.md
│   ├── 14-扩展.md
│   └── 15-高级特性.md
└── Examples/                   # 综合示例项目
    ├── TodoApp/                # 待办事项应用
    └── WeatherApp/             # 天气应用
```

## 🚀 快速开始

### 环境要求

- macOS 12.0+ / Linux (Ubuntu 20.04+)
- Swift 5.5+
- Xcode 13.0+ (macOS) 或 Swift 命令行工具

### 安装 Swift

**macOS:**
```bash
# 通过 Xcode 安装（推荐）
# 从 App Store 下载 Xcode

# 或使用 Homebrew
brew install swift
```

**Linux:**
```bash
# Ubuntu/Debian
wget https://swift.org/builds/swift-5.9-release/ubuntu2004/swift-5.9-RELEASE/swift-5.9-RELEASE-ubuntu20.04.tar.gz
tar xzf swift-5.9-RELEASE-ubuntu20.04.tar.gz
sudo mv swift-5.9-RELEASE-ubuntu20.04 /usr/share/swift
echo "export PATH=/usr/share/swift/usr/bin:$PATH" >> ~/.bashrc
source ~/.bashrc
```

### 验证安装

```bash
swift --version
```

## 📖 使用方法

### 1. 克隆项目

```bash
git clone <repository-url>
cd SwiftLearning
```

### 2. 运行示例代码

每个源文件都可以直接运行：

```bash
# 运行单个文件
swift Sources/01-Basics/Variables.swift

# 运行所有基础示例
cd Sources/01-Basics
for file in *.swift; do
    echo "运行 $file..."
    swift "$file"
done
```

### 3. 学习建议

1. **按顺序学习**：从 `01-Basics` 开始，逐步深入
2. **阅读文档**：先阅读 `Docs/` 中对应的文档
3. **运行代码**：实际运行 `Sources/` 中的示例代码
4. **动手实践**：修改代码，观察结果变化
5. **完成项目**：学习完基础后，尝试 `Examples/` 中的综合项目

## 📝 学习路线

### 初级（第 1-5 周）
- ✅ 基础知识：变量、常量、数据类型
- ✅ 控制流：条件语句、循环
- ✅ 函数：定义、调用、参数
- ✅ 集合类型：数组、字典、集合
- ✅ 结构体和类：面向对象基础

### 中级（第 6-10 周）
- ✅ 属性：存储属性、计算属性、观察器
- ✅ 方法：实例方法、类型方法
- ✅ 继承：子类、重写、多态
- ✅ 协议：定义、遵循、协议组合
- ✅ 闭包：语法、捕获值、逃逸闭包

### 高级（第 11-15 周）
- ✅ 泛型：泛型函数、泛型类型、约束
- ✅ 错误处理：抛出、捕获、传播错误
- ✅ 并发编程：async/await、Task、Actor
- ✅ 扩展：为现有类型添加功能
- ✅ 高级特性：内存管理、类型转换、嵌套类型

## 🎓 学习资源

- [Swift 官方中文文档](https://doc.swiftgg.team/documentation/the-swift-programming-language/)
- [Swift.org 官方网站](https://swift.org)
- [Swift 标准库文档](https://developer.apple.com/documentation/swift)
- [SwiftUI 教程](https://developer.apple.com/tutorials/swiftui)

## 💡 贡献指南

欢迎贡献代码和文档！请查看 [CONTRIBUTING.md](CONTRIBUTING.md) 了解详情。

## 📄 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。

## 🤝 联系方式

如有问题或建议，请提交 Issue 或 Pull Request。

---

**Happy Learning! 祝你学习愉快！** 🎉
