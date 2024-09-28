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

            RaiseRequestClose(new DialogResult(result));
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

        public ucViewerPanelViewModel(IDisplayEmentsCollection displayElements)
        {
            this.DisplayElements = displayElements;
        }

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
                        SetupTimer();

                        // ブラウザの初期化
                        InitializeAsync(wnd);
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

        #region キャッシュの保存先ディレクトリ
        /// <summary>
        /// キャッシュの保存先ディレクトリ
        /// </summary>
        private string _WebViewDir = "EBWebView";
        #endregion

        #region 初期化処理(WebView2の配布)
        /// <summary>
        /// 初期化処理(WebView2の配布)
        /// </summary>
        private async void InitializeAsync(ucViewerPanel wnd)
        {
            try
            {
                var browserExecutableFolder = Path.Combine(PathManager.GetApplicationFolder(), _WebViewDir);

                // カレントディレクトリの作成
                PathManager.CreateDirectory(browserExecutableFolder);

                this.DisplayElements!.WebView2Object = wnd.WebView2Ctrl;


                // 環境の作成
                var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, browserExecutableFolder);
                await this.DisplayElements!.WebView2Object.EnsureCoreWebView2Async(webView2Environment);


                // 最初の要素を選択
                this.DisplayElements!.SelectFirst();

                // 1つめのURLを表示
                this.DisplayElements.SelectedItem.Navigate(this.DisplayElements!.WebView2Object);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        // タイマのインスタンス
        private DispatcherTimer? _timer;

        // タイマを設定する
        private void SetupTimer()
        {
            // タイマのインスタンスを生成
            _timer = new DispatcherTimer(); // 優先度はDispatcherPriority.Background
                                            // インターバルを設定
            _timer.Interval = new TimeSpan(0, 0, 30);
            // タイマメソッドを設定
            _timer.Tick += new EventHandler(LoopExecute!);
        }

        #region タイマーの停止
        /// <summary>
        /// タイマーの停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopTimer(object sender, CancelEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }
        #endregion

        #region タイマーメソッド
        /// <summary>
        /// タイマーメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopExecute(object sender, EventArgs e)
        {
            try
            {
                this.DisplayElements!.NextNavigate();
            }
            catch
            {

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
                this.DisplayElements!.NextNavigate();
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
                this.DisplayElements!.PrevNavigate();
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
                if (_timer != null)
                {
                    // タイマを開始
                    _timer.Start();
                }
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
                if (_timer != null)
                {
                    // タイマを開始
                    _timer.Stop();
                }
            }
            catch
            {

            }
        }
        #endregion
    }
}
