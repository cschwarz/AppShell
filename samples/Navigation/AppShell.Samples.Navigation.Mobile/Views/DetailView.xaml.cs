using AppShell.Mobile.Views;
using Xamarin.Forms;

namespace AppShell.Samples.Navigation.Mobile.Views
{
    [View(typeof(DetailViewModel))]
    public partial class DetailView : ShellView
    {
        public DetailView()
        {
            InitializeComponent();
        }
    }
}
