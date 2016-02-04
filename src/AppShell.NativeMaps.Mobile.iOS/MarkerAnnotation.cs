using CoreLocation;
using MapKit;

namespace AppShell.NativeMaps.Mobile.iOS
{
    public class MarkerAnnotation : MKAnnotation
    {
        public Marker Marker { get; private set; }

        public MarkerAnnotation(Marker marker)
        {
            Marker = marker;
        }

        public override CLLocationCoordinate2D Coordinate { get { return new CLLocationCoordinate2D(Marker.Center.Latitude, Marker.Center.Longitude); } }
        public override string Title { get { return Marker.Title; } }
        public override string Subtitle { get { return Marker.Content; } }
    }
}
