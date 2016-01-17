using AppShell.NativeMaps;

namespace AppShell.Samples.NativeMaps
{
    public class NativeMapsShellCore : ShellCore
    {
        public override void Run()
        {
            Push<MasterDetailShellViewModel>(new { Name = "Main", Master = new NativeMapsMasterViewModel(serviceDispatcher) });

            serviceDispatcher.Dispatch<INavigationService>(n => n.Push<MapViewModel>(new { Title = "Single Map", ZoomLevel = 15.0, Center = new Location(48.21, 16.37) }));
        }
    }
}
