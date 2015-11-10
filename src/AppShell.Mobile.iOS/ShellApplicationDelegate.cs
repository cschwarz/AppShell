using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppShell.Mobile.iOS
{
    public class ShellApplicationDelegate<T> : FormsApplicationDelegate where T : Application
    {
        protected virtual void ConfigurePlatform()
        {
        }

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();

            ShellCore.InitializeContainer();
            ShellCore.Container.RegisterSingleton<IPlatformProvider, iOSPlatformProvider>();

            ConfigurePlatform();

            LoadApplication(Activator.CreateInstance<T>());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
