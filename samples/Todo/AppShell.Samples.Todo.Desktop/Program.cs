using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
