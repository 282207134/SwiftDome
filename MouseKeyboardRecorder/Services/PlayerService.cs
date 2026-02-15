using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MouseKeyboardRecorder.Helpers;
using MouseKeyboardRecorder.Models;
using MouseKeyboardRecorder.Utilities;

namespace MouseKeyboardRecorder.Services
{
    /// <summary>
    /// 播放服务实现
    /// 支持播放速度调节、循环播放、人类行为模拟等功能
    /// </summary>
    public class PlayerService : IPlayerService, IDisposable
    {
        #region 私有字段

        /// <summary>操作列表</summary>
        private readonly List<RecordedAction> _actions = new();

        /// <summary>取消令牌源</summary>
        private CancellationTokenSource? _cancellationTokenSource;

        /// <summary>播放暂停信号</summary>
        private ManualResetEventSlim? _pauseEvent;

        /// <summary>同步锁</summary>
        private readonly object _lockObject = new();

        /// <summary>是否已释放</summary>
        private bool _disposed;

        #endregion

        #region 公共属性

        /// <inheritdoc />
        public PlaybackState State { get; private set; } = PlaybackState.Idle;

        /// <inheritdoc />
        public double PlaybackSpeed { get; set; } = 1.0;

        /// <inheritdoc />
        public bool LoopEnabled { get; set; } = false;

        /// <inheritdoc />
        public int LoopCount { get; set; } = 0;

        /// <inheritdoc />
        public bool SimulateHumanBehavior { get; set; } = true;

        /// <inheritdoc />
        public int CurrentIndex { get; private set; } = 0;

        /// <inheritdoc />
        public int TotalActions
        {
            get
            {
                lock (_lockObject)
                {
                    return _actions.Count;
                }
            }
        }

        /// <inheritdoc />
        public double Progress
        {
            get
            {
                lock (_lockObject)
                {
                    if (_actions.Count == 0)
                        return 0.0;
                    return (double)CurrentIndex / _actions.Count;
                }
            }
        }

        /// <inheritdoc />
        public int CurrentLoop { get; private set; } = 0;

        #endregion

        #region 事件

        /// <inheritdoc />
        public event EventHandler? PlaybackStarted;

        /// <inheritdoc />
        public event EventHandler? PlaybackPaused;

        /// <inheritdoc />
        public event EventHandler? PlaybackResumed;

        /// <inheritdoc />
        public event EventHandler? PlaybackStopped;

        /// <inheritdoc />
        public event EventHandler? PlaybackCompleted;

        /// <inheritdoc />
        public event EventHandler<double>? ProgressChanged;

        /// <inheritdoc />
        public event EventHandler<RecordedAction>? CurrentActionChanged;

        /// <inheritdoc />
        public event EventHandler<Exception>? PlaybackError;

        #endregion

        #region 公共方法

        /// <inheritdoc />
        public void LoadRecording(RecordingFile recordingFile)
        {
            if (recordingFile?.Actions == null)
                return;

            Stop();

            lock (_lockObject)
            {
                _actions.Clear();
                foreach (var action in recordingFile.Actions)
                {
                    _actions.Add(action);
                }
            }

            CurrentIndex = 0;
            CurrentLoop = 0;
        }

        /// <inheritdoc />
        public void LoadActions(IEnumerable<RecordedAction> actions)
        {
            if (actions == null)
                return;

            Stop();

            lock (_lockObject)
            {
                _actions.Clear();
                _actions.AddRange(actions);
            }

            CurrentIndex = 0;
            CurrentLoop = 0;
        }

        /// <inheritdoc />
        public async Task PlayAsync()
        {
            await PlayFromAsync(0);
        }

        /// <inheritdoc />
        public async Task PlayFromAsync(int startIndex)
        {
            if (State == PlaybackState.Playing)
                return;

            lock (_lockObject)
            {
                if (_actions.Count == 0)
                    return;
            }

            // 确保速度在有效范围内
            PlaybackSpeed = Math.Clamp(PlaybackSpeed, 0.1, 5.0);

            // 创建取消令牌
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // 创建暂停信号
            _pauseEvent?.Dispose();
            _pauseEvent = new ManualResetEventSlim(true);

            // 设置起始位置
            CurrentIndex = Math.Clamp(startIndex, 0, TotalActions);
            CurrentLoop = 0;

            State = PlaybackState.Playing;
            PlaybackStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                await Task.Run(() => PlaybackLoop(token), token);
            }
            catch (OperationCanceledException)
            {
                // 正常取消
            }
            catch (Exception ex)
            {
                PlaybackError?.Invoke(this, ex);
                State = PlaybackState.Error;
            }
        }

        /// <inheritdoc />
        public void Pause()
        {
            if (State == PlaybackState.Playing)
            {
                State = PlaybackState.Paused;
                _pauseEvent?.Reset();
                PlaybackPaused?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public void Resume()
        {
            if (State == PlaybackState.Paused)
            {
                State = PlaybackState.Playing;
                _pauseEvent?.Set();
                PlaybackResumed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            if (State == PlaybackState.Idle)
                return;

            _cancellationTokenSource?.Cancel();
            _pauseEvent?.Set();

            State = PlaybackState.Stopped;
            CurrentIndex = 0;
            CurrentLoop = 0;
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void SeekTo(int index)
        {
            lock (_lockObject)
            {
                CurrentIndex = Math.Clamp(index, 0, _actions.Count);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 播放循环
        /// </summary>
        private void PlaybackLoop(CancellationToken token)
        {
            bool shouldContinue = true;

            while (shouldContinue && !token.IsCancellationRequested)
            {
                // 执行单次播放
                bool completed = ExecuteSinglePlayback(token);

                if (!completed || token.IsCancellationRequested)
                    break;

                // 处理循环逻辑
                if (LoopEnabled)
                {
                    CurrentLoop++;

                    // 检查是否达到循环次数限制
                    if (LoopCount > 0 && CurrentLoop >= LoopCount)
                    {
                        shouldContinue = false;
                    }
                    else
                    {
                        // 重置到开始位置
                        CurrentIndex = 0;
                    }
                }
                else
                {
                    shouldContinue = false;
                }
            }

            if (!token.IsCancellationRequested)
            {
                State = PlaybackState.Completed;
                PlaybackCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 执行单次播放
        /// </summary>
        /// <returns>是否完整播放完成</returns>
        private bool ExecuteSinglePlayback(CancellationToken token)
        {
            while (CurrentIndex < TotalActions && !token.IsCancellationRequested)
            {
                // 等待暂停恢复
                _pauseEvent?.Wait(token);

                if (token.IsCancellationRequested)
                    return false;

                RecordedAction action;
                lock (_lockObject)
                {
                    if (CurrentIndex >= _actions.Count)
                        return true;
                    action = _actions[CurrentIndex];
                }

                try
                {
                    // 执行操作
                    ExecuteAction(action);

                    // 更新当前索引
                    CurrentIndex++;

                    // 触发事件
                    CurrentActionChanged?.Invoke(this, action);
                    ProgressChanged?.Invoke(this, Progress);

                    // 计算延迟（考虑播放速度和人类模拟）
                    int delay = CalculateDelay(action.DelayMs);
                    if (delay > 0 && CurrentIndex < TotalActions)
                    {
                        Thread.Sleep(delay);
                    }
                }
                catch (Exception ex)
                {
                    PlaybackError?.Invoke(this, ex);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 执行单个操作
        /// </summary>
        private void ExecuteAction(RecordedAction action)
        {
            // 移动鼠标到指定位置（如果是点击操作）
            if (action.ActionType is ActionType.MouseLeftDown or ActionType.MouseLeftUp
                or ActionType.MouseRightDown or ActionType.MouseRightUp
                or ActionType.MouseMiddleDown or ActionType.MouseMiddleUp
                or ActionType.MouseWheel)
            {
                InputSimulator.MoveMouse(action.X, action.Y);
                Thread.Sleep(5); // 短暂延迟确保鼠标到位
            }

            // 执行具体操作
            InputSimulator.ExecuteAction(action);
        }

        /// <summary>
        /// 计算实际延迟
        /// </summary>
        private int CalculateDelay(int originalDelay)
        {
            // 应用播放速度
            int adjustedDelay = (int)(originalDelay / PlaybackSpeed);

            // 模拟人类行为（添加随机波动）
            if (SimulateHumanBehavior && adjustedDelay > 10)
            {
                // 添加 ±5-10% 的随机延迟
                double randomFactor = RandomHelper.NextDouble(0.9, 1.1);
                adjustedDelay = (int)(adjustedDelay * randomFactor);
            }

            // 确保延迟不为负
            return Math.Max(0, adjustedDelay);
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            Stop();

            _cancellationTokenSource?.Dispose();
            _pauseEvent?.Dispose();

            _disposed = true;
        }

        #endregion
    }
}
