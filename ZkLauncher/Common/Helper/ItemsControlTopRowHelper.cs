using Microsoft.Xaml.Behaviors;
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
                case ListBox vl:
                    {
                        if (vl.SelectedIndex >= 0)
                        {
                            ScrollbarUtility.TopRow(vl);
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
