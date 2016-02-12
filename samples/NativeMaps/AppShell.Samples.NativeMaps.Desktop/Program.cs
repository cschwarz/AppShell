using System;

namespace AppShell.Samples.NativeMaps
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            NativeMapsApp app = new NativeMapsApp();
            app.Run();
        }
    }
}
