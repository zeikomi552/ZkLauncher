using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using ZkLauncher.ViewModels.UserControl;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.Common
{
    public class ViewerModule : IModule
    {
        IContainerExtension _Container;
        public ViewerModule(IContainerExtension container, IRegionManager regionManager)
        {
            _Container = container;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {           
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucSlideshow));
            regionManager.RegisterViewWithRegion("ViewerRegion", typeof(ucWhitebord));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ucSlideshow, ucSlideshowViewModel>();
            containerRegistry.RegisterForNavigation<ucWhitebord, ucWhitebordViewModel>();
        }
    }
}
