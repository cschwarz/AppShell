using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    public interface IShellConfigurationProvider
    {
        IEnumerable<Type> GetSplashScreens();
        IEnumerable<Type> GetPlugins();
        IEnumerable<Type> GetViewModels();
        Type GetShellViewModel();
    }
}
