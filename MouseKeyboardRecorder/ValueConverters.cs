using System;
using System.Globalization;
using System.Windows.Data;
using MouseKeyboardRecorder.Models;

namespace MouseKeyboardRecorder
{
    /// <summary>
    /// Guid 到索引转换器（用于显示序号）
    /// </summary>
    public class GuidToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 这里返回空，实际序号在 Loaded 事件中通过绑定索引处理
            return value?.ToString()?.Substring(0, 8) + "..." ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 操作类型到显示文本转换器
    /// </summary>
    public class ActionTypeToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActionType actionType)
            {
                return actionType.GetDisplayName();
            }
            return value?.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 操作到描述文本转换器
    /// </summary>
    public class ActionToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecordedAction action)
            {
                return action.GetDetailDescription();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 操作到值文本转换器（显示坐标或键码）
    /// </summary>
    public class ActionToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecordedAction action)
            {
                if (action.ActionType.IsMouseAction())
                {
                    if (action.ActionType == ActionType.MouseWheel)
                    {
                        return $"({action.X}, {action.Y}) Δ{action.WheelDelta}";
                    }
                    return $"({action.X}, {action.Y})";
                }
                else if (action.ActionType.IsKeyboardAction())
                {
                    return $"VK:{action.VirtualKeyCode} SC:{action.ScanCode}";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 布尔值到可见性转换器
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Visibility visibility)
            {
                return visibility == System.Windows.Visibility.Visible;
            }
            return false;
        }
    }

    /// <summary>
    /// 播放速度到文本转换器
    /// </summary>
    public class SpeedToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double speed)
            {
                return $"{speed:F1}x";
            }
            return "1.0x";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
