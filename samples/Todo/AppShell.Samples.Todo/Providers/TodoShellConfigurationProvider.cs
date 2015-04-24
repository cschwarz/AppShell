using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.Todo
{
    public class TodoShellConfigurationProvider : ShellConfigurationProvider
    {
        public TodoShellConfigurationProvider()
        {
            RegisterShellViewModel<StackShellViewModel>();

            RegisterViewModel<TodoViewModel>();
        }
    }
}
