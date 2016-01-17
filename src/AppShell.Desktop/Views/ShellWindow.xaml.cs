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
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        private ShellViewModel shellViewModel;

        public ShellWindow()
        {
            InitializeComponent();

            DataContextChanged += ShellWindow_DataContextChanged;
        }

        private void ShellWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ShellViewModel oldShellViewModel = e.OldValue as ShellViewModel;

                oldShellViewModel.CloseRequested += ShellViewModel_CloseRequested;
                oldShellViewModel.DetachViewModelRequested += ShellViewModel_DetachViewModelRequested;

            }
            if (e.NewValue != null)
            {
                ShellViewModel newShellViewModel = e.NewValue as ShellViewModel;

                shellViewModel = newShellViewModel;

                newShellViewModel.CloseRequested += ShellViewModel_CloseRequested;
                newShellViewModel.DetachViewModelRequested += ShellViewModel_DetachViewModelRequested;
            }
        }
                
        private void ShellViewModel_CloseRequested(object sender, EventArgs e)
        {
            Close();
        }

        private void ShellViewModel_DetachViewModelRequested(object sender, IViewModel e)
        {
            DetachedWindow detachedWindow = new DetachedWindow();
            detachedWindow.DataContext = e;
            detachedWindow.Show();
        }
    }
}
