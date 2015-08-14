using AppShell;

namespace AppShell.Samples.Navigation
{
    public class NavigationShellConfigurationProvider : ShellConfigurationProvider
    {
        public NavigationShellConfigurationProvider()
        {
            RegisterShellViewModel<StackShellViewModel>();

            RegisterViewModel<ViewModel1>();
        }
    }
}
