using System;
using System.Collections.Generic;

namespace AppShell
{
    public interface IPluginProvider
    {
        void StartPlugin<T>(Dictionary<string, object> data = null);
        void StartPlugin(Type pluginType, Dictionary<string, object> data = null);
        void ShutdownPlugins();
    }
}
