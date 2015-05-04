using SimpleInjector;
using System.Collections.Generic;

namespace AppShell
{
    public class AppShellCore
    {
        public static Container Container { get; private set; }

        protected List<IPlugin> plugins;

        static AppShellCore()
        {
            Container = new Container();
        }

        public AppShellCore()
        {
            plugins = new List<IPlugin>();
        }

        public virtual void Configure()
        {
            Container.RegisterSingle<IServiceDispatcher, ServiceDispatcher>();
            Container.RegisterSingle<IViewModelFactory, ViewModelFactory>();
            Container.RegisterSingle<IPluginFactory, PluginFactory>();
        }

        public virtual void Initialize()
        {
            Container.GetInstance<IViewFactory>().Initialize();
            Container.GetInstance<IServiceDispatcher>().Initialize();

            foreach (TypeConfiguration pluginConfiguration in Container.GetInstance<IShellConfigurationProvider>().GetPlugins())
            {
                IPlugin plugin = Container.GetInstance<IPluginFactory>().GetPlugin(pluginConfiguration.Type, pluginConfiguration.Data);
                plugin.Start();

                plugins.Add(plugin);
            }
        }

        public virtual void Shutdown()
        {
            foreach (IPlugin plugin in plugins)
                plugin.Stop();
        }
    }
}