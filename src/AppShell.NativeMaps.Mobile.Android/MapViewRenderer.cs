using Android.Gms.Maps.Model;
using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.Android;
using Java.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private Dictionary<Marker, GMaps.Model.Marker> markers;

        public MapViewRenderer()
        {
            AutoPackage = false;
            markers = new Dictionary<Marker, GMaps.Model.Marker>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                GMaps.MapView nativeMapView = new GMaps.MapView(Context);

                nativeMapView.OnCreate(null);
                nativeMapView.OnResume();
                nativeMapView.GetMapAsync(this);

                SetNativeControl(nativeMapView);
            }

            if (e.OldElement != null)
            {
                foreach (var marker in markers)
                    marker.Value.Remove();
                markers.Clear();

                if (e.OldElement.Markers != null)
                {
                    if (e.OldElement.Markers is ObservableCollection<Marker>)
                        (e.OldElement.Markers as ObservableCollection<Marker>).CollectionChanged -= Markers_CollectionChanged;
                }

                if (e.OldElement.TileOverlays != null)
                {
                    if (e.OldElement.TileOverlays is ObservableCollection<TileOverlay>)
                        (e.OldElement.TileOverlays as ObservableCollection<TileOverlay>).CollectionChanged -= TileOverlays_CollectionChanged;
                }
            }

            if (e.NewElement != null && googleMap != null)
            {
                InitializeElement();
            }
        }
        
        public void OnMapReady(GMaps.GoogleMap googleMap)
        {
            this.googleMap = googleMap;
            InitializeElement();
        }

        private void InitializeElement()
        {
            SetMapType();
            SetCenter();

            if (Element.Markers != null)
            {
                if (Element.Markers is ObservableCollection<Marker>)
                    (Element.Markers as ObservableCollection<Marker>).CollectionChanged += Markers_CollectionChanged;

                foreach (Marker marker in Element.Markers)
                    AddMarker(marker);
            }

            if (Element.TileOverlays != null)
            {
                if (Element.TileOverlays is ObservableCollection<TileOverlay>)
                    (Element.TileOverlays as ObservableCollection<TileOverlay>).CollectionChanged += TileOverlays_CollectionChanged;

                foreach (TileOverlay tileOverlay in Element.TileOverlays)
                {
                    TileOverlayOptions options = new TileOverlayOptions();

                    if (tileOverlay is UrlTileOverlay)
                        options.InvokeTileProvider(new UrlTileOverlayProvider(tileOverlay as UrlTileOverlay));

                    googleMap.AddTileOverlay(options);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MapView.CenterProperty.PropertyName)
                SetCenter();
            else if (e.PropertyName == MapView.ZoomLevelProperty.PropertyName)
                SetCenter();
            else if (e.PropertyName == MapView.MapTypeProperty.PropertyName)
                SetMapType();
        }

        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var marker in markers)
                    marker.Value.Remove();
                markers.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Marker marker in e.NewItems)
                    AddMarker(marker);
            }
        }

        private void TileOverlays_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void AddMarker(Marker marker)
        {
            MarkerOptions options = new MarkerOptions();
            options.SetPosition(new LatLng(marker.Center.Latitude, marker.Center.Longitude));

            if (!string.IsNullOrEmpty(marker.Icon))
                options.SetIcon(BitmapDescriptorFactory.FromResource(ResourceManager.GetDrawableByName(marker.Icon)));

            options.SetTitle(marker.Title);
            options.SetSnippet(marker.Content);

            markers.Add(marker, googleMap.AddMarker(options));
        }

        private void SetCenter()
        {
            if (Element.Center != null)
                googleMap.MoveCamera(GMaps.CameraUpdateFactory.NewLatLngZoom(new LatLng(Element.Center.Latitude, Element.Center.Longitude), (float)Element.ZoomLevel));
        }

        private void SetMapType()
        {
            googleMap.MapType = Element.MapType.ToNativeMapType();
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(ContextExtensions.ToPixels(Context, 40.0), ContextExtensions.ToPixels(Context, 40.0)));
        }
    }
}