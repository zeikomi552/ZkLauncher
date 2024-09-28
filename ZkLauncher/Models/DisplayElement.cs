using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZkLauncher.Common.Utilities;

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
                string dir = PathManager.GetApplicationFolder();
                return Path.Combine(dir, "Config", "Images", FileName);
            }
        }
        #endregion

        #region ファイル名
        /// <summary>
        /// ファイル名
        /// </summary>
        string _FileName = string.Empty;
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (_FileName == null || !_FileName.Equals(value))
                {
                    _FileName = value;
                    RaisePropertyChanged("FileName");
                    RaisePropertyChanged("ImagePath");
                }
            }
        }
        #endregion

        #region 画面遷移
        /// <summary>
        ///  画面遷移
        /// </summary>
        /// <param name="wv2">WebView2コントロール</param>
        public void Navigate(WebView2 web, params object?[] param)
        {
            // nullチェック
            if (web != null && web.CoreWebView2 != null)
            {
                // URLを開く
                web.CoreWebView2.Navigate(string.Format(this.URI, param));
            }
        }
        #endregion

        #region ファイル選択ダイアログの表示
        /// <summary>
        /// ファイル選択ダイアログの表示
        /// </summary>
        public void SelectedFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.FileName = Path.GetFileName(dialog.FileName);

                    PathManager.CreateCurrentDirectory(this.ImagePath);
                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                    File.Copy(dialog.FileName, this.ImagePath, true);
                }
            }
            catch { }
        }
        #endregion
    }
}
