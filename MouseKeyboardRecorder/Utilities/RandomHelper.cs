using System;

namespace MouseKeyboardRecorder.Utilities
{
    /// <summary>
    /// 随机数生成器助手类
    /// 提供用于模拟人类行为的随机数生成功能
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 随机数生成器实例（线程安全）
        /// </summary>
        private static readonly Random _random = new();

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _lockObject = new();

        /// <summary>
        /// 生成指定范围内的随机整数
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>随机整数</returns>
        public static int Next(int minValue, int maxValue)
        {
            lock (_lockObject)
            {
                return _random.Next(minValue, maxValue);
            }
        }

        /// <summary>
        /// 生成指定范围内的随机整数（0 到 maxValue）
        /// </summary>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>随机整数</returns>
        public static int Next(int maxValue)
        {
            lock (_lockObject)
            {
                return _random.Next(maxValue);
            }
        }

        /// <summary>
        /// 生成 0.0 到 1.0 之间的随机双精度浮点数
        /// </summary>
        /// <returns>随机双精度浮点数</returns>
        public static double NextDouble()
        {
            lock (_lockObject)
            {
                return _random.NextDouble();
            }
        }

        /// <summary>
        /// 生成指定范围内的随机双精度浮点数
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（包含）</param>
        /// <returns>随机双精度浮点数</returns>
        public static double NextDouble(double minValue, double maxValue)
        {
            lock (_lockObject)
            {
                return minValue + (_random.NextDouble() * (maxValue - minValue));
            }
        }

        /// <summary>
        /// 生成带有随机波动的延迟值（用于模拟人类操作）
        /// </summary>
        /// <param name="baseDelay">基础延迟（毫秒）</param>
        /// <param name="variancePercent">波动百分比（默认 10%）</param>
        <returns>带有随机波动的延迟</returns>
        public static int GetHumanizedDelay(int baseDelay, double variancePercent = 10.0)
        {
            if (baseDelay <= 0)
                return 0;

            lock (_lockObject)
            {
                double variance = baseDelay * (variancePercent / 100.0);
                double randomOffset = (_random.NextDouble() * 2 - 1) * variance; // -variance 到 +variance
                return Math.Max(0, (int)(baseDelay + randomOffset));
            }
        }

        /// <summary>
        /// 生成带有轻微随机偏移的坐标（用于模拟人类鼠标移动）
        /// </summary>
        /// <param name="baseX">基础 X 坐标</param>
        /// <param name="baseY">基础 Y 坐标</param>
        /// <param name="maxOffset">最大偏移量（像素）</param>
        /// <returns>偏移后的坐标元组 (x, y)</returns>
        public static (int x, int y) GetHumanizedPosition(int baseX, int baseY, int maxOffset = 2)
        {
            lock (_lockObject)
            {
                int offsetX = _random.Next(-maxOffset, maxOffset + 1);
                int offsetY = _random.Next(-maxOffset, maxOffset + 1);
                return (baseX + offsetX, baseY + offsetY);
            }
        }

        /// <summary>
        /// 生成贝塞尔曲线控制点（用于模拟自然鼠标移动轨迹）
        /// </summary>
        /// <param name="startX">起始 X 坐标</param>
        /// <param name="startY">起始 Y 坐标</param>
        /// <param name="endX">目标 X 坐标</param>
        /// <param name="endY">目标 Y 坐标</param>
        /// <returns>控制点坐标元组 (cp1x, cp1y, cp2x, cp2y)</returns>
        public static (int cp1x, int cp1y, int cp2x, int cp2y) GetBezierControlPoints(
            int startX, int startY, int endX, int endY)
        {
            lock (_lockObject)
            {
                // 计算中点
                int midX = (startX + endX) / 2;
                int midY = (startY + endY) / 2;

                // 在中点周围随机偏移控制点
                int offsetRange = Math.Max(Math.Abs(endX - startX), Math.Abs(endY - startY)) / 4;
                offsetRange = Math.Max(offsetRange, 20); // 最小偏移量

                int cp1x = midX + _random.Next(-offsetRange, offsetRange);
                int cp1y = midY + _random.Next(-offsetRange, offsetRange);
                int cp2x = midX + _random.Next(-offsetRange, offsetRange);
                int cp2y = midY + _random.Next(-offsetRange, offsetRange);

                return (cp1x, cp1y, cp2x, cp2y);
            }
        }

        /// <summary>
        /// 计算贝塞尔曲线上的点
        /// </summary>
        /// <param name="t">参数（0.0 到 1.0）</param>
        /// <param name="p0">起点</param>
        /// <param name="p1">控制点1</param>
        /// <param name="p2">控制点2</param>
        /// <param name="p3">终点</param>
        /// <returns>曲线上的点坐标</returns>
        public static (int x, int y) GetBezierPoint(
            double t,
            (int x, int y) p0,
            (int x, int y) p1,
            (int x, int y) p2,
            (int x, int y) p3)
        {
            double u = 1 - t;
            double tt = t * t;
            double uu = u * u;
            double uuu = uu * u;
            double ttt = tt * t;

            double x = uuu * p0.x + 3 * uu * t * p1.x + 3 * u * tt * p2.x + ttt * p3.x;
            double y = uuu * p0.y + 3 * uu * t * p1.y + 3 * u * tt * p2.y + ttt * p3.y;

            return ((int)x, (int)y);
        }

        /// <summary>
        /// 生成随机布尔值
        /// </summary>
        /// <returns>随机布尔值</returns>
        public static bool NextBool()
        {
            lock (_lockObject)
            {
                return _random.Next(2) == 0;
            }
        }

        /// <summary>
        /// 根据概率返回布尔值
        /// </summary>
        /// <param name="probability">概率（0.0 到 1.0）</param>
        /// <returns>根据概率返回 true 或 false</returns>
        public static bool NextBool(double probability)
        {
            lock (_lockObject)
            {
                return _random.NextDouble() < probability;
            }
        }

        /// <summary>
        /// 生成随机的停顿时间（用于模拟人类思考时间）
        /// </summary>
        /// <param name="minMs">最小停顿时间（毫秒）</param>
        /// <param name="maxMs">最大停顿时间（毫秒）</param>
        /// <returns>随机停顿时间</returns>
        public static int GetThinkingDelay(int minMs = 50, int maxMs = 200)
        {
            lock (_lockObject)
            {
                return _random.Next(minMs, maxMs + 1);
            }
        }

        /// <summary>
        /// 从数组中随机选择一个元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="items">数组</param>
        /// <returns>随机选择的元素</returns>
        public static T? RandomChoice<T>(T[] items)
        {
            if (items == null || items.Length == 0)
                return default;

            lock (_lockObject)
            {
                return items[_random.Next(items.Length)];
            }
        }

        /// <summary>
        /// 打乱数组顺序
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="array">要打乱的数组</param>
        public static void Shuffle<T>(T[] array)
        {
            if (array == null || array.Length <= 1)
                return;

            lock (_lockObject)
            {
                for (int i = array.Length - 1; i > 0; i--)
                {
                    int j = _random.Next(i + 1);
                    (array[i], array[j]) = (array[j], array[i]);
                }
            }
        }
    }
}
