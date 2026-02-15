using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MouseKeyboardRecorder.Helpers;
using MouseKeyboardRecorder.Models;
using static MouseKeyboardRecorder.Helpers.NativeMethods;

namespace MouseKeyboardRecorder.Services
{
    /// <summary>
    /// 录制服务实现
    /// 使用 Windows 低级别钩子捕获全局输入事件
    /// </summary>
    public class RecorderService : IRecorderService, IDisposable
    {
        #region 私有字段

        /// <summary>鼠标钩子句柄</summary>
        private IntPtr _mouseHookHandle = IntPtr.Zero;

        /// <summary>键盘钩子句柄</summary>
        private IntPtr _keyboardHookHandle = IntPtr.Zero;

        /// <summary>鼠标钩子过程委托</summary>
        private LowLevelHookProc? _mouseHookProc;

        /// <summary>键盘钩子过程委托</summary>
        private LowLevelHookProc? _keyboardHookProc;

        /// <summary>录制操作列表</summary>
        private readonly List<RecordedAction> _actions = new();

        /// <summary>上次操作时间戳</summary>
        private long _lastActionTime;

        /// <summary>上次鼠标位置</summary>
        private System.Drawing.Point _lastMousePosition = new(0, 0);

        /// <summary>同步锁</summary>
        private readonly object _lockObject = new();

        /// <summary>是否已释放</summary>
        private bool _disposed;

        /// <summary>鼠标移动节流间隔（毫秒）</summary>
        private const int MouseMoveThrottleMs = 10;

        /// <summary>最小距离阈值（像素）</summary>
        private const int MinMoveDistance = 2;

        #endregion

        #region 公共属性

        /// <inheritdoc />
        public bool IsRecording { get; private set; }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public int ActionCount
        {
            get
            {
                lock (_lockObject)
                {
                    return _actions.Count;
                }
            }
        }

        #endregion

        #region 事件

        /// <inheritdoc />
        public event EventHandler? RecordingStarted;

        /// <inheritdoc />
        public event EventHandler? RecordingStopped;

        /// <inheritdoc />
        public event EventHandler<RecordedAction>? ActionRecorded;

        /// <inheritdoc />
        public event EventHandler<Exception>? RecordingError;

        #endregion

        #region 公共方法

        /// <inheritdoc />
        public bool StartRecording()
        {
            if (IsRecording)
                return false;

            try
            {
                // 清空之前的录制
                lock (_lockObject)
                {
                    _actions.Clear();
                }

                // 安装鼠标钩子
                _mouseHookProc = MouseHookCallback;
                _mouseHookHandle = SetWindowsHookEx(
                    WH_MOUSE_LL,
                    _mouseHookProc,
                    GetModuleHandle(null),
                    0);

                if (_mouseHookHandle == IntPtr.Zero)
                {
                    RecordingError?.Invoke(this, new Exception($"安装鼠标钩子失败，错误码: {GetLastError()}"));
                    return false;
                }

                // 安装键盘钩子
                _keyboardHookProc = KeyboardHookCallback;
                _keyboardHookHandle = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    _keyboardHookProc,
                    GetModuleHandle(null),
                    0);

                if (_keyboardHookHandle == IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_mouseHookHandle);
                    _mouseHookHandle = IntPtr.Zero;
                    RecordingError?.Invoke(this, new Exception($"安装键盘钩子失败，错误码: {GetLastError()}"));
                    return false;
                }

                // 初始化时间戳
                _lastActionTime = Stopwatch.GetTimestamp();
                _lastMousePosition = InputSimulator.GetCurrentMousePosition();

                IsRecording = true;
                RecordingStarted?.Invoke(this, EventArgs.Empty);

                return true;
            }
            catch (Exception ex)
            {
                RecordingError?.Invoke(this, ex);
                StopRecording();
                return false;
            }
        }

        /// <inheritdoc />
        public void StopRecording()
        {
            if (!IsRecording)
                return;

            // 卸载鼠标钩子
            if (_mouseHookHandle != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_mouseHookHandle);
                _mouseHookHandle = IntPtr.Zero;
            }
            _mouseHookProc = null;

            // 卸载键盘钩子
            if (_keyboardHookHandle != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_keyboardHookHandle);
                _keyboardHookHandle = IntPtr.Zero;
            }
            _keyboardHookProc = null;

            IsRecording = false;
            RecordingStopped?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void ClearRecording()
        {
            lock (_lockObject)
            {
                _actions.Clear();
            }
        }

        /// <inheritdoc />
        public RecordingFile GetRecordingFile()
        {
            lock (_lockObject)
            {
                var recordingFile = new RecordingFile
                {
                    Name = $"录制_{DateTime.Now:yyyyMMdd_HHmmss}",
                    Description = "自动生成的录制文件",
                    ScreenWidth = InputSimulator.ScreenWidth,
                    ScreenHeight = InputSimulator.ScreenHeight,
                    OsVersion = Environment.OSVersion.ToString()
                };

                foreach (var action in _actions)
                {
                    recordingFile.Actions.Add(action.Clone());
                }

                return recordingFile;
            }
        }

        /// <inheritdoc />
        public void LoadFromRecordingFile(RecordingFile recordingFile)
        {
            if (recordingFile?.Actions == null)
                return;

            lock (_lockObject)
            {
                _actions.Clear();
                foreach (var action in recordingFile.Actions)
                {
                    _actions.Add(action.Clone());
                }
            }
        }

        /// <inheritdoc />
        public void AddAction(RecordedAction action)
        {
            if (action == null)
                return;

            lock (_lockObject)
            {
                _actions.Add(action);
            }

            ActionRecorded?.Invoke(this, action);
        }

        /// <inheritdoc />
        public bool RemoveAction(Guid actionId)
        {
            lock (_lockObject)
            {
                var action = _actions.Find(a => a.Id == actionId);
                if (action != null)
                {
                    return _actions.Remove(action);
                }
            }
            return false;
        }

        /// <inheritdoc />
        public bool UpdateAction(RecordedAction action)
        {
            if (action == null)
                return false;

            lock (_lockObject)
            {
                var index = _actions.FindIndex(a => a.Id == action.Id);
                if (index >= 0)
                {
                    _actions[index] = action;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 钩子回调

        /// <summary>
        /// 鼠标钩子回调函数
        /// </summary>
        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && IsRecording)
            {
                try
                {
                    var hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                    var message = (int)wParam;

                    // 计算延迟
                    var currentTime = Stopwatch.GetTimestamp();
                    var delayMs = (int)((currentTime - _lastActionTime) * 1000 / Stopwatch.Frequency);
                    _lastActionTime = currentTime;

                    // 获取当前鼠标位置
                    var currentPos = new System.Drawing.Point(hookStruct.pt.x, hookStruct.pt.y);

                    switch (message)
                    {
                        case WM_MOUSEMOVE:
                            HandleMouseMove(currentPos, delayMs);
                            break;

                        case WM_LBUTTONDOWN:
                            AddMouseAction(ActionType.MouseLeftDown, currentPos.X, currentPos.Y, delayMs);
                            break;

                        case WM_LBUTTONUP:
                            AddMouseAction(ActionType.MouseLeftUp, currentPos.X, currentPos.Y, delayMs);
                            break;

                        case WM_RBUTTONDOWN:
                            AddMouseAction(ActionType.MouseRightDown, currentPos.X, currentPos.Y, delayMs);
                            break;

                        case WM_RBUTTONUP:
                            AddMouseAction(ActionType.MouseRightUp, currentPos.X, currentPos.Y, delayMs);
                            break;

                        case WM_MBUTTONDOWN:
                            AddMouseAction(ActionType.MouseMiddleDown, currentPos.X, currentPos.Y, delayMs);
                            break;

                        case WM_MBUTTONUP:
                            AddMouseAction(ActionType.MouseMiddleUp, currentPos.X, currentPos.Y, delayMs);
                            break;

                        case WM_MOUSEWHEEL:
                            int wheelDelta = (short)((hookStruct.mouseData >> 16) & 0xFFFF);
                            AddMouseWheelAction(currentPos.X, currentPos.Y, wheelDelta, delayMs);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    RecordingError?.Invoke(this, ex);
                }
            }

            return CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
        }

        /// <summary>
        /// 键盘钩子回调函数
        /// </summary>
        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && IsRecording)
            {
                try
                {
                    var hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                    var message = (int)wParam;

                    // 计算延迟
                    var currentTime = Stopwatch.GetTimestamp();
                    var delayMs = (int)((currentTime - _lastActionTime) * 1000 / Stopwatch.Frequency);
                    _lastActionTime = currentTime;

                    // 忽略某些特殊按键（如用于停止录制的快捷键）
                    if (ShouldIgnoreKey((int)hookStruct.vkCode))
                        return CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);

                    switch (message)
                    {
                        case WM_KEYDOWN:
                        case WM_SYSKEYDOWN:
                            AddKeyboardAction(ActionType.KeyDown, (int)hookStruct.vkCode, (int)hookStruct.scanCode, delayMs);
                            break;

                        case WM_KEYUP:
                        case WM_SYSKEYUP:
                            AddKeyboardAction(ActionType.KeyUp, (int)hookStruct.vkCode, (int)hookStruct.scanCode, delayMs);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    RecordingError?.Invoke(this, ex);
                }
            }

            return CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 处理鼠标移动事件（节流）
        /// </summary>
        private void HandleMouseMove(System.Drawing.Point currentPos, int delayMs)
        {
            // 计算移动距离
            int dx = Math.Abs(currentPos.X - _lastMousePosition.X);
            int dy = Math.Abs(currentPos.Y - _lastMousePosition.Y);

            // 只有当移动距离超过阈值时才记录
            if (dx >= MinMoveDistance || dy >= MinMoveDistance)
            {
                // 节流：限制鼠标移动事件频率
                if (delayMs >= MouseMoveThrottleMs || _actions.Count == 0)
                {
                    AddMouseAction(ActionType.MouseMove, currentPos.X, currentPos.Y, delayMs);
                    _lastMousePosition = currentPos;
                }
            }
        }

        /// <summary>
        /// 添加鼠标操作
        /// </summary>
        private void AddMouseAction(ActionType type, int x, int y, int delayMs)
        {
            var action = new RecordedAction(type)
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

        /// <summary>
        /// 添加鼠标滚轮操作
        /// </summary>
        private void AddMouseWheelAction(int x, int y, int delta, int delayMs)
        {
            var action = new RecordedAction(ActionType.MouseWheel)
            {
                X = x,
                Y = y,
                WheelDelta = delta,
                DelayMs = Math.Max(0, delayMs),
                Timestamp = DateTime.UtcNow
            };

            lock (_lockObject)
            {
                _actions.Add(action);
            }

            ActionRecorded?.Invoke(this, action);
        }

        /// <summary>
        /// 添加键盘操作
        /// </summary>
        private void AddKeyboardAction(ActionType type, int vkCode, int scanCode, int delayMs)
        {
            // 获取字符表示
            string? character = GetKeyCharacter(vkCode);

            var action = new RecordedAction(type)
            {
                VirtualKeyCode = vkCode,
                ScanCode = scanCode,
                Character = character,
                DelayMs = Math.Max(0, delayMs),
                Timestamp = DateTime.UtcNow
            };

            lock (_lockObject)
            {
                _actions.Add(action);
            }

            ActionRecorded?.Invoke(this, action);
        }

        /// <summary>
        /// 判断是否应忽略特定按键
        /// </summary>
        private bool ShouldIgnoreKey(int vkCode)
        {
            // 可以在这里添加忽略特定按键的逻辑
            // 例如：忽略 F9 停止录制快捷键
            return false;
        }

        /// <summary>
        /// 获取按键的字符表示
        /// </summary>
        private string? GetKeyCharacter(int vkCode)
        {
            // 常用键的名称映射
            return vkCode switch
            {
                VK_ESCAPE => "Esc",
                VK_TAB => "Tab",
                VK_CAPITAL => "CapsLock",
                VK_SHIFT => "Shift",
                VK_CONTROL => "Ctrl",
                VK_MENU => "Alt",
                VK_SPACE => "Space",
                VK_RETURN => "Enter",
                VK_BACK => "Backspace",
                VK_DELETE => "Delete",
                VK_INSERT => "Insert",
                VK_HOME => "Home",
                VK_END => "End",
                VK_PRIOR => "PageUp",
                VK_NEXT => "PageDown",
                VK_LEFT => "Left",
                VK_UP => "Up",
                VK_RIGHT => "Right",
                VK_DOWN => "Down",
                VK_LBUTTON => "LButton",
                VK_RBUTTON => "RButton",
                VK_MBUTTON => "MButton",
                _ => vkCode >= 0x30 && vkCode <= 0x5A ? ((char)vkCode).ToString() : $"0x{vkCode:X2}"
            };
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            StopRecording();
            _disposed = true;
        }

        #endregion
    }
}
