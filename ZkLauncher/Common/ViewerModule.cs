using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZkLauncher.ViewModels.UserControl;
using ZkLauncher.Views;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.Common
{
    public class ViewerModule : IModule
    {
        public ViewerModule(IContainerExtension container, IRegionManager regionManager)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("ContentRegion", typeof(ucSlideshow));


            //IRegion region = regionManager.Regions["ContentRegion"];

            //var tabA = containerProvider.Resolve<ucSlideshow>();
            //SetTitle(tabA, "Tab A");
            //region.Add(tabA);

            ////var tabB = containerProvider.Resolve<TabView>();
            ////SetTitle(tabB, "Tab B");
            ////region.Add(tabB);

            ////var tabC = containerProvider.Resolve<TabView>();
            ////SetTitle(tabC, "Tab C");
            ////region.Add(tabC);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterDialogWindow<BaseDialogWindow>("Viewer");
            containerRegistry.RegisterDialog<ucViewerPanel, ucViewerPanelViewModel>();
            ViewModelLocationProvider.Register(typeof(ucSlideshow).ToString(), typeof(ucSlideshowViewModel));
        }

        void SetTitle(ucSlideshow tab, string title)
        {
            var tmp = (tab.DataContext as ucSlideshowViewModel);
        }
    }
}
