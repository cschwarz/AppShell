using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.UWP;
using System.ComponentModel;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]

namespace AppShell.NativeMaps.Mobile.UWP
{
    public class MapViewRenderer : ViewRenderer<MapView, MapControl>
    {
        public static string ApiKey { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null && e.NewElement != null)
            {
                MapControl mapControl = new MapControl();

                if (!string.IsNullOrEmpty(ApiKey))
                    mapControl.MapServiceToken = ApiKey;
                
                SetNativeControl(mapControl);
            }

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                SetMapType();
                SetCenter();
                SetZoomLevel();
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

        private void SetMapType()
        {
            Control.Style = Element.MapType.ToNativeMapType();
        }

        private void SetCenter()
        {
            if (Element.Center != null)
                Control.Center = new Geopoint(new BasicGeoposition() { Latitude = Element.Center.Latitude, Longitude = Element.Center.Longitude });
        }

        private void SetZoomLevel()
        {
            Control.ZoomLevel = Element.ZoomLevel;
        }
    }
}
