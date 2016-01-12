using AppShell.NativeMaps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppShell.Samples.NativeMaps
{
    public class MenuItem
    {
        public string Title { get; private set; }
        public TypeConfiguration TypeConfiguration { get; private set; }
                
        public MenuItem(string title, TypeConfiguration typeConfiguration)
        {
            Title = title;
            TypeConfiguration = typeConfiguration;
        }
    }

    public class MenuViewModel : ViewModel
    {
        public ObservableCollection<MenuItem> Items { get; private set; }

        private MenuItem selectedItem;
        public MenuItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;

                    if (selectedItem != null)
                    {
                        serviceDispatcher.Dispatch<IMasterDetailNavigationService>(n => n.PushRoot(selectedItem.TypeConfiguration.Type, selectedItem.TypeConfiguration.Data));
                        IsPresented = false;
                    }

                    OnPropertyChanged();
                }
            }
        }

        private bool isPresented;
        public bool IsPresented
        {
            get { return isPresented; }
            set
            {
                if (isPresented != value)
                {
                    isPresented = value;                    
                    OnPropertyChanged();
                }
            }
        }

        private IServiceDispatcher serviceDispatcher;

        public MenuViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "Menu";
            Items = new ObservableCollection<MenuItem>();

            Items.Add(new MenuItem("Single Map", new TypeConfiguration(typeof(MapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Single Map",
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37)
            }))));

            Items.Add(new MenuItem("Multiple Maps", new TypeConfiguration(typeof(MultipleMapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Multiple Maps",
                ZoomLevel1 = 12.0,
                Center1 = new Location(48.21, 16.37),

                Markers1 = new ObservableCollection<Marker>(new List<Marker>()
                {
                    new Marker() { Center = new Location(48.20, 16.37), Icon = "bus" },
                    new Marker() { Center = new Location(48.22, 16.37), Icon = "underground" },
                    new Marker() { Center = new Location(48.21, 16.36) },
                    new Marker() { Center = new Location(48.21, 16.38), Title = "Title", Content = "Content" }
                }),
                ZoomLevel2 = 9.0,
                Center2 = new Location(40.7536868, -73.9982661)
            }))));
            
            Items.Add(new MenuItem("Tile Overlay Map", new TypeConfiguration(typeof(MapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Tile Overlay Map",
                ZoomLevel = 12.0,
                Center = new Location(48.21, 16.37),
                MapType = MapType.None,
                TileOverlays = new ObservableCollection<TileOverlay>(new List<TileOverlay>() { new UrlTileOverlay("http://maps.wien.gv.at/basemap/bmaphidpi/normal/google3857/{z}/{y}/{x}.jpeg", 512, 512) })
            }))));

            Items.Add(new MenuItem("Satellite Map", new TypeConfiguration(typeof(MapViewModel), ObjectExtensions.ToDictionary(new
            {
                Title = "Satellite Map",
                ZoomLevel = 15.0,
                Center = new Location(48.21, 16.37),
                MapType = MapType.Satellite
            }))));
        }        
    }
}
