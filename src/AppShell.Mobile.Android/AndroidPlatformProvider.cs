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
using System.Reflection;
using Xamarin.Forms;

namespace AppShell.Mobile.Android
{
    public class AndroidPlatformProvider : IPlatformProvider
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
        
        public void ExecuteOnUIThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }
    }
}