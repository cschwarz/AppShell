using AppShell;
using System.Collections.Generic;

namespace AppShell.Samples.Navigation
{
    public class NavigationShellConfigurationProvider : ShellConfigurationProvider
    {
        public NavigationShellConfigurationProvider()
        {
            RegisterSplashScreen<SplashScreenNormalViewModel>();
            RegisterSplashScreen<SplashScreenSmallViewModel>();
            RegisterSplashScreen<SplashScreenLargeViewModel>();

            RegisterShellViewModel<StackShellViewModel>(new Dictionary<string, object>() { { "Name", "StackShell" } });

            RegisterViewModel<ViewModel1>();
        }
    }
}
