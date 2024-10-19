using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucSlideshowViewModel : BindableBase
    {
        #region 背景の保存先ディレクトリ
        /// <summary>
        /// 背景の保存先ディレクトリ
        /// </summary>
        private string ImageSaveDirectory
        {
            get
            {
                // アプリケーションフォルダの取得
                var dir = Path.Combine(PathManager.GetApplicationFolder(), "SaveImage", DateTime.Today.ToString("yyyyMM"));
                PathManager.CreateDirectory(dir);

                return dir;
            }
        }
        #endregion

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayElements">表示要素</param>
        /// <param name="windowPosition">ウィンドウ位置</param>
        public ucSlideshowViewModel(IDisplayEmentsCollection displayElements)
        {
            this.DisplayElements = displayElements;
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Init(object sender, EventArgs e)
        {
            try
            {
                var wnd = sender as ucViewerPanel;
                if (wnd != null)
                {
                    try
                    {
                        // タイマーのセット
                        this.DisplayElements!.SetupTimer();
                    }
                    catch
                    {
                        ShowMessage.ShowNoticeOK("WebView2ランタイムがインストールされていないようです。\r\nインストールしてください", "通知");
                        URLUtility.OpenUrl("https://developer.microsoft.com/en-us/microsoft-edge/webview2/");
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 次へ画面(URL)遷移
        /// <summary>
        /// 次へ画面(URL)遷移
        /// </summary>
        public void Next()
        {
            try
            {
                this.DisplayElements?.NextNavigate();
            }
            catch
            {

            }
        }
        #endregion

        #region 直前へ画面(URL)遷移
        /// <summary>
        /// 直前へ画面(URL)遷移
        /// </summary>
        public void Prev()
        {
            try
            {
                this.DisplayElements?.PrevNavigate();

            }
            catch
            {

            }
        }
        #endregion

        #region ループフラグ
        /// <summary>
        /// ループフラグ
        /// </summary>
        bool _LoopF = false;
        /// <summary>
        /// ループフラグ
        /// </summary>
        public bool LoopF
        {
            get
            {
                return _LoopF;
            }
            set
            {
                if (!_LoopF.Equals(value))
                {
                    _LoopF = value;
                    RaisePropertyChanged("LoopF");
                }
            }
        }
        #endregion

        #region ループ処理の開始
        /// <summary>
        /// ループ処理の開始
        /// </summary>
        public void Loop()
        {
            try
            {
                this.DisplayElements?.StartTimer();
            }
            catch
            {

            }
        }
        #endregion

        #region ループの一時停止
        /// <summary>
        /// ループの一時停止
        /// </summary>
        public void Pose()
        {
            try
            {
                this.DisplayElements?.StopTimer();
            }
            catch
            {

            }
        }
        #endregion

        #region 背景の保存先ディレクトリ
        /// <summary>
        /// 背景の保存先ディレクトリ
        /// </summary>
        private string BackgroundDirectory
        {
            get
            {
                // アプリケーションフォルダの取得
                var dir = Path.Combine(PathManager.GetApplicationFolder(), "Config", "Background");
                PathManager.CreateDirectory(dir);

                return dir;
            }
        }
        #endregion

        #region 背景の登録
        /// <summary>
        /// 背景の登録
        /// </summary>
        public void RegistBackground()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new Microsoft.Win32.OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "メディアファイル (*.mp4;*.wav;*.jpg;*.png)|*.mp4;*.wav;*.jpg;*.png|全てのファイル (*.*)|*.*";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var path = Path.Combine(BackgroundDirectory, "Viewer" + Path.GetExtension(dialog.FileName));

                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                    File.Copy(dialog.FileName, path, true);
                    this.DisplayElements!.ViewerBackgroundMediaPath = path;
                    this.DisplayElements.SaveConfig();
                    this.DisplayElements.LoadConfig();
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 画面のリロード処理
        /// <summary>
        /// 画面のリロード処理
        /// </summary>
        public void Reload()
        {
            try
            {
                this.DisplayElements?.ReloadURL();
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ページの保存処理
        /// <summary>
        /// ページの保存処理
        /// </summary>
        public void SavePage()
        {
            try
            {
                var ctrl = this.DisplayElements!.SelectedItem.WebView2Object!;
                var targetPoint = ctrl.PointToScreen(new System.Windows.Point(0.0d, 0.0d));

                // キャプチャ領域の生成
                var targetRect = new Rect(targetPoint.X, targetPoint.Y, ctrl.ActualWidth, ctrl.ActualHeight);

                //// スクリーンショット実行
                ExcuteScreenShot(targetRect, fileName(ImageSaveDirectory));
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ファイルパスの作成処理
        /// <summary>
        /// ファイルパスの作成処理
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <returns>ファイルパス</returns>
        private string fileName(string folderPath)
        {
            var dtNow = DateTime.Now;
            var file = dtNow.ToString("yyyyMMdd_hhmmss") + dtNow.Millisecond.ToString() + ".jpg";
            return Path.Combine(folderPath, file);
        }
        #endregion

        #region スクリーンショットの作成処理
        /// <summary>
        /// スクリーンショットの作成処理
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <param name="fileName">ファイル名</param>
        private void ExcuteScreenShot(Rect rect, string fileName)
        {
            // 矩形と同じサイズのBitmapを作成
            using (var bitmap = new Bitmap((int)rect.Width, (int)rect.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                // 画面から指定された矩形と同じ条件でコピー
                graphics.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bitmap.Size);

                // 画像ファイルとして保存
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        #endregion
    }
}
