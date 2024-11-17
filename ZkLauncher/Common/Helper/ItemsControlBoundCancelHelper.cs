using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZkLauncher.Common.Helper
{
    public class ItemsControlBoundCancelHelper
    {
        public static bool GetBoundaryCancelF(DependencyObject obj)
        {
            return (bool)obj.GetValue(GetBoundaryCancelFProperty);
        }

        public static void SetGetBoundaryCancelF(DependencyObject obj, bool value)
        {
            obj.SetValue(GetBoundaryCancelFProperty, value);
        }

        public static readonly DependencyProperty GetBoundaryCancelFProperty =
            DependencyProperty.RegisterAttached("GetBoundaryCancelF", typeof(bool), typeof(ItemsControlBoundCancelHelper), new PropertyMetadata(false, GetBoundaryCancelFChanged));

        private static void GetBoundaryCancelFChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemsControl ic && e.NewValue is bool isCanceled)
            {
                if (isCanceled)
                {
                    ic.ManipulationBoundaryFeedback += Ic_ManipulationBoundaryFeedback;
                }
                else
                {
                    ic.ManipulationBoundaryFeedback -= Ic_ManipulationBoundaryFeedback;
                }
            }
        }

        private static void Ic_ManipulationBoundaryFeedback(object? sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}
