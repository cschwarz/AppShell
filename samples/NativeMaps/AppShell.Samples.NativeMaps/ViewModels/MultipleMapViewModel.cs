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

        private Marker selectedMarker1;
        public Marker SelectedMarker1
        {
            get { return selectedMarker1; }
            set
            {
                if (selectedMarker1 != value)
                {
                    selectedMarker1 = value;
                    OnPropertyChanged();

                    ToolbarItems.Clear();

                    if (selectedMarker1 != null)
                        ToolbarItems.Add(new ToolbarItemViewModel() { Command = new Command(NavigateTo), Title = "Navigate To" });
                }
            }
        }

        private Location navigationDestination1;
        public Location NavigationDestination1
        {
            get { return navigationDestination1; }
            set
            {
                if (navigationDestination1 != value)
                {
                    navigationDestination1 = value;
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
            Markers1.Clear();
        }

        private void NavigateTo()
        {
            if (SelectedMarker1 != null)
            {
                NavigationDestination1 = selectedMarker1.Center;
                NavigationDestination1 = null;
            }
        }
    }
}
