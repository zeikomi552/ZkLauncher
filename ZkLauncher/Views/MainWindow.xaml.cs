using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZkLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //// ウィンドウのサイズを復元
            //RecoverWindowBounds();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // ウィンドウのサイズを保存
            SaveWindowBounds();
            base.OnClosing(e);
        }
        /// <summary>
        /// ウィンドウの位置・サイズを保存します。
        /// </summary>
        void SaveWindowBounds()
        {
            var settings = Properties.Settings.Default;
            //settings.WindowMaximized = WindowState == WindowState.Maximized;
            WindowState = WindowState.Normal; // 最大化解除
            settings.ViewerWindowHeight = this.Height;
            settings.ViewerWindowWidth = this.Width;
            settings.ViewerWindowTop = this.Top;
            settings.ViewerWindowLeft = this.Left;
            settings.Save();
        }

        /// <summary>
        /// ウィンドウの位置・サイズを復元します。
        /// </summary>
        void RecoverWindowBounds()
        {
            var settings = Properties.Settings.Default;
            // 左
            if (settings.ViewerWindowLeft >= 0 &&
                (settings.ViewerWindowLeft + settings.ViewerWindowWidth) < SystemParameters.VirtualScreenWidth)
            { Left = settings.ViewerWindowLeft; }
            // 上
            if (settings.ViewerWindowTop >= 0 &&
                (settings.ViewerWindowTop + settings.ViewerWindowHeight) < SystemParameters.VirtualScreenHeight)
            { Top = settings.ViewerWindowTop; }
            // 幅
            if (settings.ViewerWindowWidth > 0 &&
                settings.ViewerWindowWidth <= SystemParameters.WorkArea.Width)
            { Width = settings.ViewerWindowWidth; }
            // 高さ
            if (settings.ViewerWindowHeight > 0 &&
                settings.ViewerWindowHeight <= SystemParameters.WorkArea.Height)
            { Height = settings.ViewerWindowHeight; }
            //// 最大化
            //if (settings.WindowMaximized)
            //{
            //    // ロード後に最大化
            //    Loaded += (o, e) => WindowState = WindowState.Maximized;
            //}
        }

    }
}