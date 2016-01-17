using SimpleInjector;
using System;
using System.Collections.Generic;

namespace AppShell
{
    public abstract class ShellCore : IShellNavigationService
    {
        public static Container Container { get; private set; }
        public static Platform CurrentPlatform { get; private set; }

        public string Name { get { return "ShellCore"; } }        
        public event EventHandler<IViewModel> ShellViewModelPushed;

        protected IDictionary<string, IViewModel> shells;
        protected IPluginProvider pluginProvider;
        protected IServiceDispatcher serviceDispatcher;
        
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

        public ShellCore()
        {
            shells = new Dictionary<string, IViewModel>();
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

            /*
            foreach (TypeConfiguration pluginConfiguration in Container.GetInstance<IShellConfigurationProvider>().GetPlugins())
                pluginProvider.StartPlugin(pluginConfiguration.Type, pluginConfiguration.Data);*/

            serviceDispatcher = Container.GetInstance<IServiceDispatcher>();
            serviceDispatcher.Subscribe<IShellNavigationService>(this);

            Run();
        }

        public abstract void Run();

        public virtual void Shutdown()
        {
            serviceDispatcher.Unsubscribe<IShellNavigationService>(this);

            pluginProvider.ShutdownPlugins();
        }

        public void Push(string name)
        {
            if (!shells.ContainsKey(name))
                throw new Exception();

            if (ShellViewModelPushed != null)
                ShellViewModelPushed(this, shells[name]);
        }

        public void Push<TViewModel>(Dictionary<string, object> data = null) where TViewModel : ShellViewModel
        {
            Push(typeof(TViewModel), data);
        }

        public void Push<TViewModel>(dynamic data) where TViewModel : ShellViewModel
        {
            Push(typeof(TViewModel), ObjectExtensions.ToDictionary(data));
        }

        public void Push(string viewModelType, Dictionary<string, object> data = null)
        {
            Push(Type.GetType(viewModelType), data);
        }

        public void Push(Type viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = Container.GetInstance<IViewModelFactory>().GetViewModel(viewModelType, data);
            shells.Add(viewModel.Name, viewModel);

            if (ShellViewModelPushed != null)
                ShellViewModelPushed(this, viewModel);
        }

        public void Pop()
        {
            throw new NotImplementedException();
        }
    }
}