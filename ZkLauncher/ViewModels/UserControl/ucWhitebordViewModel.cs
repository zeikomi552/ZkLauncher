using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZkLauncher.Common.Helper;
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


        IRegionManager _regionManager;
        IContainerExtension _container;
        public ucWhitebordViewModel(IContainerExtension container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // このViewが表示されるときに実行される
            this.FilePath = navigationContext.Parameters["filepath"]?.ToString()!;
        }

        #region 保存ボタン処理(.png)
        /// <summary>
        /// 保存ボタン処理(.png)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, RoutedEventArgs e)
        {
            var wnd = VisualTreeHelperWrapper.GetWindow<ucWhitebord>(sender) as ucWhitebord;

            if (wnd != null)
            {
                Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

                dlgSave.Filter = "PNGファイル(*.png)|*.png";
                dlgSave.AddExtension = true;

                if ((bool)dlgSave.ShowDialog()!)
                {
                    // レンダリング
                    var bmp = new RenderTargetBitmap(
                        (int)wnd.Drawgrid.ActualWidth,
                        (int)wnd.Drawgrid.ActualHeight,
                        96, 96, // DPI
                        PixelFormats.Pbgra32);
                    bmp.Render(wnd.Drawgrid);

                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(dlgSave.FileName, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
            }
        }
        #endregion
    }
}
