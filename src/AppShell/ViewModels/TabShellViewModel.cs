using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    public class TabShellViewModel : ShellViewModel
    {
        public TabShellViewModel(IShellConfigurationProvider configurationProvider, IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
            : base(configurationProvider, serviceDispatcher, viewModelFactory)
        {
        }
    }
}
