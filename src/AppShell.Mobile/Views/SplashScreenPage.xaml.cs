using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(SplashScreenHostViewModel))]
    public partial class SplashScreenPage : ContentPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();

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
            Application.Current.MainPage = AppShellCore.Container.GetInstance<IViewFactory>().GetView(AppShellCore.Container.GetInstance<IShellConfigurationProvider>().GetShellViewModel()) as Page;
        }
    }
}
