using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucWebView2ContainerViewModel : BindableBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ucWebView2ContainerViewModel()
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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Init(object sender, RoutedEventArgs e)
        {
            try
            {
                var wnd = sender as ucWebView2Container;
                if (wnd != null)
                {
                    try
                    {
                        //// ブラウザの初期化
                        //InitializeAsync(wnd);
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

        //#region キャッシュの保存先ディレクトリ
        ///// <summary>
        ///// キャッシュの保存先ディレクトリ
        ///// </summary>
        //private string _WebViewDir = "EBWebView";
        //#endregion

        //#region 初期化処理(WebView2の配布)
        ///// <summary>
        ///// 初期化処理(WebView2の配布)
        ///// </summary>
        //private async void InitializeAsync(ucWebView2Container wnd)
        //{
        //    try
        //    {
        //        //var browserExecutableFolder = Path.Combine(PathManager.GetApplicationFolder(), _WebViewDir);

        //        //// カレントディレクトリの作成
        //        //PathManager.CreateDirectory(browserExecutableFolder);

        //        //this.DisplayElements!.WebView2Object = wnd.WebView2Ctrl;


        //        //// 環境の作成
        //        //var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, browserExecutableFolder);
        //        //await this.DisplayElements!.WebView2Object.EnsureCoreWebView2Async(webView2Environment);


        //        //// 最初の要素を選択
        //        //this.DisplayElements!.SelectFirst();

        //        //// 1つめのURLを表示
        //        //this.DisplayElements.SelectedItem.Navigate(this.DisplayElements!.WebView2Object);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage.ShowErrorOK(ex.Message, "Error");
        //    }
        //}
        //#endregion
    }
}
