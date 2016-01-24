using AppShell.NativeMaps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppShell.Samples.NativeMaps
{
    public class NativeMapsMasterViewModel : MasterViewModel
    {
        public NativeMapsMasterViewModel(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            Items.Add(new ViewModelMenuItem("Single Map", new TypeConfiguration(typeof(MapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Single Map",
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37)
            }))));

            Items.Add(new ViewModelMenuItem("Multiple Maps", new TypeConfiguration(typeof(MultipleMapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Multiple Maps",
                ZoomLevel1 = 12.0,
                Center1 = new Location(48.21, 16.37),

                Markers1 = new ObservableCollection<Marker>(new List<Marker>()
                {
                    new Marker() { Center = new Location(48.20, 16.37), Icon = "bus.png" },
                    new Marker() { Center = new Location(48.22, 16.37), Icon = "underground.png" },
                    new Marker() { Center = new Location(48.21, 16.36) },
                    new Marker() { Center = new Location(48.21, 16.38), Title = "Title", Content = "Content" }
                }),
                ZoomLevel2 = 9.0,
                Center2 = new Location(40.7536868, -73.9982661)
            }))));
            
            Items.Add(new ViewModelMenuItem("Tile Overlay Map", new TypeConfiguration(typeof(MapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Tile Overlay Map",
                ZoomLevel = 12.0,
                Center = new Location(48.21, 16.37),
                MapType = MapType.None,
                TileOverlays = new ObservableCollection<TileOverlay>(new List<TileOverlay>() { new UrlTileOverlay("http://maps.wien.gv.at/basemap/bmaphidpi/normal/google3857/{z}/{y}/{x}.jpeg", 512, 512) })
            }))));

            Items.Add(new ViewModelMenuItem("Satellite Map", new TypeConfiguration(typeof(MapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Satellite Map",
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37),
                MapType = MapType.Satellite
            }))));
        }        
    }
}
