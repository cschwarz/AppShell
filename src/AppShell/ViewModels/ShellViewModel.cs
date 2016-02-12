using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppShell
{
    public class ShellViewModel : ViewModel, INavigationService
    {
        public event EventHandler<IViewModel> ViewModelPushed;
        public event EventHandler<IViewModel> ViewModelPopped;

        public event EventHandler<IViewModel> DetachViewModelRequested;
        public event EventHandler CloseRequested;

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

        protected IServiceDispatcher serviceDispatcher;
        protected IViewModelFactory viewModelFactory;

        public ShellViewModel(IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
        {
            this.serviceDispatcher = serviceDispatcher;
            this.viewModelFactory = viewModelFactory;

            serviceDispatcher.Subscribe<INavigationService>(this);

            Items = new ObservableCollection<IViewModel>();
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (IViewModel viewModel in Items)
                viewModel.Dispose();

            serviceDispatcher.Unsubscribe<INavigationService>(this);
        }

        public void Push<TViewModel>(Dictionary<string, object> data = null, bool replace = false) where TViewModel : class, IViewModel
        {
            Push(typeof(TViewModel), data, replace);
        }

        public void Push<TViewModel>(dynamic data, bool replace = false) where TViewModel : class, IViewModel
        {
            Push(typeof(TViewModel), ObjectExtensions.ToDictionary(data), replace);
        }

        public void Push(string viewModelType, Dictionary<string, object> data = null, bool replace = false)
        {
            Push(Type.GetType(viewModelType), data, replace);
        }

        public void Push(Type viewModelType, Dictionary<string, object> data = null, bool replace = false)
        {
            IViewModel viewModel = viewModelFactory.GetViewModel(viewModelType, data);

            Items.Add(viewModel);

            if (replace)
                Items.Remove(ActiveItem);

            ActiveItem = viewModel;

            ActiveItem.OnActivated();

            if (ViewModelPushed != null)
                ViewModelPushed(this, viewModel);
        }

        public void Pop()
        {
            IViewModel viewModel = Items.Last();
            viewModel.Dispose();

            Items.Remove(viewModel);

            if (Items.Any())
                ActiveItem = Items.Last();
            else
                ActiveItem = null;

            if (ActiveItem != null)
                ActiveItem.OnActivated();

            if (ViewModelPopped != null)
                ViewModelPopped(this, viewModel);

            if (!Items.Any())
                OnCloseRequested();
        }

        public IViewModel GetActive()
        {
            return ActiveItem;
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

        protected virtual void OnCloseRequested()
        {
            if (CloseRequested != null)
                CloseRequested(this, EventArgs.Empty);
        }
    }
}
