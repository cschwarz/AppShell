using AppShell.NativeMaps;

namespace AppShell.Samples.NativeMaps
{
    public class TileOverlayMapViewModel : MapViewModel
    {
        public static readonly UrlTileOverlay HighDpiTileOverlay = new UrlTileOverlay("http://maps.wien.gv.at/basemap/bmaphidpi/normal/google3857/{z}/{y}/{x}.jpeg", 512, 512);
        public static readonly UrlTileOverlay AerialTileOverlay = new UrlTileOverlay("http://maps.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{z}/{y}/{x}.jpeg", 256, 256);

        private bool isAerial;

        public TileOverlayMapViewModel(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            MapType = MapType.None;
            TileOverlays.Add(HighDpiTileOverlay);

            ToolbarItems.Add(new ToolbarItemViewModel() { Command = new Command(SwitchTileOverlay), Title = "Switch" });
        }

        private void SwitchTileOverlay()
        {
            isAerial = !isAerial;

            TileOverlays.Clear();
            TileOverlays.Add(isAerial ? AerialTileOverlay : HighDpiTileOverlay);
        }
    }
}
