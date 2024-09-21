using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.Models
{
    public class WebView2Module
    {


        #region キャッシュの保存先ディレクトリ
        /// <summary>
        /// キャッシュの保存先ディレクトリ
        /// </summary>
        private string _WebViewDir = "EBWebView";
        #endregion

        public WebView2? _WebView2Obj { get; private set; }


        public WebView2Module(WebView2 core)
        {
            _WebView2Obj = core;
        }

        #region 初期化処理(WebView2の配布)
        /// <summary>
        /// 初期化処理(WebView2の配布)
        /// </summary>
        private async void InitializeAsync()
        {
            try
            {
                if (_WebView2Obj != null)
                {
                    var browserExecutableFolder = Path.Combine(PathManager.GetApplicationFolder(), _WebViewDir);

                    // カレントディレクトリの作成
                    PathManager.CreateDirectory(browserExecutableFolder);

                    // 環境の作成
                    var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, browserExecutableFolder);
                    await _WebView2Obj.EnsureCoreWebView2Async(webView2Environment);
                }


                //this.DisplayElements!.Elements.Add(new DisplayElement() { ImagePath = "test", URI = "https://github.com/zeikomi552/ZkLauncher" });

                //// 最初の要素を選択
                //this.DisplayElements.SelectFirst();

                //if (this.DisplayElements != null && this.DisplayElements.Elements.Count > 0)
                //{
                //    this.DisplayElements.Elements.ElementAt(0).Navigate(wnd.WebView2Ctrl);
                //}
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
