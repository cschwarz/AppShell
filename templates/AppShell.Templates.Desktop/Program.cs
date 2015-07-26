using System;

namespace AppShell.Templates.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            TemplateApp app = new TemplateApp();
            app.Run();
        }
    }
}
