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
            _dialogService = dialogService;
            this.DisplayElements = displayElements;
            this.DisplayElements.LoadConfig();

            this.WindowPosition = windowPosition;
            this.WindowPosition.LoadConfig();
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
                _dialogService.ShowDialog("ucSettingLauncher", new DialogParameters(/*$"message={message}"*/), r =>
                {
                    string test = string.Empty;
                    if (r.Result == ButtonResult.OK)
                    {
                        this.DisplayElements!.LoadConfig();
                    }
                });
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
                        if (this.WindowPosition != null)
                        {
                            this.WindowPosition.SaveConfig();
                        }
                        Environment.Exit(0); // 正常終了
                    }));
            }
            catch(Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
        #endregion

        #region 位置調整コマンド
        private DelegateCommand? _AjustPositionCommand;
        public DelegateCommand? AjustPositionCommand =>
            _AjustPositionCommand ?? (_AjustPositionCommand = new DelegateCommand(AjustPosition));

        /// <summary>
        /// 位置調整
        /// </summary>
        private void AjustPosition()
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
    }
}
