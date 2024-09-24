using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Models
{
    public class WindowPositionConfig : BindableBase, IWindowPositionConfig
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
        const string ConfigFile = "WindowPosition.conf";
        #endregion

        #region ビュワー位置
        /// <summary>
        /// ビュワー位置
        /// </summary>
        WindowPosition _ViewerPosition = new WindowPosition();
        /// <summary>
        /// ビュワー位置
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

        #region コントロールパネル位置
        /// <summary>
        /// コントロールパネル位置
        /// </summary>
        WindowPosition _ControlPanelPosition = new WindowPosition();
        /// <summary>
        /// コントロールパネル位置
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


        #region Configファイルの保存処理
        /// <summary>
        /// Configファイルの保存処理
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                ConfigManager<WindowPositionConfig> conf = new ConfigManager<WindowPositionConfig>(ConfigDir, ConfigFile, new WindowPositionConfig());

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
                ConfigManager<WindowPositionConfig> conf = new ConfigManager<WindowPositionConfig>(ConfigDir, ConfigFile, new WindowPositionConfig());

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
                SetElements(conf.Item!);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 表示要素のセット
        /// <summary>
        /// 表示要素のセット
        /// </summary>
        /// <param name="elements">表示要素</param>
        public void SetElements(WindowPositionConfig conf)
        {
            this.ViewerPosition = conf.ViewerPosition;
            this.ControlPanelPosition = conf.ControlPanelPosition;

        }
        #endregion
    }
}
