using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppShell.Samples.Todo.Mobile.Views
{
    [View(typeof(TodoViewModel))]
    public partial class TodoPage : ContentPage
    {
        public TodoPage()
        {
            InitializeComponent();
        }
    }
}
