using Microsoft.Xaml.Behaviors;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZkLauncher.Common;
using ZkLauncher.Models;
using ZkLauncher.ViewModels.UserControl;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // シングルトンクラスとして登録したい時
            containerRegistry.RegisterSingleton<IDisplayEmentsCollection?, DisplayElemetCollection>();
            containerRegistry.RegisterSingleton<IWindowPostionCollection?, WindowPostionCollection>();
            containerRegistry.RegisterSingleton<IFileElementCollection?, FileElementCollection>();

            containerRegistry.RegisterDialogWindow<BaseDialogWindow>("ControlPanel");
            //containerRegistry.RegisterDialogWindow<BaseDialogWindow>("Viewer");
            containerRegistry.RegisterDialogWindow<BaseDialogMetroWindow>("NameChange");
            containerRegistry.RegisterDialogWindow<BaseDialogMetroWindow>("Setting");

            containerRegistry.RegisterDialog<ucControlPanel, ucControlPanelViewModel>();
            containerRegistry.RegisterDialog<ucSettingLauncher, ucSettingLauncherViewModel>();
            containerRegistry.RegisterDialog<ucNameChange, ucNameChangeViewModel>();
            containerRegistry.RegisterDialog<ucViewerPanel, ucViewerPanelViewModel>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ViewerModule>();
        }
    }
}
