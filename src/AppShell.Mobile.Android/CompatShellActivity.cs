using Android.OS;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppShell.Mobile.Android
{
    public class CompatShellActivity<TApplication, TResource> : FormsAppCompatActivity where TApplication : Application
    {
        protected virtual void ConfigurePlatform()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState, typeof(TResource).Assembly);

            ShellCore.InitializeContainer();
            ShellCore.Container.RegisterSingleton<IPlatformProvider, AndroidPlatformProvider>();

            ConfigurePlatform();

            LoadApplication(Activator.CreateInstance<TApplication>());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ShellCore.ShutdownContainer();
        }
    }
}