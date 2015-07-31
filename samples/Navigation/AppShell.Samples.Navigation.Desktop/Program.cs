using System;

namespace AppShell.Samples.Navigation.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            NavigationApp app = new NavigationApp();
            app.Run();
        }
    }
}
