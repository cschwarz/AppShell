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
                    annotationView = new MKAnnotationView(annotation, annotationId);

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
        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);

            MKMapView mapView = new MKMapView();

            mapView.MapType = Element.MapType.ToNativeMapType();
            
            if (Element.Markers != null)
            {
                foreach (Marker marker in Element.Markers)
                    mapView.AddAnnotation(new MarkerAnnotation(marker));
            }

            if (Element.TileOverlays != null)
            {
                foreach (TileOverlay tileOverlay in Element.TileOverlays)
                {
                    if (tileOverlay is UrlTileOverlay)
                    {
                        MKTileOverlay overlay = new MKTileOverlay((tileOverlay as UrlTileOverlay).Url);
                        overlay.CanReplaceMapContent = true;
                        overlay.TileSize = new CoreGraphics.CGSize(tileOverlay.TileWidth, tileOverlay.TileHeight);
                        mapView.AddOverlay(overlay, MKOverlayLevel.AboveLabels);
                    }
                }
            }

            mapView.Delegate = new MapViewDelegate();

            SetNativeControl(mapView);

            if (e.OldElement != null)
                e.OldElement.SizeChanged -= SizeChanged;
            if (e.NewElement != null)
                e.NewElement.SizeChanged += SizeChanged;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MapView.CenterProperty.PropertyName)
                SetCenter();
        }

        private void SizeChanged(object sender, EventArgs e)
        {
            SetCenter();
        }

        private void SetCenter()
        {
            if (Control.Center != null)
                Control.SetRegion(new MKCoordinateRegion(new CLLocationCoordinate2D(Element.Center.Latitude, Element.Center.Longitude), new MKCoordinateSpan(0, 360.0 / Math.Pow(2, Element.ZoomLevel + 1) * Element.Width / 256.0)), false);
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return UIViewExtensions.GetSizeRequest(Control, widthConstraint, heightConstraint, -1.0, -1.0);
        }
    }
}
