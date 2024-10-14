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

            bool nextf = oldidx < newidx;
            int loopmax =  Math.Abs(newidx - oldidx) > 2 ? 200 : 100;

            if (scrollProvider != null)
            {
                for (int i = 0; i < loopmax; i++)
                {
                    // ちょっとスクロール
                    scrollProvider.Scroll(
                        // 水平方向にはスクロールしない
                        nextf ? ScrollAmount.SmallIncrement : ScrollAmount.SmallDecrement,
                        // 垂直方向にはちょっとだけ下にスクロール
                        ScrollAmount.NoAmount);
                    newitem.UpdateLayout();
                }

            }

            newitem.ScrollIntoView(newitem.Items[newitem.SelectedIndex]); // 選択行にスクロールが移動
            newitem.UpdateLayout();

        }
        #endregion
    }
}
