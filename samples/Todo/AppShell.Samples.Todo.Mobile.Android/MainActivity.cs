using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppShell.Mobile;
using AppShell.Mobile.Android;
using System;

namespace AppShell.Samples.Todo.Mobile.Android
{
    [Activity(Label = "AppShell.Samples.Todo.Mobile.Android", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : ShellActivity<ShellApplication<TodoShellCore>>
    {
    }
}