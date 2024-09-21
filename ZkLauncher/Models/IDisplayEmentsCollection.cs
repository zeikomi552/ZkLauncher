using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public interface IDisplayEmentsCollection
    {
        #region 表示要素
        /// <summary>
        /// 表示要素
        /// </summary>
        public ObservableCollection<DisplayElement> Elements
        {
            get;set;
        }
        #endregion

        #region 選択要素
        /// <summary>
        /// 選択要素
        /// </summary>
        public DisplayElement SelectedItem
        {
            get;set;
        }
        #endregion
    }
}
