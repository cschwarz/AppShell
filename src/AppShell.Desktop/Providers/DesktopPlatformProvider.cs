using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace AppShell.Desktop
{
    public class DesktopPlatformProvider : IPlatformProvider
    {
        public Platform GetPlatform()
        {
            return Platform.Windows;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();

            foreach (string assemblyPath in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe").Concat(Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")))
            {
                try
                {
                    assemblies.Add(Assembly.LoadFile(assemblyPath));
                }
                catch
                {
                }
            }

            return assemblies;
        }

        public void ExecuteOnUIThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public void ShowMessage(string title, string message, string cancel)
        {
            MessageBox.Show(message, title);
        }
    }
}
