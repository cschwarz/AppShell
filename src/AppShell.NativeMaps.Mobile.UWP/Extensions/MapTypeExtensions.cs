using Windows.UI.Xaml.Controls.Maps;

namespace AppShell.NativeMaps
{
    public static class MapTypeExtensions
    {
        public static MapStyle ToNativeMapType(this MapType mapType)
        {
            switch (mapType)
            {
                case MapType.None: return MapStyle.None;
                case MapType.Roads: return MapStyle.Road;
                case MapType.Satellite: return MapStyle.Aerial;
                case MapType.Hybrid: return MapStyle.AerialWithRoads;
                default: return MapStyle.Road;
            }
        }
    }
}