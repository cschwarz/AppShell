using System;
using System.Collections.Generic;

namespace AppShell
{
    public interface IPluginFactory
    {
        IPlugin GetPlugin(Type pluginType, Dictionary<string, object> data = null);
    }
}
