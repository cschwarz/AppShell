using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class ViewModel1 : ViewModel
    {
        public Command OpenViewModel2Command { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel1(IServiceDispatcher serviceDispatcher)
        {
            Title = "ViewModel1";

            this.serviceDispatcher = serviceDispatcher;

            OpenViewModel2Command = new Command(OpenViewModel2);
        }

        public void OpenViewModel2()
        {
            serviceDispatcher.Dispatch<INavigationService>(n => n.Push<ViewModel2>());
        }
    }
}
