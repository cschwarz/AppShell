using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Storage;

namespace AppShell.Mobile.WinRT
{
    public class WinRTPlatformProvider : MobilePlatformProvider
    {
        public override Platform GetPlatform()
        {
            return Platform.WindowsPhone;
        }

        public override IEnumerable<Assembly> GetAssemblies()
        {
            var files = Package.Current.InstalledLocation.GetFilesAsync().AsTask().Result;

            List<Assembly> assemblies = new List<Assembly>();
            foreach (StorageFile file in files)
            {
                if (file.FileType == ".dll" || file.FileType == ".exe")
                {
                    AssemblyName name = new AssemblyName() { Name = file.DisplayName };
                    assemblies.Add(Assembly.Load(name));
                }
            }

            return assemblies;
        }
    }
}
