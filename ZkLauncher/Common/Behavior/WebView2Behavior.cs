using Microsoft.Web.WebView2.Wpf;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZkLauncher.Common.Helper;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Common.Behavior
{
    #region WebView2用のビヘイビア
    public class WebView2Behavior : Behavior<WebView2>
    {
        public static readonly DependencyProperty WebView2CtlProperty
            = DependencyProperty.Register("WebView2Ctl", typeof(WebView2), typeof(WebView2Behavior), null);

        public WebView2 WebView2Ctl
        {
            get { return (WebView2)this.GetValue(WebView2CtlProperty); }
            set { this.SetValue(WebView2CtlProperty, value); }
        }

        public string URL
        {
            get { return (string)this.GetValue(URLProperty); }
            set { this.SetValue(URLProperty, value); }
        }

        public static readonly DependencyProperty URLProperty =
            DependencyProperty.RegisterAttached("URL", typeof(string), typeof(WebView2Behavior), new PropertyMetadata(string.Empty, URLChanged));

        private static void URLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var url = d.GetValue(URLProperty).ToString();
            var web2 = d.GetValue(WebView2CtlProperty) as WebView2;


            if (web2 != null && !string.IsNullOrEmpty(url) )
            {

                web2.CoreWebView2.Navigate(url);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += OnWebView2CtlNotify;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= OnWebView2CtlNotify;
        }

        public void OnWebView2CtlNotify(object sender, EventArgs e)
        {
            var tmp = sender as WebView2;

            if (tmp != null)
            {
                this.WebView2Ctl = tmp;
                InitializeAsync();
            }

            // ここに処理を書く
        }


        #region キャッシュの保存先ディレクトリ
        /// <summary>
        /// キャッシュの保存先ディレクトリ
        /// </summary>
        private string _WebViewDir = "EBWebView";
        #endregion

        private bool _IsInitialized = false;
        #region 初期化処理(WebView2の配布)
        /// <summary>
        /// 初期化処理(WebView2の配布)
        /// </summary>
        private async void InitializeAsync()
        {
            try
            {
                if (_IsInitialized == true)
                    return;


                var browserExecutableFolder = Path.Combine(PathManager.GetApplicationFolder(), _WebViewDir);

                // カレントディレクトリの作成
                PathManager.CreateDirectory(browserExecutableFolder);

                // 環境の作成
                var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, browserExecutableFolder);
                await this.WebView2Ctl.EnsureCoreWebView2Async(webView2Environment);

                _IsInitialized = true;

                if (!string.IsNullOrEmpty(this.URL))
                {
                    this.WebView2Ctl.CoreWebView2.Navigate(this.URL);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

    }
    #endregion
}
