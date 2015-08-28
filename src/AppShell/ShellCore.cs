using SimpleInjector;
using System.Collections.Generic;
using System.Linq;

namespace AppShell
{
    public class ShellCore
    {
        public static Container Container { get; private set; }

        protected IPluginProvider pluginProvider;

        static ShellCore()
        {
            Container = new Container();
        }

        public virtual void Configure()
        {
            Container.RegisterSingleton<IServiceDispatcher, ServiceDispatcher>();
            Container.RegisterSingleton<IViewModelFactory, ViewModelFactory>();
            Container.RegisterSingleton<IPluginFactory, PluginFactory>();
            Container.RegisterSingleton<IPluginProvider, PluginProvider>();
            Container.RegisterSingleton<IViewModelResolution, ViewModelResolution>();
            Container.RegisterSingleton<IViewResolution, ViewResolution>();
        }

        public virtual void Initialize()
        {
            Container.GetInstance<IViewModelFactory>().Initialize();
            Container.GetInstance<IViewFactory>().Initialize();
            Container.GetInstance<IServiceDispatcher>().Initialize();
            
            pluginProvider = Container.GetInstance<IPluginProvider>();

            foreach (TypeConfiguration pluginConfiguration in Container.GetInstance<IShellConfigurationProvider>().GetPlugins())
                pluginProvider.StartPlugin(pluginConfiguration.Type, pluginConfiguration.Data);
        }

        public virtual void Shutdown()
        {
            pluginProvider.ShutdownPlugins();
        }
    }
}