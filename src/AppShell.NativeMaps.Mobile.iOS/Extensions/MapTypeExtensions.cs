using MapKit;

namespace AppShell.NativeMaps
{
    public static class MapTypeExtensions
    {
        public static MKMapType ToNativeMapType(this MapType mapType)
        {
            switch (mapType)
            {
                case MapType.None: return MKMapType.Standard;
                case MapType.Roads: return MKMapType.Standard;
                case MapType.Satellite: return MKMapType.Satellite;
                case MapType.Hybrid: return MKMapType.Hybrid;
                default: return MKMapType.Standard;
            }
        }
    }
}