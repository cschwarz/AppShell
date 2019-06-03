using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.iOS;
using CoreLocation;
using MapKit;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]

namespace AppShell.NativeMaps.Mobile.iOS
{
    public class MapViewRenderer : ViewRenderer<MapView, MKMapView>
    {
        public TwoWayDictionary<Marker, MarkerAnnotation> Markers { get; private set; }
        public TwoWayDictionary<Marker, IMKOverlay> Overlays { get; private set; }
        public TwoWayDictionary<TileOverlay, MKTileOverlay> TileOverlays { get; private set; }

        public MapViewRenderer()
        {
            Markers = new TwoWayDictionary<Marker, MarkerAnnotation>();
            Overlays = new TwoWayDictionary<Marker, IMKOverlay>();
            TileOverlays = new TwoWayDictionary<TileOverlay, MKTileOverlay>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                MKMapView mapView = new MKMapView();
                SetNativeControl(mapView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.SizeChanged -= SizeChanged;

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

            if (e.NewElement != null)
            {
                e.NewElement.SizeChanged += SizeChanged;

                SetMapType();

                if (e.NewElement.Markers != null)
                {
                    if (e.NewElement.Markers is ObservableCollection<Marker>)
                        (e.NewElement.Markers as ObservableCollection<Marker>).CollectionChanged += Markers_CollectionChanged;

                    foreach (Marker marker in e.NewElement.Markers)
                        AddMarker(marker);
                }

                if (e.NewElement.TileOverlays != null)
                {
                    if (e.NewElement.TileOverlays is ObservableCollection<TileOverlay>)
                        (e.NewElement.TileOverlays as ObservableCollection<TileOverlay>).CollectionChanged += TileOverlays_CollectionChanged;

                    foreach (TileOverlay tileOverlay in e.NewElement.TileOverlays)
                        AddTileOverlay(tileOverlay);
                }

                Control.Delegate = new MapViewDelegate(this, Element);
            }
        }

        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (Marker marker in Markers.Select(m => m.Key).ToList())
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
                foreach (TileOverlay tileOverlay in TileOverlays.Select(m => m.Key).ToList())
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
            if (marker is Polyline)
            {
                AddPolyline(marker as Polyline);
                return;
            }
            MarkerAnnotation annotation = new MarkerAnnotation(marker);
            Control.AddAnnotation(annotation);
            Markers.Add(marker, annotation);

            marker.PropertyChanged += Marker_PropertyChanged;
        }

        private void AddPolyline(Polyline polyline)
        {
            MKPolyline naitivePolyline = MKPolyline.FromCoordinates(polyline.Points.Select(p => new CLLocationCoordinate2D(p.Latitude, p.Longitude)).ToArray());
            Overlays.Add(polyline, naitivePolyline);
            Control.InsertOverlay(naitivePolyline, (nuint)polyline.ZIndex, MKOverlayLevel.AboveLabels);
        }

        private void RemoveMarker(Marker marker)
        {
            marker.PropertyChanged -= Marker_PropertyChanged;
            if (Markers.ContainsKey(marker)){
                Control.RemoveAnnotation(Markers[marker]);
                Markers.Remove(marker);
            }
        }

        private void AddTileOverlay(TileOverlay tileOverlay)
        {
            if (tileOverlay is UrlTileOverlay)
            {
                MKTileOverlay overlay = new MKTileOverlay((tileOverlay as UrlTileOverlay).Url);
                overlay.CanReplaceMapContent = true;
                overlay.TileSize = new CoreGraphics.CGSize(tileOverlay.TileWidth, tileOverlay.TileHeight);
                Control.AddOverlay(overlay, MKOverlayLevel.AboveLabels);

                TileOverlays.Add(tileOverlay, overlay);
            }
        }

        private void RemoveTileOverlay(TileOverlay tileOverlay)
        {
            Control.RemoveOverlay(TileOverlays[tileOverlay]);
            
            TileOverlays.Remove(tileOverlay);
        }

        private void Marker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Marker marker = sender as Marker;
            if (e.PropertyName == "Center")
                Markers[marker].SetCoordinate(new CLLocationCoordinate2D(marker.Center.Latitude, marker.Center.Longitude));
            Control.RemoveAnnotation(Markers[marker]);
            Control.AddAnnotation(Markers[marker]);
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

        private void SizeChanged(object sender, EventArgs e)
        {
            SetCenter();
        }

        private void SetCenter()
        {
            if (Element.Center != null)
                Control.SetRegion(new MKCoordinateRegion(new CLLocationCoordinate2D(Element.Center.Latitude, Element.Center.Longitude), new MKCoordinateSpan(0, 360.0 / Math.Pow(2, Element.ZoomLevel + 1) * Element.Width / 256.0)), false);
        }
        private void SetMapType()
        {
            Control.MapType = Element.MapType.ToNativeMapType();
        }

        private void NavigateTo()
        {
            if (Element.NavigationDestination != null)
            {
                MKMapItem destinationMapItem = new MKMapItem(new MKPlacemark(new CLLocationCoordinate2D(Element.NavigationDestination.Latitude, Element.NavigationDestination.Longitude), (MKPlacemarkAddress)null));
                destinationMapItem.OpenInMaps(new MKLaunchOptions() { DirectionsMode = MKDirectionsMode.Driving });
            }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return UIViewExtensions.GetSizeRequest(Control, widthConstraint, heightConstraint, -1.0, -1.0);
        }
    }
}
