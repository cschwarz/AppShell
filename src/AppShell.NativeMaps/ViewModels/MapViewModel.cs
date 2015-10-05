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

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude != value)
                {
                    latitude = value;
                    OnPropertyChanged("Latitude");
                }
            }
        }

        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude != value)
                {
                    longitude = value;
                    OnPropertyChanged("Longitude");
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
            Latitude = latitude;
            Longitude = longitude;
        }

        public void SetZoomLevel(double zoomLevel)
        {
            ZoomLevel = zoomLevel;
        }
    }
}