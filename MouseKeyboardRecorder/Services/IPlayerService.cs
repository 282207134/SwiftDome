using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MouseKeyboardRecorder.Models;

namespace MouseKeyboardRecorder.Services
{
    /// <summary>
    /// 播放状态枚举
    /// </summary>
    public enum PlaybackState
    {
        /// <summary>空闲状态</summary>
        Idle = 0,

        /// <summary>正在播放</summary>
        Playing = 1,

        /// <summary>已暂停</summary>
        Paused = 2,

        /// <summary>已完成</summary>
        Completed = 3,

        /// <summary>已停止</summary>
        Stopped = 4,

        /// <summary>发生错误</summary>
        Error = 5
    }

    /// <summary>
    /// 播放服务接口
    /// 定义播放录制操作的核心功能
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// 当前播放状态
        /// </summary>
        PlaybackState State { get; }

        /// <summary>
        /// 当前播放速度（倍数，范围 0.1 - 5.0）
        /// </summary>
        double PlaybackSpeed { get; set; }

        /// <summary>
        /// 是否启用循环播放
        /// </summary>
        bool LoopEnabled { get; set; }

        /// <summary>
        /// 循环播放次数（0 表示无限循环）
        /// </summary>
        int LoopCount { get; set; }

        /// <summary>
        /// 是否模拟人类操作（添加随机延迟）
        /// </summary>
        bool SimulateHumanBehavior { get; set; }

        /// <summary>
        /// 当前播放位置索引
        /// </summary>
        int CurrentIndex { get; }

        /// <summary>
        /// 总操作数量
        /// </summary>
        int TotalActions { get; }

        /// <summary>
        /// 播放进度（0.0 - 1.0）
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// 当前循环次数
        /// </summary>
        int CurrentLoop { get; }

        /// <summary>
        /// 播放开始事件
        /// </summary>
        event EventHandler? PlaybackStarted;

        /// <summary>
        /// 播放暂停事件
        /// </summary>
        event EventHandler? PlaybackPaused;

        /// <summary>
        /// 播放恢复事件
        /// </summary>
        event EventHandler? PlaybackResumed;

        /// <summary>
        /// 播放停止事件
        /// </summary>
        event EventHandler? PlaybackStopped;

        /// <summary>
        /// 播放完成事件
        /// </summary>
        event EventHandler? PlaybackCompleted;

        /// <summary>
        /// 播放进度更新事件
        /// </summary>
        event EventHandler<double>? ProgressChanged;

        /// <summary>
        /// 当前操作变更事件
        /// </summary>
        event EventHandler<RecordedAction>? CurrentActionChanged;

        /// <summary>
        /// 播放错误事件
        /// </summary>
        event EventHandler<Exception>? PlaybackError;

        /// <summary>
        /// 加载录制文件
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        void LoadRecording(RecordingFile recordingFile);

        /// <summary>
        /// 加载操作列表
        /// </summary>
        /// <param name="actions">操作列表</param>
        void LoadActions(IEnumerable<RecordedAction> actions);

        /// <summary>
        /// 开始播放
        /// </summary>
        /// <returns>播放任务</returns>
        Task PlayAsync();

        /// <summary>
        /// 从指定位置开始播放
        /// </summary>
        /// <param name="startIndex">起始索引</param>
        /// <returns>播放任务</returns>
        Task PlayFromAsync(int startIndex);

        /// <summary>
        /// 暂停播放
        /// </summary>
        void Pause();

        /// <summary>
        /// 恢复播放
        /// </summary>
        void Resume();

        /// <summary>
        /// 停止播放
        /// </summary>
        void Stop();

        /// <summary>
        /// 跳转到指定位置
        /// </summary>
        /// <param name="index">目标索引</param>
        void SeekTo(int index);
    }
}
