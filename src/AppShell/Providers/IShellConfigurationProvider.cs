using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
