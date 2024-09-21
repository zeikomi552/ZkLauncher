using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class DisplayElement : BindableBase
    {
        #region タイトル
        /// <summary>
        /// タイトル
        /// </summary>
        String _Title = string.Empty;
        /// <summary>
        /// タイトル
        /// </summary>
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title == null || !_Title.Equals(value))
                {
                    _Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
        #endregion


        #region URI
        /// <summary>
        /// URI
        /// </summary>
        string _URI = string.Empty;
        /// <summary>
        /// URI
        /// </summary>
        public string URI
        {
            get
            {
                return _URI;
            }
            set
            {
                if (_URI == null || !_URI.Equals(value))
                {
                    _URI = value;
                    RaisePropertyChanged("URI");
                }
            }
        }
        #endregion

        #region イメージファイルパス
        /// <summary>
        /// イメージファイルパス
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// イメージファイルパス
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                if (_ImagePath == null || !_ImagePath.Equals(value))
                {
                    _ImagePath = value;
                    RaisePropertyChanged("ImagePath");
                }
            }
        }
        #endregion

        #region WebView2用オブジェクト
        /// <summary>
        /// WebView2用オブジェクト
        /// </summary>
        WebView2? _WebView2Object;
        /// <summary>
        /// WebView2用オブジェクト
        /// </summary>
        public WebView2? WebView2Object
        {
            get
            {
                return _WebView2Object;
            }
            set
            {
                if (_WebView2Object == null || !_WebView2Object.Equals(value))
                {
                    _WebView2Object = value;
                }
            }
        }
        #endregion



        #region フレーズの翻訳
        /// <summary>
        /// フレーズの翻訳
        /// </summary>
        /// <param name="wv2">WebView2コントロール</param>
        public void Navigate(params object?[] param)
        {
            // nullチェック
            if (WebView2Object != null && WebView2Object.CoreWebView2 != null)
            {
                // URLを開く
                WebView2Object.CoreWebView2.Navigate(string.Format(this.URI, param));
            }
        }
        #endregion
    }
}
