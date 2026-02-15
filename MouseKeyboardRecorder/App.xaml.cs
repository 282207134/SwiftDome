using System.Windows;

namespace MouseKeyboardRecorder
{
    /// <summary>
    /// 应用程序入口类
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用程序启动时执行
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 可以在这里添加初始化逻辑
            // 例如：加载配置、检查更新等
        }

        /// <summary>
        /// 应用程序退出时执行
        /// </summary>
        protected override void OnExit(ExitEventArgs e)
        {
            // 清理资源
            if (MainWindow is MainWindow mainWindow)
            {
                mainWindow.Cleanup();
            }

            base.OnExit(e);
        }
    }
}
