using AppShell.NativeMaps;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

                Markers1 = new ObservableCollection<Marker>(new List<Marker>()
                {
                    new Marker() { Center = new Location(48.20, 16.37), Icon = "bus" },
                    new Marker() { Center = new Location(48.22, 16.37), Icon = "underground" }
                }),
                ZoomLevel2 = 9.0,
                Center2 = new Location(40.7536868, -73.9982661)
            });

            RegisterViewModel<MapViewModel>(new
            {
                Title = "Tile Overlay Map",
                ZoomLevel = 12.0,
                Center = new Location(48.21, 16.37),
                MapType = MapType.None,
                TileOverlays = new ObservableCollection<TileOverlay>(new List<TileOverlay>() { new UrlTileOverlay("http://maps.wien.gv.at/basemap/bmaphidpi/normal/google3857/{z}/{y}/{x}.jpeg", 512, 512) })
            });

            RegisterViewModel<MapViewModel>(new
            {
                Title = "Satellite Map",
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37),
                MapType = MapType.Satellite
            });
        }
    }
}
