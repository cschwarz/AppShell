using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public abstract class MobilePlatformProvider : IPlatformProvider
    {
        public abstract IEnumerable<Assembly> GetAssemblies();
        public abstract Platform GetPlatform();

        public void ExecuteOnUIThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }
    }
}
