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

        public ShellViewModel(IShellConfigurationProvider configurationProvider, IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
        {
            this.serviceDispatcher = serviceDispatcher;
            this.viewModelFactory = viewModelFactory;

            serviceDispatcher.Subscribe<INavigationService>(this);

            Items = new ObservableCollection<IViewModel>();
        }

        public override void Dispose()
        {
            base.Dispose();

            serviceDispatcher.Unsubscribe<INavigationService>(this);
        }

        public void Push<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel
        {
            Push(typeof(TViewModel), data);
        }

        public void Push(string viewModelType, Dictionary<string, object> data = null)
        {
            Push(Type.GetType(viewModelType), data);
        }

        public void Push(Type viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = viewModelFactory.GetViewModel(viewModelType, data);

            Items.Add(viewModel);
            ActiveItem = viewModel;

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

            if (ViewModelPopped != null)
                ViewModelPopped(this, viewModel);

            if (!Items.Any())
            {
                if (CloseRequested != null)
                    CloseRequested(this, EventArgs.Empty);
            }
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
    }
}
