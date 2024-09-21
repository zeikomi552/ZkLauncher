using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class DisplayElemetCollection : BindableBase, IDisplayEmentsCollection
    {
        #region 表示要素
        /// <summary>
        /// 表示要素
        /// </summary>
        ObservableCollection<DisplayElement> _Elements = new ObservableCollection<DisplayElement>();
        /// <summary>
        /// 表示要素
        /// </summary>
        public ObservableCollection<DisplayElement> Elements
        {
            get
            {
                return _Elements;
            }
            set
            {
                if (_Elements == null || !_Elements.Equals(value))
                {
                    _Elements = value;
                    RaisePropertyChanged("Elements");
                }
            }
        }
        #endregion

        #region 選択要素
        /// <summary>
        /// 選択要素
        /// </summary>
        DisplayElement _SelectedItem = new DisplayElement();
        /// <summary>
        /// 選択要素
        /// </summary>
        public DisplayElement SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem == null || !_SelectedItem.Equals(value))
                {
                    _SelectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }
        }
        #endregion

        /// <summary>
        /// 最初の値を取得する
        /// </summary>
        /// <returns>最初の値 存在しない場合はnull</returns>
        public DisplayElement? First()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                return this.Elements.First();
            }
            else
            {
                return null;
            }
        }

        public void SelectFirst()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                this.SelectedItem = this.Elements.First();
            }
        }

        public void SelectLast()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                this.SelectedItem = this.Elements.Last();
            }
        }
    }
}
