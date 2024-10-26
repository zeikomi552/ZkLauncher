using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucControlPanelForWhiteboardViewModel : BindableBase, INavigationAware
    {
        #region INavigationAware override
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // このViewが表示された状態から切り替わるときに実行される
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // このViewが表示されるときに実行される

            var parameters = navigationContext.Parameters;
            var dir = parameters["SaveDirectory"]!.ToString();

            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
            {
                this.FileCollection!.ReadDirectory(dir);

            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ucControlPanelForWhiteboardViewModel(IFileElementCollection filecollection)
        {
            this.FileCollection = filecollection;
        }

        #region ファイルデータリスト
        /// <summary>
        /// ファイルデータリスト
        /// </summary>
        IFileElementCollection? _FileCollection;
        /// <summary>
        /// ファイルデータリスト
        /// </summary>
        public IFileElementCollection? FileCollection
        {
            get
            {
                return _FileCollection;
            }
            set
            {
                if (_FileCollection == null || !_FileCollection.Equals(value))
                {
                    _FileCollection = value;
                    RaisePropertyChanged("FileCollection");
                }
            }
        }
        #endregion

        public void SelectionChanged()
        {

        }

    }
}
