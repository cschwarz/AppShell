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

        private IServiceDispatcher serviceDispatcher;

        public MapViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            serviceDispatcher.Subscribe<IMapService>(this);
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