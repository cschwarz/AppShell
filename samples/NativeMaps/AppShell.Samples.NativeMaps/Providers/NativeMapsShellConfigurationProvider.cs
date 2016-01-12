using AppShell.NativeMaps;

namespace AppShell.Samples.NativeMaps
{
    public class NativeMapsShellConfigurationProvider : ShellConfigurationProvider
    {        
        public NativeMapsShellConfigurationProvider(IServiceDispatcher serviceDispatcher)
        {
            RegisterShellViewModel<MasterDetailShellViewModel>(new { Master = new MenuViewModel(serviceDispatcher) });

            RegisterViewModel<MapViewModel>(new
            {
                Title = "Single Map",                
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37)
            });            
        }
    }
}
