using AppShell.Mobile.WinRT;

namespace AppShell.Samples.Navigation.Mobile.WinRT
{
    public class NavigationShellPage : ShellPage<NavigationApplication>
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
