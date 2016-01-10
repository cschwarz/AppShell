using Android.Gms.Maps.Model;
using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.Android;
using Java.Net;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GMaps = Android.Gms.Maps;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]

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

    public class MapViewRenderer : ViewRenderer<MapView, GMaps.MapView>, GMaps.IOnMapReadyCallback
    {
        private GMaps.GoogleMap googleMap;

        public void OnMapReady(GMaps.GoogleMap googleMap)
        {
            this.googleMap = googleMap;

            googleMap.MapType = Element.MapType.ToNativeMapType();
            
            SetCenter();

            if (Element.Markers != null)
            {
                foreach (Marker marker in Element.Markers)
                {
                    MarkerOptions options = new MarkerOptions();
                    options.SetPosition(new LatLng(marker.Center.Latitude, marker.Center.Longitude));

                    if (!string.IsNullOrEmpty(marker.Icon))
                        options.SetIcon(BitmapDescriptorFactory.FromResource(ResourceManager.GetDrawableByName(marker.Icon)));

                    options.SetTitle(marker.Title);
                    options.SetSnippet(marker.Content);

                    googleMap.AddMarker(options);
                }
            }

            if (Element.TileOverlays != null)
            {
                foreach (TileOverlay tileOverlay in Element.TileOverlays)
                {
                    TileOverlayOptions options = new TileOverlayOptions();

                    if (tileOverlay is UrlTileOverlay)
                        options.InvokeTileProvider(new UrlTileOverlayProvider(tileOverlay as UrlTileOverlay));

                    googleMap.AddTileOverlay(options);
                }
            }
        }
                
        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            GMaps.MapView nativeMapView = new GMaps.MapView(Context);
            nativeMapView.OnCreate(null);
            nativeMapView.OnResume();
            nativeMapView.GetMapAsync(this);

            SetNativeControl(nativeMapView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MapView.CenterProperty.PropertyName)
                SetCenter();
        }

        private void SetCenter()
        {
            if (Element.Center != null)
                googleMap.MoveCamera(GMaps.CameraUpdateFactory.NewLatLngZoom(new LatLng(Element.Center.Latitude, Element.Center.Longitude), (float)Element.ZoomLevel));
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(ContextExtensions.ToPixels(Context, 40.0), ContextExtensions.ToPixels(Context, 40.0)));
        }
    }
}