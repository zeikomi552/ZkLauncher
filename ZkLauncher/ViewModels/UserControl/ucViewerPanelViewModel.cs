﻿using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucViewerPanelViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware overwrite
        public string Title
        {
            get { return "Viewer"; }
        }

        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));


        public DialogCloseListener RequestClose { get; }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new Prism.Dialogs.DialogResult(result));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
        #endregion

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

        public DelegateCommand<string> NavigateCommand { get; private set; }


        IContainerExtension _container;

        public IRegionManager _regionManager { get; set; }

        bool _SlideshowF { get; set; } = true;

        #region 画像保存先ファイルパス
        /// <summary>
        /// 画像保存先ファイルパス
        /// </summary>
        string _FilePath = string.Empty;
        /// <summary>
        /// 画像保存先ファイルパス
        /// </summary>
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                if (_FilePath == null || !_FilePath.Equals(value))
                {
                    _FilePath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayElements">表示要素</param>
        /// <param name="windowPosition">ウィンドウ位置</param>
        public ucViewerPanelViewModel(IRegionManager regionManager,
            IContainerExtension container,
            IDisplayEmentsCollection displayElements,
            IWindowPostionCollection windowPosition)
        {
            _container = container;
            this._regionManager = regionManager;
            //_regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucViewerPanel));
            this.DisplayElements = displayElements;
            this.WindowPosition = windowPosition;


            NavigateCommand = new DelegateCommand<string>(Navigate);

        }
        #endregion
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                if (this._SlideshowF)
                {
                    SavePage();
                    var parameters = new NavigationParameters();
                    parameters.Add("filepath", this.FilePath);
                    _regionManager.RequestNavigate("ViewerRegion", nameof(ucWhitebord), parameters);
                }
                else
                {
                    _regionManager.RequestNavigate("ViewerRegion", nameof(ucSlideshow));

                }

                this._SlideshowF = !this._SlideshowF;
            }
        }
        public void Init()
        {
            //this._regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucWhitebord));
            //this._regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucSlideshow));
            //this._regionManager.RequestNavigate("ViewerRegion", "ucWhitebord");
        }

        #region 背景の保存先ディレクトリ
        /// <summary>
        /// 背景の保存先ディレクトリ
        /// </summary>
        private string ImageSaveDirectory
        {
            get
            {
                // アプリケーションフォルダの取得
                var dir = Path.Combine(PathManager.GetApplicationFolder(), "SaveImage");
                PathManager.CreateDirectory(dir);

                return dir;
            }
        }
        #endregion

        #region ページの保存処理
        /// <summary>
        /// ページの保存処理
        /// </summary>
        public void SavePage()
        {
            try
            {
                var ctrl = this.DisplayElements!.SelectedItem.WebView2Object!;
                var targetPoint = ctrl.PointToScreen(new System.Windows.Point(0.0d, 0.0d));

                // キャプチャ領域の生成
                var targetRect = new Rect(targetPoint.X, targetPoint.Y, ctrl.ActualWidth, ctrl.ActualHeight);

                // ファイルパスの取得
                string file = GetfileName(ImageSaveDirectory);

                //// スクリーンショット実行
                ExcuteScreenShot(targetRect, file);

                this.FilePath = file;

            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ファイルパスの作成処理
        /// <summary>
        /// ファイルパスの作成処理
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <returns>ファイルパス</returns>
        private string GetfileName(string folderPath)
        {
            var dtNow = DateTime.Now;
            var file = "Temporary.jpg";
            return Path.Combine(folderPath, file);
        }
        #endregion

        #region スクリーンショットの作成処理
        /// <summary>
        /// スクリーンショットの作成処理
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <param name="fileName">ファイル名</param>
        private void ExcuteScreenShot(Rect rect, string fileName)
        {
            // 矩形と同じサイズのBitmapを作成
            using (var bitmap = new Bitmap((int)rect.Width, (int)rect.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                // 画面から指定された矩形と同じ条件でコピー
                graphics.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bitmap.Size);

                // 画像ファイルとして保存
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        #endregion
    }
}
