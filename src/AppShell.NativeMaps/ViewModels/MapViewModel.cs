namespace AppShell.NativeMaps
{
    public class MapViewModel : ViewModel, IMapService
    {
        private string apiKey;
        public string ApiKey
        {
            get { return apiKey; }
            set
            {
                if (apiKey != value)
                {
                    apiKey = value;
                    OnPropertyChanged("ApiKey");
                }
            }
        }

        private Location center;
        public Location Center
        {
            get { return center; }
            set
            {
                if (center != value)
                {
                    center = value;
                    OnPropertyChanged("Center");
                }
            }
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
                    OnPropertyChanged("ZoomLevel");
                }
            }
        }

        public MapViewModel(IServiceDispatcher serviceDispatcher)
        {
            serviceDispatcher.Subscribe<IMapService>(this);
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