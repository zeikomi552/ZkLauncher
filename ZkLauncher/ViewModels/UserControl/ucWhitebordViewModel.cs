using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZkLauncher.Common.Helper;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using ZkLauncher.Views.UserControls;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucWhitebordViewModel : BindableBase, INavigationAware
    {
        #region 画像保存先ファイルパス
        /// <summary>
        /// 画像保存先ファイルパス
        /// </summary>
        string _FilePath = string.Empty;
        /// <summary>
        /// 画像保存先ファイルパス
        /// </summary>
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                if (_FilePath == null || !_FilePath.Equals(value))
                {
                    _FilePath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        }
        #endregion

        #region 書き込みモード[EditingMode]プロパティ
        /// <summary>
        /// 書き込みモード[EditingMode]プロパティ用変数
        /// </summary>
        InkCanvasEditingMode _EditingMode = InkCanvasEditingMode.Ink;
        /// <summary>
        /// 書き込みモード[EditingMode]プロパティ
        /// </summary>
        public InkCanvasEditingMode EditingMode
        {
            get
            {
                return _EditingMode;
            }
            set
            {
                if (!_EditingMode.Equals(value))
                {
                    _EditingMode = value;
                    RaisePropertyChanged("EditingMode");
                }
            }
        }
        #endregion

        #region ペンサイズ[Size]プロパティ
        /// <summary>
        /// ペンサイズ[Size]プロパティ用変数
        /// </summary>
        int _Size = 5;
        /// <summary>
        /// ペンサイズ[Size]プロパティ
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                if (!_Size.Equals(value))
                {
                    _Size = value;
                    RaisePropertyChanged("Size");
                }
            }
        }
        #endregion

        #region ホワイトボード用マーカーカラー[MarkerColor]プロパティ
        /// <summary>
        /// ホワイトボード用マーカーカラー[MarkerColor]プロパティ用変数
        /// </summary>
        Color _MarkerColor = Colors.Black;
        /// <summary>
        /// ホワイトボード用マーカーカラー[MarkerColor]プロパティ
        /// </summary>
        public Color MarkerColor
        {
            get
            {
                return _MarkerColor;
            }
            set
            {
                if (!_MarkerColor.Equals(value))
                {
                    _MarkerColor = value;
                    RaisePropertyChanged("MarkerColor");
                }
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


        IRegionManager _regionManager;
        IContainerExtension _container;
        public ucWhitebordViewModel(IContainerExtension container, IRegionManager regionManager, IDisplayEmentsCollection displayElements)
        {
            _regionManager = regionManager;
            _container = container;
            this.DisplayElements = displayElements;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // このViewが表示されるときに実行される
            this.FilePath = GetfileName(DirectoryPathDictionary.ImageSaveDirectory);
        }

        #region ファイルパスの作成処理
        /// <summary>
        /// ファイルパスの作成処理
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <returns>ファイルパス</returns>
        private string GetfileName(string folderPath)
        {
            var file = "Temporary.jpg";
            return Path.Combine(folderPath, file);
        }
        #endregion

        private bool _SaveF = false;

        #region ファイルパスの取得処理
        /// <summary>
        /// ファイルパスの取得処理
        /// </summary>
        /// <returns>ファイルパス</returns>
        private string GetFilepath()
        {
            // まだ保存していない場合
            if (!_SaveF)
            {
                string dir = string.Empty;
                if (this.DisplayElements == null ||
                    string.IsNullOrEmpty(this.DisplayElements.DrawPictureSaveDirectoryPath))
                {
                    // アプリケーションフォルダの取得
                    dir = DirectoryPathDictionary.ImageSaveDirectoryDefault;
                }
                else
                {
                    // アプリケーションフォルダの取得
                    dir = Path.Combine(this.DisplayElements.DrawPictureSaveDirectoryPath);

                }

                PathManager.CreateDirectory(dir);
                return Path.Combine(dir, "Picture-" + DateTime.Today.ToString("yyyyMMdd-HHmmss") + ".png");
            }
            // 保存されている場合
            else
            {
                return this.FilePath;
            }
        }
        #endregion

        #region 保存ボタン処理(.png)
        /// <summary>
        /// 保存ボタン処理(.png)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, RoutedEventArgs ex)
        {
            try
            {
                var wnd = VisualTreeHelperWrapper.GetWindow<ucWhitebord>(sender) as ucWhitebord;

                PresentationSource source = PresentationSource.FromVisual(wnd);

                if (wnd != null)
                {
                    // ↓画像がにじむ問題に対応
                    var size = new Size((int)(wnd.Drawgrid.ActualWidth), (int)(wnd.Drawgrid.ActualHeight));
                    wnd.Drawgrid.Measure(size);
                    wnd.Drawgrid.Arrange(new Rect(size));
                    // ↑画像がにじむ問題に対応

                    // レンダリング
                    var bmp = new RenderTargetBitmap(
                        (int)(wnd.Drawgrid.ActualWidth),
                        (int)(wnd.Drawgrid.ActualHeight),
                        96, 96, // DPI
                        PixelFormats.Pbgra32);
                    bmp.Render(wnd.Drawgrid);

                    string filepath = GetFilepath();
                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(filepath, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    this.FilePath = filepath;
                    RaisePropertyChanged("FilePath");
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        public void Init(object sender, RoutedEventArgs e)
        {
            try
            {
                //var wnd = VisualTreeHelperWrapper.GetWindow<ucWhitebord>(sender) as ucWhitebord;

                //if (wnd != null)
                //{
                //    wnd.theInkCanvas.Strokes.StrokesChanged -= Strokes_StrokesChanged;
                //    wnd.theInkCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
                //}

                //Clear(sender, e);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region クリア処理
        /// <summary>
        /// クリア処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Clear(object sender, RoutedEventArgs e)
        {
            try
            {
                var wnd = VisualTreeHelperWrapper.GetWindow<ucWhitebord>(sender) as ucWhitebord;

                if (wnd != null)
                {
                    wnd.theInkCanvas.Strokes.Clear();
                }
            }
            catch (Exception ex)
            {
                ///_logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

    }
}
