using System;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace AppShell.Mobile.UWP
{
    public class ShellPage<TApplication> :  WindowsPage where TApplication : Application
    {
        public ShellPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;

            ShellCore.InitializeContainer();
            ShellCore.Container.RegisterSingleton<IPlatformProvider, UWPPlatformProvider>();

            ConfigurePlatform();
        }

        protected void Init()
        {
            LoadApplication(Activator.CreateInstance<TApplication>());
        }

        protected virtual void ConfigurePlatform()
        {
        }
    }
}
