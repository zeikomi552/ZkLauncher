﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZkLauncher.Models
{
    /// <summary>
    /// スクリーンショット作成クラス
    /// </summary>
    public class ScreenShotM : BindableBase
    {
        #region スクリーンショットの実行処理
        /// <summary>
        /// スクリーンショットの実行処理
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <param name="filepath">ファイルパス</param>
        public static void ExecuteScreenShot(FrameworkElement ctrl, string filepath)
        {
            var targetPoint = ctrl.PointToScreen(new System.Windows.Point(0.0d, 0.0d));

            // キャプチャ領域の生成
            var targetRect = new Rect(targetPoint.X, targetPoint.Y, ctrl.ActualWidth, ctrl.ActualHeight);

            //// スクリーンショット実行
            ExecuteScreenShot(targetRect, filepath);
        }
        #endregion


        #region スクリーンショットの作成処理
        /// <summary>
        /// スクリーンショットの作成処理
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <param name="fileName">ファイル名</param>
        public static void ExecuteScreenShot(Rect rect, string fileName)
        {
            using (var bitmap = ExecuteScreenShotToBitmap(rect))
            {
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        #endregion


        #region スクリーンショットの実行処理
        /// <summary>
        /// スクリーンショットの実行処理
        /// 上位でBitmapをDisposeする必要あり
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ExecuteScreenShot(FrameworkElement ctrl)
        {
            var targetPoint = ctrl.PointToScreen(new System.Windows.Point(0.0d, 0.0d));

            // キャプチャ領域の生成
            var targetRect = new Rect(targetPoint.X, targetPoint.Y, ctrl.ActualWidth, ctrl.ActualHeight);

            //// スクリーンショット実行
            return ExecuteScreenShotToBitmap(targetRect);
        }
        #endregion

        #region スクリーンショットの作成処理
        /// <summary>
        /// スクリーンショットの作成処理
        /// 上位でBitmapをDisposeする必要あり
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ExecuteScreenShotToBitmap(Rect rect)
        {
            // 矩形と同じサイズのBitmapを作成
            var bitmap = new Bitmap((int)rect.Width, (int)rect.Height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                // 画面から指定された矩形と同じ条件でコピー
                graphics.CopyFromScreen((int)rect.X, (int)rect.Y, 0, 0, bitmap.Size);

                // 画像ファイルとして保存
                return bitmap;
            }
        }
        #endregion
    }
}
