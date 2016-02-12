using Android.Gms.Maps.Model;
using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.Android;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GMaps = Android.Gms.Maps;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]

namespace AppShell.NativeMaps.Mobile.Android
{
    public class MapViewRenderer : ViewRenderer<MapView, GMaps.MapView>, GMaps.IOnMapReadyCallback
    {
        private GMaps.GoogleMap googleMap;
        private TwoWayDictionary<Marker, GMaps.Model.Marker> markers;

        public MapViewRenderer()
        {
            markers = new TwoWayDictionary<Marker, GMaps.Model.Marker>(new LambdaEqualityComparer<GMaps.Model.Marker>((m1, m2) => m1.Id == m2.Id));
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
                if (e.OldElement.Markers != null)
                {
                    foreach (Marker marker in e.OldElement.Markers)
                        RemoveMarker(marker);

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

            googleMap.CameraChange += GoogleMap_CameraChange;
            googleMap.MarkerClick += GoogleMap_MarkerClick;
            googleMap.MarkerDrag += GoogleMap_MarkerDrag;
            googleMap.MarkerDragEnd += GoogleMap_MarkerDragEnd;
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

        private void GoogleMap_CameraChange(object sender, GMaps.GoogleMap.CameraChangeEventArgs e)
        {
            //Element.Center = new Location(e.Position.Target.Latitude, e.Position.Target.Longitude);
            //Element.ZoomLevel = e.Position.Zoom;
        }

        private void GoogleMap_MarkerClick(object sender, GMaps.GoogleMap.MarkerClickEventArgs e)
        {
            e.Handled = false;
            Element.SelectedMarker = markers[e.Marker];
        }

        private void GoogleMap_MarkerDrag(object sender, GMaps.GoogleMap.MarkerDragEventArgs e)
        {
            markers[e.Marker].Center = new Location(e.Marker.Position.Latitude, e.Marker.Position.Longitude);
        }

        private void GoogleMap_MarkerDragEnd(object sender, GMaps.GoogleMap.MarkerDragEndEventArgs e)
        {
            markers[e.Marker].Center = new Location(e.Marker.Position.Latitude, e.Marker.Position.Longitude);
        }

        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (Marker marker in markers.Select(m => m.Key).ToList())
                    RemoveMarker(marker);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Marker marker in e.NewItems)
                    AddMarker(marker);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Marker marker in e.OldItems)
                    RemoveMarker(marker);
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
            options.Draggable(marker.Draggable);

            markers.Add(marker, googleMap.AddMarker(options));

            marker.PropertyChanged += Marker_PropertyChanged;
        }

        private void RemoveMarker(Marker marker)
        {
            marker.PropertyChanged -= Marker_PropertyChanged;

            markers[marker].Remove();
            markers.Remove(marker);
        }

        private void Marker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Marker marker = sender as Marker;

            switch (e.PropertyName)
            {
                case "Center": markers[marker].Position = new LatLng(marker.Center.Latitude, marker.Center.Longitude); break;
                case "Icon":
                    {
                        if (!string.IsNullOrEmpty(marker.Icon))
                            markers[marker].SetIcon(BitmapDescriptorFactory.FromResource(ResourceManager.GetDrawableByName(marker.Icon)));
                        break;
                    }
                case "Title": markers[marker].Title = marker.Title; break;
                case "Content": markers[marker].Snippet = marker.Icon; break;
                case "Draggable": markers[marker].Draggable = marker.Draggable; break;
            }
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