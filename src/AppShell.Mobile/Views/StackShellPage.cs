using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(StackShellViewModel))]
    [View(typeof(SplashScreenShellViewModel))]
    public class StackShellPage : NavigationPage, IPageReady
    {
        public static readonly BindableProperty ViewModelsProperty = BindableProperty.Create<StackShellPage, IEnumerable<IViewModel>>(d => d.ViewModels, null, propertyChanged: ViewModelsPropertyChanged);

        public IEnumerable<IViewModel> ViewModels { get { return (IEnumerable<IViewModel>)GetValue(ViewModelsProperty); } set { SetValue(ViewModelsProperty, value); } }

        public static void ViewModelsPropertyChanged(BindableObject d, IEnumerable<IViewModel> oldValue, IEnumerable<IViewModel> newValue)
        {
            StackShellPage stackShellPage = d as StackShellPage;

            if (oldValue != null)
            {
                if (oldValue is ObservableCollection<IViewModel>)
                    (oldValue as ObservableCollection<IViewModel>).CollectionChanged -= stackShellPage.StackShellPage_CollectionChanged;
            }

            if (newValue != null)
            {
                stackShellPage.IsReady = false;

                if (newValue is ObservableCollection<IViewModel>)
                    (newValue as ObservableCollection<IViewModel>).CollectionChanged += stackShellPage.StackShellPage_CollectionChanged;

                foreach (IViewModel viewModel in newValue)
                    stackShellPage.AddView(viewModel);
            }
        }

        private void StackShellPage_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
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
                    Navigation.RemovePage(Navigation.NavigationStack.Single(p => p.BindingContext == viewModel));
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
            await PushAsync(ShellViewPage.Create(viewFactory.GetView(viewModel)));
            IsReady = true;
        }

        private IViewFactory viewFactory;
        private bool ignoreUpdate;

        public StackShellPage()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();

            Popped += StackShellPage_Popped;

            SetBinding(ViewModelsProperty, new Binding("Items"));
        }

        private void StackShellPage_Popped(object sender, NavigationEventArgs e)
        {
            if (ignoreUpdate)
            {
                ignoreUpdate = false;
                return;
            }

            if (ViewModels is ObservableCollection<IViewModel>)
                (ViewModels as ObservableCollection<IViewModel>).CollectionChanged -= StackShellPage_CollectionChanged;

            if (BindingContext is ShellViewModel)
                (BindingContext as ShellViewModel).Pop();

            if (ViewModels is ObservableCollection<IViewModel>)
                (ViewModels as ObservableCollection<IViewModel>).CollectionChanged += StackShellPage_CollectionChanged;
        }
    }
}
