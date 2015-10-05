namespace AppShell.NativeMaps
{
    [Service("mapService")]
    public interface IMapService
    {
        [ServiceMethod("setCenter")]
        void SetCenter(double latitude, double longitude);
        [ServiceMethod("setZoomLevel")]
        void SetZoomLevel(double zoomLevel);
    }
}
