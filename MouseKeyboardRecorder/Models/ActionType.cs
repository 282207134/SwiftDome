namespace MouseKeyboardRecorder.Models
{
    /// <summary>
    /// 录制操作类型枚举
    /// 定义所有支持录制的操作类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>鼠标移动</summary>
        MouseMove = 0,

        /// <summary>鼠标左键按下</summary>
        MouseLeftDown = 1,

        /// <summary>鼠标左键释放</summary>
        MouseLeftUp = 2,

        /// <summary>鼠标右键按下</summary>
        MouseRightDown = 3,

        /// <summary>鼠标右键释放</summary>
        MouseRightUp = 4,

        /// <summary>鼠标中键按下</summary>
        MouseMiddleDown = 5,

        /// <summary>鼠标中键释放</summary>
        MouseMiddleUp = 6,

        /// <summary>鼠标滚轮滚动</summary>
        MouseWheel = 7,

        /// <summary>键盘按键按下</summary>
        KeyDown = 10,

        /// <summary>键盘按键释放</summary>
        KeyUp = 11,

        /// <summary>键盘字符输入</summary>
        KeyPress = 12,

        /// <summary>等待/延迟</summary>
        Wait = 20,

        /// <summary>特殊操作标记</summary>
        Special = 99
    }

    /// <summary>
    /// 操作类型扩展方法
    /// </summary>
    public static class ActionTypeExtensions
    {
        /// <summary>
        /// 获取操作类型的中文显示名称
        /// </summary>
        /// <param name="actionType">操作类型</param>
        /// <returns>中文显示名称</returns>
        public static string GetDisplayName(this ActionType actionType)
        {
            return actionType switch
            {
                ActionType.MouseMove => "🖱️ 鼠标移动",
                ActionType.MouseLeftDown => "🖱️ 左键按下",
                ActionType.MouseLeftUp => "🖱️ 左键释放",
                ActionType.MouseRightDown => "🖱️ 右键按下",
                ActionType.MouseRightUp => "🖱️ 右键释放",
                ActionType.MouseMiddleDown => "🖱️ 中键按下",
                ActionType.MouseMiddleUp => "🖱️ 中键释放",
                ActionType.MouseWheel => "🖱️ 滚轮滚动",
                ActionType.KeyDown => "⌨️ 按键按下",
                ActionType.KeyUp => "⌨️ 按键释放",
                ActionType.KeyPress => "⌨️ 按键输入",
                ActionType.Wait => "⏱️ 等待延迟",
                ActionType.Special => "⭐ 特殊操作",
                _ => "❓ 未知操作"
            };
        }

        /// <summary>
        /// 判断是否为鼠标操作
        /// </summary>
        public static bool IsMouseAction(this ActionType actionType)
        {
            return actionType >= ActionType.MouseMove && actionType <= ActionType.MouseWheel;
        }

        /// <summary>
        /// 判断是否为键盘操作
        /// </summary>
        public static bool IsKeyboardAction(this ActionType actionType)
        {
            return actionType >= ActionType.KeyDown && actionType <= ActionType.KeyPress;
        }
    }
}
