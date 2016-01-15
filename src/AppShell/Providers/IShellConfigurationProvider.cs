using System.Collections.Generic;

namespace AppShell
{
    public interface IShellConfigurationProvider
    {
        IEnumerable<TypeConfiguration> GetSplashScreens();
        IEnumerable<TypeConfiguration> GetPlugins();
        IEnumerable<TypeConfiguration> GetViewModels();
        TypeConfiguration GetShellViewModel();
    }
}
