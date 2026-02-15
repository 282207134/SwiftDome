using System;
using System.Text.Json.Serialization;

namespace MouseKeyboardRecorder.Models
{
    /// <summary>
    /// 录制的单个操作数据模型
    /// 包含操作的完整信息，用于序列化和回放
    /// </summary>
    public class RecordedAction
    {
        /// <summary>
        /// 操作唯一标识符
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// 操作发生时的 X 坐标（鼠标操作使用）
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 操作发生时的 Y 坐标（鼠标操作使用）
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 虚拟键码（键盘操作使用）
        /// </summary>
        public int VirtualKeyCode { get; set; }

        /// <summary>
        /// 扫描码（键盘操作使用）
        /// </summary>
        public int ScanCode { get; set; }

        /// <summary>
        /// 字符（键盘输入使用）
        /// </summary>
        public string? Character { get; set; }

        /// <summary>
        /// 滚轮滚动量（正值向上，负值向下）
        /// </summary>
        public int WheelDelta { get; set; }

        /// <summary>
        /// 与上一个操作的时间间隔（毫秒）
        /// </summary>
        public int DelayMs { get; set; }

        /// <summary>
        /// 操作发生的绝对时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 扩展标志位（用于特殊功能）
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// 操作描述（可选，用于显示）
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 无参构造函数（用于序列化）
        /// </summary>
        public RecordedAction()
        {
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 创建新的录制操作
        /// </summary>
        /// <param name="actionType">操作类型</param>
        public RecordedAction(ActionType actionType)
        {
            ActionType = actionType;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取操作的详细描述
        /// </summary>
        /// <returns>格式化后的描述字符串</returns>
        public string GetDetailDescription()
        {
            var baseDesc = ActionType.GetDisplayName();

            return ActionType switch
            {
                ActionType.MouseMove => $"{baseDesc} 到 ({X}, {Y})",
                ActionType.MouseLeftDown or ActionType.MouseLeftUp => $"{baseDesc} 于 ({X}, {Y})",
                ActionType.MouseRightDown or ActionType.MouseRightUp => $"{baseDesc} 于 ({X}, {Y})",
                ActionType.MouseMiddleDown or ActionType.MouseMiddleUp => $"{baseDesc} 于 ({X}, {Y})",
                ActionType.MouseWheel => $"{baseDesc} {(WheelDelta > 0 ? "向上" : "向下")} {Math.Abs(WheelDelta)} 单位",
                ActionType.KeyDown or ActionType.KeyUp or ActionType.KeyPress => 
                    $"{baseDesc} [{Character ?? $"VK:{VirtualKeyCode}"}]",
                ActionType.Wait => $"{baseDesc} {DelayMs} 毫秒",
                _ => baseDesc
            };
        }

        /// <summary>
        /// 创建此操作的深拷贝
        /// </summary>
        /// <returns>操作副本</returns>
        public RecordedAction Clone()
        {
            return new RecordedAction
            {
                Id = Guid.NewGuid(),
                ActionType = this.ActionType,
                X = this.X,
                Y = this.Y,
                VirtualKeyCode = this.VirtualKeyCode,
                ScanCode = this.ScanCode,
                Character = this.Character,
                WheelDelta = this.WheelDelta,
                DelayMs = this.DelayMs,
                Timestamp = this.Timestamp,
                Flags = this.Flags,
                Description = this.Description
            };
        }

        /// <summary>
        /// 验证操作数据的有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            // 延迟不能为负数
            if (DelayMs < 0)
                return false;

            // 鼠标操作需要有效坐标
            if (ActionType.IsMouseAction())
            {
                if (X < -32768 || X > 32767 || Y < -32768 || Y > 32767)
                    return false;
            }

            // 键盘操作需要有效键码
            if (ActionType.IsKeyboardAction())
            {
                if (VirtualKeyCode < 0 || VirtualKeyCode > 255)
                    return false;
            }

            return true;
        }
    }
}
