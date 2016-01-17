using Android.App;
using Android.Content.PM;
using Android.OS;
using AppShell.Mobile;
using AppShell.Mobile.Android;

namespace AppShell.Samples.Navigation.Mobile.Android
{
    [Activity(Label = "AppShell.Samples.Navigation.Mobile.Android", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : CompatShellActivity<ShellApplication<NavigationShellCore>, MainActivity>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(savedInstanceState);
        }
    }
}

