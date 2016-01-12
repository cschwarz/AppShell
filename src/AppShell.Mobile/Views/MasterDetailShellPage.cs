using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(MasterDetailShellViewModel))]
    public class MasterDetailShellPage : MasterDetailPage
    {
        public static readonly BindableProperty MasterViewModelProperty = BindableProperty.Create<MasterDetailShellPage, IViewModel>(d => d.MasterViewModel, null, propertyChanged: MasterViewModelPropertyChanged);

        public IViewModel MasterViewModel { get { return (IViewModel)GetValue(MasterViewModelProperty); } set { SetValue(MasterViewModelProperty, value); } }

        public static void MasterViewModelPropertyChanged(BindableObject d, IViewModel oldValue, IViewModel newValue)
        {
            MasterDetailShellPage masterDetailShellPage = d as MasterDetailShellPage;

            if (newValue != null)
                masterDetailShellPage.Master = ShellViewPage.Create(masterDetailShellPage.viewFactory.GetView(newValue));
        }

        private IViewFactory viewFactory;
        private NavigationPage detailNavigationPage;
        private MasterDetailShellViewModel shellViewModel;

        public MasterDetailShellPage()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();

            SetBinding(MasterViewModelProperty, new Binding("Master"));
            SetBinding(IsPresentedProperty, new Binding("Master.IsPresented", BindingMode.TwoWay));

            detailNavigationPage = new NavigationPage();
            Detail = detailNavigationPage;

            detailNavigationPage.Popped += DetailNavigationPage_Popped;
        }

        private void DetailNavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            if (shellViewModel != null)
            {
                shellViewModel.ViewModelPopped -= ShellViewModel_ViewModelPopped;
                shellViewModel.Pop();
                shellViewModel.ViewModelPopped += ShellViewModel_ViewModelPopped;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            shellViewModel = BindingContext as MasterDetailShellViewModel;

            shellViewModel.ViewModelPushed += ShellViewModel_ViewModelPushed;
            shellViewModel.ViewModelPopped += ShellViewModel_ViewModelPopped;
            shellViewModel.RootViewModelPushed += ShellViewModel_RootViewModelPushed;

            foreach (IViewModel viewModel in shellViewModel.Items)
                detailNavigationPage.PushAsync(ShellViewPage.Create(viewFactory.GetView(viewModel)));
        }

        private void ShellViewModel_ViewModelPushed(object sender, IViewModel e)
        {
            detailNavigationPage.PushAsync(ShellViewPage.Create(viewFactory.GetView(e)));
        }

        private void ShellViewModel_ViewModelPopped(object sender, IViewModel e)
        {
            detailNavigationPage.Popped -= DetailNavigationPage_Popped;
            detailNavigationPage.PopAsync().ContinueWith(t => detailNavigationPage.Popped += DetailNavigationPage_Popped);
        }

        private void ShellViewModel_RootViewModelPushed(object sender, IViewModel e)
        {
            detailNavigationPage.Popped -= DetailNavigationPage_Popped;
            detailNavigationPage = new NavigationPage(ShellViewPage.Create(viewFactory.GetView(e)));
            detailNavigationPage.Popped += DetailNavigationPage_Popped;
            Detail = detailNavigationPage;
        }
    }
}
