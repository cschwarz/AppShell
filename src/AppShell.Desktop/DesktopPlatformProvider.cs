using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace AppShell.Desktop
{
    public class DesktopPlatformProvider : IPlatformProvider
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public void ExecuteOnUIThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
