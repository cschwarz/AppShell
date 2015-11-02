using AppShell.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppShell.Samples.Navigation.Mobile.Views
{
    [View(typeof(ViewModel1))]
    public partial class View1 : ShellView
    {
        public View1()
        {
            InitializeComponent();
        }
    }
}
