using System;
using System.Collections.Generic;
using MouseKeyboardRecorder.Models;

namespace MouseKeyboardRecorder.Services
{
    /// <summary>
    /// 录制服务接口
    /// 定义录制操作的核心功能
    /// </summary>
    public interface IRecorderService
    {
        /// <summary>
        /// 是否正在录制
        /// </summary>
        bool IsRecording { get; }

        /// <summary>
        /// 已录制的操作列表
        /// </summary>
        IReadOnlyList<RecordedAction> RecordedActions { get; }

        /// <summary>
        /// 当前录制中的操作数量
        /// </summary>
        int ActionCount { get; }

        /// <summary>
        /// 录制开始事件
        /// </summary>
        event EventHandler? RecordingStarted;

        /// <summary>
        /// 录制停止事件
        /// </summary>
        event EventHandler? RecordingStopped;

        /// <summary>
        /// 录制操作添加事件
        /// </summary>
        event EventHandler<RecordedAction>? ActionRecorded;

        /// <summary>
        /// 录制错误事件
        /// </summary>
        event EventHandler<Exception>? RecordingError;

        /// <summary>
        /// 开始录制
        /// </summary>
        /// <returns>是否成功开始录制</returns>
        bool StartRecording();

        /// <summary>
        /// 停止录制
        /// </summary>
        void StopRecording();

        /// <summary>
        /// 清空已录制的操作
        /// </summary>
        void ClearRecording();

        /// <summary>
        /// 获取录制文件数据
        /// </summary>
        /// <returns>录制文件对象</returns>
        RecordingFile GetRecordingFile();

        /// <summary>
        /// 从录制文件加载
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        void LoadFromRecordingFile(RecordingFile recordingFile);

        /// <summary>
        /// 添加自定义操作
        /// </summary>
        /// <param name="action">要添加的操作</param>
        void AddAction(RecordedAction action);

        /// <summary>
        /// 删除指定操作
        /// </summary>
        /// <param name="actionId">操作ID</param>
        /// <returns>是否成功删除</returns>
        bool RemoveAction(Guid actionId);

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="action">更新后的操作</param>
        /// <returns>是否成功更新</returns>
        bool UpdateAction(RecordedAction action);
    }
}
