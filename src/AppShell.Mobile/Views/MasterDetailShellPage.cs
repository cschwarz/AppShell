using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(MasterDetailShellViewModel))]
    public class MasterDetailShellPage : MasterDetailPage, IPageReady
    {
        public static readonly BindableProperty MasterViewModelProperty = BindableProperty.Create("MasterViewModel", typeof(IViewModel), typeof(MasterDetailShellPage), null, propertyChanged: MasterViewModelPropertyChanged);
        public static readonly BindableProperty DetailViewModelsProperty = BindableProperty.Create("DetailViewModels", typeof(IEnumerable<IViewModel>), typeof(MasterDetailShellPage), null, propertyChanged: DetailViewModelsPropertyChanged);

        public IViewModel MasterViewModel { get { return (IViewModel)GetValue(MasterViewModelProperty); } set { SetValue(MasterViewModelProperty, value); } }
        public IEnumerable<IViewModel> DetailViewModels { get { return (IEnumerable<IViewModel>)GetValue(DetailViewModelsProperty); } set { SetValue(DetailViewModelsProperty, value); } }

        public static void MasterViewModelPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            MasterDetailShellPage masterDetailShellPage = (MasterDetailShellPage)bindableObject;
            IViewModel newViewModel = (IViewModel)newValue;

            if (newViewModel != null)
                masterDetailShellPage.Master = ShellViewPage.Create(masterDetailShellPage.viewFactory.GetView(newViewModel));
        }

        public static void DetailViewModelsPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            MasterDetailShellPage masterDetailShellPage = (MasterDetailShellPage)bindableObject;
            IEnumerable<IViewModel> oldViewModels = (IEnumerable<IViewModel>)oldValue;
            IEnumerable<IViewModel> newViewModels = (IEnumerable<IViewModel>)newValue;

            if (oldViewModels != null)
            {
                if (oldViewModels is ObservableCollection<IViewModel>)
                    (oldViewModels as ObservableCollection<IViewModel>).CollectionChanged -= masterDetailShellPage.MasterDetailShellPage_CollectionChanged;
            }

            if (newViewModels != null)
            {
                masterDetailShellPage.CreateDetailPage();
                masterDetailShellPage.IsReady = false;

                if (newViewModels is ObservableCollection<IViewModel>)
                    (newViewModels as ObservableCollection<IViewModel>).CollectionChanged += masterDetailShellPage.MasterDetailShellPage_CollectionChanged;

                foreach (IViewModel viewModel in newViewModels)
                    masterDetailShellPage.AddView(viewModel);
            }
        }

        private void MasterDetailShellPage_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                CreateDetailPage();
                IsReady = false;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IViewModel viewModel in e.NewItems)
                    AddView(viewModel);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                ignoreUpdate = true;

                foreach (IViewModel viewModel in e.OldItems)
                    detailNavigationPage.Navigation.RemovePage(detailNavigationPage.Navigation.NavigationStack.Single(p => p.BindingContext == viewModel));
            }
        }

        public event EventHandler<Page> Ready;

        private bool isReady;
        public bool IsReady
        {
            get { return isReady; }
            private set
            {
                if (isReady != value)
                {
                    if (value && !isReady && Ready != null)
                        Ready(this, this);

                    isReady = value;
                }
            }
        }

        private async void AddView(IViewModel viewModel)
        {
            await detailNavigationPage.PushAsync(ShellViewPage.Create(viewFactory.GetView(viewModel)));
            Detail = detailNavigationPage;
            IsReady = true;
        }

        private IViewFactory viewFactory;
        private NavigationPage detailNavigationPage;
        private bool ignoreUpdate;

        public MasterDetailShellPage()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();

            SetBinding(MasterViewModelProperty, new Binding("Master"));
            SetBinding(DetailViewModelsProperty, new Binding("Items"));
            SetBinding(IsPresentedProperty, new Binding("Master.IsPresented", BindingMode.TwoWay));
        }

        private void CreateDetailPage()
        {
            if (detailNavigationPage != null)
                detailNavigationPage.Popped -= DetailNavigationPage_Popped;

            detailNavigationPage = new NavigationPage();
            detailNavigationPage.Popped += DetailNavigationPage_Popped;
        }

        private void DetailNavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            if (ignoreUpdate)
            {
                ignoreUpdate = false;
                return;
            }

            if (DetailViewModels is ObservableCollection<IViewModel>)
                (DetailViewModels as ObservableCollection<IViewModel>).CollectionChanged -= MasterDetailShellPage_CollectionChanged;

            if (BindingContext is ShellViewModel)
                (BindingContext as ShellViewModel).Pop();

            if (DetailViewModels is ObservableCollection<IViewModel>)
                (DetailViewModels as ObservableCollection<IViewModel>).CollectionChanged += MasterDetailShellPage_CollectionChanged;
        }
    }
}