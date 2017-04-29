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
        int Density { get; set; }
        Stream Resolve(String layer, String image);
        Stream Resolve(Marker marker);
    }
}
