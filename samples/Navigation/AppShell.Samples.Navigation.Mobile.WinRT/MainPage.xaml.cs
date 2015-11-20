using AppShell.Mobile;
using AppShell.Mobile.WinRT;

namespace AppShell.Samples.Navigation.Mobile.WinRT
{
    public class NavigationShellPage : ShellPage<ShellApplication<NavigationShellCore>>
    {
    }

    public sealed partial class MainPage : NavigationShellPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Init();
        }
    }
}
