using CoreGraphics;
using Foundation;
using MapKit;
using ObjCRuntime;
using System;
using UIKit;

namespace AppShell.NativeMaps.Mobile.iOS
{
    public class MapViewDelegate : MKMapViewDelegate
    {
        static string annotationId = "MarkerAnnotation";

        private MapViewRenderer mapViewRenderer;
        private MapView mapViewElement;

        public MapViewDelegate(MapViewRenderer mapViewRenderer, MapView mapView)
        {
            this.mapViewRenderer = mapViewRenderer;
            this.mapViewElement = mapView;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation is MarkerAnnotation)
            {
                Marker marker = (annotation as MarkerAnnotation).Marker;
                MKAnnotationView annotationView = marker == null || (!marker.Draggable && marker.Label == null) ? mapView.DequeueReusableAnnotation(annotationId) : null;

                if (annotationView == null)
                    annotationView = string.IsNullOrEmpty(marker.Icon) ? new MKPinAnnotationView(annotation, annotationId) : new MKAnnotationView(annotation, annotationId);

                if (!string.IsNullOrEmpty(marker.Icon))
                    annotationView.Image = UIImage.FromBundle(marker.Icon);

                if (!string.IsNullOrEmpty(marker.Title) || !string.IsNullOrEmpty(marker.Content))
                    annotationView.CanShowCallout = true;

                annotationView.Draggable = marker.Draggable;
                if (!marker.Draggable && marker.Label != null)
                    AddLabelSubView(marker, annotationView);
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

        public override void RegionChanged(MKMapView mapView, bool animated)
        {
            mapViewElement.MapZoomLevel = Math.Log(45.0 * mapViewElement.Width / mapView.Region.Span.LongitudeDelta) / Math.Log(2) - 6.0;
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            if (view.Annotation is MarkerAnnotation)
                mapViewElement.SelectedMarker = mapViewRenderer.Markers[view.Annotation as MarkerAnnotation];
        }

        private void AddLabelSubView(Marker marker, MKAnnotationView annotationView)
        {
            if (annotationView == null || annotationView.Image == null)
                return;
            UILabel label = new UILabel();
            label.TextAlignment = UITextAlignment.Center;
            label.TextColor = UIColor.Black;
            label.Font = UIFont.FromName("GillSans-Light", annotationView.Image.Size.Height / 2.0f);
            label.Text = marker.Label.Text;
            label.SizeToFit();
            label.Frame = new CGRect(-label.Bounds.Width * 0.25, annotationView.Image.Size.Height, label.Bounds.Width, label.Bounds.Height);
            annotationView.AddSubview(label);
        }
    }
}
