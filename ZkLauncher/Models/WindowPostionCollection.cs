using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Models
{
    public class WindowPostionCollection : BindableBase, IWindowPostionCollection
    {
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
        const string ConfigFile = "WindowPos.conf";
        #endregion

        #region ビューワーの位置
        /// <summary>
        /// ビューワーの位置
        /// </summary>
        WindowPosition _ViewerPosition = new WindowPosition();
        /// <summary>
        /// ビューワーの位置
        /// </summary>
        public WindowPosition ViewerPosition
        {
            get
            {
                return _ViewerPosition;
            }
            set
            {
                if (_ViewerPosition == null || !_ViewerPosition.Equals(value))
                {
                    _ViewerPosition = value;
                    RaisePropertyChanged("ViewerPosition");
                }
            }
        }
        #endregion

        #region コントロールパネルの位置
        /// <summary>
        /// コントロールパネルの位置
        /// </summary>
        WindowPosition _ControlPanelPosition = new WindowPosition();
        /// <summary>
        /// コントロールパネルの位置
        /// </summary>
        public WindowPosition ControlPanelPosition
        {
            get
            {
                return _ControlPanelPosition;
            }
            set
            {
                if (_ControlPanelPosition == null || !_ControlPanelPosition.Equals(value))
                {
                    _ControlPanelPosition = value;
                    RaisePropertyChanged("ControlPanelPosition");
                }
            }
        }
        #endregion

        #region ファイルの存在確認
        /// <summary>
        /// ファイルの存在確認
        /// </summary>
        public bool FileExists
        {
            get
            {
                try
                {
                    ConfigManager<WindowPostionCollection> conf = new ConfigManager<WindowPostionCollection>(ConfigDir, ConfigFile, new WindowPostionCollection());

                    return conf.FileExist;
                }
                catch
                {
                    throw;
                }
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
                ConfigManager<WindowPostionCollection> conf = new ConfigManager<WindowPostionCollection>(ConfigDir, ConfigFile, new WindowPostionCollection());

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

        #region Configファイルの読み込み
        /// <summary>
        /// Configファイルの読み込み
        /// </summary>
        public void LoadConfig()
        {
            try
            {
                ConfigManager<WindowPostionCollection> conf = new ConfigManager<WindowPostionCollection>(ConfigDir, ConfigFile, new WindowPostionCollection());

                // ファイルの存在確認
                if (!File.Exists(conf.ConfigFile))
                {
                    conf.SaveXML(); // XMLのセーブ
                }
                else
                {
                    conf.LoadXML(); // XMLのロード
                }

                if (conf.Item != null)
                {
                    this.ViewerPosition = conf.Item.ViewerPosition;
                    this.ControlPanelPosition = conf.Item.ControlPanelPosition;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion


    }
}
