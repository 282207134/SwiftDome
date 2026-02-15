using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MouseKeyboardRecorder.Helpers;
using MouseKeyboardRecorder.Models;

namespace MouseKeyboardRecorder.Views
{
    /// <summary>
    /// 操作编辑窗口
    /// </summary>
    public partial class EditActionWindow : Window
    {
        /// <summary>
        /// 编辑的操作对象
        /// </summary>
        public RecordedAction? Action { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">要编辑的操作</param>
        public EditActionWindow(RecordedAction action)
        {
            InitializeComponent();
            Action = action;
            LoadActionData();
        }

        /// <summary>
        /// 加载操作数据到界面
        /// </summary>
        private void LoadActionData()
        {
            if (Action == null)
                return;

            // 设置操作类型
            SelectActionType(Action.ActionType);

            // 设置坐标
            TxtX.Text = Action.X.ToString();
            TxtY.Text = Action.Y.ToString();

            // 设置滚轮值
            SliderWheel.Value = Action.WheelDelta;
            LblWheelValue.Text = Action.WheelDelta.ToString();

            // 设置键盘值
            TxtVirtualKey.Text = Action.VirtualKeyCode.ToString();
            TxtScanCode.Text = Action.ScanCode.ToString();
            TxtCharacter.Text = Action.Character ?? "";

            // 设置延迟
            TxtDelay.Text = Action.DelayMs.ToString();

            // 更新界面可见性
            UpdateVisibility();
        }

        /// <summary>
        /// 选择操作类型
        /// </summary>
        private void SelectActionType(ActionType actionType)
        {
            foreach (ComboBoxItem item in CmbActionType.Items)
            {
                if (item.Tag?.ToString() == actionType.ToString())
                {
                    CmbActionType.SelectedItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// 获取选中的操作类型
        /// </summary>
        private ActionType GetSelectedActionType()
        {
            if (CmbActionType.SelectedItem is ComboBoxItem selectedItem &&
                selectedItem.Tag != null)
            {
                if (Enum.TryParse<ActionType>(selectedItem.Tag.ToString(), out var actionType))
                {
                    return actionType;
                }
            }
            return ActionType.MouseMove;
        }

        /// <summary>
        /// 更新界面元素的可见性
        /// </summary>
        private void UpdateVisibility()
        {
            var actionType = GetSelectedActionType();

            // 鼠标操作显示坐标设置
            bool isMouseAction = actionType.IsMouseAction();
            GroupCoordinates.Visibility = isMouseAction ? Visibility.Visible : Visibility.Collapsed;

            // 滚轮操作显示滚轮设置
            bool isWheelAction = actionType == ActionType.MouseWheel;
            GroupWheel.Visibility = isWheelAction ? Visibility.Visible : Visibility.Collapsed;

            // 键盘操作显示键盘设置
            bool isKeyboardAction = actionType.IsKeyboardAction();
            GroupKeyboard.Visibility = isKeyboardAction ? Visibility.Visible : Visibility.Collapsed;

            // 延迟设置对所有操作都可见
            GroupDelay.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 操作类型变更事件
        /// </summary>
        private void CmbActionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateVisibility();
        }

        /// <summary>
        /// 数字输入验证
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // 允许负号和数字
            Regex regex = new Regex(@"^-?\d*$");
            string newText = ((TextBox)sender).Text + e.Text;
            e.Handled = !regex.IsMatch(newText);
        }

        /// <summary>
        /// 获取当前鼠标位置按钮
        /// </summary>
        private void BtnPickPosition_Click(object sender, RoutedEventArgs e)
        {
            var pos = InputSimulator.GetCurrentMousePosition();
            TxtX.Text = pos.X.ToString();
            TxtY.Text = pos.Y.ToString();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (Action == null)
            {
                DialogResult = false;
                Close();
                return;
            }

            try
            {
                // 更新操作类型
                Action.ActionType = GetSelectedActionType();

                // 更新坐标
                if (int.TryParse(TxtX.Text, out int x))
                    Action.X = x;
                if (int.TryParse(TxtY.Text, out int y))
                    Action.Y = y;

                // 更新滚轮值
                Action.WheelDelta = (int)SliderWheel.Value;

                // 更新键盘值
                if (int.TryParse(TxtVirtualKey.Text, out int vkCode))
                    Action.VirtualKeyCode = vkCode;
                if (int.TryParse(TxtScanCode.Text, out int scanCode))
                    Action.ScanCode = scanCode;
                Action.Character = TxtCharacter.Text;

                // 更新延迟
                if (int.TryParse(TxtDelay.Text, out int delay))
                    Action.DelayMs = Math.Max(0, delay);

                // 更新时间戳
                Action.Timestamp = DateTime.UtcNow;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存操作失败：{ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
