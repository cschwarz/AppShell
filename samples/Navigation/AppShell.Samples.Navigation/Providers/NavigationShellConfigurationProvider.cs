using AppShell;

namespace AppShell.Samples.Navigation
{
    public class NavigationShellConfigurationProvider : ShellConfigurationProvider
    {
        public NavigationShellConfigurationProvider()
        {
            RegisterSplashScreen<SplashScreenNormalViewModel>();
            RegisterSplashScreen<SplashScreenSmallViewModel>();
            RegisterSplashScreen<SplashScreenLargeViewModel>();

            RegisterShellViewModel<StackShellViewModel>();

            RegisterViewModel<ViewModel1>();
        }
    }
}
