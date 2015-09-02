using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public interface IPlatformProvider
    {
        Platform GetPlatform();
        IEnumerable<Assembly> GetAssemblies();
        void ExecuteOnUIThread(Action action);
    }
}
