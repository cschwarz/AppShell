using System.Collections.ObjectModel;

namespace AppShell.NativeMaps
{
    public class MapViewModel : ViewModel, IMapService
    {
        private Location center;
        public Location Center
        {
            get { return center; }
            set
            {
                if (center != value)
                {
                    center = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MapZoomLevel
        {
            get;set;
        }

        private double zoomLevel;
        public double ZoomLevel
        {
            get { return zoomLevel; }
            set
            {
                if (zoomLevel != value)
                {
                    zoomLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private MapType mapType;
        public MapType MapType
        {
            get { return mapType; }
            set
            {
                if (mapType != value)
                {
                    mapType = value;
                    OnPropertyChanged();
                }
            }
        }

        private Marker selectedMarker;
        public Marker SelectedMarker
        {
            get { return selectedMarker; }
            set
            {
                if (selectedMarker != value)
                {
                    selectedMarker = value;
                    OnPropertyChanged();
                }
            }
        }

        private Location navigationDestination;
        public Location NavigationDestination
        {
            get { return navigationDestination; }
            set
            {
                navigationDestination = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Marker> Markers { get; set; }
        public ObservableCollection<TileOverlay> TileOverlays { get; set; }

        private IServiceDispatcher serviceDispatcher;

        public MapViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            serviceDispatcher.Subscribe<IMapService>(this);

            MapType = MapType.Roads;
            Markers = new ObservableCollection<Marker>();
            TileOverlays = new ObservableCollection<TileOverlay>();
        }

        public override void Dispose()
        {
            base.Dispose();

            serviceDispatcher.Unsubscribe<IMapService>(this);
        }

        public void SetCenter(double latitude, double longitude)
        {
            Center = new Location(latitude, longitude);
        }

        public void SetZoomLevel(double zoomLevel)
        {
            ZoomLevel = zoomLevel;
        }
    }
}