﻿using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private IDialogService? _dialogService;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ucControlPanelForWhiteboardViewModel(IDialogService dialogService, IFileElementCollection filecollection, IDisplayEmentsCollection? displayElement)
        {
            this.FileCollection = filecollection;
            this.DisplayElements = displayElement;
            this._dialogService = dialogService;
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

        #region ファイルを選択した状態でエクスプローラーを開く
        /// <summary>
        /// ファイルを選択した状態でエクスプローラーを開く
        /// </summary>
        public void RevealInFileExplore()
        {
            try
            {
                if (this.FileCollection != null && this.FileCollection.SelectedItem != null &&
                    !string.IsNullOrEmpty(this.FileCollection.SelectedItem.Filepath))
                {
                    var filepath = this.FileCollection.SelectedItem.Filepath;
                    // ファイルパスのnullチェック
                    if (!string.IsNullOrEmpty(filepath))
                    {
                        Process.Start("explorer.exe", string.Format(@"/select,""{0}", filepath));  // 指定したフォルダを選択した状態でエクスプローラを開く
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }
        }
        #endregion


        #region ChangeNameWindow表示画面の呼び出し
        /// <summary>
        /// ChangeNameWindow表示画面の呼び出し
        /// </summary>
        public void ChangeFileName()
        {
            try
            {
                if (this.FileCollection != null && this.FileCollection.SelectedItem != null)
                {
                    _dialogService.ShowDialog("ucNameChange", new DialogParameters($"CurrentName={this.FileCollection.SelectedItem.Filename}"), r =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            // 戻り値の取り出し
                            var result = r.Parameters.GetValue<string>("AfterName");


                            var dir = System.IO.Path.GetDirectoryName(this.FileCollection.SelectedItem.Filepath);
                            var ext = System.IO.Path.GetExtension(this.FileCollection.SelectedItem.Filepath);

                            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(ext))
                            {
                                string filepath = Path.Combine(dir, result) + ext;

                                // ファイル名に変更がない場合はそのまま抜ける
                                if (this.FileCollection.SelectedItem.Filepath.Equals(filepath))
                                    return;

                                int count = 1;
                                while (File.Exists(filepath))
                                {
                                    filepath = Path.Combine(dir, result) + $"({count++})" + ext;
                                }
                                // ファイル名の変更
                                File.Move(this.FileCollection.SelectedItem.Filepath, filepath);
                                this.FileCollection.SelectedItem.Filepath = filepath;
                            }
                        }
                    });
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
