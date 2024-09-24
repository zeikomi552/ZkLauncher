﻿using DryIoc.ImTools;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        #region 配置
        /// <summary>
        /// 配置
        /// </summary>
        IWindowPositionConfig? _WindowPosition;
        /// <summary>
        /// 配置
        /// </summary>
        public IWindowPositionConfig? WindowPosition
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
        public MainWindowViewModel(IDialogService dialogService, IRegionManager regionManager, IDisplayEmentsCollection displayElements, IWindowPositionConfig widowpos)
        {
            _dialogService = dialogService;
            this.DisplayElements = displayElements;
            this.DisplayElements.LoadConfig();
            this.WindowPosition = widowpos;

            regionManager.RegisterViewWithRegion("ControlPanel", typeof(ucViewerPanel));
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow? wnd = sender as MainWindow;

                if (wnd != null)
                {
                    wnd.SaveWindowPosition = true;
                }



                ShowControlPanelDialog();
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
            var message = "This is a message that should be shown in the dialog.";
            //using the dialog service as-is
            _dialogService.Show("ucViewerPanel", new DialogParameters($"message={message}"), r =>
            {
                string test = string.Empty;
            });
        }
        #endregion

        #region Viewer表示画面の呼び出し
        private DelegateCommand? _showControlPanelCommand;
        public DelegateCommand? ShowControlPanelCommand =>
            _showControlPanelCommand ?? (_showViewerCommand = new DelegateCommand(ShowControlPanelDialog));
        /// <summary>
        /// Viewer表示画面の呼び出し
        /// </summary>
        private void ShowControlPanelDialog()
        {
            var message = "This is a message that should be shown in the dialog.";
            //using the dialog service as-is
            _dialogService.Show("ucControlPanel", new DialogParameters($"message={message}"), r =>
            {
                string test = string.Empty;
            });
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
            var message = "This is a message that should be shown in the dialog.";
            //using the dialog service as-is
            _dialogService.ShowDialog("ucSettingLauncher", new DialogParameters($"message={message}"), r =>
            {
                string test = string.Empty;
                //if (r.Result == ButtonResult.None)
                //    test = "Result is None";
                //else if (r.Result == ButtonResult.OK)
                //{
                //    this.DisplayElements!.LoadConfig();
                //}
                //else if (r.Result == ButtonResult.Cancel)
                //    test = "Result is Cancel";
                //else
                //    test = "I Don't know what you did!?";
            });
        }
        #endregion
    }
}
