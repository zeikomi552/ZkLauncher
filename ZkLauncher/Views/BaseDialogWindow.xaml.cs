using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZkLauncher.Views
{
    /// <summary>
    /// BaseDialogWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BaseDialogWindow : Window, IDialogWindow
    {
        public IDialogResult? Result { get; set; }
        public BaseDialogWindow()
        {
            this.Owner = Application.Current.MainWindow;
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
            settings.ControlPanelWindowHeight = this.Height;
            settings.ControlPanelWindowWidth = this.Width;
            settings.ControlPanelWindowTop = this.Top;
            settings.ControlPanelWindowLeft = this.Left;
            settings.Save();
        }

        /// <summary>
        /// ウィンドウの位置・サイズを復元します。
        /// </summary>
        void RecoverWindowBounds()
        {
            var settings = Properties.Settings.Default;
            // 左
            if (settings.ControlPanelWindowLeft >= 0 &&
                (settings.ControlPanelWindowLeft + settings.ControlPanelWindowWidth) < SystemParameters.VirtualScreenWidth)
            { Left = settings.ControlPanelWindowLeft; }
            // 上
            if (settings.ControlPanelWindowTop >= 0 &&
                (settings.ControlPanelWindowTop + settings.ControlPanelWindowHeight) < SystemParameters.VirtualScreenHeight)
            { Top = settings.ControlPanelWindowTop; }
            // 幅
            if (settings.ControlPanelWindowWidth > 0 &&
                settings.ControlPanelWindowWidth <= SystemParameters.WorkArea.Width)
            { Width = settings.ControlPanelWindowWidth; }
            // 高さ
            if (settings.ControlPanelWindowHeight > 0 &&
                settings.ControlPanelWindowHeight <= SystemParameters.WorkArea.Height)
            { Height = settings.ControlPanelWindowHeight; }
            //// 最大化
            //if (settings.WindowMaximized)
            //{
            //    // ロード後に最大化
            //    Loaded += (o, e) => WindowState = WindowState.Maximized;
            //}
        }
    }
}
