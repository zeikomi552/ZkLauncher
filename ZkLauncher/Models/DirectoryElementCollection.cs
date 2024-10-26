using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class DirectoryElementCollection : BindableBase
    {
        #region 画像の保存先ディレクトリの一覧
        /// <summary>
        /// 画像の保存先ディレクトリの一覧
        /// </summary>
        ObservableCollection<DirectoryElement> _Elements = new ObservableCollection<DirectoryElement>();
        /// <summary>
        /// 画像の保存先ディレクトリの一覧
        /// </summary>
        public ObservableCollection<DirectoryElement> Elements
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

        #region 選択している画像の保存先ディレクトリ
        /// <summary>
        /// 選択している画像の保存先ディレクトリ
        /// </summary>
        DirectoryElement _SelectedItem = new DirectoryElement();
        /// <summary>
        /// 選択している画像の保存先ディレクトリ
        /// </summary>
        public DirectoryElement SelectedItem
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


        #region 最初の値を取得する
        /// <summary>
        /// 最初の値を取得する
        /// </summary>
        /// <returns>最初の値 存在しない場合はnull</returns>
        public DirectoryElement? First()
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
        #endregion

        #region 最初の項目の選択
        /// <summary>
        /// 最初の項目の選択
        /// </summary>
        public void SelectFirst()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                this.SelectedItem = this.Elements.First();
            }
        }
        #endregion

    }
}
