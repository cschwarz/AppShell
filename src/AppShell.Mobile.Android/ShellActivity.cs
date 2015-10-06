using Android.OS;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppShell.Mobile.Android
{
    public class ShellActivity<TApplication, TResource> : FormsApplicationActivity where TApplication : Application
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState, typeof(TResource).Assembly);

            ShellCore.InitializeContainer();
            ShellCore.Container.RegisterSingleton<IPlatformProvider, AndroidPlatformProvider>();

            LoadApplication(Activator.CreateInstance<TApplication>());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ShellCore.ShutdownContainer();
        }
    }
}