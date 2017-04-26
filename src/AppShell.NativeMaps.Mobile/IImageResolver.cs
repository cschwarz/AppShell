using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.NativeMaps.Mobile
{
    public interface IImageResolver
    {
        Stream Resolve(String layer, String image);
    }
}
