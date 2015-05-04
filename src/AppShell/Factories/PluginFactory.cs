using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public class PluginFactory : IPluginFactory
    {
        public IPlugin GetPlugin(Type pluginType, Dictionary<string, object> data = null)
        {
            IPlugin plugin = AppShellCore.Container.GetInstance(pluginType) as IPlugin;

            if (data != null)
            {
                foreach (KeyValuePair<string, object> pair in data)
                {
                    PropertyInfo propertyInfo = pluginType.GetRuntimeProperty(pair.Key);

                    if (propertyInfo != null)
                        propertyInfo.SetValue(plugin, pair.Value);
                }
            }

            return plugin;
        }
    }
}
