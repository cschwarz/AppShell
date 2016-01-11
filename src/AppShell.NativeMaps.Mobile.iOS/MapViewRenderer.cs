using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.iOS;
using CoreLocation;
using Foundation;
using MapKit;
using ObjCRuntime;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]

namespace AppShell.NativeMaps.Mobile.iOS
{
    public class MapViewDelegate : MKMapViewDelegate
    {
        static string annotationId = "MarkerAnnotation";

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation is MarkerAnnotation)
            {
                Marker marker = (annotation as MarkerAnnotation).Marker;
                
                MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationId);

                if (annotationView == null)
                    annotationView = string.IsNullOrEmpty(marker.Icon) ? new MKPinAnnotationView(annotation, annotationId) : new MKAnnotationView(annotation, annotationId);

                if (!string.IsNullOrEmpty(marker.Icon))
                    annotationView.Image = UIImage.FromBundle(marker.Icon);

                if (!string.IsNullOrEmpty(marker.Title) || !string.IsNullOrEmpty(marker.Content))
                    annotationView.CanShowCallout = true;

                return annotationView;
            }

            return base.GetViewForAnnotation(mapView, annotation);
        }

        public override MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            NSObject nsObject = Runtime.GetNSObject(overlay.Handle);

            if (nsObject is MKTileOverlay)
                return new MKTileOverlayRenderer((MKTileOverlay)nsObject);

            return null;
        }
    }

    public class MarkerAnnotation : MKAnnotation
    {
        public Marker Marker { get; private set; }

        public MarkerAnnotation(Marker marker)
        {
            Marker = marker;
        }

        public override CLLocationCoordinate2D Coordinate { get { return new CLLocationCoordinate2D(Marker.Center.Latitude, Marker.Center.Longitude); } }
        public override string Title { get { return Marker.Title; } }
        public override string Subtitle { get { return Marker.Content; } }
    }

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

                Control.Delegate = new MapViewDelegate();
            }
        }

        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Control.RemoveAnnotations(markers.Select(m => m.Value).ToArray());
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
            MarkerAnnotation annotation = new MarkerAnnotation(marker);
            Control.AddAnnotation(annotation);
            markers.Add(marker, annotation);
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
