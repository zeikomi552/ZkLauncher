﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Models
{
    public class FileElementCollection : BindableBase, IFileElementCollection
    {
        #region ファイル保存先ディレクトリパス
        /// <summary>
        /// ファイル保存先ディレクトリパス
        /// </summary>
        string _SaveDirectoryPath = string.Empty;
        /// <summary>
        /// ファイル保存先ディレクトリパス
        /// </summary>
        public string SaveDirectoryPath
        {
            get
            {
                return _SaveDirectoryPath;
            }
            set
            {
                if (_SaveDirectoryPath == null || !_SaveDirectoryPath.Equals(value))
                {
                    _SaveDirectoryPath = value;
                    RaisePropertyChanged("SaveDirectoryPath");
                }
            }
        }
        #endregion

        #region ファイルリスト
        /// <summary>
        /// ファイルリスト
        /// </summary>
        ObservableCollection<FileElement> _Elements = new ObservableCollection<FileElement>();
        /// <summary>
        /// ファイルリスト
        /// </summary>
        public ObservableCollection<FileElement> Elements
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

        #region ファイルパスの取得処理
        /// <summary>
        /// ファイルパスの取得処理
        /// </summary>
        /// <returns>ファイルパス</returns>
        public string GetFilepath()
        {
            // まだ一時ファイルの場合
            if (GetTemporaryFilepath().Equals(this.SelectedItem.Filepath))
            {
                string dir;
                if (string.IsNullOrEmpty(this.SaveDirectoryPath))
                {
                    // アプリケーションフォルダの取得
                    dir = DirectoryPathDictionary.ImageSaveDirectoryDefault;
                }
                else
                {
                    // アプリケーションフォルダの取得
                    dir = Path.Combine(this.SaveDirectoryPath, DateTime.Today.ToString("yyyyMMdd"));
                }

                PathManager.CreateDirectory(dir);
                return Path.Combine(dir, "Picture-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png");
            }
            // 正規フォルダに保存している場合
            else
            {
                if (this.SelectedItem == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.SelectedItem.Filepath;
                }
            }
        }
        #endregion

        #region 1つ上の要素と入れ替える
        /// <summary>
        /// 1つ上の要素と入れ替える
        /// </summary>
        public void MoveUP()
        {
            if (this.SelectedItem != null)
            {
                int index = this.Elements.IndexOf(this.SelectedItem);

                if (index > 0)
                {
                    // 指定した位置の要素を取り出す
                    var elem = this.Elements.ElementAt(index);
                    // 指定した位置の要素を削除する
                    this.Elements.RemoveAt(index);
                    // 一つ上の要素に挿入する
                    this.Elements.Insert(index - 1, elem);
                    // 選択要素をセット
                    this.SelectedItem = elem;
                }
            }
        }
        #endregion

        #region 一つ下の要素と入れ替える
        /// <summary>
        /// 一つ下の要素と入れ替える
        /// </summary>
        public void MoveDown()
        {
            if (this.SelectedItem != null)
            {
                int index = this.Elements.IndexOf(this.SelectedItem);

                if (index >= 0 && index < this.Elements.Count - 1)
                {
                    // 指定した位置の要素を取り出す
                    var elem = this.Elements.ElementAt(index);
                    // 指定した位置の要素を削除する
                    this.Elements.RemoveAt(index);
                    // 一つ上の要素に挿入する
                    this.Elements.Insert(index + 1, elem);
                    // 選択要素をセット
                    this.SelectedItem = elem;
                }
            }
        }
        #endregion

        #region 選択行を削除する処理
        /// <summary>
        /// 選択行を削除する処理
        /// </summary>
        public void SelectedItemDelete()
        {
            try
            {
                
                // 選択行を削除
                if (this.SelectedItem != null)
                {
                    int index = this.Elements.IndexOf(this.SelectedItem);

                    var tmp = (from x in this.Elements
                               where x.Equals(this.SelectedItem)
                               select x).First();

                    Remove(tmp);

                    // 選択されており要素が削除後1個以上存在する場合のみ選択アイテムをセットする
                    if (index >= 0 && this.Elements.Count > 0)
                    {
                        // 最初の要素を選択していた場合
                        if (index == 0)
                        {
                            this.SelectFirst();
                        }
                        // 最後の要素を選択していた場合
                        else if (this.Elements.Count  >= index)
                        {
                            this.SelectedItem = this.Elements.ElementAt(index - 1);
                        }
                        // 上記以外の要素を選択していた場合
                        else
                        {
                            this.SelectedItem = this.Elements.ElementAt(index);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 選択要素
        /// <summary>
        /// 選択要素
        /// </summary>
        FileElement _SelectedItem = new FileElement();
        /// <summary>
        /// 選択要素
        /// </summary>
        public FileElement SelectedItem
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
        public FileElement? First()
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

        #region 最後の項目の選択
        /// <summary>
        /// 最後の項目の選択
        /// </summary>
        public void SelectLast()
        {
            if (this.Elements != null && this.Elements.Count > 0)
            {
                this.SelectedItem = this.Elements.Last();
            }
        }
        #endregion

        #region 要素の削除処理
        /// <summary>
        /// 要素の削除処理
        /// </summary>
        public void Remove(FileElement delete_item)
        {
            if (File.Exists(delete_item.Filepath))
            {
                if (ShowMessage.ShowQuestionYesNo("ファイルを削除してもよろしいですか？", "確認") == MessageBoxResult.Yes)
                {
                    File.Delete(delete_item.Filepath);
                }
            }

            this.Elements.Remove(delete_item);
        }
        #endregion

        #region 要素の追加
        /// <summary>
        /// 要素の追加
        /// </summary>
        /// <param name="item">追加要素</param>
        public void Add(FileElement item)
        {
            this.Elements.Add(item);
        }
        #endregion

        #region 読み込んだディレクトリ[DirectoryPath]プロパティ
        /// <summary>
        /// 読み込んだディレクトリ[DirectoryPath]プロパティ用変数
        /// </summary>
        string _DirectoryPath = string.Empty;
        /// <summary>
        /// 読み込んだディレクトリ[DirectoryPath]プロパティ
        /// </summary>
        public string DirectoryPath
        {
            get
            {
                return _DirectoryPath;
            }
            set
            {
                if (_DirectoryPath == null || !_DirectoryPath.Equals(value))
                {
                    _DirectoryPath = value;
                    RaisePropertyChanged("DirectoryPath");
                }
            }
        }
        #endregion

        #region ディレクトリ読み込み処理実行中[ExecuteReadDirF]プロパティ
        /// <summary>
        /// ディレクトリ読み込み処理実行中[ExecuteReadDirF]プロパティ用変数
        /// </summary>
        static bool _ExecuteReadDirF = false;
        /// <summary>
        /// ディレクトリ読み込み処理実行中[ExecuteReadDirF]プロパティ
        /// </summary>
        public bool ExecuteReadDirF
        {
            get
            {
                return _ExecuteReadDirF;
            }
            set
            {
                if (!_ExecuteReadDirF.Equals(value))
                {
                    _ExecuteReadDirF = value;
                    RaisePropertyChanged("ExecuteReadDirF");
                }
            }
        }
        #endregion

        #region ディレクトリ内のpngファイルを全て読み込む
        /// <summary>
        /// ディレクトリ内のpngファイルを全て読み込む
        /// </summary>
        public void ReadDirectory()
        {
            try
            {
                // ディレクトリを開く
                if (OpenDirectory())
                {
                    // ディレクトリ内のpngファイルの読み込み
                    ReadDirectory(this.DirectoryPath);
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
            finally
            {
            }
        }
        #endregion

        #region ディレクトリのファイル全て読み込み
        /// <summary>
        /// ディレクトリのファイル全て読み込み
        /// </summary>
        /// <param name="dir">ディレクトリパス</param>
        /// <param name="tempf">一時ファイルをリスト上に追加するかどうか</param>
        public void ReadDirectory(string dir, bool tempf = true)
        {
            try
            {
                // ファイル情報のセット
                this.Elements.Clear();

                Task.Run(() =>
                {
                    // 読み込み中の場合は無視
                    if (this.ExecuteReadDirF)
                        return;

                    // スレッドセーフの呼び出し
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                this.ExecuteReadDirF = true;    // 読み込み処理実行
                            }
                            catch { }
                        }));


                    // フォルダ内のファイル一覧を取得
                    var fileArray = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories);


                    List<FileElement> list = new List<FileElement>();
                    foreach (string file in fileArray)
                    {
                        list.Add(new FileElement()
                        { 
                            EditingF = false,
                            Filepath = file,
                            TemporaryF = false
                        });
                    }

                    if (tempf)
                    {
                        list.Insert(0, new FileElement()
                        {
                            Filepath = GetTemporaryFilepath(),
                            EditingF = true,
                            TemporaryF = true
                        });
                    }

                    // スレッドセーフの呼び出し
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                this.Elements = new ObservableCollection<FileElement>(list);
                                this.SelectedItem = this.Elements.First();
                            }
                            catch { }
                        }));

                    // スレッドセーフの呼び出し
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                this.ExecuteReadDirF = false;    // 読み込み処理終了
                            }
                            catch { }
                        }));
                });
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 一時ファイルパスの作成処理
        /// <summary>
        /// 一時ファイルパスの作成処理
        /// </summary>
        /// <returns>一時ファイルパス</returns>
        private string GetTemporaryFilepath()
        {
            var file = "Temporary.jpg";
            return Path.Combine(DirectoryPathDictionary.ImageSaveDirectory, file);
        }
        #endregion

        #region ディレクトリを開く処理
        /// <summary>
        /// ディレクトリを開く処理
        /// </summary>
        private bool OpenDirectory()
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "フォルダを選択してください",
                // フォルダ選択モードにする
                IsFolderPicker = true,
            })
            {
                // フォルダ選択ダイアログを開く
                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    this.DirectoryPath = cofd.FileName; // フォルダパスのセット
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}
