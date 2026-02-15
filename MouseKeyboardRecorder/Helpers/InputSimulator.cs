using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using MouseKeyboardRecorder.Models;
using static MouseKeyboardRecorder.Helpers.NativeMethods;

namespace MouseKeyboardRecorder.Helpers
{
    /// <summary>
    /// 输入模拟器
    /// 提供鼠标和键盘操作的模拟功能
    /// </summary>
    public class InputSimulator
    {
        // 虚拟屏幕尺寸常量
        private const int MOUSEEVENTF_ABSOLUTE_MAX = 65535;

        /// <summary>
        /// 当前屏幕宽度
        /// </summary>
        public static int ScreenWidth => Screen.PrimaryScreen?.Bounds.Width ?? 1920;

        /// <summary>
        /// 当前屏幕高度
        /// </summary>
        public static int ScreenHeight => Screen.PrimaryScreen?.Bounds.Height ?? 1080;

        #region 鼠标模拟

        /// <summary>
        /// 模拟鼠标移动到指定位置
        /// </summary>
        /// <param name="x">目标X坐标</param>
        /// <param name="y">目标Y坐标</param>
        public static void MoveMouse(int x, int y)
        {
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
        }

        /// <summary>
        /// 模拟鼠标左键按下
        /// </summary>
        public static void MouseLeftDown()
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_LEFTDOWN,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标左键释放
        /// </summary>
        public static void MouseLeftUp()
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_LEFTUP,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标左键点击
        /// </summary>
        public static void MouseLeftClick()
        {
            var inputs = new INPUT[2];
            
            // 按下
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_LEFTDOWN,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            // 释放
            inputs[1].type = INPUT_MOUSE;
            inputs[1].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_LEFTUP,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标右键按下
        /// </summary>
        public static void MouseRightDown()
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_RIGHTDOWN,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标右键释放
        /// </summary>
        public static void MouseRightUp()
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_RIGHTUP,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标右键点击
        /// </summary>
        public static void MouseRightClick()
        {
            var inputs = new INPUT[2];
            
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_RIGHTDOWN,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            inputs[1].type = INPUT_MOUSE;
            inputs[1].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_RIGHTUP,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标中键按下
        /// </summary>
        public static void MouseMiddleDown()
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_MIDDLEDOWN,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标中键释放
        /// </summary>
        public static void MouseMiddleUp()
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_MIDDLEUP,
                mouseData = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟鼠标滚轮滚动
        /// </summary>
        /// <param name="delta">滚动量（正值向上，负值向下）</param>
        public static void MouseWheel(int delta)
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi = new MOUSEINPUT
            {
                dx = 0,
                dy = 0,
                dwFlags = MOUSEEVENTF_WHEEL,
                mouseData = (uint)delta,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        #endregion

        #region 键盘模拟

        /// <summary>
        /// 模拟键盘按键按下
        /// </summary>
        /// <param name="virtualKeyCode">虚拟键码</param>
        public static void KeyDown(int virtualKeyCode)
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki = new KEYBDINPUT
            {
                wVk = (ushort)virtualKeyCode,
                wScan = (ushort)MapVirtualKey((uint)virtualKeyCode, 0),
                dwFlags = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟键盘按键释放
        /// </summary>
        /// <param name="virtualKeyCode">虚拟键码</param>
        public static void KeyUp(int virtualKeyCode)
        {
            var inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki = new KEYBDINPUT
            {
                wVk = (ushort)virtualKeyCode,
                wScan = (ushort)MapVirtualKey((uint)virtualKeyCode, 0),
                dwFlags = KEYEVENTF_KEYUP,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟键盘按键（按下+释放）
        /// </summary>
        /// <param name="virtualKeyCode">虚拟键码</param>
        public static void KeyPress(int virtualKeyCode)
        {
            var inputs = new INPUT[2];
            
            // 按下
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki = new KEYBDINPUT
            {
                wVk = (ushort)virtualKeyCode,
                wScan = (ushort)MapVirtualKey((uint)virtualKeyCode, 0),
                dwFlags = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            // 释放
            inputs[1].type = INPUT_KEYBOARD;
            inputs[1].u.ki = new KEYBDINPUT
            {
                wVk = (ushort)virtualKeyCode,
                wScan = (ushort)MapVirtualKey((uint)virtualKeyCode, 0),
                dwFlags = KEYEVENTF_KEYUP,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };

            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 模拟组合键（如 Ctrl+C）
        /// </summary>
        /// <param name="modifiers">修饰键虚拟键码数组</param>
        /// <param name="key">主键虚拟键码</param>
        public static void KeyCombination(int[] modifiers, int key)
        {
            int inputCount = modifiers.Length * 2 + 2;
            var inputs = new INPUT[inputCount];
            int index = 0;

            // 按下所有修饰键
            foreach (var modifier in modifiers)
            {
                inputs[index].type = INPUT_KEYBOARD;
                inputs[index].u.ki = new KEYBDINPUT
                {
                    wVk = (ushort)modifier,
                    wScan = (ushort)MapVirtualKey((uint)modifier, 0),
                    dwFlags = 0,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                };
                index++;
            }

            // 按下主键
            inputs[index].type = INPUT_KEYBOARD;
            inputs[index].u.ki = new KEYBDINPUT
            {
                wVk = (ushort)key,
                wScan = (ushort)MapVirtualKey((uint)key, 0),
                dwFlags = 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };
            index++;

            // 释放主键
            inputs[index].type = INPUT_KEYBOARD;
            inputs[index].u.ki = new KEYBDINPUT
            {
                wVk = (ushort)key,
                wScan = (ushort)MapVirtualKey((uint)key, 0),
                dwFlags = KEYEVENTF_KEYUP,
                time = 0,
                dwExtraInfo = IntPtr.Zero
            };
            index++;

            // 释放所有修饰键（逆序）
            for (int i = modifiers.Length - 1; i >= 0; i--)
            {
                inputs[index].type = INPUT_KEYBOARD;
                inputs[index].u.ki = new KEYBDINPUT
                {
                    wVk = (ushort)modifiers[i],
                    wScan = (ushort)MapVirtualKey((uint)modifiers[i], 0),
                    dwFlags = KEYEVENTF_KEYUP,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                };
                index++;
            }

            SendInput((uint)inputCount, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        #endregion

        #region 执行录制操作

        /// <summary>
        /// 执行单个录制操作
        /// </summary>
        /// <param name="action">录制的操作</param>
        public static void ExecuteAction(RecordedAction action)
        {
            switch (action.ActionType)
            {
                case ActionType.MouseMove:
                    MoveMouse(action.X, action.Y);
                    break;

                case ActionType.MouseLeftDown:
                    MouseLeftDown();
                    break;

                case ActionType.MouseLeftUp:
                    MouseLeftUp();
                    break;

                case ActionType.MouseRightDown:
                    MouseRightDown();
                    break;

                case ActionType.MouseRightUp:
                    MouseRightUp();
                    break;

                case ActionType.MouseMiddleDown:
                    MouseMiddleDown();
                    break;

                case ActionType.MouseMiddleUp:
                    MouseMiddleUp();
                    break;

                case ActionType.MouseWheel:
                    MouseWheel(action.WheelDelta);
                    break;

                case ActionType.KeyDown:
                    KeyDown(action.VirtualKeyCode);
                    break;

                case ActionType.KeyUp:
                    KeyUp(action.VirtualKeyCode);
                    break;

                case ActionType.KeyPress:
                    KeyPress(action.VirtualKeyCode);
                    break;

                case ActionType.Wait:
                    // 延迟在调用方处理
                    break;

                default:
                    // 未知操作类型，忽略
                    break;
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 将屏幕X坐标转换为绝对坐标
        /// </summary>
        private static int ConvertToAbsoluteX(int x)
        {
            return (x * MOUSEEVENTF_ABSOLUTE_MAX) / ScreenWidth + 1;
        }

        /// <summary>
        /// 将屏幕Y坐标转换为绝对坐标
        /// </summary>
        private static int ConvertToAbsoluteY(int y)
        {
            return (y * MOUSEEVENTF_ABSOLUTE_MAX) / ScreenHeight + 1;
        }

        /// <summary>
        /// 获取当前鼠标位置
        /// </summary>
        public static System.Drawing.Point GetCurrentMousePosition()
        {
            GetCursorPos(out POINT pt);
            return new System.Drawing.Point(pt.x, pt.y);
        }

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        public static void SetMousePosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        #endregion
    }
}
