using Android.App;
using Android.Content.PM;
using AppShell.Mobile;
using AppShell.Mobile.Android;
using AppShell.Data;
using AppShell.Data.Mobile.Android;

namespace AppShell.Samples.Todo.Mobile.Android
{
    [Activity(Label = "AppShell.Samples.Todo.Mobile.Android", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : ShellActivity<ShellApplication<TodoShellCore>, MainActivity>
    {
        protected override void ConfigurePlatform()
        {
            base.ConfigurePlatform();

            ShellCore.Container.RegisterSingleton<ISQLiteDatabase, AndroidSQLiteDatabase>();
        }
    }
}