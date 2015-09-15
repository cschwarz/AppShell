using AppShell.NativeMaps;
using System.Collections.Generic;

namespace AppShell.Samples.NativeMaps
{
    public class NativeMapsShellConfigurationProvider : ShellConfigurationProvider
    {
        public NativeMapsShellConfigurationProvider()
        {
            RegisterShellViewModel<StackShellViewModel>();

            RegisterViewModel<MapViewModel>(new Dictionary<string, object>() 
            {
                { "ApiKey", "AocChPqL03_3u67XOXhITEjyx1P9sOLLTUIuOGZ52iySaBM7jzwmxM7bNIZPpb-Y" },
                { "ZoomLevel", 15.0 },
                { "Latitude", 48.21 },
                { "Longitude", 16.37 }
            });
        }
    }
}
