using System;
using System.Collections.Generic;

namespace AppShell
{
    public class MasterDetailShellViewModel : ShellViewModel, IMasterDetailNavigationService
    {
        public event EventHandler<IViewModel> RootViewModelPushed;

        private IViewModel master;
        public IViewModel Master
        {
            get { return master; }
            set
            {
                if (master != value)
                {
                    master = value;
                    OnPropertyChanged();
                }
            }
        }

        public MasterDetailShellViewModel(IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
            : base(serviceDispatcher, viewModelFactory)
        {
            serviceDispatcher.Subscribe<IMasterDetailNavigationService>(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            serviceDispatcher.Unsubscribe<IMasterDetailNavigationService>(this);
        }

        public void PushRoot<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel
        {
            PushRoot(typeof(TViewModel), data);
        }

        public void PushRoot<TViewModel>(dynamic data) where TViewModel : class, IViewModel
        {
            PushRoot(typeof(TViewModel), ObjectExtensions.ToDictionary(data));
        }

        public void PushRoot(string viewModelType, Dictionary<string, object> data = null)
        {
            PushRoot(Type.GetType(viewModelType), data);
        }

        public void PushRoot(Type viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = viewModelFactory.GetViewModel(viewModelType, data);

            foreach (IViewModel item in Items)
                item.Dispose();

            Items.Clear();
            Items.Add(viewModel);
            ActiveItem = viewModel;

            if (RootViewModelPushed != null)
                RootViewModelPushed(this, viewModel);
        }
    }
}
