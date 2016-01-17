using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(MasterDetailShellViewModel))]
    public class MasterDetailShellPage : MasterDetailPage, IPageReady
    {
        public static readonly BindableProperty MasterViewModelProperty = BindableProperty.Create<MasterDetailShellPage, IViewModel>(d => d.MasterViewModel, null, propertyChanged: MasterViewModelPropertyChanged);
        public static readonly BindableProperty DetailViewModelsProperty = BindableProperty.Create<MasterDetailShellPage, IEnumerable<IViewModel>>(d => d.DetailViewModels, null, propertyChanged: DetailViewModelsPropertyChanged);

        public IViewModel MasterViewModel { get { return (IViewModel)GetValue(MasterViewModelProperty); } set { SetValue(MasterViewModelProperty, value); } }
        public IEnumerable<IViewModel> DetailViewModels { get { return (IEnumerable<IViewModel>)GetValue(DetailViewModelsProperty); } set { SetValue(DetailViewModelsProperty, value); } }

        public static void MasterViewModelPropertyChanged(BindableObject d, IViewModel oldValue, IViewModel newValue)
        {
            MasterDetailShellPage masterDetailShellPage = d as MasterDetailShellPage;

            if (newValue != null)
                masterDetailShellPage.Master = ShellViewPage.Create(masterDetailShellPage.viewFactory.GetView(newValue));
        }

        public static void DetailViewModelsPropertyChanged(BindableObject d, IEnumerable<IViewModel> oldValue, IEnumerable<IViewModel> newValue)
        {
            MasterDetailShellPage masterDetailShellPage = d as MasterDetailShellPage;

            if (oldValue != null)
            {
                if (oldValue is ObservableCollection<IViewModel>)
                    (oldValue as ObservableCollection<IViewModel>).CollectionChanged -= masterDetailShellPage.MasterDetailShellPage_CollectionChanged;
            }

            if (newValue != null)
            {
                masterDetailShellPage.detailNavigationPage = new NavigationPage();
                masterDetailShellPage.IsReady = false;

                if (newValue is ObservableCollection<IViewModel>)
                    (newValue as ObservableCollection<IViewModel>).CollectionChanged += masterDetailShellPage.MasterDetailShellPage_CollectionChanged;

                foreach (IViewModel viewModel in newValue)
                    masterDetailShellPage.AddView(viewModel);
            }
        }

        private void MasterDetailShellPage_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                detailNavigationPage = new NavigationPage();
                IsReady = false;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IViewModel viewModel in e.NewItems)
                    AddView(viewModel);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IViewModel viewModel in e.OldItems)
                    detailNavigationPage.PopAsync();
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

        public MasterDetailShellPage()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();

            SetBinding(MasterViewModelProperty, new Binding("Master"));
            SetBinding(DetailViewModelsProperty, new Binding("Items"));
            SetBinding(IsPresentedProperty, new Binding("Master.IsPresented", BindingMode.TwoWay));
        }
    }
}