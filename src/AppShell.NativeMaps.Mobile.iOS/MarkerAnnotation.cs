using CoreLocation;
using MapKit;

namespace AppShell.NativeMaps.Mobile.iOS
{
    public class MarkerAnnotation : MKAnnotation
    {
        private CLLocationCoordinate2D coordinate;

        public Marker Marker { get; private set; }

        public MarkerAnnotation(Marker marker)
        {
            Marker = marker;
            SetCoordinate(new CLLocationCoordinate2D(Marker.Center.Latitude, Marker.Center.Longitude));
        }

        public override CLLocationCoordinate2D Coordinate { get { return coordinate; } }
        public override string Title { get { return Marker.Title; } }
        public override string Subtitle { get { return Marker.Content; } }

        public override void SetCoordinate(CLLocationCoordinate2D value)
        {
            coordinate = value;
        }
    }
}
