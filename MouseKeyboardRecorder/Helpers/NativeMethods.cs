using System;
using System.Runtime.InteropServices;

namespace MouseKeyboardRecorder.Helpers
{
    /// <summary>
    /// Windows API P/Invoke 声明
    /// 包含钩子、输入模拟、键盘状态等 Windows API 函数
    /// </summary>
    public static class NativeMethods
    {
        #region 钩子相关常量

        /// <summary>低级别鼠标钩子 ID</summary>
        public const int WH_MOUSE_LL = 14;

        /// <summary>低级别键盘钩子 ID</summary>
        public const int WH_KEYBOARD_LL = 13;

        /// <summary>鼠标消息：移动</summary>
        public const int WM_MOUSEMOVE = 0x0200;

        /// <summary>鼠标消息：左键按下</summary>
        public const int WM_LBUTTONDOWN = 0x0201;

        /// <summary>鼠标消息：左键释放</summary>
        public const int WM_LBUTTONUP = 0x0202;

        /// <summary>鼠标消息：右键按下</summary>
        public const int WM_RBUTTONDOWN = 0x0204;

        /// <summary>鼠标消息：右键释放</summary>
        public const int WM_RBUTTONUP = 0x0205;

        /// <summary>鼠标消息：中键按下</summary>
        public const int WM_MBUTTONDOWN = 0x0207;

        /// <summary>鼠标消息：中键释放</summary>
        public const int WM_MBUTTONUP = 0x0208;

        /// <summary>鼠标消息：滚轮滚动</summary>
        public const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>鼠标消息：X 按钮按下</summary>
        public const int WM_XBUTTONDOWN = 0x020B;

        /// <summary>鼠标消息：X 按钮释放</summary>
        public const int WM_XBUTTONUP = 0x020C;

        /// <summary>键盘消息：按键按下</summary>
        public const int WM_KEYDOWN = 0x0100;

        /// <summary>键盘消息：按键释放</summary>
        public const int WM_KEYUP = 0x0101;

        /// <summary>键盘消息：字符输入</summary>
        public const int WM_CHAR = 0x0102;

        /// <summary>键盘消息：系统按键按下</summary>
        public const int WM_SYSKEYDOWN = 0x0104;

        /// <summary>键盘消息：系统按键释放</summary>
        public const int WM_SYSKEYUP = 0x0105;

        /// <summary>滚轮高位字节掩码</summary>
        public const uint WHEEL_DELTA_MASK = 0xFFFF0000;

        #endregion

        #region 输入类型常量

        /// <summary>输入类型：鼠标</summary>
        public const uint INPUT_MOUSE = 0;

        /// <summary>输入类型：键盘</summary>
        public const uint INPUT_KEYBOARD = 1;

        /// <summary>输入类型：硬件</summary>
        public const uint INPUT_HARDWARE = 2;

        #endregion

        #region 鼠标事件标志

        /// <summary>鼠标移动</summary>
        public const uint MOUSEEVENTF_MOVE = 0x0001;

        /// <summary>绝对坐标</summary>
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        /// <summary>左键按下</summary>
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;

        /// <summary>左键释放</summary>
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;

        /// <summary>右键按下</summary>
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;

        /// <summary>右键释放</summary>
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        /// <summary>中键按下</summary>
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;

        /// <summary>中键释放</summary>
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        /// <summary>滚轮滚动</summary>
        public const uint MOUSEEVENTF_WHEEL = 0x0800;

        /// <summary>水平滚轮</summary>
        public const uint MOUSEEVENTF_HWHEEL = 0x1000;

        /// <summary>X 按钮按下</summary>
        public const uint MOUSEEVENTF_XDOWN = 0x0080;

        /// <summary>X 按钮释放</summary>
        public const uint MOUSEEVENTF_XUP = 0x0100;

        #endregion

        #region 键盘事件标志

        /// <summary>扩展键</summary>
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

        /// <summary>键释放</summary>
        public const uint KEYEVENTF_KEYUP = 0x0002;

        /// <summary>扫描码</summary>
        public const uint KEYEVENTF_SCANCODE = 0x0008;

        /// <summary>Unicode</summary>
        public const uint KEYEVENTF_UNICODE = 0x0004;

        #endregion

        #region 虚拟键码（常用键）

        public const byte VK_LBUTTON = 0x01;      // 鼠标左键
        public const byte VK_RBUTTON = 0x02;      // 鼠标右键
        public const byte VK_CANCEL = 0x03;       // Control-break 处理
        public const byte VK_MBUTTON = 0x04;      // 鼠标中键
        public const byte VK_XBUTTON1 = 0x05;     // X1 鼠标按钮
        public const byte VK_XBUTTON2 = 0x06;     // X2 鼠标按钮
        public const byte VK_BACK = 0x08;         // BACKSPACE 键
        public const byte VK_TAB = 0x09;          // TAB 键
        public const byte VK_CLEAR = 0x0C;        // CLEAR 键
        public const byte VK_RETURN = 0x0D;       // ENTER 键
        public const byte VK_SHIFT = 0x10;        // SHIFT 键
        public const byte VK_CONTROL = 0x11;      // CTRL 键
        public const byte VK_MENU = 0x12;         // ALT 键
        public const byte VK_PAUSE = 0x13;        // PAUSE 键
        public const byte VK_CAPITAL = 0x14;      // CAPS LOCK 键
        public const byte VK_ESCAPE = 0x1B;       // ESC 键
        public const byte VK_SPACE = 0x20;        // SPACEBAR
        public const byte VK_PRIOR = 0x21;        // PAGE UP 键
        public const byte VK_NEXT = 0x22;         // PAGE DOWN 键
        public const byte VK_END = 0x23;          // END 键
        public const byte VK_HOME = 0x24;         // HOME 键
        public const byte VK_LEFT = 0x25;         // LEFT ARROW 键
        public const byte VK_UP = 0x26;           // UP ARROW 键
        public const byte VK_RIGHT = 0x27;        // RIGHT ARROW 键
        public const byte VK_DOWN = 0x28;         // DOWN ARROW 键
        public const byte VK_SELECT = 0x29;       // SELECT 键
        public const byte VK_PRINT = 0x2A;        // PRINT 键
        public const byte VK_EXECUTE = 0x2B;      // EXECUTE 键
        public const byte VK_SNAPSHOT = 0x2C;     // PRINT SCREEN 键
        public const byte VK_INSERT = 0x2D;       // INS 键
        public const byte VK_DELETE = 0x2E;       // DEL 键
        public const byte VK_HELP = 0x2F;         // HELP 键

        #endregion

        #region 委托类型

        /// <summary>
        /// 低级别钩子过程委托
        /// </summary>
        /// <param name="nCode">钩子代码</param>
        /// <param name="wParam">消息标识符</param>
        /// <param name="lParam">消息参数</param>
        /// <returns>处理结果</returns>
        public delegate IntPtr LowLevelHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        #region 结构体定义

        /// <summary>
        /// 鼠标低级别钩子结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// 键盘低级别钩子结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// 点坐标结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        /// <summary>
        /// 输入结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public INPUTUNION u;
        }

        /// <summary>
        /// 输入联合体
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUTUNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        /// <summary>
        /// 鼠标输入结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// 键盘输入结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// 硬件输入结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        #endregion

        #region 钩子相关 API

        /// <summary>
        /// 安装钩子过程
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelHookProc lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// 卸载钩子过程
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// 将钩子信息传递到下一个钩子过程
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        #region 输入模拟 API

        /// <summary>
        /// 合成鼠标事件和键盘事件
        /// </summary>
        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray)] INPUT[] pInputs, int cbSize);

        /// <summary>
        /// 合成鼠标运动或按钮点击
        /// </summary>
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        /// <summary>
        /// 合成按键
        /// </summary>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        #endregion

        #region 键盘状态 API

        /// <summary>
        /// 获取指定虚拟键的状态
        /// </summary>
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        /// <summary>
        /// 获取指定虚拟键的状态
        /// </summary>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int vKey);

        /// <summary>
        /// 将虚拟键转换为扫描码
        /// </summary>
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        #endregion

        #region 鼠标相关 API

        /// <summary>
        /// 获取当前鼠标位置
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取当前模块句柄
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// 获取上次错误代码
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        /// <summary>
        /// 从输入联合体获取鼠标输入
        /// </summary>
        public static MOUSEINPUT GetMouseInput(INPUT input)
        {
            return input.u.mi;
        }

        /// <summary>
        /// 从输入联合体获取键盘输入
        /// </summary>
        public static KEYBDINPUT GetKeyboardInput(INPUT input)
        {
            return input.u.ki;
        }

        #endregion
    }
}
