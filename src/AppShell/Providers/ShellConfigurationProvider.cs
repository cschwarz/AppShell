using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    public class TypeConfiguration
    {
        public Type Type { get; private set; }
        public Dictionary<string, object> Data { get; private set; }

        public TypeConfiguration(Type type, Dictionary<string, object> data = null)
        {
            Type = type;
            Data = data;
        }
    }

    public abstract class ShellConfigurationProvider : IShellConfigurationProvider
    {
        protected List<TypeConfiguration> splashScreens;
        protected List<TypeConfiguration> plugins;
        protected List<TypeConfiguration> viewModels;
        protected TypeConfiguration shellViewModel;

        public ShellConfigurationProvider()
        {
            splashScreens = new List<TypeConfiguration>();
            plugins = new List<TypeConfiguration>();
            viewModels = new List<TypeConfiguration>();            
        }

        public virtual IEnumerable<TypeConfiguration> GetSplashScreens()
        {
            return splashScreens;
        }

        public virtual IEnumerable<TypeConfiguration> GetPlugins()
        {
            return plugins;
        }

        public virtual IEnumerable<TypeConfiguration> GetViewModels()
        {
            return viewModels;
        }

        public virtual TypeConfiguration GetShellViewModel()
        {
            return shellViewModel;
        }

        public void RegisterPlugin<T>(Dictionary<string, object> data = null)
        {
            plugins.Add(new TypeConfiguration(typeof(T), data));
        }

        public void RegisterSplashScreen<T>(Dictionary<string, object> data = null)
        {
            splashScreens.Add(new TypeConfiguration(typeof(T), data));
        }

        public void RegisterViewModel<T>(Dictionary<string, object> data = null)
        {
            viewModels.Add(new TypeConfiguration(typeof(T), data));
        }

        public void RegisterShellViewModel<T>(Dictionary<string, object> data = null)
        {
            shellViewModel = new TypeConfiguration(typeof(T), data);
        }
    }
}
