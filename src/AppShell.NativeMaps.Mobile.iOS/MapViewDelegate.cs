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

                MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationId);

                if (annotationView == null)
                    annotationView = string.IsNullOrEmpty(marker.Icon) ? new MKPinAnnotationView(annotation, annotationId) : new MKAnnotationView(annotation, annotationId);

                if (!string.IsNullOrEmpty(marker.Icon))
                    annotationView.Image = UIImage.FromBundle(marker.Icon);

                if (!string.IsNullOrEmpty(marker.Title) || !string.IsNullOrEmpty(marker.Content))
                    annotationView.CanShowCallout = true;

                annotationView.Draggable = marker.Draggable;

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
            //mapViewElement.ZoomLevel = Math.Log(45.0 * mapViewElement.Width / mapView.Region.Span.LongitudeDelta) / Math.Log(2) - 6.0;
            //mapViewElement.Center = new Location(mapView.Region.Center.Latitude, mapView.Region.Center.Longitude);
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            if (view.Annotation is MarkerAnnotation)
                mapViewElement.SelectedMarker = mapViewRenderer.Markers[view.Annotation as MarkerAnnotation];
        }
    }
}
