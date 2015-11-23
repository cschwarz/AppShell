using AppShell.NativeMaps;

namespace AppShell.Samples.NativeMaps
{
    public class MultipleMapViewModel : ViewModel
    {
        private double zoomLevel1;
        public double ZoomLevel1
        {
            get { return zoomLevel1; }
            set
            {
                if (zoomLevel1 != value)
                {
                    zoomLevel1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Location center1;
        public Location Center1
        {
            get { return center1; }
            set
            {
                if (center1 != value)
                {
                    center1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private double zoomLevel2;
        public double ZoomLevel2
        {
            get { return zoomLevel2; }
            set
            {
                if (zoomLevel2 != value)
                {
                    zoomLevel2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Location center2;
        public Location Center2
        {
            get { return center2; }
            set
            {
                if (center2 != value)
                {
                    center2 = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
