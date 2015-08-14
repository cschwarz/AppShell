using AppShell;

namespace AppShell.Samples.Navigation
{
    public class NavigationShellConfigurationProvider : ShellConfigurationProvider
    {
        public NavigationShellConfigurationProvider()
        {
            RegisterShellViewModel<TabShellViewModel>();

            RegisterViewModel<ViewModel1>();
            RegisterViewModel<ViewModel2>();
        }
    }
}
