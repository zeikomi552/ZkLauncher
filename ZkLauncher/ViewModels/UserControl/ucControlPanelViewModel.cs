using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucControlPanelViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware Overwrite

        public string Title
        {
            get { return "Control Panel"; }
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



        private IDialogService? _dialogService;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="displayElements"></param>
        public ucControlPanelViewModel(IDialogService dialogService, IDisplayEmentsCollection displayElements)
        {
            try
            {
                _dialogService = dialogService;
                this.DisplayElements = displayElements;
                this.DisplayElements.LoadConfig();
            }
            catch
            {

            }
        }


        public void ContextMenu_Regist()
        {
            try
            {
                //クリップボードに文字列データがあるか確認
                if (Clipboard.ContainsText())
                {
                    string text = Clipboard.GetText();

                    if (text.Contains("http://") || text.Contains("https://"))
                    {
                        this.DisplayElements!.Add(new DisplayElement() { Title = "リンク", URI = text });
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// リンクの削除処理
        /// </summary>
        public void ContextMenu_Delete()
        {
            try
            {
                if (this.DisplayElements != null && this.DisplayElements.SelectedItem != null)
                {
                    if (ShowMessage.ShowQuestionYesNo("選択しているリンクを削除します。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
                    {
                        // イメージファイルが存在する場合は削除
                        if (File.Exists(this.DisplayElements.SelectedItem.ImagePath))
                        {
                            File.Delete(this.DisplayElements.SelectedItem.ImagePath);
                        }
                        this.DisplayElements.Elements.Remove(this.DisplayElements.SelectedItem);
                        this.DisplayElements.SaveConfig();
                    }
                }
                else
                {
                    ShowMessage.ShowNoticeOK("削除対象が選択されていません。", "通知");
                }
            }
            catch
            {

            }
        }



        #region 選択要素の変更
        /// <summary>
        /// 選択要素の変更
        /// </summary>
        public void SelectionChanged()
        {
            try
            {
                this.DisplayElements!.SelecctedNavigate();
            }
            catch
            {

            }
        }
        #endregion
    }
}
