using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Models
{
    public class DisplayElemetCollection : BindableBase, IDisplayEmentsCollection
    {
        #region コントロールパネル背景用メディアファイル
        /// <summary>
        /// コントロールパネル背景用メディアファイル
        /// </summary>
        string _ControlBackgroundMediaPath = string.Empty;
        /// <summary>
        /// コントロールパネル背景用メディアファイル
        /// </summary>
        public string ControlBackgroundMediaPath
        {
            get
            {
                return _ControlBackgroundMediaPath;
            }
            set
            {
                if (_ControlBackgroundMediaPath == null || !_ControlBackgroundMediaPath.Equals(value))
                {
                    _ControlBackgroundMediaPath = value;
                    RaisePropertyChanged("ControlBackgroundMediaPath");
                }
            }
        }
        #endregion

        #region Viewer背景用メディアファイル
        /// <summary>
        /// Viewer背景用メディアファイル
        /// </summary>
        string _ViewerBackgroundMediaPath = string.Empty;
        /// <summary>
        /// Viewer背景用メディアファイル
        /// </summary>
        public string ViewerBackgroundMediaPath
        {
            get
            {
                return _ViewerBackgroundMediaPath;
            }
            set
            {
                if (_ViewerBackgroundMediaPath == null || !_ViewerBackgroundMediaPath.Equals(value))
                {
                    _ViewerBackgroundMediaPath = value;
                    RaisePropertyChanged("ViewerBackgroundMediaPath");
                }
            }
        }
        #endregion

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

        public DisplayElement? SelectedItemBk { get; set; } = null;

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

                if (index < this.Elements.Count - 1)
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
                    var tmp = (from x in this.Elements
                               where x.Equals(this.SelectedItem)
                               select x).First();

                    Remove(tmp);
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
                    this.SelectedItemBk = _SelectedItem;
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

        #region 表示要素のセット
        /// <summary>
        /// 表示要素のセット
        /// </summary>
        /// <param name="elements">表示要素</param>
        public void SetElements(DisplayElemetCollection item)
        {
            this.Elements.Clear();
            foreach (var tmp in item.Elements)
            {
                this.Elements.Add(tmp);
            }
            this.ControlBackgroundMediaPath = item.ControlBackgroundMediaPath;
            this.ViewerBackgroundMediaPath = item.ViewerBackgroundMediaPath;
        }
        #endregion

        #region ディレクトリ名
        /// <summary>
        /// ディレクトリ名
        /// </summary>
        const string ConfigDir = "Config";
        #endregion

        #region Configファイル名
        /// <summary>
        /// Configファイル名
        /// </summary>
        const string ConfigFile = "Setting.conf";
        #endregion

        #region Configファイルの読み込み
        /// <summary>
        /// Configファイルの読み込み
        /// </summary>
        public void LoadConfig()
        {
            try
            {
                ConfigManager<DisplayElemetCollection> conf = new ConfigManager<DisplayElemetCollection>(ConfigDir, ConfigFile, new DisplayElemetCollection());

                // ファイルの存在確認
                if (!File.Exists(conf.ConfigFile))
                {
                    conf.SaveXML(); // XMLのセーブ
                }
                else
                {
                    conf.LoadXML(); // XMLのロード
                }

                // 要素のセット
                SetElements(conf.Item!);
                this.ControlBackgroundMediaPath = conf.Item!.ControlBackgroundMediaPath;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Configファイルの保存処理
        /// <summary>
        /// Configファイルの保存処理
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                ConfigManager<DisplayElemetCollection> conf = new ConfigManager<DisplayElemetCollection>(ConfigDir, ConfigFile, new DisplayElemetCollection());

                // 値の受け渡し
                conf.Item = this;
                conf.SaveXML(); // XMLのセーブ
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 選択要素をViewに表示
        /// <summary>
        /// 選択要素をViewに表示
        /// </summary>
        public void SelectedNavigate()
        {
            try
            {
                if (this.Elements == null || this.Elements.Count <= 0)
                    return;

                if (this.SelectedItem == null)
                    this.SelectFirst();

                // 待ち時間の初期化
                InitWaitTime();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 次の要素をViewに表示
        /// <summary>
        /// 次の要素をViewに表示
        /// </summary>
        public void NextNavigate()
        {
            try
            {
                if (this.Elements == null || this.Elements.Count <= 0)
                    return;

                if (this.SelectedItem == null)
                    this.SelectFirst();


                int index = this.Elements.IndexOf(this.SelectedItem!);

                if (this.Elements.Count > index + 1)
                {
                    this.SelectedItem = this.Elements.ElementAt(index + 1);
                }
                else
                {
                    this.SelectFirst();
                }

                // 待ち時間の初期化
                InitWaitTime();
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region 前の要素をViewに表示
        /// <summary>
        /// 前の要素をViewに表示
        /// </summary>
        public void PrevNavigate()
        {
            try
            {
                if (this.Elements == null || this.Elements.Count <= 0)
                    return;

                if (this.SelectedItem == null)
                    this.SelectFirst();

                int index = this.Elements.IndexOf(this.SelectedItem!);

                if (index - 1 >= 0)
                {
                    this.SelectedItem = this.Elements.ElementAt(index - 1);
                }
                else
                {
                    this.SelectLast();

                }
                // 待ち時間の初期化
                InitWaitTime();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region URLのリロード処理
        /// <summary>
        /// URLのリロード処理
        /// </summary>
        public void ReloadURL()
        {
            try
            {
                this.SelectedItem!.Navigate();

                // 待ち時間の初期化
                InitWaitTime();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 要素の削除処理
        /// <summary>
        /// 要素の削除処理
        /// </summary>
        public void Remove(DisplayElement delete_item)
        {
            // イメージファイルが存在する場合は削除
            if (File.Exists(delete_item.ImagePath))
            {
                File.Delete(delete_item.ImagePath);
            }
            this.Elements.Remove(delete_item);
        }
        #endregion

        #region 要素の追加
        /// <summary>
        /// 要素の追加
        /// </summary>
        /// <param name="item">追加要素</param>
        public void Add(DisplayElement item)
        {
            this.Elements.Add(item);
        }
        #endregion


        #region クリップボード上の要素の追加
        /// <summary>
        /// クリップボード上の要素の追加
        /// </summary>
        public bool AddClipboardElement()
        {
            bool regist = false;
            //クリップボードに文字列データがあるか確認
            if (System.Windows.Clipboard.ContainsText())
            {
                string text = System.Windows.Clipboard.GetText();

                if (text.Contains("http://") || text.Contains("https://"))
                {
                    var tmp = text.Replace("http://", "").Replace("https://", "").Split('/');
                    var title = tmp.ElementAt(0);

                    this.Add(new DisplayElement() { Title = title, URI = text });
                    regist = true;
                }
                else
                {
                    text = text.Replace("\"", "");
                    if (File.Exists(text) && System.IO.Path.GetExtension(text).ToLower().Equals(".pdf"))
                    {
                        this.Add(new DisplayElement() { Title = "ファイル", URI = text });
                        regist = true;
                    }
                }
            }
            //クリップボードにBitmapデータがあるか調べる（調べなくても良い）
            else if (System.Windows.Clipboard.ContainsImage())
            {

                if (this.SelectedItem != null)
                {
                    //クリップボードにあるデータの取得
                    var bmp = Clipboard.GetImage();
                    if (bmp != null)
                    {
                        PathManager.CreateCurrentDirectory(this.SelectedItem.ImagePath);
                        SaveBitmapSourceToPng(bmp, this.SelectedItem.ImagePath);
                        regist = true;
                    }
                }
                else
                {
                    ShowMessage.ShowNoticeOK("画像登録対象が選択されていません。", "通知");
                }
            }
            return regist;
        }
        #endregion


        #region BitmapSourceのファイル保存処理
        /// <summary>
        /// BitmapSourceのファイル保存処理
        /// </summary>
        /// <param name="bitmapSource">BitmapSource</param>
        /// <param name="filePath">ファイルパス</param>
        private static void SaveBitmapSourceToPng(BitmapSource bitmapSource, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                System.Windows.Media.Imaging.BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }
        #endregion


        #region 残り待ち時間
        /// <summary>
        /// 残り待ち時間
        /// </summary>
        int _WaitSecond = 30;
        /// <summary>
        /// 残り待ち時間
        /// </summary>
        public int WaitSecond
        {
            get
            {
                return _WaitSecond;
            }
            set
            {
                if (!_WaitSecond.Equals(value))
                {
                    _WaitSecond = value;
                    RaisePropertyChanged("WaitSecond");
                }
            }
        }
        #endregion

        // タイマのインスタンス
        private DispatcherTimer? _timer;

        // タイマを設定する
        public void SetupTimer()
        {
            if (_timer == null)
            {
                // タイマのインスタンスを生成
                _timer = new DispatcherTimer(); // 優先度はDispatcherPriority.Background
                                                // インターバルを設定
                _timer.Interval = new TimeSpan(0, 0, 1);
                // タイマメソッドを設定
                _timer.Tick += new EventHandler(LoopExecute!);
            }
        }

        #region タイマーの開始処理
        /// <summary>
        /// タイマーの開始処理
        /// </summary>
        public void StartTimer()
        {
            if (_timer != null)
            {
                _timer.Start();
            }
        }
        #endregion
        
        #region タイマーの停止
        /// <summary>
        /// タイマーの停止
        /// </summary>
        public void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }
        #endregion

        #region タイマーメソッド
        /// <summary>
        /// タイマーメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopExecute(object sender, EventArgs e)
        {
            try
            {
                this.WaitSecond--;
                if (this.WaitSecond <= 0)
                {
                    this.NextNavigate();
                }
            }
            catch
            {

            }
        }
        #endregion


        #region 待ち時間の初期化
        /// <summary>
        /// 待ち時間の初期化
        /// </summary>
        private void InitWaitTime()
        {
            this.WaitSecond = 30;
        }
        #endregion
    }
}
