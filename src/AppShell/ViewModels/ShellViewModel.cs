﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppShell
{
    public class ShellViewModel : ViewModel, INavigationService
    {
        public event EventHandler<IViewModel> DetachViewModelRequested;

        public ObservableCollection<IViewModel> Items { get; private set; }

        private IViewModel activeItem;
        public IViewModel ActiveItem
        {
            get { return activeItem; }
            set
            {
                activeItem = value;
                OnPropertyChanged("ActiveItem");
            }
        }

        private IViewModelFactory viewModelFactory;

        public ShellViewModel(IShellConfigurationProvider configurationProvider, IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;

            serviceDispatcher.Subscribe<INavigationService>(this);

            Items = new ObservableCollection<IViewModel>(configurationProvider.GetViewModels().Select(v => viewModelFactory.GetViewModel(v.Type, v.Data)));

            if (Items.Any())
                ActiveItem = Items.First();
        }

        public void Push<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel
        {
            Push(typeof(TViewModel).AssemblyQualifiedName, data);
        }

        public void Push(string viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = viewModelFactory.GetViewModel(Type.GetType(viewModelType), data);

            Items.Add(viewModel);
            ActiveItem = viewModel;
        }

        public void Pop()
        {
            Items.Remove(Items.Last());

            if (Items.Any())
                ActiveItem = Items.Last();
            else
                ActiveItem = null;
        }

        public void DetachActive()
        {
            IViewModel viewModel = Items.Last();

            Items.Remove(viewModel);

            if (Items.Any())
                ActiveItem = Items.Last();
            else
                ActiveItem = null;

            if (DetachViewModelRequested != null)
                DetachViewModelRequested(this, viewModel);
        }
    }
}
