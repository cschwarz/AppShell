using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell.Mobile.Android
{
    public class AndroidPlatformProvider : MobilePlatformProvider
    {
        public override Platform GetPlatform()
        {
            return Platform.Android;
        }

        public override IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}