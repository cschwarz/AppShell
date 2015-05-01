using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            ServiceDispatcherApp app = new ServiceDispatcherApp();
            app.Run();
        }
    }
}
