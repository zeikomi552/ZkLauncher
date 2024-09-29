﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZkLauncher.Views.UserControls
{
    /// <summary>
    /// ucViewerPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class ucViewerPanel : UserControl
    {
        public ucViewerPanel()
        {
            InitializeComponent();
            myMediaElement.Play();
        }

        private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            myMediaElement.Position = new TimeSpan(0, 0, 1);
            myMediaElement.Play();
        }
    }
}
