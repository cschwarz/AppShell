using SimpleInjector;

namespace AppShell
{
    public class ShellCore
    {
        public static Container Container { get; private set; }
        public static Platform CurrentPlatform { get; private set; }

        protected IPluginProvider pluginProvider;
        
        public static void InitializeContainer()
        {
            Container = new Container();
        }

        public static void ShutdownContainer()
        {
            if (Container != null)
            {
                Container.Dispose();
                Container = null;
            }
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
            CurrentPlatform = Container.GetInstance<IPlatformProvider>().GetPlatform();

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