using Android.Gms.Maps;

namespace AppShell.NativeMaps
{
    public static class MapTypeExtensions
    {
        public static int ToNativeMapType(this MapType mapType)
        {
            switch (mapType)
            {
                case MapType.None: return GoogleMap.MapTypeNone;
                case MapType.Roads: return GoogleMap.MapTypeNormal;
                case MapType.Satellite: return GoogleMap.MapTypeSatellite;
                case MapType.Hybrid: return GoogleMap.MapTypeHybrid;
                default: return GoogleMap.MapTypeNormal;
            }
        }
    }
}