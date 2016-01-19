﻿using System;
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
            return Platform.WinRT;
        }

        public override IEnumerable<Assembly> GetAssemblies()
        {
            var files = Package.Current.InstalledLocation.GetFilesAsync().AsTask().Result;

            List<Assembly> assemblies = new List<Assembly>();
            foreach (StorageFile file in files)
            {
                if (file.FileType == ".dll" || file.FileType == ".exe")
                {
                    try
                    {
                        AssemblyName name = new AssemblyName() { Name = file.DisplayName };
                        assemblies.Add(Assembly.Load(name));
                    }
                    catch
                    {
                    }
                }
            }

            return assemblies;
        }
    }
}
