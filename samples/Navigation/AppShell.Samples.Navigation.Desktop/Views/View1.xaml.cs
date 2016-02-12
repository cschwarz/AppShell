using AppShell.Desktop.Views;

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
