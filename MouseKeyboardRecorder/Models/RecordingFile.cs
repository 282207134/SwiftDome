using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MouseKeyboardRecorder.Models
{
    /// <summary>
    /// 录制文件数据模型
    /// 包含录制文件的元数据和操作序列
    /// </summary>
    public class RecordingFile
    {
        /// <summary>
        /// 文件格式版本号（用于向后兼容）
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// 录制文件唯一标识符
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 录制名称/标题
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 录制描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 录制创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 录制时的屏幕分辨率宽度
        /// </summary>
        public int ScreenWidth { get; set; }

        /// <summary>
        /// 录制时的屏幕分辨率高度
        /// </summary>
        public int ScreenHeight { get; set; }

        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string? OsVersion { get; set; }

        /// <summary>
        /// 录制的操作序列
        /// </summary>
        public List<RecordedAction> Actions { get; set; } = new();

        /// <summary>
        /// 录制总时长（毫秒）
        /// </summary>
        public int TotalDurationMs => CalculateTotalDuration();

        /// <summary>
        /// 录制中的操作数量
        /// </summary>
        public int ActionCount => Actions?.Count ?? 0;

        /// <summary>
        /// 计算录制总时长
        /// </summary>
        /// <returns>总时长（毫秒）</returns>
        private int CalculateTotalDuration()
        {
            if (Actions == null || Actions.Count == 0)
                return 0;

            int total = 0;
            foreach (var action in Actions)
            {
                total += action.DelayMs;
            }
            return total;
        }

        /// <summary>
        /// 创建录制文件的深拷贝
        /// </summary>
        /// <returns>录制文件副本</returns>
        public RecordingFile Clone()
        {
            var clone = new RecordingFile
            {
                Version = this.Version,
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                ScreenWidth = this.ScreenWidth,
                ScreenHeight = this.ScreenHeight,
                OsVersion = this.OsVersion
            };

            if (this.Actions != null)
            {
                foreach (var action in this.Actions)
                {
                    clone.Actions.Add(action.Clone());
                }
            }

            return clone;
        }

        /// <summary>
        /// 验证录制文件的有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            // 检查版本号格式
            if (string.IsNullOrEmpty(Version))
                return false;

            // 检查操作列表
            if (Actions == null)
                return false;

            // 验证每个操作
            foreach (var action in Actions)
            {
                if (!action.IsValid())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 获取录制文件的摘要信息
        /// </summary>
        /// <returns>格式化的摘要字符串</returns>
        public string GetSummary()
        {
            var duration = TimeSpan.FromMilliseconds(TotalDurationMs);
            return $"📁 {Name ?? "未命名录制"}\n" +
                   $"   操作数: {ActionCount} | " +
                   $"总时长: {duration.Minutes:D2}:{duration.Seconds:D2}.{duration.Milliseconds:D3} | " +
                   $"分辨率: {ScreenWidth}x{ScreenHeight}\n" +
                   $"   创建时间: {CreatedAt:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
