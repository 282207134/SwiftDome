# 👨‍💻 开发者指南

本指南帮助开发者理解和扩展鼠标键盘录制器项目。

## 📚 目录

1. [架构概述](#架构概述)
2. [核心模块](#核心模块)
3. [扩展开发](#扩展开发)
4. [最佳实践](#最佳实践)
5. [调试技巧](#调试技巧)

---

## 架构概述

### 分层架构

```
┌─────────────────────────────────────────┐
│              UI 层 (WPF)                 │
│  - MainWindow                           │
│  - EditActionWindow                     │
├─────────────────────────────────────────┤
│            服务层 (Services)              │
│  - IRecorderService                     │
│  - IPlayerService                       │
├─────────────────────────────────────────┤
│           助手层 (Helpers)                │
│  - NativeMethods (Windows API)          │
│  - InputSimulator                       │
├─────────────────────────────────────────┤
│           模型层 (Models)                 │
│  - RecordedAction                       │
│  - RecordingFile                        │
│  - ActionType                           │
├─────────────────────────────────────────┤
│           工具层 (Utilities)              │
│  - JsonHelper                           │
│  - RandomHelper                         │
│  - RecentFilesHelper                    │
└─────────────────────────────────────────┘
```

### 数据流

```
用户输入 → Windows 钩子 → RecorderService → RecordedAction → JSON 文件
                                                         ↓
用户触发 → PlayerService → InputSimulator → Windows API → 系统响应
```

---

## 核心模块

### 1. 录制服务 (RecorderService)

**功能**：使用 Windows 低级别钩子捕获全局输入

**关键实现**：
```csharp
// 安装鼠标钩子
_mouseHookHandle = SetWindowsHookEx(
    WH_MOUSE_LL,
    MouseHookCallback,
    GetModuleHandle(null),
    0);

// 钩子回调处理
private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
{
    if (nCode >= 0 && IsRecording)
    {
        var hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
        // 处理鼠标事件...
    }
    return CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
}
```

**性能优化**：
- 鼠标移动节流（10ms 间隔）
- 最小移动距离阈值（2 像素）
- 使用 `Stopwatch` 获取高精度时间

### 2. 播放服务 (PlayerService)

**功能**：按时间顺序播放操作序列

**播放流程**：
```csharp
private bool ExecuteSinglePlayback(CancellationToken token)
{
    while (CurrentIndex < TotalActions && !token.IsCancellationRequested)
    {
        var action = _actions[CurrentIndex];
        
        // 执行操作
        InputSimulator.ExecuteAction(action);
        
        // 计算延迟（考虑速度和随机因素）
        int delay = CalculateDelay(action.DelayMs);
        Thread.Sleep(delay);
        
        CurrentIndex++;
    }
}
```

**特性**：
- 支持暂停/恢复/停止
- 可配置播放速度
- 人类行为模拟

### 3. 输入模拟器 (InputSimulator)

**功能**：使用 `SendInput` API 合成输入事件

**鼠标移动示例**：
```csharp
var inputs = new INPUT[1];
inputs[0].type = INPUT_MOUSE;
inputs[0].u.mi = new MOUSEINPUT
{
    dx = ConvertToAbsoluteX(x),
    dy = ConvertToAbsoluteY(y),
    dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
    mouseData = 0,
    time = 0,
    dwExtraInfo = IntPtr.Zero
};

SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
```

**坐标转换**：
- 屏幕坐标 → 绝对坐标（0-65535 范围）
- 支持多显示器环境

---

## 扩展开发

### 添加新的操作类型

**步骤 1**：在 `ActionType.cs` 中添加新类型

```csharp
public enum ActionType
{
    // ... 现有类型
    
    /// <summary>双击操作</summary>
    MouseDoubleClick = 8,
}
```

**步骤 2**：在 `ActionTypeExtensions.cs` 中添加显示名称

```csharp
public static string GetDisplayName(this ActionType actionType)
{
    return actionType switch
    {
        // ... 现有映射
        ActionType.MouseDoubleClick => "🖱️ 双击",
        _ => "❓ 未知操作"
    };
}
```

**步骤 3**：在 `RecorderService.cs` 中添加捕获逻辑

```csharp
private void AddMouseDoubleClickAction(int x, int y, int delayMs)
{
    var action = new RecordedAction(ActionType.MouseDoubleClick)
    {
        X = x,
        Y = y,
        DelayMs = Math.Max(0, delayMs),
        Timestamp = DateTime.UtcNow
    };
    
    lock (_lockObject)
    {
        _actions.Add(action);
    }
    
    ActionRecorded?.Invoke(this, action);
}
```

**步骤 4**：在 `InputSimulator.cs` 中添加模拟逻辑

```csharp
public static void MouseDoubleClick()
{
    // 两次快速点击
    MouseLeftClick();
    Thread.Sleep(50);
    MouseLeftClick();
}
```

**步骤 5**：在 `ExecuteAction` 方法中添加处理

```csharp
public static void ExecuteAction(RecordedAction action)
{
    switch (action.ActionType)
    {
        // ... 现有 case
        case ActionType.MouseDoubleClick:
            MoveMouse(action.X, action.Y);
            Thread.Sleep(5);
            MouseDoubleClick();
            break;
    }
}
```

### 自定义导出格式

**示例：导出为 Python 脚本**

```csharp
public static class PythonExporter
{
    public static string ExportToPython(RecordingFile recordingFile)
    {
        var sb = new StringBuilder();
        sb.AppendLine("import pyautogui");
        sb.AppendLine("import time");
        sb.AppendLine();
        
        foreach (var action in recordingFile.Actions)
        {
            sb.AppendLine(ConvertActionToPython(action));
        }
        
        return sb.ToString();
    }
    
    private static string ConvertActionToPython(RecordedAction action)
    {
        return action.ActionType switch
        {
            ActionType.MouseMove => $"pyautogui.moveTo({action.X}, {action.Y})",
            ActionType.MouseLeftDown => $"pyautogui.mouseDown()",
            ActionType.MouseLeftUp => $"pyautogui.mouseUp()",
            ActionType.KeyDown => $"pyautogui.keyDown('{GetKeyName(action.VirtualKeyCode)}')",
            _ => $"# Unknown action: {action.ActionType}"
        };
    }
}
```

### 添加新的播放模式

**示例：随机播放模式**

```csharp
public class RandomPlayerService : IPlayerService
{
    private readonly Random _random = new();
    
    private bool ExecuteSinglePlayback(CancellationToken token)
    {
        // 随机打乱操作顺序
        var shuffledActions = _actions.OrderBy(a => _random.Next()).ToList();
        
        foreach (var action in shuffledActions)
        {
            if (token.IsCancellationRequested)
                return false;
                
            InputSimulator.ExecuteAction(action);
            
            // 随机延迟
            int randomDelay = _random.Next(100, 1000);
            Thread.Sleep(randomDelay);
        }
        
        return true;
    }
}
```

---

## 最佳实践

### 线程安全

```csharp
// 使用锁保护共享资源
private readonly object _lockObject = new();

public void AddAction(RecordedAction action)
{
    lock (_lockObject)
    {
        _actions.Add(action);
    }
}

public IReadOnlyList<RecordedAction> RecordedActions
{
    get
    {
        lock (_lockObject)
        {
            return _actions.AsReadOnly();
        }
    }
}
```

### 异常处理

```csharp
try
{
    // 可能抛出异常的代码
}
catch (SEHException ex)
{
    // Windows API 异常
    Logger.Error($"Windows API 错误: {ex.Message}");
}
catch (Exception ex)
{
    // 其他异常
    Logger.Error($"意外错误: {ex.Message}");
}
```

### 资源管理

```csharp
public class RecorderService : IDisposable
{
    private bool _disposed;
    
    public void Dispose()
    {
        if (_disposed)
            return;
            
        StopRecording();
        
        _disposed = true;
    }
}
```

---

## 调试技巧

### 启用详细日志

```csharp
// 在 App.xaml.cs 中
protected override void OnStartup(StartupEventArgs e)
{
    #if DEBUG
    // 启用详细日志
    LogManager.EnableDebugLogging();
    #endif
    
    base.OnStartup(e);
}
```

### 钩子调试

```csharp
private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
{
    #if DEBUG
    Debug.WriteLine($"Mouse Hook: nCode={nCode}, wParam={wParam}");
    #endif
    
    // ...
}
```

### 性能分析

```csharp
var stopwatch = Stopwatch.StartNew();

// 待测代码
for (int i = 0; i < 1000; i++)
{
    InputSimulator.MouseLeftClick();
}

stopwatch.Stop();
Debug.WriteLine($"执行时间: {stopwatch.ElapsedMilliseconds}ms");
```

### 内存分析

```csharp
// 使用 .NET 诊断工具
#if DEBUG
GC.Collect();
GC.WaitForPendingFinalizers();
var memoryBefore = GC.GetTotalMemory(true);

// 执行操作

GC.Collect();
GC.WaitForPendingFinalizers();
var memoryAfter = GC.GetTotalMemory(true);

Debug.WriteLine($"内存使用: {(memoryAfter - memoryBefore) / 1024}KB");
#endif
```

---

## 参考资料

- [Windows Hooks](https://docs.microsoft.com/en-us/windows/win32/winmsg/about-hooks)
- [SendInput](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput)
- [Virtual-Key Codes](https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes)
- [WPF Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)

---

如有更多问题，欢迎提交 Issue 或参与讨论！
