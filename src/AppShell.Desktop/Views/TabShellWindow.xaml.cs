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
using System.Windows.Shapes;

namespace AppShell.Desktop
{
    /// <summary>
    /// Interaction logic for TabShellWindow.xaml
    /// </summary>
    [View(typeof(TabShellViewModel))]
    public partial class TabShellWindow : Window
    {
        public TabShellWindow()
        {
            InitializeComponent();
        }
    }
}
