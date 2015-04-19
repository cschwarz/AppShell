using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public interface IPlatformProvider
    {
        IEnumerable<Assembly> GetAssemblies();
        void ExecuteOnUIThread(Action action);
    }
}
