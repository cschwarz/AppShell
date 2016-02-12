using System;

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
