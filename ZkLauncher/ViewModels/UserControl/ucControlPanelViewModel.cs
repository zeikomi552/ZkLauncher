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
    public class ucControlPanelViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware Overwrite
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



        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
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
