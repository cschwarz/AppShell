using AppShell.NativeMaps;

namespace AppShell.Samples.NativeMaps
{
    public class NativeMapsShellConfigurationProvider : ShellConfigurationProvider
    {
        public NativeMapsShellConfigurationProvider()
        {
            RegisterShellViewModel<TabShellViewModel>();

            RegisterViewModel<MapViewModel>(new
            {
                Title = "Single Map",                
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37)
            });

            RegisterViewModel<MultipleMapViewModel>(new
            {
                Title = "Multiple Maps",
                ZoomLevel1 = 12.0,
                Center1 = new Location(48.21, 16.37),
                ZoomLevel2 = 9.0,
                Center2 = new Location(40.7536868, -73.9982661)
            });
        }
    }
}
