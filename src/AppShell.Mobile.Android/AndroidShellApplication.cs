using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppShell.Mobile.Android
{
    public class AndroidShellApplication<T> : ShellApplication<T> where T : AppShellCore
    {
        protected override void ConfigurePlatform()
        {
            base.ConfigurePlatform();

            AppShellCore.Container.RegisterSingle<IPlatformProvider, AndroidPlatformProvider>();
        }
    }
}