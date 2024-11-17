using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucNameChangeViewModel : BindableBase, IDialogAware
    {

        #region IDialogAware Overwrite

        public string Title
        {
            get { return "Name Change"; }
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
                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new Prism.Dialogs.DialogResult(result));
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
            try
            {
                this.AfterName = this.BeforeName = parameters.GetValue<string>("CurrentName");
            }
            catch { }
        }
        #endregion

        #region 変更前の名称
        /// <summary>
        /// 変更前の名称
        /// </summary>
        string _BeforeName = string.Empty;
        /// <summary>
        /// 変更前の名称
        /// </summary>
        public string BeforeName
        {
            get
            {
                return _BeforeName;
            }
            set
            {
                if (_BeforeName == null || !_BeforeName.Equals(value))
                {
                    _BeforeName = value;
                    RaisePropertyChanged("BeforeName");
                }
            }
        }
        #endregion

        #region 変更後の名称
        /// <summary>
        /// 変更後の名称
        /// </summary>
        string _AfterName = string.Empty;
        /// <summary>
        /// 変更後の名称
        /// </summary>
        public string AfterName
        {
            get
            {
                return _AfterName;
            }
            set
            {
                if (_AfterName == null || !_AfterName.Equals(value))
                {
                    _AfterName = value;
                    RaisePropertyChanged("AfterName");
                }
            }
        }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayElements">表示要素</param>
        public ucNameChangeViewModel()
        {

        }
        #endregion

        /// <summary>
        /// OKコマンド
        /// </summary>
        public DelegateCommand OkCommand => new DelegateCommand(() =>
        {
            // ダイアログの結果オブジェクトを作成
            var result = new Prism.Dialogs.DialogResult(ButtonResult.OK);

            // ダイアログのOKボタンが押下された際の処理(戻り値のセット)
            result.Parameters.Add("AfterName", this.AfterName);

            // ダイアログを閉じる
            RequestClose.Invoke(result);
        });


        /// <summary>
        /// Cancelコマンド
        /// </summary>
        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            // ダイアログの結果オブジェクトを作成
            var result = new Prism.Dialogs.DialogResult(ButtonResult.Cancel);

            // ダイアログを閉じる
            RequestClose.Invoke(result);
        });
    }
}
