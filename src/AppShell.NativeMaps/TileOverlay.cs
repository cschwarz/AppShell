namespace AppShell.NativeMaps
{
    public abstract class TileOverlay
    {
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public TileOverlay(int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }
    }
}
