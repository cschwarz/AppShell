using System;

namespace AppShell.Templates
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
