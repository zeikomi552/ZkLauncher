using Prism.Navigation.Regions;
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
    /// <summary>
    /// Viewerパネル用ビューモデル
    /// </summary>
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

        #region ナビゲートコマンド
        /// <summary>
        /// ナビゲートコマンド
        /// </summary>
        public DelegateCommand<string> NavigateCommand { get; private set; }
        #endregion

        #region コンテナマネージャー
        /// <summary>
        /// コンテナマネージャー
        /// </summary>
        IContainerExtension _container;
        #endregion

        #region 領域マネージャー
        /// <summary>
        /// 領域マネージャー
        /// </summary>
        public IRegionManager _regionManager { get; set; }
        #endregion

        #region スライドショー画面モード
        /// <summary>
        /// スライドショー画面モード
        /// </summary>
        bool _SlideshowF { get; set; } = true;
        #endregion

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

        #region URLへのナビゲート処理
        /// <summary>
        /// URLへのナビゲート処理
        /// </summary>
        /// <param name="navigatePath">ナビゲートパス（URL or ファイルパス(.pdfのみ)）</param>
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                if (this._SlideshowF)
                {
                    SavePage();
                    _regionManager.RequestNavigate("ViewerRegion", nameof(ucWhitebord));

                    var parameters = new NavigationParameters();
                    parameters.Add("SaveDirectory", this.DisplayElements!.DrawPictureSaveDirectoryPath);
                    _regionManager.RequestNavigate("CotrolPanelRegion", nameof(ucControlPanelForWhiteboard), parameters);
                }
                else
                {
                    _regionManager.RequestNavigate("ViewerRegion", nameof(ucSlideshow));
                    _regionManager.RequestNavigate("CotrolPanelRegion", nameof(ucControlPanelForSlideshow));
                }

                this._SlideshowF = !this._SlideshowF;
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {

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
                if (this.DisplayElements != null && this.DisplayElements.SelectedItem != null)
                {
                    // オブジェクトの取得
                    var ctrl = this.DisplayElements!.SelectedItem.WebView2Object!;

                    // 座標の取得
                    var targetPoint = ctrl.PointToScreen(new System.Windows.Point(0.0d, 0.0d));

                    var dirctorypath = DirectoryPathDictionary.ImageSaveDirectory;
                    // ファイルパスの取得
                    string file = GetfileName(dirctorypath);

                    // スクリーンショットの作成
                    ScreenShotM.ExecuteScreenShot(ctrl, file);

                    this.FilePath = file;
                }
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
            var file = "Temporary.jpg";
            return Path.Combine(folderPath, file);
        }
        #endregion

    }
}
