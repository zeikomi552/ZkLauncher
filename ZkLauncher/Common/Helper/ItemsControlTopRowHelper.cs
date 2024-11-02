using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Common.Helper
{
    public class ItemsControlTopRowHelper
    {
        public static readonly DependencyProperty AutoScrollToSelectedItemProperty =
                DependencyProperty.RegisterAttached(
                    "AutoScrollToSelectedItem",
                    typeof(bool),
                    typeof(ItemsControlTopRowHelper),
                    new PropertyMetadata(false, OnAutoScrollToSelectedItemChanged));

        public static bool GetAutoScrollToSelectedItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToSelectedItemProperty);
        }

        public static void SetAutoScrollToSelectedItem(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToSelectedItemProperty, value);
        }

        private static void OnAutoScrollToSelectedItemChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox && e.NewValue is bool isEnabled)
            {
                if (isEnabled)
                {
                    listBox.SelectionChanged += ListBox_SelectionChanged;
                }
                else
                {
                    listBox.SelectionChanged -= ListBox_SelectionChanged;
                }
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                listBox.Dispatcher.BeginInvoke(new Action(() =>
                {
                    listBox.UpdateLayout();
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
