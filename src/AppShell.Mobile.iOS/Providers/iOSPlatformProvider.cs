using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell.Mobile.iOS
{
    public class iOSPlatformProvider : MobilePlatformProvider
    {
        public override Platform GetPlatform()
        {
            return Platform.iOS;
        }

        public override IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public override string GetDocumentFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}
