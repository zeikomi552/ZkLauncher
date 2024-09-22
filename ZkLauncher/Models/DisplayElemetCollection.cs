using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Models
{
    public class DisplayElemetCollection : BindableBase, IDisplayEmentsCollection
    {

        #region WebView2用オブジェクト
        /// <summary>
        /// WebView2用オブジェクト
        /// </summary>
        WebView2? _WebView2Object;
        /// <summary>
        /// WebView2用オブジェクト
        /// </summary>
        [XmlIgnore]
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

        #region 表示要素
        /// <summary>
        /// 表示要素
        /// </summary>
        ObservableCollection<DisplayElement> _Elements = new ObservableCollection<DisplayElement>();
        /// <summary>
        /// 表示要素
        /// </summary>
        public ObservableCollection<DisplayElement> Elements
        {
            get
            {
                return _Elements;
            }
            set
            {
                if (_Elements == null || !_Elements.Equals(value))
                {
                    _Elements = value;
                    RaisePropertyChanged("Elements");
                }
            }
        }
        #endregion

        #region 選択要素
        /// <summary>
        /// 選択要素
        /// </summary>
        DisplayElement _SelectedItem = new DisplayElement();
        /// <summary>
        /// 選択要素
        /// </summary>
        public DisplayElement SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem == null || !_SelectedItem.Equals(value))
                {
                    _SelectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }
        }
        #endregion

        /// <summary>
        /// 最初の値を取得する
        /// </summary>
        /// <returns>最初の値 存在しない場合はnull</returns>
        public DisplayElement? First()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                return this.Elements.First();
            }
            else
            {
                return null;
            }
        }

        public void SelectFirst()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                this.SelectedItem = this.Elements.First();
            }
        }

        public void SelectLast()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                this.SelectedItem = this.Elements.Last();
            }
        }

        public void SetElements(List<DisplayElement> elements)
        {
            this.Elements.Clear();
            foreach (var tmp in elements)
            {
                this.Elements.Add(tmp);
            }
        }

        #region ディレクトリ名
        /// <summary>
        /// ディレクトリ名
        /// </summary>
        const string ConfigDir = "Config";
        #endregion

        #region Configファイル名
        /// <summary>
        /// Configファイル名
        /// </summary>
        const string ConfigFile = "Setting.conf";
        #endregion

        #region Configファイルの読み込み
        /// <summary>
        /// Configファイルの読み込み
        /// </summary>
        public void LoadConfig()
        {
            try
            {
                ConfigManager<DisplayElemetCollection> conf = new ConfigManager<DisplayElemetCollection>(ConfigDir, ConfigFile, new DisplayElemetCollection());

                // ファイルの存在確認
                if (!File.Exists(conf.ConfigFile))
                {
                    conf.SaveXML(); // XMLのセーブ
                }
                else
                {
                    conf.LoadXML(); // XMLのロード
                }

                // 要素のセット
                SetElements(conf.Item!.Elements.ToList<DisplayElement>());
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Configファイルの保存処理
        /// <summary>
        /// Configファイルの保存処理
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                ConfigManager<DisplayElemetCollection> conf = new ConfigManager<DisplayElemetCollection>(ConfigDir, ConfigFile, new DisplayElemetCollection());

                // 値の受け渡し
                conf.Item = this;
                conf.SaveXML(); // XMLのセーブ
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
