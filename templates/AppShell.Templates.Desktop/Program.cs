using System;

namespace AppShell.Templates.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            __shellname__App app = new __shellname__App();
            app.Run();
        }
    }
}
