using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Message = parameters.GetValue<string>("message");
        }
        #endregion

        private string? _message;
        public string? Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayElements">表示要素</param>
        /// <param name="windowPosition">ウィンドウ位置</param>
        public ucViewerPanelViewModel(IDisplayEmentsCollection displayElements,
            IWindowPostionCollection windowPosition, IRegionManager regionManager)
        {
            this.DisplayElements = displayElements;
            this.WindowPosition = windowPosition;
            //regionManager.RegisterViewWithRegion("ContentRegion", typeof(ucWebView2Container));
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Init(object sender, EventArgs e)
        {
            try
            {
                var wnd = sender as ucViewerPanel;
                if (wnd != null)
                {
                    try
                    {
                        // タイマーのセット
                        this.DisplayElements!.SetupTimer();
                    }
                    catch
                    {
                        ShowMessage.ShowNoticeOK("WebView2ランタイムがインストールされていないようです。\r\nインストールしてください", "通知");
                        URLUtility.OpenUrl("https://developer.microsoft.com/en-us/microsoft-edge/webview2/");
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 次へ画面(URL)遷移
        /// <summary>
        /// 次へ画面(URL)遷移
        /// </summary>
        public void Next()
        {
            try
            {
                this.DisplayElements?.NextNavigate();
            }
            catch
            {
                
            }
        }
        #endregion

        #region 直前へ画面(URL)遷移
        /// <summary>
        /// 直前へ画面(URL)遷移
        /// </summary>
        public void Prev()
        {
            try
            {
                this.DisplayElements?.PrevNavigate();

            }
            catch
            {

            }
        }
        #endregion

        #region ループフラグ
        /// <summary>
        /// ループフラグ
        /// </summary>
        bool _LoopF = false;
        /// <summary>
        /// ループフラグ
        /// </summary>
        public bool LoopF
        {
            get
            {
                return _LoopF;
            }
            set
            {
                if (!_LoopF.Equals(value))
                {
                    _LoopF = value;
                    RaisePropertyChanged("LoopF");
                }
            }
        }
        #endregion

        #region ループ処理の開始
        /// <summary>
        /// ループ処理の開始
        /// </summary>
        public void Loop()
        {
            try
            {
                this.DisplayElements?.StartTimer();
            }
            catch
            {

            }
        }
        #endregion

        #region ループの一時停止
        /// <summary>
        /// ループの一時停止
        /// </summary>
        public void Pose()
        {
            try
            {
                this.DisplayElements?.StopTimer();
            }
            catch
            {

            }
        }
        #endregion

        #region 背景の保存先ディレクトリ
        /// <summary>
        /// 背景の保存先ディレクトリ
        /// </summary>
        private string BackgroundDirectory
        {
            get
            {
                // アプリケーションフォルダの取得
                var dir = Path.Combine(PathManager.GetApplicationFolder(), "Config", "Background");
                PathManager.CreateDirectory(dir);

                return dir;
            }
        }
        #endregion

        #region 背景の登録
        /// <summary>
        /// 背景の登録
        /// </summary>
        public void RegistBackground()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new Microsoft.Win32.OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "メディアファイル (*.mp4;*.wav;*.jpg;*.png)|*.mp4;*.wav;*.jpg;*.png|全てのファイル (*.*)|*.*";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var path = Path.Combine(BackgroundDirectory, "Viewer" + Path.GetExtension(dialog.FileName));

                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                    File.Copy(dialog.FileName, path, true);
                    this.DisplayElements!.ViewerBackgroundMediaPath = path;
                    this.DisplayElements.SaveConfig();
                    this.DisplayElements.LoadConfig();
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        public void Reload()
        {
            try
            {
                this.DisplayElements?.ReloadURL();
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
    }
}
