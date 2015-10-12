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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppShell.Desktop.Views
{
    /// <summary>
    /// Interaction logic for StackShellView.xaml
    /// </summary>
    [View(typeof(StackShellViewModel))]
    public partial class StackShellView : UserControl
    {
        public StackShellView()
        {
            InitializeComponent();
        }
    }
}
