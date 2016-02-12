using System;

namespace AppShell.Samples.Todo
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            TodoApp app = new TodoApp();
            app.Run();
        }
    }
}
