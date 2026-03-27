# 快速开始 · Swift 学习项目

欢迎！这里是 5 分钟快速入门指南。

## 第一步：验证环境

```bash
swift --version
```

如未安装 Swift，请参考 [README.md](README.md) 的环境配置章节。

## 第二步：运行第一个示例

```bash
# 运行基础知识示例
swift Sources/01-Basics/Variables.swift
```

你会看到关于常量、变量、可选类型等的输出。

## 第三步：使用交互式运行器

```bash
# 赋予执行权限（如果还没有）
chmod +x run_examples.sh

# 运行
./run_examples.sh
```

选择菜单选项即可运行相应章节的示例。

## 第四步：阅读文档

```bash
# 打开文档目录
cd Docs
ls -l

# 推荐阅读顺序：
# 01-基础知识.md
# 02-控制流.md
# 03-函数.md
# ...
```

每个文档对应 `Sources/` 中的一个章节。

## 第五步：实战项目

```bash
# TodoApp - 待办事项应用
swift Examples/TodoApp/TodoApp.swift

# WeatherApp - 天气应用
swift Examples/WeatherApp/WeatherApp.swift
```

## 学习建议

1. **每天 1-2 小时**：先读文档，再运行代码
2. **动手修改**：不要只是看，改改代码观察结果
3. **做笔记**：记录重点和疑问
4. **实践项目**：学完基础后尝试写自己的小项目

## 常用命令

```bash
# 运行单个文件
swift path/to/file.swift

# 进入 Swift REPL（交互环境）
swift

# 编译为可执行文件
swiftc file.swift -o output

# 运行编译后的程序
./output
```

## 遇到问题？

- 查看 [使用文档.md](使用文档.md) 了解详细说明
- 查看 [CONTRIBUTING.md](CONTRIBUTING.md) 提交 Issue 或 PR

**祝你学习愉快！** 🎉