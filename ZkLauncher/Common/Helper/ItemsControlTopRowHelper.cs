﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ZkLauncher.Common.Helper;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Common.Helper
{
    #region 説明
    /// <summary>
    /// ItemsControl(ListBox, ListView, DataGrid)の選択時にスクロールを合わせる添付プロパティ
    /// </summary>
    public class ItemsControlTopRowHelper
    {
        public static readonly DependencyProperty SelectedItemProperty
            = DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(ItemsControlTopRowHelper), new PropertyMetadata(null, SelectedItemChanged));

        public static bool GetSelectedItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch (d)
            {
                case ListBox lb:
                    {
                        if (lb.SelectedIndex >= 0)
                        {
                            ScrollbarUtility.TopRow(lb);
                        }
                        break;
                    }
                case DataGrid dg:
                    {
                        if (dg.SelectedIndex >= 0)
                            ScrollbarUtility.TopRow(dg);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }



        }
    }
    #endregion

}
