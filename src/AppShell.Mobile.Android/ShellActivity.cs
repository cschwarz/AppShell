using Android.OS;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppShell.Mobile.Android
{
    public class ShellActivity<T> : FormsApplicationActivity where T : Application
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            ShellCore.Container.RegisterSingleton<IPlatformProvider, AndroidPlatformProvider>();

            LoadApplication(Activator.CreateInstance<T>());
        }
    }
}