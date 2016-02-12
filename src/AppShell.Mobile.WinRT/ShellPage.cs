using System;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;

namespace AppShell.Mobile.WinRT
{
    public class ShellPage<TApplication> : WindowsPhonePage where TApplication : Application
    {
        public ShellPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;

            ShellCore.InitializeContainer();
            ShellCore.Container.RegisterSingleton<IPlatformProvider, WinRTPlatformProvider>();

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
