using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucViewerPanelViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private string? _message;
        public string? Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = "Notification";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

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
                        InitializeAsync(wnd);
                    }
                    catch
                    {
                        ShowMessage.ShowNoticeOK("WebView2ランタイムがインストールされていないようです。\r\nインストールしてください", "通知");
                        URLUtility.OpenUrl("https://developer.microsoft.com/en-us/microsoft-edge/webview2/");
                    }
                }

                //// 合成音声の初期化
                //this.Story.InitVoice();
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

                // 環境の作成
                var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, browserExecutableFolder);
                await wnd.WebView2Ctrl.EnsureCoreWebView2Async(webView2Environment);

                this.DisplayElements!.Elements.Add(new DisplayElement()
                {
                    ImagePath = "test",
                    URI = "https://github.com/zeikomi552/ZkLauncher",
                    WebView2Object = wnd.WebView2Ctrl
                });

                // 最初の要素を選択
                this.DisplayElements.SelectFirst();

                // 1つめのURLを表示
                this.DisplayElements.SelectedItem.Navigate();
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
