using AppShell.NativeMaps.Mobile;
using AppShell.NativeMaps.Mobile.UWP;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.Storage.Streams;
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
                if (e.OldElement.Markers != null)
                {
                    if (e.OldElement.Markers is ObservableCollection<Marker>)
                        (e.OldElement.Markers as ObservableCollection<Marker>).CollectionChanged -= Markers_CollectionChanged;
                }
            }

            if (e.NewElement != null)
            {
                SetMapType();
                SetCenter();
                SetZoomLevel();

                if (e.NewElement.Markers != null)
                {
                    if (e.NewElement.Markers is ObservableCollection<Marker>)
                        (e.NewElement.Markers as ObservableCollection<Marker>).CollectionChanged += Markers_CollectionChanged;

                    foreach (Marker marker in e.NewElement.Markers)
                        AddMarker(marker);
                }
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

        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Control.MapElements.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Marker marker in e.NewItems)
                    AddMarker(marker);
            }
        }

        private void AddMarker(Marker marker)
        {
            MapIcon mapIcon = new MapIcon();
            mapIcon.Location = new Geopoint(new BasicGeoposition() { Latitude = marker.Center.Latitude, Longitude = marker.Center.Longitude });

            if (!string.IsNullOrEmpty(marker.Icon))
                mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/{0}", marker.Icon)));
            
            if (!string.IsNullOrEmpty(marker.Title))
                mapIcon.Title = marker.Title;

            Control.MapElements.Add(mapIcon);
        }
    }
}
