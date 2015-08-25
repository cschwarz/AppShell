using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    public class ServiceDispatcherShellConfigurationProvider : ShellConfigurationProvider
    {
        public ServiceDispatcherShellConfigurationProvider()
        {
            RegisterShellViewModel<StackShellViewModel>();

            RegisterPlugin<SamplePlugin>();
            
            RegisterViewModel<ManifestResourceWebBrowserViewModel>(new Dictionary<string, object>() { { "ManifestResource", "AppShell.Samples.ServiceDispatcher.ServiceDispatcher.html" } });
        }
    }
}
