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

        IRegionManager _regionManager;
        IContainerExtension _container;
        public ucWhitebordViewModel(IContainerExtension container, 
            IRegionManager regionManager, IDisplayEmentsCollection displayElements, IFileElementCollection filecollection)
        {
            _regionManager = regionManager;
            _container = container;
            this.DisplayElements = displayElements;
            this.FileCollection = filecollection;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (this.FileCollection != null && navigationContext != null && navigationContext.Parameters["SaveDirectory"] != null)
            {
                this.FileCollection.SaveDirectoryPath = navigationContext.Parameters["SaveDirectory"]!.ToString()!;
            }
            //_SaveF = false;
        }



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

                    string filepath = this.FileCollection!.GetFilepath();
                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(filepath, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    this.FileCollection!.SelectedItem.Filepath = filepath;
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
                Clear(sender, e);
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
