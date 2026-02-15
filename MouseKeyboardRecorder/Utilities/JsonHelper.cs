using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MouseKeyboardRecorder.Models;

namespace MouseKeyboardRecorder.Utilities
{
    /// <summary>
    /// JSON 序列化助手类
    /// 提供录制文件的序列化和反序列化功能
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 默认 JSON 序列化选项
        /// </summary>
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// 序列化录制文件为 JSON 字符串
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        /// <returns>JSON 字符串</returns>
        public static string Serialize(RecordingFile recordingFile)
        {
            if (recordingFile == null)
                throw new ArgumentNullException(nameof(recordingFile));

            return JsonSerializer.Serialize(recordingFile, DefaultOptions);
        }

        /// <summary>
        /// 异步序列化录制文件并写入文件
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        /// <param name="filePath">目标文件路径</param>
        /// <returns>异步任务</returns>
        public static async Task SerializeToFileAsync(RecordingFile recordingFile, string filePath)
        {
            if (recordingFile == null)
                throw new ArgumentNullException(nameof(recordingFile));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("文件路径不能为空", nameof(filePath));

            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = Serialize(recordingFile);
            await File.WriteAllTextAsync(filePath, json);
        }

        /// <summary>
        /// 同步序列化录制文件并写入文件
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        /// <param name="filePath">目标文件路径</param>
        public static void SerializeToFile(RecordingFile recordingFile, string filePath)
        {
            if (recordingFile == null)
                throw new ArgumentNullException(nameof(recordingFile));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("文件路径不能为空", nameof(filePath));

            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = Serialize(recordingFile);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// 从 JSON 字符串反序列化录制文件
        /// </summary>
        /// <param name="json">JSON 字符串</param>
        /// <returns>录制文件对象</returns>
        public static RecordingFile? Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var recordingFile = JsonSerializer.Deserialize<RecordingFile>(json, DefaultOptions);
                return recordingFile;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        /// <summary>
        /// 异步从文件读取并反序列化录制文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>录制文件对象，如果失败则返回 null</returns>
        public static async Task<RecordingFile?> DeserializeFromFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            if (!File.Exists(filePath))
                return null;

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                return Deserialize(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 同步从文件读取并反序列化录制文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>录制文件对象，如果失败则返回 null</returns>
        public static RecordingFile? DeserializeFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            if (!File.Exists(filePath))
                return null;

            try
            {
                var json = File.ReadAllText(filePath);
                return Deserialize(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 验证录制文件的有效性
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        /// <returns>是否有效</returns>
        public static bool Validate(RecordingFile? recordingFile)
        {
            if (recordingFile == null)
                return false;

            // 检查版本号
            if (string.IsNullOrEmpty(recordingFile.Version))
                return false;

            // 检查操作列表
            if (recordingFile.Actions == null)
                return false;

            // 验证每个操作
            foreach (var action in recordingFile.Actions)
            {
                if (!action.IsValid())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 将录制文件转换为美化后的 JSON 字符串
        /// </summary>
        /// <param name="recordingFile">录制文件对象</param>
        /// <returns>美化的 JSON 字符串</returns>
        public static string ToFormattedJson(RecordingFile recordingFile)
        {
            return Serialize(recordingFile);
        }
    }
}
