namespace AppShell.NativeMaps
{
    public class UrlTileOverlay : TileOverlay
    {
        public string Url { get; private set; }

        public UrlTileOverlay(string url, int tileWidth, int tileHeight)
            : base(tileWidth, tileHeight)
        {
            Url = url;
        }
    }
}
