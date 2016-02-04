using Android.Gms.Maps.Model;
using Java.Net;

namespace AppShell.NativeMaps.Mobile.Android
{
    public class UrlTileOverlayProvider : UrlTileProvider
    {
        private UrlTileOverlay urlTileOverlay;

        public UrlTileOverlayProvider(UrlTileOverlay urlTileOverlay) : base(urlTileOverlay.TileWidth, urlTileOverlay.TileHeight)
        {
            this.urlTileOverlay = urlTileOverlay;
        }

        public override URL GetTileUrl(int x, int y, int zoom)
        {
            return new URL(urlTileOverlay.Url.Replace("{z}", zoom.ToString()).Replace("{y}", y.ToString()).Replace("{x}", x.ToString()));
        }
    }
}