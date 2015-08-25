using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.Todo
{
    public class TodoShellCore : ShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingleton<IShellConfigurationProvider, TodoShellConfigurationProvider>();
        }
    }
}
