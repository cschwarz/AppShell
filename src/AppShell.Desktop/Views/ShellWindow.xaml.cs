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

        public ShellWindow(ShellViewModel shellViewModel)
        {
            InitializeComponent();

            this.shellViewModel = shellViewModel;

            shellViewModel.CloseRequested += ShellViewModel_CloseRequested;
            shellViewModel.DetachViewModelRequested += ShellViewModel_DetachViewModelRequested;
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
