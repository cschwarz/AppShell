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

            using (StreamReader streamReader = new StreamReader(GetType().GetTypeInfo().Assembly.GetManifestResourceStream("AppShell.Samples.ServiceDispatcher.ServiceDispatcher.html")))
                RegisterViewModel<WebBrowserViewModel>(new Dictionary<string, object>() { { "Html", streamReader.ReadToEnd() } });
        }
    }
}
