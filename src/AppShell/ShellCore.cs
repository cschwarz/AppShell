﻿using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AppShell
{
    public abstract class ShellCore : IShellNavigationService, INotifyPropertyChanged
    {
        public static Container Container { get; private set; }
        public static Platform CurrentPlatform { get; private set; }

        public string Name { get { return "ShellCore"; } }

        private ShellViewModel activeShell;
        public ShellViewModel ActiveShell
        {
            get { return activeShell; }
            private set
            {
                if (activeShell != value)
                {
                    activeShell = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("ActiveShell"));
                }
            }
        }

        public ObservableCollection<ShellViewModel> Shells { get; private set; }

        protected IPluginProvider pluginProvider;
        protected IServiceDispatcher serviceDispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

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
            Shells = new ObservableCollection<ShellViewModel>();
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
            ShellViewModel viewModel = Container.GetInstance<IViewModelFactory>().GetViewModel(viewModelType, data) as ShellViewModel;
            Shells.Add(viewModel);
            ActiveShell = viewModel;

            ActiveShell.OnActivated();
        }

        public void Pop()
        {
            if (!Shells.Any())
                return;
            Shells.Remove(Shells.Last());
            if (!Shells.Any())
                return;
            ActiveShell = Shells.Last();

            ActiveShell.OnActivated();
            if (ActiveShell.ActiveItem != null)
                ActiveShell.ActiveItem.OnActivated();
        }
    }
}