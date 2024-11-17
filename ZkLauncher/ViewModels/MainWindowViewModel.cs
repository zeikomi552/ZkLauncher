using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 表示要素
        /// <summary>
        /// 表示要素
        /// </summary>
        IDisplayEmentsCollection? _DisplayElements;
        /// <summary>
        /// 表示要素
        /// </summary>
        public IDisplayEmentsCollection? DisplayElements
        {
            get
            {
                return _DisplayElements;
            }
            set
            {
                if (_DisplayElements == null || !_DisplayElements.Equals(value))
                {
                    _DisplayElements = value;
                    RaisePropertyChanged("DisplayElements");
                }
            }
        }
        #endregion

        #region ウィンドウ位置
        /// <summary>
        /// ウィンドウ位置
        /// </summary>
        IWindowPostionCollection? _WindowPosition;
        /// <summary>
        /// ウィンドウ位置
        /// </summary>
        public IWindowPostionCollection? WindowPosition
        {
            get
            {
                return _WindowPosition;
            }
            set
            {
                if (_WindowPosition == null || !_WindowPosition.Equals(value))
                {
                    _WindowPosition = value;
                    RaisePropertyChanged("WindowPosition");
                }
            }
        }
        #endregion

        private IDialogService? _dialogService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager">RegionManager</param>
        public MainWindowViewModel(IDialogService dialogService, IRegionManager regionManager,
            IDisplayEmentsCollection displayElements, IWindowPostionCollection windowPosition)
        {
            try
            {
                _dialogService = dialogService;


                this.DisplayElements = displayElements;

                if (!this.DisplayElements.FileExists)
                {
                    this.DisplayElements.Add("https://www.yahoo.co.jp/");
                    this.DisplayElements.Add("https://www.google.co.jp/");
                    this.DisplayElements.SaveConfig();
                }
                else
                {
                    this.DisplayElements.LoadConfig();
                }

                // ウィンドウ位置情報の復帰
                this.WindowPosition = windowPosition;

                // ファイルの存在確認
                if (!this.WindowPosition.FileExists)
                {
                    // 画面サイズの調整
                    AjustPositionRight();

                    // 位置情報を保存
                    this.WindowPosition.SaveConfig();
                }
                else
                {
                    // ウィンドウ情報のロード
                    this.WindowPosition.LoadConfig();
                }
            }
            catch(Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DisplayElements?.SelectFirst();
            }
            catch
            {

            }
        }

        #region Viewer表示画面の呼び出し
        private DelegateCommand? _showViewerCommand;
        public DelegateCommand? ShowViewerCommand =>
            _showViewerCommand ?? (_showViewerCommand = new DelegateCommand(ShowViewerDialog));
        /// <summary>
        /// Viewer表示画面の呼び出し
        /// </summary>
        private void ShowViewerDialog()
        {
            try
            {
                _dialogService.Show("ucViewerPanel", new DialogParameters(), r =>
                {
                    string test = string.Empty;
                }, "Viewer");
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ControlPanel表示画面の呼び出し
        private DelegateCommand? _showControlPanelCommand;
        public DelegateCommand? ShowControlPanelCommand =>
            _showControlPanelCommand ?? (_showControlPanelCommand = new DelegateCommand(ShowControlPanelDialog));
        /// <summary>
        ///ControlPanel画面の呼び出し
        /// </summary>
        private void ShowControlPanelDialog()
        {
            try
            {
                _dialogService.Show("ucControlPanel", new DialogParameters(/*$"message={message}"*/), r =>
                {
                    string test = string.Empty;
                }, "ControlPanel");
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ランチャー設定画面の呼び出し
        private DelegateCommand? _showSettingLauncherCommand;
        public DelegateCommand? ShowSettingLauncherCommand =>
            _showSettingLauncherCommand ?? (_showSettingLauncherCommand = new DelegateCommand(ShowSettingLauncher));

        /// <summary>
        /// ランチャー設定画面の呼び出し
        /// </summary>
        private void ShowSettingLauncher()
        {
            try
            {
                _dialogService.Show("ucSettingLauncher", new DialogParameters(/*$"message={message}"*/), r =>
                {
                    string test = string.Empty;
                    if (r.Result == ButtonResult.OK)
                    {
                        this.DisplayElements!.LoadConfig();
                    }
                }, "Setting");
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
        #endregion

        #region アプリケーション終了コマンド
        private DelegateCommand? _AppShutdownCommand;
        public DelegateCommand? AppShutdownCommand =>
            _AppShutdownCommand ?? (_AppShutdownCommand = new DelegateCommand(AppShutdown));

        /// <summary>
        /// アプリケーションの終了
        /// </summary>
        private void AppShutdown()
        {
            try
            {
                // スレッドセーフの呼び出し
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        Environment.Exit(0); // 正常終了
                    }));
            }
            catch(Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
        #endregion

        #region 位置保存
        private DelegateCommand? _SavePositionCommand;
        public DelegateCommand? SavePositionCommand =>
            _SavePositionCommand ?? (_SavePositionCommand = new DelegateCommand(SavePosition));

        /// <summary>
        /// 位置調整
        /// </summary>
        private void SavePosition()
        {
            try
            {
                // 位置情報の再読み込み
                WindowPosition!.SaveConfig();
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
        #endregion

        #region 位置読み込み
        private DelegateCommand? _LoadPositionCommand;
        public DelegateCommand? LoadPositionCommand =>
            _LoadPositionCommand ?? (_LoadPositionCommand = new DelegateCommand(LoadPosition));

        /// <summary>
        /// 位置調整
        /// </summary>
        private void LoadPosition()
        {
            try
            {
                // 位置情報の再読み込み
                WindowPosition!.LoadConfig();
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
        #endregion


        #region 位置調整(右)コマンド
        private DelegateCommand? _AjustPositionRightCommand;
        public DelegateCommand? AjustPositionRightCommand =>
            _AjustPositionRightCommand ?? (_AjustPositionRightCommand = new DelegateCommand(AjustPositionRight));

        /// <summary>
        /// 位置調整(右)
        /// </summary>
        private void AjustPositionRight()
        {
            try
            {
                if (System.Windows.Forms.Screen.PrimaryScreen != null)
                {
                    //ディスプレイの高さ
                    int h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    //ディスプレイの幅
                    int w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

                    if(this.WindowPosition != null)
                    {
                        double controlpanel_w = 360;
                        double diff = 15;


                        this.WindowPosition.ViewerPosition.Left = 0;
                        this.WindowPosition.ViewerPosition.Top = 0;
                        this.WindowPosition.ViewerPosition.Width = w - (controlpanel_w - diff);
                        this.WindowPosition.ViewerPosition.Height = h;

                        this.WindowPosition.ControlPanelPosition.Left = w - controlpanel_w;
                        this.WindowPosition.ControlPanelPosition.Top = 0;
                        this.WindowPosition.ControlPanelPosition.Width = controlpanel_w;
                        this.WindowPosition.ControlPanelPosition.Height = h;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 位置調整(左)コマンド
        private DelegateCommand? _AjustPositionLeftCommand;
        public DelegateCommand? AjustPositionLeftCommand =>
            _AjustPositionLeftCommand ?? (_AjustPositionLeftCommand = new DelegateCommand(AjustPositionLeft));

        /// <summary>
        /// 位置調整(左)
        /// </summary>
        private void AjustPositionLeft()
        {
            try
            {
                if (System.Windows.Forms.Screen.PrimaryScreen != null)
                {
                    //ディスプレイの高さ
                    int h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    //ディスプレイの幅
                    int w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

                    if (this.WindowPosition != null)
                    {
                        double controlpanel_w = 360;
                        double diff = 15;


                        this.WindowPosition.ViewerPosition.Left = controlpanel_w - diff;
                        this.WindowPosition.ViewerPosition.Top = 0;
                        this.WindowPosition.ViewerPosition.Width = w - controlpanel_w + diff;
                        this.WindowPosition.ViewerPosition.Height = h;

                        this.WindowPosition.ControlPanelPosition.Left = 0;
                        this.WindowPosition.ControlPanelPosition.Top = 0;
                        this.WindowPosition.ControlPanelPosition.Width = controlpanel_w;
                        this.WindowPosition.ControlPanelPosition.Height = h;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 位置調整（下）コマンド
        private DelegateCommand? _AjustPositionBottomCommand;
        public DelegateCommand? AjustPositionBottomCommand =>
            _AjustPositionBottomCommand ?? (_AjustPositionBottomCommand = new DelegateCommand(AjustPositionBottom));

        /// <summary>
        /// 位置調整（下）
        /// </summary>
        private void AjustPositionBottom()
        {
            try
            {
                if (System.Windows.Forms.Screen.PrimaryScreen != null)
                {
                    //ディスプレイの高さ
                    int h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    //ディスプレイの幅
                    int w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

                    if (this.WindowPosition != null)
                    {
                        double controlpanel_h = 300;
                        double diff = 15;

                        this.WindowPosition.ViewerPosition.Left = 0;
                        this.WindowPosition.ViewerPosition.Top = 0;
                        this.WindowPosition.ViewerPosition.Width = w;
                        this.WindowPosition.ViewerPosition.Height = h - (controlpanel_h - diff);

                        this.WindowPosition.ControlPanelPosition.Left = 0;
                        this.WindowPosition.ControlPanelPosition.Top = h - controlpanel_h;
                        this.WindowPosition.ControlPanelPosition.Width = w;
                        this.WindowPosition.ControlPanelPosition.Height = controlpanel_h;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 位置調整（下）コマンド
        private DelegateCommand? _AjustPositionTopCommand;
        public DelegateCommand? AjustPositionTopCommand =>
            _AjustPositionTopCommand ?? (_AjustPositionTopCommand = new DelegateCommand(AjustPositionTop));

        /// <summary>
        /// 位置調整（下）
        /// </summary>
        private void AjustPositionTop()
        {
            try
            {
                if (System.Windows.Forms.Screen.PrimaryScreen != null)
                {
                    //ディスプレイの高さ
                    int h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    //ディスプレイの幅
                    int w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

                    if (this.WindowPosition != null)
                    {
                        double controlpanel_h = 300;
                        double diff = 15;

                        this.WindowPosition.ViewerPosition.Left = 0;
                        this.WindowPosition.ViewerPosition.Top = controlpanel_h - diff/2;
                        this.WindowPosition.ViewerPosition.Width = w;
                        this.WindowPosition.ViewerPosition.Height = h - controlpanel_h;

                        this.WindowPosition.ControlPanelPosition.Left = 0;
                        this.WindowPosition.ControlPanelPosition.Top = 0;
                        this.WindowPosition.ControlPanelPosition.Width = w;
                        this.WindowPosition.ControlPanelPosition.Height = controlpanel_h;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
