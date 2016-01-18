using AppShell.Mobile;

namespace AppShell.Samples.Navigation.Mobile
{
    public class NavigationShellApplication : ShellApplication<NavigationShellCore>
    {
    }

    public partial class NavigationApplication : NavigationShellApplication
    {
        public NavigationApplication()
        {
            InitializeComponent();
        }
    }
}
