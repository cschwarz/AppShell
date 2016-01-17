using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    /*
    [View(typeof(SplashScreenHostViewModel))]
    public partial class SplashScreenPage : ContentPage
    {
        private IServiceDispatcher serviceDispatcher;

        public SplashScreenPage(IServiceDispatcher serviceDispatcher)
        {
            InitializeComponent();

            this.serviceDispatcher = serviceDispatcher;

            BindingContextChanged += SplashScreenPage_BindingContextChanged;
        }

        private void SplashScreenPage_BindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is SplashScreenHostViewModel)
            {
                (BindingContext as SplashScreenHostViewModel).ExitRequested += ExitRequested;
                (BindingContext as SplashScreenHostViewModel).ShellViewRequested += ShellViewRequested;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is SplashScreenHostViewModel)
                (BindingContext as SplashScreenHostViewModel).ShowSplashScreens();
        }

        private void ExitRequested(object sender, EventArgs e)
        {
        }

        private void ShellViewRequested(object sender, EventArgs e)
        {
            IShellConfigurationProvider configurationProvider = ShellCore.Container.GetInstance<IShellConfigurationProvider>();
            TypeConfiguration shellViewModelConfiguration = configurationProvider.GetShellViewModel();
            IViewModel shellViewModel = ShellCore.Container.GetInstance<IViewModelFactory>().GetViewModel(shellViewModelConfiguration.Type, shellViewModelConfiguration.Data);

            foreach (TypeConfiguration viewModelConfiguration in configurationProvider.GetViewModels())
                serviceDispatcher.Dispatch<INavigationService>(n => n.Push(viewModelConfiguration.Type, viewModelConfiguration.Data));

            Application.Current.MainPage = ShellCore.Container.GetInstance<IViewFactory>().GetView(shellViewModel) as Page;
        }
    }*/
}
