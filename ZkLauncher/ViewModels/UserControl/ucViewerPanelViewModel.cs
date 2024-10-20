using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucViewerPanelViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware overwrite
        public string Title
        {
            get { return "Viewer"; }
        }

        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));


        public DialogCloseListener RequestClose { get; }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
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

        #region ウィンドウ位置
        /// <summary>
        /// ウィンドウ位置
        /// </summary>
        IWindowPostionCollection? _WindowPosition;
        /// <summary>
        /// ウィンドウ位置
        /// </summary>
        public IWindowPostionCollection? WindowPosition
        {
            get
            {
                return _WindowPosition;
            }
            set
            {
                if (_WindowPosition == null || !_WindowPosition.Equals(value))
                {
                    _WindowPosition = value;
                    RaisePropertyChanged("WindowPosition");
                }
            }
        }
        #endregion

        public DelegateCommand<string> NavigateCommand { get; private set; }


        IContainerExtension _container;

        public IRegionManager _regionManager { get; set; }

        bool _SlideshowF { get; set; } = true;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayElements">表示要素</param>
        /// <param name="windowPosition">ウィンドウ位置</param>
        public ucViewerPanelViewModel(IRegionManager regionManager,
            IContainerExtension container,
            IDisplayEmentsCollection displayElements,
            IWindowPostionCollection windowPosition)
        {
            _container = container;
            this._regionManager = regionManager;
            //_regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucViewerPanel));
            this.DisplayElements = displayElements;
            this.WindowPosition = windowPosition;


            NavigateCommand = new DelegateCommand<string>(Navigate);

        }
        #endregion
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                if (this._SlideshowF)
                    _regionManager.RequestNavigate("ViewerRegion", nameof(ucWhitebord));
                else
                    _regionManager.RequestNavigate("ViewerRegion", nameof(ucSlideshow));

                this._SlideshowF = !this._SlideshowF;

                //var region = _regionManager.Regions["ViewerRegion"];

                //var uc = _container.Resolve<ucWhitebord>();
                //region.Activate(uc);
            }
        }
        public void Init()
        {
            //this._regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucWhitebord));
            //this._regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucSlideshow));
            //this._regionManager.RequestNavigate("ViewerRegion", "ucWhitebord");
        }

        public void ChangeWindow()
        {
            //this._regionManager.RequestNavigate("ViewerRegion", "ucWhitebord");

            //this._regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucWhitebord));
        }
    }
}
