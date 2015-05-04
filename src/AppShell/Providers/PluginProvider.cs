using System;
using System.Collections.Generic;

namespace AppShell
{
    public class PluginProvider : IPluginProvider
    {
        protected IPluginFactory pluginFactory;

        protected List<IPlugin> plugins;

        public PluginProvider(IPluginFactory pluginFactory)
        {
            this.pluginFactory = pluginFactory;

            plugins = new List<IPlugin>();
        }

        public void StartPlugin<T>(Dictionary<string, object> data = null)
        {
            IPlugin plugin = pluginFactory.GetPlugin(typeof(T), data);
            plugin.Start();

            plugins.Add(plugin);
        }

        public void StartPlugin(Type pluginType, Dictionary<string, object> data = null)
        {
            IPlugin plugin = pluginFactory.GetPlugin(pluginType, data);
            plugin.Start();

            plugins.Add(plugin);
        }

        public void ShutdownPlugins()
        {
            foreach (IPlugin plugin in plugins)
                plugin.Stop();
        }
    }
}
