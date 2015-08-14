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
            Container.RegisterSingle<IServiceDispatcher, ServiceDispatcher>();
            Container.RegisterSingle<IViewModelFactory, ViewModelFactory>();
            Container.RegisterSingle<IPluginFactory, PluginFactory>();
            Container.RegisterSingle<IPluginProvider, PluginProvider>();
        }

        public virtual void Initialize()
        {
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