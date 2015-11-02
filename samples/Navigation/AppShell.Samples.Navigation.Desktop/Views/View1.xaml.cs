using AppShell.Desktop.Views;
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

namespace AppShell.Samples.Navigation.Desktop.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    [View(typeof(ViewModel1))]
    public partial class View1 : ShellControl
    {
        public View1()
        {
            InitializeComponent();
        }
    }
}
