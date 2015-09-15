using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
