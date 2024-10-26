using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            try
            {
                // このViewが表示されるときに実行される

                var parameters = navigationContext.Parameters;
                var dir = parameters["SaveDirectory"]!.ToString();


                var list = new List<DirectoryElement>();

                if (!string.IsNullOrEmpty(dir))
                {
                    // 画像保存先の作成
                    var save_dir = Path.Combine(dir, DateTime.Today.ToString("yyyyMMdd"));
                    PathManager.CreateDirectory(save_dir);           // ディレクトリの作成

                    // 読み込んだディレクトリ一覧を登録
                    var directories = Directory.GetDirectories(dir);
                    foreach (var diritem in directories)
                    {
                        list.Add(new DirectoryElement() { DirectoryPath = diritem });
                    }
                    this.DirectoryCollection.Elements = new ObservableCollection<DirectoryElement>(list);


                    // 今日の日付のディレクトリを選択
                    this.DirectoryCollection.SelectedItem = (from x in this.DirectoryCollection.Elements
                                                             where x.DirectoryPath.Equals(save_dir)
                                                             select x).First();

                    ComboboxSelectionChanged();
                }
            }
            catch(Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }

        }
        #endregion

        #region ディレクトリ一覧
        /// <summary>
        /// ディレクトリ一覧
        /// </summary>
        DirectoryElementCollection _DirectoryCollection = new DirectoryElementCollection();
        /// <summary>
        /// ディレクトリ一覧
        /// </summary>
        public DirectoryElementCollection DirectoryCollection
        {
            get
            {
                return _DirectoryCollection;
            }
            set
            {
                if (_DirectoryCollection == null || !_DirectoryCollection.Equals(value))
                {
                    _DirectoryCollection = value;
                    RaisePropertyChanged("DirectoryCollection");
                }
            }
        }
        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ucControlPanelForWhiteboardViewModel(IFileElementCollection filecollection, IDisplayEmentsCollection? displayElement)
        {
            this.FileCollection = filecollection;
            this.DisplayElements = displayElement;
        }

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

        #region コンボボックスの選択が変更された
        /// <summary>
        /// コンボボックスの選択が変更された
        /// </summary>
        public void ComboboxSelectionChanged()
        {
            try
            {
                if (this.DirectoryCollection.SelectedItem != null)
                {
                    var list = new List<DirectoryElement>();
                    string dir = this.DirectoryCollection.SelectedItem.DirectoryPath;
                    SetFileList(dir);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region リストボックスの選択が変更された
        /// <summary>
        /// リストボックスの選択が変更された
        /// </summary>
        public void ListboxSelectionChanged()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region ファイル一覧のセット処理
        /// <summary>
        /// ファイル一覧のセット処理
        /// </summary>
        /// <param name="dir">対象ディレクトリ</param>
        private void SetFileList(string dir)
        {
            try
            {
                var list = new List<DirectoryElement>();

                if (!string.IsNullOrEmpty(dir))
                {
                    PathManager.CreateDirectory(dir);           // ディレクトリの作成

                    if (System.IO.Path.GetFileName(dir).Equals(DateTime.Today.ToString("yyyyMMdd")))
                    {
                        this.FileCollection!.ReadDirectory(dir, true);    // 指定フォルダ配下のファイルを読み込み
                    }
                    else
                    {
                        this.FileCollection!.ReadDirectory(dir, false);    // 指定フォルダ配下のファイルを読み込み
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 選択要素の削除処理
        /// <summary>
        /// 選択要素の削除処理
        /// </summary>
        public void ContextMenu_Delete()
        {
            try
            {
                if (this.FileCollection != null)
                {
                    this.FileCollection.SelectedItemDelete();
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
