using Android.Content;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Net;
using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.Android;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GMaps = Android.Gms.Maps;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]

namespace AppShell.NativeMaps.Mobile.Android
{
    public class MapViewRenderer : ViewRenderer<MapView, GMaps.MapView>, GMaps.IOnMapReadyCallback
    {
        private IImageResolver imageResolver;
        private GMaps.GoogleMap googleMap;
        private TwoWayDictionary<Marker, GMaps.Model.Marker> markers;
        private TwoWayDictionary<TileOverlay, GMaps.Model.TileOverlay> tileOverlays;

        public MapViewRenderer()
        {
            markers = new TwoWayDictionary<Marker, GMaps.Model.Marker>(new LambdaEqualityComparer<GMaps.Model.Marker>((m1, m2) => m1.Id == m2.Id));
            tileOverlays = new TwoWayDictionary<TileOverlay, GMaps.Model.TileOverlay>(new LambdaEqualityComparer<GMaps.Model.TileOverlay>((m1, m2) => m1.Id == m2.Id));
            imageResolver = AppShell.ShellCore.Container.GetInstance<IImageResolver>();
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
                    foreach (TileOverlay tileOverlay in e.OldElement.TileOverlays)
                        RemoveTileOverlay(tileOverlay);

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
                    AddTileOverlay(tileOverlay);
            }

            googleMap.UiSettings.MapToolbarEnabled = false;
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
            else if (e.PropertyName == MapView.NavigationDestinationProperty.PropertyName)
                NavigateTo();
        }

        private void GoogleMap_CameraChange(object sender, GMaps.GoogleMap.CameraChangeEventArgs e)
        {
            if (Element != null)
                Element.MapZoomLevel = e.Position.Zoom;
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
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (TileOverlay tileOverlay in tileOverlays.Select(m => m.Key).ToList())
                    RemoveTileOverlay(tileOverlay);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TileOverlay tileOverlay in e.NewItems)
                    AddTileOverlay(tileOverlay);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TileOverlay tileOverlay in e.OldItems)
                    RemoveTileOverlay(tileOverlay);
            }
        }

        private void AddMarker(Marker marker)
        {
            MarkerOptions options = new MarkerOptions();
            if (marker.ZIndex.HasValue)
                options.InvokeZIndex(marker.ZIndex.Value);
            options.SetPosition(new LatLng(marker.Center.Latitude, marker.Center.Longitude));

            if (!string.IsNullOrEmpty(marker.Icon))
            {
                if (imageResolver != null)
                {
                    System.IO.Stream stream = imageResolver.Resolve(marker.Layer, marker.Icon);
                    if (stream ==null)
                        stream = imageResolver.Resolve(marker);
                    if (stream != null)
                        options.SetIcon(BitmapDescriptorFactory.FromBitmap(BitmapFactory.DecodeStream(stream)));
                }
                else
                    options.SetIcon(BitmapDescriptorFactory.FromResource(ResourceManager.GetDrawableByName(marker.Icon)));
                if (marker.Label != null)
                    CreateLabel(marker);
            }

            options.SetTitle(marker.Title);
            options.SetSnippet(marker.Content);
            options.Draggable(marker.Draggable);

            markers.Add(marker, googleMap.AddMarker(options));

            marker.PropertyChanged += Marker_PropertyChanged;
        }

        private void RemoveMarker(Marker marker)
        {
            marker.PropertyChanged -= Marker_PropertyChanged;
            if (marker.Label != null)
                RemoveLabel(marker);

            if (markers.ContainsKey(marker))
            {
                markers[marker].Remove();
                markers.Remove(marker);
            }
        }

        private void AddTileOverlay(TileOverlay tileOverlay)
        {
            TileOverlayOptions options = new TileOverlayOptions();

            if (tileOverlay is UrlTileOverlay)
                options.InvokeTileProvider(new UrlTileOverlayProvider(tileOverlay as UrlTileOverlay));

            tileOverlays.Add(tileOverlay, googleMap.AddTileOverlay(options));
        }

        private void RemoveTileOverlay(TileOverlay tileOverlay)
        {
            tileOverlays[tileOverlay].Remove();
            tileOverlays.Remove(tileOverlay);
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
            if (e.PropertyName == "Center" && marker.Id != null && marker.Label != null && markers.Any(m => m.Key.Id == marker.Id + "-Label"))
            {
                var labelMarker = markers.FirstOrDefault(m => m.Key.Id == marker.Id + "-Label");
                if (labelMarker.Key != null)
                    markers[labelMarker.Key].Position = new LatLng(marker.Center.Latitude, marker.Center.Longitude);
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

        private void CreateLabel(Marker marker)
        {
            if (marker.Label.Text == null)
                return;
            Paint labelTextPaint = new Paint();
            labelTextPaint.Flags = PaintFlags.AntiAlias;
            labelTextPaint.TextSize = marker.Label.TextSize != 0 ? marker.Label.TextSize : 25.0f;
            labelTextPaint.SetStyle(Paint.Style.Stroke);
            labelTextPaint.Color = Xamarin.Forms.Color.White.ToAndroid();
            labelTextPaint.StrokeWidth = 8.0f;

            Rect boundsText = new Rect();
            labelTextPaint.GetTextBounds(marker.Label.Text, 0, marker.Label.Text.Length, boundsText);
            MarkerOptions options = new MarkerOptions();
            if (marker.ZIndex.HasValue)
                options.InvokeZIndex(marker.ZIndex.Value - 0.1f);
            options.SetPosition(new LatLng(marker.Center.Latitude, marker.Center.Longitude));
            options.Anchor(marker.Label.AnchorPointX, marker.Label.AnchorPointY);
            Bitmap labelBitmap = Bitmap.CreateBitmap(boundsText.Width(), boundsText.Height() * 2, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(labelBitmap);
            canvas.DrawText(marker.Label.Text, 0, boundsText.Height() * 2, labelTextPaint);
            labelTextPaint.SetStyle(Paint.Style.Fill);
            labelTextPaint.Color = global::Android.Graphics.Color.DarkGray;
            canvas.DrawText(marker.Label.Text, 0, boundsText.Height() * 2, labelTextPaint);

            options.SetIcon(BitmapDescriptorFactory.FromBitmap(labelBitmap));
            markers.Add(new Marker()
            {
                Id = marker.Id + "-Label",
                Center = marker.Center
            }, googleMap.AddMarker(options));
        }

        private void RemoveLabel(Marker marker)
        {
            if (marker.Id != null && markers.Any(m => m.Key.Id == marker.Id + "-Label"))
            {
                var labelMarker = markers.FirstOrDefault(m => m.Key.Id == marker.Id + "-Label");
                if (labelMarker.Key != null)
                {
                    markers[labelMarker.Key].Remove();
                    markers.Remove(labelMarker.Key);
                }
            }
        }

        private void NavigateTo()
        {
            if (Element.NavigationDestination != null)
            {
                Uri uri = Uri.Parse(string.Format(CultureInfo.InvariantCulture, "google.navigation:q={0},{1}", Element.NavigationDestination.Latitude, Element.NavigationDestination.Longitude));
                Intent navigationIntent = new Intent(Intent.ActionView, uri);
                navigationIntent.SetPackage("com.google.android.apps.maps");

                Forms.Context.StartActivity(navigationIntent);
            }
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(ContextExtensions.ToPixels(Context, 40.0), ContextExtensions.ToPixels(Context, 40.0)));
        }
    }
}