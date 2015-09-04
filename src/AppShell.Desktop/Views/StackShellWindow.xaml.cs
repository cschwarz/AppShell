using System;
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
    /// Interaction logic for StackShellWindow.xaml
    /// </summary>
    [View(typeof(StackShellViewModel))]
    public partial class StackShellWindow : Window
    {
        public StackShellWindow()
        {            
            InitializeComponent();

            DataContextChanged += StackShellWindow_DataContextChanged;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DataContextChanged -= StackShellWindow_DataContextChanged;
        }

        private void StackShellWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is StackShellViewModel)
            {
                (DataContext as StackShellViewModel).DetachViewModelRequested += DetachViewModelRequested;
                (DataContext as StackShellViewModel).CloseRequested += CloseRequested;
            }
        }

        private void DetachViewModelRequested(object sender, IViewModel e)
        {
            DetachedWindow detachedWindow = new DetachedWindow();
            detachedWindow.DataContext = e;
            detachedWindow.Show();
        }

        private void CloseRequested(object sender, EventArgs e)
        {
            Close();
        }
    }
}
