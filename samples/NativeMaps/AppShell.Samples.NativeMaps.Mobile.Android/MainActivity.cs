using Android.App;
using Android.Content.PM;
using AppShell.Mobile;
using AppShell.Mobile.Android;

namespace AppShell.Samples.NativeMaps.Mobile.Android
{
    [Activity(Label = "AppShell.Samples.NativeMaps.Mobile.Android", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : ShellActivity<ShellApplication<NativeMapsShellCore>>
    {
    }
}

