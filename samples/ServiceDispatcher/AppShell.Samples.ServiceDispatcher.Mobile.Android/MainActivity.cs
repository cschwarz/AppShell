using Android.App;
using Android.Content.PM;
using Android.OS;
using AppShell.Mobile.Android;

namespace AppShell.Samples.ServiceDispatcher.Mobile.Android
{
    [Activity(Label = "AppShell.Samples.ServiceDispatcher.Mobile.Android", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new AndroidShellApplication<ServiceDispatcherShellCore>());
        }
    }
}

