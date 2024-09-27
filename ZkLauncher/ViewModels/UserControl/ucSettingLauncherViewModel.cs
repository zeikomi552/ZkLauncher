using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucSettingLauncherViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware Overwrite

        public string Title
        {
            get { return "Setting"; }
        }
        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));
        public DialogCloseListener RequestClose { get; }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                this.Config.SaveXML();
                result = ButtonResult.OK;
            }
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

        }
        #endregion

        #region コンフィグデータ
        /// <summary>
        /// 
        /// </summary>
        ConfigManager<DisplayElemetCollection> _Config = new ConfigManager<DisplayElemetCollection>("Config", "Setting.conf", new DisplayElemetCollection());
        /// <summary>
        /// 
        /// </summary>
        public ConfigManager<DisplayElemetCollection> Config
        {
            get
            {
                return _Config;
            }
            set
            {
                if (_Config == null || !_Config.Equals(value))
                {
                    _Config = value;
                    RaisePropertyChanged("Config");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayElements">表示要素</param>
        public ucSettingLauncherViewModel()
        {
            // ファイルの存在確認
            if (!File.Exists(this.Config.ConfigFile))
            {
                this.Config.SaveXML();
            }
            else
            {
                this.Config.LoadXML();
            }
        }
        #endregion
    }
}
