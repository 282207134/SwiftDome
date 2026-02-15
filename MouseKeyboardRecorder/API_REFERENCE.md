# 📚 API 参考文档

本文档详细介绍鼠标键盘录制器的所有公共 API。

## 📑 目录

- [Models](#models)
- [Services](#services)
- [Helpers](#helpers)
- [Utilities](#utilities)

---

## Models

### ActionType

操作类型枚举，定义所有支持的操作。

```csharp
public enum ActionType
{
    MouseMove = 0,       // 鼠标移动
    MouseLeftDown = 1,   // 鼠标左键按下
    MouseLeftUp = 2,     // 鼠标左键释放
    MouseRightDown = 3,  // 鼠标右键按下
    MouseRightUp = 4,    // 鼠标右键释放
    MouseMiddleDown = 5, // 鼠标中键按下
    MouseMiddleUp = 6,   // 鼠标中键释放
    MouseWheel = 7,      // 鼠标滚轮
    KeyDown = 10,        // 键盘按键按下
    KeyUp = 11,          // 键盘按键释放
    KeyPress = 12,       // 键盘按键输入
    Wait = 20,           // 等待/延迟
    Special = 99         // 特殊操作
}
```

**扩展方法**：

| 方法 | 返回类型 | 说明 |
|------|----------|------|
| `GetDisplayName()` | `string` | 获取中文显示名称 |
| `IsMouseAction()` | `bool` | 判断是否为鼠标操作 |
| `IsKeyboardAction()` | `bool` | 判断是否为键盘操作 |

---

### RecordedAction

录制的单个操作数据模型。

```csharp
public class RecordedAction
{
    public Guid Id { get; set; }              // 唯一标识符
    public ActionType ActionType { get; set; } // 操作类型
    public int X { get; set; }                // X 坐标
    public int Y { get; set; }                // Y 坐标
    public int VirtualKeyCode { get; set; }   // 虚拟键码
    public int ScanCode { get; set; }         // 扫描码
    public string? Character { get; set; }    // 字符表示
    public int WheelDelta { get; set; }       // 滚轮滚动量
    public int DelayMs { get; set; }          // 延迟时间（毫秒）
    public DateTime Timestamp { get; set; }   // 时间戳
    public int Flags { get; set; }            // 扩展标志
    public string? Description { get; set; }  // 描述
}
```

**构造函数**：

```csharp
public RecordedAction()                          // 默认构造
public RecordedAction(ActionType actionType)     // 指定操作类型
```

**方法**：

| 方法 | 返回类型 | 说明 |
|------|----------|------|
| `GetDetailDescription()` | `string` | 获取详细描述 |
| `Clone()` | `RecordedAction` | 创建深拷贝 |
| `IsValid()` | `bool` | 验证数据有效性 |

---

### RecordingFile

录制文件数据模型。

```csharp
public class RecordingFile
{
    public string Version { get; set; }              // 版本号
    public Guid Id { get; set; }                     // 唯一标识
    public string? Name { get; set; }                // 名称
    public string? Description { get; set; }         // 描述
    public DateTime CreatedAt { get; set; }          // 创建时间
    public DateTime ModifiedAt { get; set; }         // 修改时间
    public int ScreenWidth { get; set; }             // 屏幕宽度
    public int ScreenHeight { get; set; }            // 屏幕高度
    public string? OsVersion { get; set; }           // 系统版本
    public List<RecordedAction> Actions { get; set; } // 操作列表
    
    // 计算属性
    public int TotalDurationMs { get; }              // 总时长
    public int ActionCount { get; }                  // 操作数量
}
```

**方法**：

| 方法 | 返回类型 | 说明 |
|------|----------|------|
| `Clone()` | `RecordingFile` | 创建深拷贝 |
| `IsValid()` | `bool` | 验证数据有效性 |
| `GetSummary()` | `string` | 获取摘要信息 |

---

## Services

### IRecorderService

录制服务接口。

```csharp
public interface IRecorderService
{
    // 属性
    bool IsRecording { get; }
    IReadOnlyList<RecordedAction> RecordedActions { get; }
    int ActionCount { get; }
    
    // 事件
    event EventHandler? RecordingStarted;
    event EventHandler? RecordingStopped;
    event EventHandler<RecordedAction>? ActionRecorded;
    event EventHandler<Exception>? RecordingError;
    
    // 方法
    bool StartRecording();
    void StopRecording();
    void ClearRecording();
    RecordingFile GetRecordingFile();
    void LoadFromRecordingFile(RecordingFile recordingFile);
    void AddAction(RecordedAction action);
    bool RemoveAction(Guid actionId);
    bool UpdateAction(RecordedAction action);
}
```

**使用示例**：

```csharp
var recorder = new RecorderService();

// 订阅事件
recorder.RecordingStarted += (s, e) => Console.WriteLine("开始录制");
recorder.ActionRecorded += (s, action) => Console.WriteLine($"录制: {action.ActionType}");

// 开始录制
if (recorder.StartRecording())
{
    // 录制中...
    Thread.Sleep(5000);
    
    // 停止录制
    recorder.StopRecording();
    
    // 获取录制的操作
    foreach (var action in recorder.RecordedActions)
    {
        Console.WriteLine(action.GetDetailDescription());
    }
}
```

---

### IPlayerService

播放服务接口。

```csharp
public interface IPlayerService
{
    // 属性
    PlaybackState State { get; }
    double PlaybackSpeed { get; set; }
    bool LoopEnabled { get; set; }
    int LoopCount { get; set; }
    bool SimulateHumanBehavior { get; set; }
    int CurrentIndex { get; }
    int TotalActions { get; }
    double Progress { get; }
    int CurrentLoop { get; }
    
    // 事件
    event EventHandler? PlaybackStarted;
    event EventHandler? PlaybackPaused;
    event EventHandler? PlaybackResumed;
    event EventHandler? PlaybackStopped;
    event EventHandler? PlaybackCompleted;
    event EventHandler<double>? ProgressChanged;
    event EventHandler<RecordedAction>? CurrentActionChanged;
    event EventHandler<Exception>? PlaybackError;
    
    // 方法
    void LoadRecording(RecordingFile recordingFile);
    void LoadActions(IEnumerable<RecordedAction> actions);
    Task PlayAsync();
    Task PlayFromAsync(int startIndex);
    void Pause();
    void Resume();
    void Stop();
    void SeekTo(int index);
}
```

**PlaybackState 枚举**：

```csharp
public enum PlaybackState
{
    Idle = 0,       // 空闲
    Playing = 1,    // 播放中
    Paused = 2,     // 已暂停
    Completed = 3,  // 已完成
    Stopped = 4,    // 已停止
    Error = 5       // 错误
}
```

**使用示例**：

```csharp
var player = new PlayerService();

// 加载录制
player.LoadRecording(recordingFile);

// 配置播放参数
player.PlaybackSpeed = 1.5;           // 1.5倍速
player.LoopEnabled = true;            // 启用循环
player.LoopCount = 3;                 // 循环3次
player.SimulateHumanBehavior = true;  // 模拟人类操作

// 订阅事件
player.PlaybackStarted += (s, e) => Console.WriteLine("开始播放");
player.ProgressChanged += (s, progress) => Console.WriteLine($"进度: {progress:P0}");

// 开始播放
await player.PlayAsync();
```

---

## Helpers

### NativeMethods

Windows API P/Invoke 声明。

**常用常量**：

```csharp
// 钩子类型
public const int WH_MOUSE_LL = 14;
public const int WH_KEYBOARD_LL = 13;

// 鼠标消息
public const int WM_MOUSEMOVE = 0x0200;
public const int WM_LBUTTONDOWN = 0x0201;
public const int WM_LBUTTONUP = 0x0202;
// ... 更多消息

// 虚拟键码
public const byte VK_ESCAPE = 0x1B;
public const byte VK_SPACE = 0x20;
// ... 更多键码
```

**API 函数**：

| 函数 | 说明 |
|------|------|
| `SetWindowsHookEx` | 安装钩子 |
| `UnhookWindowsHookEx` | 卸载钩子 |
| `CallNextHookEx` | 调用下一个钩子 |
| `SendInput` | 合成输入事件 |
| `mouse_event` | 合成鼠标事件 |
| `keybd_event` | 合成键盘事件 |
| `GetAsyncKeyState` | 获取异步键状态 |
| `GetCursorPos` | 获取鼠标位置 |
| `SetCursorPos` | 设置鼠标位置 |

---

### InputSimulator

输入模拟器，提供鼠标和键盘操作的模拟功能。

**鼠标方法**：

```csharp
// 移动鼠标
public static void MoveMouse(int x, int y)

// 鼠标按键
public static void MouseLeftDown()
public static void MouseLeftUp()
public static void MouseLeftClick()
public static void MouseRightDown()
public static void MouseRightUp()
public static void MouseRightClick()
public static void MouseMiddleDown()
public static void MouseMiddleUp()

// 滚轮
public static void MouseWheel(int delta)

// 获取/设置位置
public static Point GetCurrentMousePosition()
public static void SetMousePosition(int x, int y)
```

**键盘方法**：

```csharp
// 单个按键
public static void KeyDown(int virtualKeyCode)
public static void KeyUp(int virtualKeyCode)
public static void KeyPress(int virtualKeyCode)

// 组合键
public static void KeyCombination(int[] modifiers, int key)
```

**执行操作**：

```csharp
public static void ExecuteAction(RecordedAction action)
```

**使用示例**：

```csharp
// 移动鼠标并点击
InputSimulator.MoveMouse(100, 200);
InputSimulator.MouseLeftClick();

// 输入组合键 Ctrl+C
InputSimulator.KeyCombination(
    new[] { NativeMethods.VK_CONTROL }, 
    0x43); // 'C'

// 执行录制的操作
foreach (var action in recordedActions)
{
    InputSimulator.ExecuteAction(action);
    Thread.Sleep(action.DelayMs);
}
```

---

## Utilities

### JsonHelper

JSON 序列化助手。

```csharp
public static class JsonHelper
{
    // 序列化
    public static string Serialize(RecordingFile recordingFile)
    public static void SerializeToFile(RecordingFile recordingFile, string filePath)
    public static Task SerializeToFileAsync(RecordingFile recordingFile, string filePath)
    
    // 反序列化
    public static RecordingFile? Deserialize(string json)
    public static RecordingFile? DeserializeFromFile(string filePath)
    public static Task<RecordingFile?> DeserializeFromFileAsync(string filePath)
    
    // 验证
    public static bool Validate(RecordingFile? recordingFile)
}
```

**使用示例**：

```csharp
// 保存录制
var recordingFile = recorder.GetRecordingFile();
JsonHelper.SerializeToFile(recordingFile, "recording.json");

// 加载录制
var loadedRecording = JsonHelper.DeserializeFromFile("recording.json");
if (loadedRecording != null && JsonHelper.Validate(loadedRecording))
{
    recorder.LoadFromRecordingFile(loadedRecording);
}
```

---

### RandomHelper

随机数生成器，用于模拟人类行为。

```csharp
public static class RandomHelper
{
    // 基础随机数
    public static int Next(int minValue, int maxValue)
    public static int Next(int maxValue)
    public static double NextDouble()
    public static double NextDouble(double minValue, double maxValue)
    
    // 人类模拟
    public static int GetHumanizedDelay(int baseDelay, double variancePercent = 10.0)
    public static (int x, int y) GetHumanizedPosition(int baseX, int baseY, int maxOffset = 2)
    public static int GetThinkingDelay(int minMs = 50, int maxMs = 200)
    
    // 贝塞尔曲线
    public static (int cp1x, int cp1y, int cp2x, int cp2y) GetBezierControlPoints(
        int startX, int startY, int endX, int endY)
    public static (int x, int y) GetBezierPoint(double t, 
        (int x, int y) p0, (int x, int y) p1, (int x, int y) p2, (int x, int y) p3)
    
    // 其他
    public static bool NextBool()
    public static bool NextBool(double probability)
    public static T? RandomChoice<T>(T[] items)
    public static void Shuffle<T>(T[] array)
}
```

**使用示例**：

```csharp
// 获取带随机波动的延迟
int delay = RandomHelper.GetHumanizedDelay(1000, 15.0); // 1000ms ± 15%

// 获取带偏移的坐标
var (x, y) = RandomHelper.GetHumanizedPosition(100, 200, 3);

// 生成贝塞尔曲线轨迹
var (cp1x, cp1y, cp2x, cp2y) = RandomHelper.GetBezierControlPoints(0, 0, 500, 500);
for (double t = 0; t <= 1; t += 0.1)
{
    var (px, py) = RandomHelper.GetBezierPoint(t, (0, 0), (cp1x, cp1y), (cp2x, cp2y), (500, 500));
    InputSimulator.MoveMouse(px, py);
    Thread.Sleep(10);
}
```

---

### RecentFilesHelper

最近文件列表管理。

```csharp
public class RecentFilesHelper
{
    // 属性
    public IReadOnlyList<RecentFileInfo> RecentFiles { get; }
    public int Count { get; }
    
    // 事件
    public event EventHandler? RecentFilesChanged;
    
    // 方法
    public void AddRecentFile(string filePath)
    public bool RemoveRecentFile(string filePath)
    public void ClearRecentFiles()
    public bool Contains(string filePath)
    public RecentFileInfo? GetRecentFile(int index)
    public int CleanupMissingFiles()
    
    // 持久化
    public void Save()
    public Task SaveAsync()
    public void Load()
    public Task LoadAsync()
}
```

**RecentFileInfo 结构**：

```csharp
public class RecentFileInfo
{
    public string FilePath { get; set; }      // 文件路径
    public string FileName { get; }           // 文件名
    public DateTime LastAccessed { get; set; } // 最后访问时间
    public int AccessCount { get; set; }      // 访问次数
    public long FileSize { get; set; }        // 文件大小
    public string DisplayText { get; }        // 显示文本
}
```

---

## 文件格式

### 录制文件 JSON 结构

```json
{
  "version": "1.0.0",
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "name": "示例录制",
  "description": "这是一个示例录制文件",
  "createdAt": "2024-01-15T08:30:00Z",
  "modifiedAt": "2024-01-15T08:30:00Z",
  "screenWidth": 1920,
  "screenHeight": 1080,
  "osVersion": "Microsoft Windows NT 10.0.19045.0",
  "actions": [
    {
      "id": "11111111-2222-3333-4444-555555555555",
      "actionType": 0,
      "x": 100,
      "y": 200,
      "virtualKeyCode": 0,
      "scanCode": 0,
      "character": null,
      "wheelDelta": 0,
      "delayMs": 0,
      "timestamp": "2024-01-15T08:30:00.123Z",
      "flags": 0,
      "description": null
    }
  ]
}
```

---

如有更多问题，请参考源代码或提交 Issue。
