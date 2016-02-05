using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.iOS;
using CoreLocation;
using MapKit;
using System;
using System.Collections.Generic;
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
        private Dictionary<Marker, MarkerAnnotation> markers;

        public MapViewRenderer()
        {
            markers = new Dictionary<Marker, MarkerAnnotation>();
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

                Control.RemoveAnnotations(markers.Select(m => m.Value).ToArray());
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
                    {
                        if (tileOverlay is UrlTileOverlay)
                        {
                            MKTileOverlay overlay = new MKTileOverlay((tileOverlay as UrlTileOverlay).Url);
                            overlay.CanReplaceMapContent = true;
                            overlay.TileSize = new CoreGraphics.CGSize(tileOverlay.TileWidth, tileOverlay.TileHeight);
                            Control.AddOverlay(overlay, MKOverlayLevel.AboveLabels);
                        }
                    }
                }

                Control.Delegate = new MapViewDelegate(Element);
            }
        }
        
        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var marker in markers)
                    RemoveMarker(marker.Key);
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
            MarkerAnnotation annotation = new MarkerAnnotation(marker);
            Control.AddAnnotation(annotation);
            markers.Add(marker, annotation);

            marker.PropertyChanged += Marker_PropertyChanged;
        }
        
        private void RemoveMarker(Marker marker)
        {
            marker.PropertyChanged -= Marker_PropertyChanged;

            Control.RemoveAnnotation(markers[marker]);
            markers.Remove(marker);
        }

        private void Marker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Marker marker = sender as Marker;
            Control.RemoveAnnotation(markers[marker]);
            Control.AddAnnotation(markers[marker]);            
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

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return UIViewExtensions.GetSizeRequest(Control, widthConstraint, heightConstraint, -1.0, -1.0);
        }
    }
}
