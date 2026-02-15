using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using MouseKeyboardRecorder.Models;
using MouseKeyboardRecorder.Services;
using MouseKeyboardRecorder.Utilities;
using MouseKeyboardRecorder.Views;

namespace MouseKeyboardRecorder
{
    /// <summary>
    /// 主窗口
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 私有字段

        /// <summary>录制服务</summary>
        private readonly IRecorderService _recorderService;

        /// <summary>播放服务</summary>
        private readonly IPlayerService _playerService;

        /// <summary>最近文件管理器</summary>
        private readonly RecentFilesHelper _recentFilesHelper;

        /// <summary>当前加载的录制文件</summary>
        private RecordingFile? _currentRecordingFile;

        /// <summary>当前文件路径</summary>
        private string? _currentFilePath;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // 初始化服务
            _recorderService = new RecorderService();
            _playerService = new PlayerService();
            _recentFilesHelper = new RecentFilesHelper();

            // 订阅事件
            SubscribeEvents();

            // 加载最近文件列表
            _recentFilesHelper.Load();
        }

        #endregion

        #region 事件订阅

        /// <summary>
        /// 订阅服务事件
        /// </summary>
        private void SubscribeEvents()
        {
            // 录制服务事件
            _recorderService.RecordingStarted += OnRecordingStarted;
            _recorderService.RecordingStopped += OnRecordingStopped;
            _recorderService.ActionRecorded += OnActionRecorded;
            _recorderService.RecordingError += OnRecordingError;

            // 播放服务事件
            _playerService.PlaybackStarted += OnPlaybackStarted;
            _playerService.PlaybackStopped += OnPlaybackStopped;
            _playerService.PlaybackCompleted += OnPlaybackCompleted;
            _playerService.PlaybackPaused += OnPlaybackPaused;
            _playerService.PlaybackResumed += OnPlaybackResumed;
            _playerService.CurrentActionChanged += OnCurrentActionChanged;
            _playerService.ProgressChanged += OnProgressChanged;
            _playerService.PlaybackError += OnPlaybackError;
        }

        #endregion

        #region 窗口事件

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置初始状态
            UpdateUIState();

            // 添加快捷键处理
            this.KeyDown += MainWindow_KeyDown;
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // 停止录制和播放
            _recorderService.StopRecording();
            _playerService.Stop();

            // 保存最近文件列表
            _recentFilesHelper.Save();
        }

        /// <summary>
        /// 键盘快捷键处理
        /// </summary>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // F9: 开始/停止录制
            if (e.Key == Key.F9)
            {
                if (_recorderService.IsRecording)
                {
                    BtnStop_Click(sender, e);
                }
                else
                {
                    BtnRecord_Click(sender, e);
                }
                e.Handled = true;
            }
            // F10: 播放/暂停
            else if (e.Key == Key.F10)
            {
                if (_playerService.State == PlaybackState.Playing)
                {
                    BtnPause_Click(sender, e);
                }
                else if (_playerService.State == PlaybackState.Paused)
                {
                    BtnPause_Click(sender, e); // 恢复播放
                }
                else
                {
                    BtnPlay_Click(sender, e);
                }
                e.Handled = true;
            }
        }

        #endregion

        #region 按钮事件处理

        /// <summary>
        /// 录制按钮
        /// </summary>
        private void BtnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (_recorderService.StartRecording())
            {
                UpdateUIState();
            }
            else
            {
                MessageBox.Show("开始录制失败，请确保以管理员身份运行程序。", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 停止录制按钮
        /// </summary>
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _recorderService.StopRecording();
            UpdateUIState();
            RefreshActionList();
        }

        /// <summary>
        /// 播放按钮
        /// </summary>
        private async void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_recorderService.ActionCount == 0)
            {
                MessageBox.Show("没有可播放的操作，请先录制或加载文件。", "提示", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 更新播放设置
            _playerService.PlaybackSpeed = SliderSpeed.Value;
            _playerService.LoopEnabled = ChkLoop.IsChecked ?? false;
            _playerService.SimulateHumanBehavior = ChkSimulateHuman.IsChecked ?? true;

            // 解析循环次数
            if (int.TryParse(TxtLoopCount.Text, out int loopCount))
            {
                _playerService.LoopCount = loopCount;
            }

            // 加载操作并播放
            _playerService.LoadActions(_recorderService.RecordedActions);
            await _playerService.PlayAsync();
        }

        /// <summary>
        /// 暂停/恢复按钮
        /// </summary>
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_playerService.State == PlaybackState.Playing)
            {
                _playerService.Pause();
            }
            else if (_playerService.State == PlaybackState.Paused)
            {
                _playerService.Resume();
            }
        }

        /// <summary>
        /// 停止播放按钮
        /// </summary>
        private void BtnStopPlay_Click(object sender, RoutedEventArgs e)
        {
            _playerService.Stop();
        }

        /// <summary>
        /// 加载文件按钮
        /// </summary>
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "录制文件 (*.json)|*.json|所有文件 (*.*)|*.*",
                Title = "加载录制文件",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadRecordingFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// 保存文件按钮
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_recorderService.ActionCount == 0)
            {
                MessageBox.Show("没有可保存的操作。", "提示", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "录制文件 (*.json)|*.json|所有文件 (*.*)|*.*",
                Title = "保存录制文件",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"录制_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveRecordingFile(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// 清空按钮
        /// </summary>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (_recorderService.ActionCount > 0)
            {
                var result = MessageBox.Show("确定要清空所有录制的操作吗？", "确认", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _recorderService.ClearRecording();
                    _currentRecordingFile = null;
                    _currentFilePath = null;
                    RefreshActionList();
                    UpdateStatus();
                }
            }
        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridActions.SelectedItem is RecordedAction action)
            {
                var editWindow = new EditActionWindow(action.Clone());
                if (editWindow.ShowDialog() == true && editWindow.Action != null)
                {
                    _recorderService.UpdateAction(editWindow.Action);
                    RefreshActionList();
                }
            }
            else
            {
                MessageBox.Show("请先选择要编辑的操作。", "提示", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridActions.SelectedItem is RecordedAction action)
            {
                var result = MessageBox.Show("确定要删除选中的操作吗？", "确认", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _recorderService.RemoveAction(action.Id);
                    RefreshActionList();
                    UpdateStatus();
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的操作。", "提示", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region 控制面板事件

        /// <summary>
        /// 播放速度滑块值变更
        /// </summary>
        private void SliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblSpeed.Text = $"{e.NewValue:F1}x";
            _playerService.PlaybackSpeed = e.NewValue;
        }

        /// <summary>
        /// 循环设置变更
        /// </summary>
        private void ChkLoop_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _playerService.LoopEnabled = ChkLoop.IsChecked ?? false;
        }

        /// <summary>
        /// 人类模拟设置变更
        /// </summary>
        private void ChkSimulateHuman_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _playerService.SimulateHumanBehavior = ChkSimulateHuman.IsChecked ?? true;
        }

        #endregion

        #region 服务事件处理

        /// <summary>
        /// 录制开始
        /// </summary>
        private void OnRecordingStarted(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusRecording.Text = "🔴 正在录制...";
                StatusRecording.Foreground = System.Windows.Media.Brushes.Red;
                UpdateUIState();
            });
        }

        /// <summary>
        /// 录制停止
        /// </summary>
        private void OnRecordingStopped(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusRecording.Text = "⏹️ 录制停止";
                StatusRecording.Foreground = System.Windows.Media.Brushes.Gray;
                UpdateUIState();
            });
        }

        /// <summary>
        /// 新操作被录制
        /// </summary>
        private void OnActionRecorded(object? sender, RecordedAction e)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateStatus();
            });
        }

        /// <summary>
        /// 录制错误
        /// </summary>
        private void OnRecordingError(object? sender, Exception e)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"录制过程中发生错误：{e.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        /// <summary>
        /// 播放开始
        /// </summary>
        private void OnPlaybackStarted(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusPlayback.Text = "▶️ 正在播放...";
                StatusPlayback.Foreground = System.Windows.Media.Brushes.Green;
                UpdateUIState();
            });
        }

        /// <summary>
        /// 播放停止
        /// </summary>
        private void OnPlaybackStopped(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusPlayback.Text = "⏹️ 播放停止";
                StatusPlayback.Foreground = System.Windows.Media.Brushes.Gray;
                StatusProgress.Text = "";
                UpdateUIState();
            });
        }

        /// <summary>
        /// 播放完成
        /// </summary>
        private void OnPlaybackCompleted(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusPlayback.Text = "✅ 播放完成";
                StatusPlayback.Foreground = System.Windows.Media.Brushes.Green;
                StatusProgress.Text = "";
                UpdateUIState();
            });
        }

        /// <summary>
        /// 播放暂停
        /// </summary>
        private void OnPlaybackPaused(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusPlayback.Text = "⏸️ 播放暂停";
                StatusPlayback.Foreground = System.Windows.Media.Brushes.Orange;
                UpdateUIState();
            });
        }

        /// <summary>
        /// 播放恢复
        /// </summary>
        private void OnPlaybackResumed(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusPlayback.Text = "▶️ 正在播放...";
                StatusPlayback.Foreground = System.Windows.Media.Brushes.Green;
                UpdateUIState();
            });
        }

        /// <summary>
        /// 当前操作变更
        /// </summary>
        private void OnCurrentActionChanged(object? sender, RecordedAction e)
        {
            Dispatcher.Invoke(() =>
            {
                // 高亮当前播放的操作
                DataGridActions.SelectedItem = e;
                if (e != null)
                {
                    DataGridActions.ScrollIntoView(e);
                }
            });
        }

        /// <summary>
        /// 播放进度变更
        /// </summary>
        private void OnProgressChanged(object? sender, double e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusProgress.Text = $"进度: {e:P0} ({_playerService.CurrentIndex}/{_playerService.TotalActions})";
            });
        }

        /// <summary>
        /// 播放错误
        /// </summary>
        private void OnPlaybackError(object? sender, Exception e)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"播放过程中发生错误：{e.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                StatusPlayback.Text = "❌ 播放错误";
                StatusPlayback.Foreground = System.Windows.Media.Brushes.Red;
            });
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 更新 UI 状态
        /// </summary>
        private void UpdateUIState()
        {
            bool isRecording = _recorderService.IsRecording;
            bool isPlaying = _playerService.State == PlaybackState.Playing;
            bool isPaused = _playerService.State == PlaybackState.Paused;

            // 录制按钮
            BtnRecord.IsEnabled = !isRecording && !isPlaying;
            BtnStop.IsEnabled = isRecording;

            // 播放按钮
            BtnPlay.IsEnabled = !isRecording && !isPlaying && _recorderService.ActionCount > 0;
            BtnPause.IsEnabled = isPlaying || isPaused;
            BtnPause.Content = isPaused ? "▶️ 继续" : "⏸️ 暂停";
            BtnStopPlay.IsEnabled = isPlaying || isPaused;

            // 文件操作按钮
            BtnLoad.IsEnabled = !isRecording && !isPlaying;
            BtnSave.IsEnabled = !isRecording && !isPlaying && _recorderService.ActionCount > 0;

            // 编辑按钮
            BtnClear.IsEnabled = !isRecording && !isPlaying;
            BtnEdit.IsEnabled = !isRecording && !isPlaying && DataGridActions.SelectedItem != null;
            BtnDelete.IsEnabled = !isRecording && !isPlaying && DataGridActions.SelectedItem != null;

            // 控制面板
            SliderSpeed.IsEnabled = !isRecording && !isPlaying;
            TxtLoopCount.IsEnabled = !isRecording && !isPlaying;
            ChkLoop.IsEnabled = !isRecording && !isPlaying;
            ChkSimulateHuman.IsEnabled = !isRecording && !isPlaying;
        }

        /// <summary>
        /// 刷新操作列表
        /// </summary>
        private void RefreshActionList()
        {
            DataGridActions.ItemsSource = null;
            DataGridActions.ItemsSource = _recorderService.RecordedActions;
            UpdateStatus();
        }

        /// <summary>
        /// 更新状态栏
        /// </summary>
        private void UpdateStatus()
        {
            StatusActionCount.Text = $"操作数: {_recorderService.ActionCount}";
        }

        /// <summary>
        /// 加载录制文件
        /// </summary>
        private void LoadRecordingFile(string filePath)
        {
            try
            {
                var recordingFile = JsonHelper.DeserializeFromFile(filePath);

                if (recordingFile == null)
                {
                    MessageBox.Show("无法解析录制文件，文件格式可能不正确。", "错误", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!JsonHelper.Validate(recordingFile))
                {
                    MessageBox.Show("录制文件格式无效。", "错误", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _currentRecordingFile = recordingFile;
                _currentFilePath = filePath;
                _recorderService.LoadFromRecordingFile(recordingFile);
                _recentFilesHelper.AddRecentFile(filePath);

                RefreshActionList();
                UpdateUIState();

                this.Title = $"🖱️⌨️ 鼠标键盘录制器 v1.0 - {Path.GetFileName(filePath)}";

                MessageBox.Show($"成功加载录制文件！\n共 {_recorderService.ActionCount} 个操作，" +
                    $"总时长 {TimeSpan.FromMilliseconds(recordingFile.TotalDurationMs):mm\\:ss\\.fff}", 
                    "加载成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载文件失败：{ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 保存录制文件
        /// </summary>
        private void SaveRecordingFile(string filePath)
        {
            try
            {
                var recordingFile = _recorderService.GetRecordingFile();

                if (_currentRecordingFile != null)
                {
                    recordingFile.Name = _currentRecordingFile.Name;
                    recordingFile.Description = _currentRecordingFile.Description;
                }

                JsonHelper.SerializeToFile(recordingFile, filePath);
                _recentFilesHelper.AddRecentFile(filePath);

                _currentRecordingFile = recordingFile;
                _currentFilePath = filePath;

                this.Title = $"🖱️⌨️ 鼠标键盘录制器 v1.0 - {Path.GetFileName(filePath)}";

                MessageBox.Show("录制文件保存成功！", "保存成功", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件失败：{ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Cleanup()
        {
            _recorderService.StopRecording();
            _playerService.Stop();
            _recentFilesHelper.Save();

            if (_recorderService is IDisposable disposableRecorder)
            {
                disposableRecorder.Dispose();
            }

            if (_playerService is IDisposable disposablePlayer)
            {
                disposablePlayer.Dispose();
            }
        }

        #endregion
    }
}
