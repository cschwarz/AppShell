using AppShell.NativeMaps;
using System.Collections.ObjectModel;

namespace AppShell.Samples.NativeMaps
{
    public class TwoWayMapViewModel : ViewModel
    {
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
                
        public ObservableCollection<Marker> Markers { get; set; }
    }
}
