using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZkLauncher.Models;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels
{
    public class MainWindowViewModel : BindableBase
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


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager">RegionManager</param>
        public MainWindowViewModel(IRegionManager regionManager, IDisplayEmentsCollection displayElements)
        {

            this.DisplayElements = displayElements;
            regionManager.RegisterViewWithRegion("ControlPanel", typeof(ucControlPanel));
            //regionManager.RegisterViewWithRegion("DependensyPropertyRegion", typeof(ucDependencyProperty));
            //regionManager.RegisterViewWithRegion("ConverterRegion", typeof(ucConverter));
            //regionManager.RegisterViewWithRegion("ActionRegion", typeof(ucAction));
            //regionManager.RegisterViewWithRegion("BehaviorRegion", typeof(ucBehavior));
        }
    }
}
