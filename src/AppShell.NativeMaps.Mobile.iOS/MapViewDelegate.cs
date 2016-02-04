using Foundation;
using MapKit;
using ObjCRuntime;
using UIKit;

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
    }
}
