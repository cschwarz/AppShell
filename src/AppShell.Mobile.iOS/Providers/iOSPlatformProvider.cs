using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace AppShell.Mobile.iOS
{
    public class iOSPlatformProvider : IPlatformProvider
    {
        public Platform GetPlatform()
        {
            return Platform.iOS;
        }

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
