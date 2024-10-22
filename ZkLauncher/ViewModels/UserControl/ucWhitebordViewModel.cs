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

        private bool handle = true;
        //List<StrokePairM> _StrokeUndo = new List<StrokePairM>();
        //List<StrokePairM> _StrokeRedo = new List<StrokePairM>();
        InkCanvas _InkCanvas;
        private string _StorkePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Common\canvas1-stroke";

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
        public void Save(object sender, RoutedEventArgs ex)
        {
            try
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


        //#region Undo処理
        ///// <summary>
        ///// Undo処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public void Undo(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var wnd = VisualTreeHelperWrapper.GetWindow<ucWhitebord>(sender) as ucWhitebord;

        //        if (wnd != null)
        //        {
        //            handle = false;

        //            // 最後の変更を取り出す
        //            var tmp = this._StrokeUndo.LastOrDefault();

        //            // nullチェック
        //            if (tmp != null)
        //            {
        //                // Redo用に保存する
        //                _StrokeRedo.Add(new StrokePairM(tmp.AddedStroke, tmp.RemovedStroke));

        //                // 最後に追加された分は取り除く
        //                wnd.theInkCanvas.Strokes.Remove(tmp.AddedStroke);

        //                // 最後に取り除かれた分は追加する
        //                wnd.theInkCanvas.Strokes.Add(tmp.RemovedStroke);

        //                // Undoのリストから削除する
        //                this._StrokeUndo.Remove(tmp);
        //            }

        //            handle = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.Error(ex.Message);
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //#endregion

        //#region Redo処理
        ///// <summary>
        ///// Redo処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public void Redo(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var wnd = VisualTreeHelperWrapper.GetWindow<ucWhitebord>(sender) as ucWhitebord;

        //        if (wnd != null)
        //        {
        //            handle = false;

        //            // 最後の変更を取り出す
        //            var tmp = this._StrokeRedo.LastOrDefault();

        //            if (tmp != null)
        //            {
        //                // Undoで消されたストロークを追加
        //                wnd.theInkCanvas.Strokes.Add(tmp.AddedStroke);

        //                // Undoで戻されたストロークを削除
        //                wnd.theInkCanvas.Strokes.Remove(tmp.RemovedStroke);

        //                // Undo用のストロークを保存
        //                this._StrokeUndo.Add(new StrokePairM(tmp.AddedStroke, tmp.RemovedStroke));

        //                // Redo用のストロークを削除
        //                this._StrokeRedo.Remove(tmp);
        //            }

        //            handle = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.Error(ex.Message);
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //#endregion
        //#region ストロークが変化した場合の処理
        ///// <summary>
        ///// ストロークが変化した場合の処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (handle)
        //        {
        //            this._StrokeUndo.Add(new StrokePairM(e.Added, e.Removed));
        //            this._StrokeRedo.Clear();

        //            using (System.IO.FileStream fs =
        //                new System.IO.FileStream(this._StorkePath, System.IO.FileMode.Create))
        //            {
        //                this._InkCanvas.Strokes.Save(fs);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.Error(ex.Message);
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //#endregion
    }
}
