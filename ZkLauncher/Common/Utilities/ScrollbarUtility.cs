using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace ZkLauncher.Common.Utilities
{
    public class ScrollbarUtility
    {
        #region スクロールバー制御
        /// <summary>
        /// スクロールバー制御
        /// 選択している行にスクロールバーを移動する
        /// </summary>
        /// <param name="dg">DataGrid</param>
        public static void TopRow(DataGrid dg)
        {
            dg.ScrollIntoView(dg.Items[dg.SelectedIndex]); // 選択行にスクロールが移動
            dg.UpdateLayout();

        }
        #endregion

        #region スクロールバー制御
        /// <summary>
        /// スクロールバー制御
        /// 選択している行にスクロールバーを移動する
        /// </summary>
        /// <param name="item">DataGrid</param>
        public static void TopRow(ListView item)
        {
            item.ScrollIntoView(item.Items[item.SelectedIndex]); // 選択行にスクロールが移動
            item.UpdateLayout();

        }
        #endregion


        #region スクロールバー制御
        /// <summary>
        /// スクロールバー制御
        /// 選択している行にスクロールバーを移動する
        /// </summary>
        /// <param name="item">ListBox</param>
        public static void TopRow(int oldidx, int newidx, ListBox newitem)
        {
            var peer = ItemsControlAutomationPeer.CreatePeerForElement(newitem);

            // GetPatternでIScrollProviderを取得
            var scrollProvider = peer.GetPattern(PatternInterface.Scroll) as IScrollProvider;

            var count = newitem.Items.Count;

            int loopmax =  20;

            oldidx = oldidx >= 0 ? oldidx : 0;

            if (scrollProvider != null)
            {
                var tmp = scrollProvider.HorizontalScrollPercent;
                var percent = 100.0 / ((double)count-1);
                var oldpos = percent * oldidx >= 0 ? oldidx : 0;

                for (int i = 1; i <= loopmax; i++)
                {
                    var nextpos = oldpos * percent + percent * ((newidx - oldidx) / (double)loopmax * i);

                    if(nextpos < 0 ) nextpos = 0;
                    else if(nextpos > 100) nextpos = 100;


                    // ちょっとスクロール
                    scrollProvider.SetScrollPercent(
                    // 水平方向にはちょっとだけスクロール
                    nextpos,
                    // 垂直方向にはスクロールしない
                    scrollProvider.VerticalScrollPercent);
                    newitem.UpdateLayout();
                }
            }

            //newitem.ScrollIntoView(newitem.Items[newitem.SelectedIndex]); // 選択行にスクロールが移動
            newitem.UpdateLayout();

        }
        #endregion
    }
}
