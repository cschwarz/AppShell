using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppShell
{
    public class ShellViewModel : ViewModel, INavigationService, IDetachService
    {
        public event EventHandler<IViewModel> ViewModelPushed;
        public event EventHandler<IViewModel> ViewModelPopped;

        public event EventHandler<IViewModel> ActivateDetachedRequested;
        public event EventHandler CloseRequested;

        public ObservableCollection<IViewModel> Items { get; private set; }
        public ObservableCollection<IViewModel> DetachedItems { get; private set; }

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
            serviceDispatcher.Subscribe<IDetachService>(this);

            Items = new ObservableCollection<IViewModel>();
            DetachedItems = new ObservableCollection<IViewModel>();
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (IViewModel viewModel in Items)
                viewModel.Dispose();

            serviceDispatcher.Unsubscribe<INavigationService>(this);
            serviceDispatcher.Unsubscribe<IDetachService>(this);
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

            DetachedItems.Add(viewModel);
        }

        public void PushDetached<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel
        {
            PushDetached(typeof(TViewModel), data);
        }

        public void PushDetached<TViewModel>(dynamic data) where TViewModel : class, IViewModel
        {
            PushDetached(typeof(TViewModel), ObjectExtensions.ToDictionary(data));
        }

        public void PushDetached(string viewModelType, Dictionary<string, object> data = null)
        {
            PushDetached(Type.GetType(viewModelType), data);
        }

        public void PushDetached(Type viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = viewModelFactory.GetViewModel(viewModelType, data);

            viewModel.OnActivated();

            DetachedItems.Add(viewModel);
        }

        public void CloseDetached(string name)
        {
            foreach (IViewModel viewModel in DetachedItems.Where(v => v.Name == name).ToList())
                CloseDetached(viewModel);
        }

        public void ActivateDetached(string name)
        {
            foreach (IViewModel viewModel in DetachedItems.Where(v => v.Name == name))
            {
                viewModel.OnActivated();

                if (ActivateDetachedRequested != null)
                    ActivateDetachedRequested(this, viewModel);
            }
        }

        public void CloseDetached(IViewModel viewModel)
        {
            viewModel.Dispose();
            DetachedItems.Remove(viewModel);
        }

        public bool DetachedExists(string name)
        {
            return DetachedItems.Any(d => d.Name == name);
        }

        protected virtual void OnCloseRequested()
        {
            if (CloseRequested != null)
                CloseRequested(this, EventArgs.Empty);
        }
    }
}
