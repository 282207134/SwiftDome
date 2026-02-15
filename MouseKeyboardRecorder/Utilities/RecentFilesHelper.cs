using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MouseKeyboardRecorder.Utilities
{
    /// <summary>
    /// 最近文件信息
    /// </summary>
    public class RecentFileInfo
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName => Path.GetFileName(FilePath);

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessed { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int AccessCount { get; set; } = 1;

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string DisplayText => $"{FileName} ({LastAccessed:MM-dd HH:mm})";
    }

    /// <summary>
    /// 最近文件列表管理助手
    /// 管理最近使用的文件列表，支持持久化存储
    /// </summary>
    public class RecentFilesHelper
    {
        /// <summary>
        /// 最大文件数量
        /// </summary>
        private const int MaxRecentFiles = 10;

        /// <summary>
        /// 最近文件列表
        /// </summary>
        private readonly List<RecentFileInfo> _recentFiles = new();

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private readonly string _configFilePath;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object _lockObject = new();

        /// <summary>
        /// 最近文件变更事件
        /// </summary>
        public event EventHandler? RecentFilesChanged;

        /// <summary>
        /// 获取最近文件列表（按访问时间倒序）
        /// </summary>
        public IReadOnlyList<RecentFileInfo> RecentFiles
        {
            get
            {
                lock (_lockObject)
                {
                    return _recentFiles.OrderByDescending(f => f.LastAccessed).ToList().AsReadOnly();
                }
            }
        }

        /// <summary>
        /// 最近文件数量
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lockObject)
                {
                    return _recentFiles.Count;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RecentFilesHelper()
        {
            // 配置文件存储在应用程序数据目录
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "MouseKeyboardRecorder");
            
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            _configFilePath = Path.Combine(appFolder, "recent_files.json");
        }

        /// <summary>
        /// 构造函数（指定配置文件路径）
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public RecentFilesHelper(string configFilePath)
        {
            _configFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
        }

        /// <summary>
        /// 添加或更新最近文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void AddRecentFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            if (!File.Exists(filePath))
                return;

            lock (_lockObject)
            {
                var existingFile = _recentFiles.FirstOrDefault(f => 
                    f.FilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase));

                if (existingFile != null)
                {
                    // 更新现有文件信息
                    existingFile.LastAccessed = DateTime.Now;
                    existingFile.AccessCount++;
                    
                    var fileInfo = new FileInfo(filePath);
                    existingFile.FileSize = fileInfo.Length;
                }
                else
                {
                    // 添加新文件
                    var fileInfo = new FileInfo(filePath);
                    _recentFiles.Add(new RecentFileInfo
                    {
                        FilePath = filePath,
                        LastAccessed = DateTime.Now,
                        AccessCount = 1,
                        FileSize = fileInfo.Length
                    });

                    // 限制数量
                    if (_recentFiles.Count > MaxRecentFiles)
                    {
                        // 移除访问次数最少的文件
                        var fileToRemove = _recentFiles.OrderBy(f => f.AccessCount)
                                                      .ThenBy(f => f.LastAccessed)
                                                      .First();
                        _recentFiles.Remove(fileToRemove);
                    }
                }
            }

            RecentFilesChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 从最近文件列表中移除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否成功移除</returns>
        public bool RemoveRecentFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            bool removed;
            lock (_lockObject)
            {
                var file = _recentFiles.FirstOrDefault(f => 
                    f.FilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase));
                removed = file != null && _recentFiles.Remove(file);
            }

            if (removed)
            {
                RecentFilesChanged?.Invoke(this, EventArgs.Empty);
            }

            return removed;
        }

        /// <summary>
        /// 清空最近文件列表
        /// </summary>
        public void ClearRecentFiles()
        {
            lock (_lockObject)
            {
                _recentFiles.Clear();
            }

            RecentFilesChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 检查文件是否在列表中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否存在</returns>
        public bool Contains(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            lock (_lockObject)
            {
                return _recentFiles.Any(f => 
                    f.FilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// 获取指定索引的文件信息
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>文件信息，如果索引无效则返回 null</returns>
        public RecentFileInfo? GetRecentFile(int index)
        {
            lock (_lockObject)
            {
                var orderedFiles = _recentFiles.OrderByDescending(f => f.LastAccessed).ToList();
                if (index >= 0 && index < orderedFiles.Count)
                {
                    return orderedFiles[index];
                }
            }
            return null;
        }

        /// <summary>
        /// 异步保存最近文件列表到配置文件
        /// </summary>
        public async Task SaveAsync()
        {
            List<RecentFileInfo> filesToSave;
            lock (_lockObject)
            {
                filesToSave = _recentFiles.OrderByDescending(f => f.LastAccessed)
                                         .Take(MaxRecentFiles)
                                         .ToList();
            }

            var json = JsonSerializer.Serialize(filesToSave, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(_configFilePath, json);
        }

        /// <summary>
        /// 同步保存最近文件列表到配置文件
        /// </summary>
        public void Save()
        {
            List<RecentFileInfo> filesToSave;
            lock (_lockObject)
            {
                filesToSave = _recentFiles.OrderByDescending(f => f.LastAccessed)
                                         .Take(MaxRecentFiles)
                                         .ToList();
            }

            var json = JsonSerializer.Serialize(filesToSave, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_configFilePath, json);
        }

        /// <summary>
        /// 异步从配置文件加载最近文件列表
        /// </summary>
        public async Task LoadAsync()
        {
            if (!File.Exists(_configFilePath))
                return;

            try
            {
                var json = await File.ReadAllTextAsync(_configFilePath);
                var files = JsonSerializer.Deserialize<List<RecentFileInfo>>(json);

                lock (_lockObject)
                {
                    _recentFiles.Clear();
                    if (files != null)
                    {
                        // 过滤掉不存在的文件
                        foreach (var file in files.Where(f => File.Exists(f.FilePath)))
                        {
                            _recentFiles.Add(file);
                        }
                    }
                }

                RecentFilesChanged?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                // 加载失败时清空列表
                lock (_lockObject)
                {
                    _recentFiles.Clear();
                }
            }
        }

        /// <summary>
        /// 同步从配置文件加载最近文件列表
        /// </summary>
        public void Load()
        {
            if (!File.Exists(_configFilePath))
                return;

            try
            {
                var json = File.ReadAllText(_configFilePath);
                var files = JsonSerializer.Deserialize<List<RecentFileInfo>>(json);

                lock (_lockObject)
                {
                    _recentFiles.Clear();
                    if (files != null)
                    {
                        // 过滤掉不存在的文件
                        foreach (var file in files.Where(f => File.Exists(f.FilePath)))
                        {
                            _recentFiles.Add(file);
                        }
                    }
                }

                RecentFilesChanged?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                // 加载失败时清空列表
                lock (_lockObject)
                {
                    _recentFiles.Clear();
                }
            }
        }

        /// <summary>
        /// 清理不存在的文件条目
        /// </summary>
        /// <returns>清理的文件数量</returns>
        public int CleanupMissingFiles()
        {
            int removedCount = 0;

            lock (_lockObject)
            {
                var missingFiles = _recentFiles.Where(f => !File.Exists(f.FilePath)).ToList();
                foreach (var file in missingFiles)
                {
                    _recentFiles.Remove(file);
                    removedCount++;
                }
            }

            if (removedCount > 0)
            {
                RecentFilesChanged?.Invoke(this, EventArgs.Empty);
            }

            return removedCount;
        }
    }
}
