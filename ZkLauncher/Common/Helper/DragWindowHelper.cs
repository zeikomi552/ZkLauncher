using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ZkLauncher.Common.Helper
{
    public class DragWindowHelper
    {
        public static bool GetDragEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(DragEnableProperty);
        }

        public static void SetDragEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(DragEnableProperty, value);
        }

        public static readonly DependencyProperty DragEnableProperty =
            DependencyProperty.RegisterAttached("DragEnable", typeof(bool), typeof(DragWindowHelper), new PropertyMetadata(false, DragEnableChanged));

        private static void DragEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Window wnd)
            {
                wnd.MouseLeftButtonDown += (s, ee) => MouseLeftButtonDown(s, ee, wnd);
                wnd.MouseLeftButtonUp += (s, ee) => MouseLeftButtonUp(s, ee, wnd);
            }
        }


        public static double GetTop(DependencyObject obj)
        {
            return (double)obj.GetValue(TopProperty);
        }

        public static void SetTop(DependencyObject obj, double value)
        {
            obj.SetValue(TopProperty, value);
        }

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.RegisterAttached("Top", typeof(double), typeof(DragWindowHelper));


        public static double GetLeft(DependencyObject obj)
        {
            return (double)obj.GetValue(LeftProperty);
        }

        public static void SetLeft(DependencyObject obj, double value)
        {
            obj.SetValue(LeftProperty, value);
        }

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.RegisterAttached("Left", typeof(double), typeof(DragWindowHelper));


        private static void MouseLeftButtonDown(object sender, MouseButtonEventArgs e, Window wnd)
        {
            try
            {
                if (e.ButtonState != MouseButtonState.Pressed) return;

                wnd.DragMove();
            }
            catch
            {
            }
        }

        private static void MouseLeftButtonUp(object sender, MouseButtonEventArgs e, Window wnd)
        {
            try
            {
                
            }
            catch
            {
            }
        }

        private static void Window_StateChanged(object sender, MouseButtonEventArgs e, Window wnd)
        {
            wnd.SetValue(TopProperty, wnd.Top);
            wnd.SetValue(LeftProperty, wnd.Left);
        }
    }
}
