using AppShell.NativeMaps;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Marker> Markers1 { get; set; }

        public Command RemoveMarkersCommand { get; private set; }

        public MultipleMapViewModel()
        {
            RemoveMarkersCommand = new Command(RemoveMarkers);
        }

        public void RemoveMarkers()
        {
            //Markers1.Clear();
            //Markers1[0].Icon = "underground";
            Markers1[0].Center = new Location(48.23, 16.37);
            Markers1[1].Icon = "bus";
        }
    }
}
