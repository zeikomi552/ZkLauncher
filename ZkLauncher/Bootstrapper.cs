using Microsoft.Xaml.Behaviors;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            // オプションの設定などDryIocコンテナ独自の機能を使いたい場合
            var container = containerRegistry.GetContainer();

            // シングルトンクラスとして登録したい時
            containerRegistry.RegisterSingleton<IDisplayEmentsCollection?, DisplayElemetCollection>();

            containerRegistry.RegisterDialogWindow<BaseDialogWindow>();
            containerRegistry.RegisterDialog<ucViewerPanel, ucViewerPanelViewModel>();
            containerRegistry.RegisterDialog<ucControlPanel, ucControlPanelViewModel>();
            containerRegistry.RegisterDialog<ucSettingLauncher, ucSettingLauncherViewModel>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
