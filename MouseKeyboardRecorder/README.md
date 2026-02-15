# 🖱️⌨️ 鼠标键盘录制器

一款功能强大的 Windows 鼠标键盘操作录制与播放工具，使用 C# WPF 技术开发。

## ✨ 功能特性

### 🎬 录制功能
- ✅ **全局录制** - 捕获所有鼠标和键盘操作
- ✅ **鼠标支持** - 移动、点击（左/右/中键）、滚轮滚动
- ✅ **键盘支持** - 所有按键的按下和释放
- ✅ **智能节流** - 自动过滤不必要的鼠标移动事件
- ✅ **时间精确** - 高精度记录操作间隔

### ▶️ 播放功能
- ✅ **循环播放** - 支持无限循环或指定次数循环
- ✅ **速度调节** - 0.1x - 5.0x 播放速度
- ✅ **人类模拟** - 添加随机延迟，模拟真实人类操作
- ✅ **进度显示** - 实时显示播放进度和当前操作

### 📝 编辑功能
- ✅ **操作列表** - 可视化展示所有录制的操作
- ✅ **单独编辑** - 修改单个操作的参数
- ✅ **删除操作** - 移除不需要的操作
- ✅ **重新排序** - 支持操作的增删改

### 💾 文件操作
- ✅ **JSON 格式** - 使用标准 JSON 格式存储
- ✅ **最近文件** - 自动管理最近使用的文件
- ✅ **版本兼容** - 支持文件格式版本控制

### ⌨️ 快捷键
- **F9** - 开始/停止录制
- **F10** - 播放/暂停

## 🚀 快速开始

### 环境要求
- Windows 10/11 操作系统
- .NET 8.0 Runtime 或 SDK

### 编译运行
```bash
# 进入项目目录
cd MouseKeyboardRecorder

# 编译项目
dotnet build

# 运行程序
dotnet run
```

### 发布应用
```bash
# 发布为独立可执行文件
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# 输出目录
# bin/Release/net8.0-windows/win-x64/publish/
```

## 📖 使用指南

### 1. 录制操作
1. 点击 **🔴 录制** 按钮开始录制
2. 执行需要录制的鼠标键盘操作
3. 点击 **⏹️ 停止** 按钮结束录制

### 2. 播放操作
1. 调整 **播放速度** 滑块（可选）
2. 设置 **循环次数**（0 表示无限循环）
3. 勾选 **模拟人类操作**（推荐）
4. 点击 **▶️ 播放** 按钮开始播放

### 3. 编辑操作
1. 在操作列表中选择要编辑的操作
2. 点击 **✏️ 编辑** 按钮
3. 修改操作参数后点击 **确定**

### 4. 保存/加载
- 点击 **💾 保存** 将录制保存为 JSON 文件
- 点击 **📂 加载** 打开已有的录制文件

## 📁 项目结构

```
MouseKeyboardRecorder/
├── Models/                     # 数据模型
│   ├── ActionType.cs          # 操作类型枚举
│   ├── RecordedAction.cs      # 单个操作模型
│   └── RecordingFile.cs       # 录制文件模型
├── Services/                   # 业务逻辑服务
│   ├── IRecorderService.cs    # 录制服务接口
│   ├── RecorderService.cs     # 录制服务实现
│   ├── IPlayerService.cs      # 播放服务接口
│   └── PlayerService.cs       # 播放服务实现
├── Helpers/                    # Windows API 封装
│   ├── NativeMethods.cs       # P/Invoke 声明
│   └── InputSimulator.cs      # 输入模拟器
├── Utilities/                  # 工具类
│   ├── JsonHelper.cs          # JSON 序列化
│   ├── RandomHelper.cs        # 随机数生成
│   └── RecentFilesHelper.cs   # 最近文件管理
├── Views/                      # 视图层
│   ├── EditActionWindow.xaml  # 操作编辑窗口
│   └── EditActionWindow.xaml.cs
├── MainWindow.xaml            # 主窗口 UI
├── MainWindow.xaml.cs         # 主窗口逻辑
├── App.xaml                   # 应用程序定义
├── App.xaml.cs                # 应用程序入口
└── ValueConverters.cs         # 值转换器
```

## 🔧 技术架构

### 核心技术
- **.NET 8.0** - 运行时框架
- **WPF** - 用户界面框架
- **Windows API** - 底层钩子与输入模拟

### 关键组件
| 组件 | 说明 |
|------|------|
| `SetWindowsHookEx` | 低级别输入钩子 |
| `SendInput` | 合成输入事件 |
| `System.Text.Json` | JSON 序列化 |

## 🛡️ 权限要求

由于使用 Windows 低级别钩子捕获全局输入，程序需要 **管理员权限** 运行。

首次运行时会自动请求管理员权限（通过 `app.manifest` 配置）。

## 🐛 故障排除

### 无法开始录制
- 确保以管理员身份运行程序
- 检查是否有其他程序占用了输入钩子

### 播放位置不准确
- 录制和播放时保持相同的屏幕分辨率
- 避免在播放过程中移动窗口位置

### 某些按键无法录制
- 某些系统级快捷键（如 Ctrl+Alt+Del）无法捕获
- 这是 Windows 安全机制的限制

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request！

### 提交规范
- 使用中文或英文提交说明
- 描述问题时请提供复现步骤
- 提交代码前请确保通过编译

## 📝 许可证

本项目采用 MIT 许可证 - 详见 LICENSE 文件

## 🙏 致谢

感谢使用本工具！如有任何问题或建议，欢迎反馈。

---

**Made with ❤️ using C# + WPF**
