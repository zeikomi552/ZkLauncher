﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucControlPanelViewModel : BindableBase
    {
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

        private DelegateCommand? _showViewerCommand;
        public DelegateCommand? ShowDialogCommand =>
            _showViewerCommand ?? (_showViewerCommand = new DelegateCommand(ShowViewerDialog));

        private void ShowViewerDialog()
        {
            var message = "This is a message that should be shown in the dialog.";
            //using the dialog service as-is
            _dialogService.Show("ucViewerPanel", new DialogParameters($"message={message}"), r =>
            {
                if (r.Result == ButtonResult.None)
                    Title = "Result is None";
                else if (r.Result == ButtonResult.OK)
                    Title = "Result is OK";
                else if (r.Result == ButtonResult.Cancel)
                    Title = "Result is Cancel";
                else
                    Title = "I Don't know what you did!?";
            });
        }

        private DelegateCommand? _showSettingLauncherCommand;
        public DelegateCommand? ShowSettingLauncherCommand =>
            _showSettingLauncherCommand ?? (_showSettingLauncherCommand = new DelegateCommand(ShowSettingLauncher));

        private void ShowSettingLauncher()
        {
            var message = "This is a message that should be shown in the dialog.";
            //using the dialog service as-is
            _dialogService.ShowDialog("ucSettingLauncher", new DialogParameters($"message={message}"), r =>
            {
                if (r.Result == ButtonResult.None)
                    Title = "Result is None";
                else if (r.Result == ButtonResult.OK)
                {
                    this.DisplayElements!.LoadConfig();
                }
                else if (r.Result == ButtonResult.Cancel)
                    Title = "Result is Cancel";
                else
                    Title = "I Don't know what you did!?";
            });
        }

        public void SelectionChanged()
        {
            try
            {
                this.DisplayElements!.SelectedItem.Navigate();
            }
            catch
            {

            }
        }
    }
}
