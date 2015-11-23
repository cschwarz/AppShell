using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppShell.NativeMaps.Mobile
{
    [View(typeof(MapViewModel))]
    public class MapView : ContentView
    {
        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create<MapView, double>(d => d.ZoomLevel, 10.0, BindingMode.TwoWay, propertyChanged: ZoomLevelPropertyChanged);
        public static readonly BindableProperty CenterProperty = BindableProperty.Create<MapView, Location>(d => d.Center, null, BindingMode.TwoWay, propertyChanged: CenterPropertyChanged);
        
        public double ZoomLevel { get { return (double)GetValue(ZoomLevelProperty); } set { SetValue(ZoomLevelProperty, value); } }
        public Location Center { get { return (Location)GetValue(CenterProperty); } set { SetValue(CenterProperty, value); } }
        public double Radius
        {
            get { return 16000000 / Math.Pow(2, ZoomLevel); }
            set { ZoomLevel = Math.Round(Math.Log(16000000 / value) / Math.Log(2), 2); }
        }

        public static void ZoomLevelPropertyChanged(BindableObject d, double oldValue, double newValue)
        {    
            MapView mapView = d as MapView;
            
            if (mapView.map.VisibleRegion != null && !mapView.ignoreInternalUpdate)
                mapView.map.MoveToRegion(MapSpan.FromCenterAndRadius(mapView.map.VisibleRegion.Center, new Distance(mapView.Radius)));
        }

        public static void CenterPropertyChanged(BindableObject d, Location oldValue, Location newValue)
        {
            MapView mapView = d as MapView;
            
            if (newValue != null && !mapView.ignoreInternalUpdate)
                mapView.map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(newValue.Latitude, newValue.Longitude), new Distance(mapView.Radius)));
        }
        
        private Map map;
        private bool ignoreInternalUpdate;

        public MapView()
        {
            map = new Map();
            map.PropertyChanged += Map_PropertyChanged;

            Content = map;

            SetBinding(ZoomLevelProperty, new Binding("ZoomLevel"));
            SetBinding(CenterProperty, new Binding("Center"));
        }

        private void Map_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Map map = sender as Map;

            if (e.PropertyName == "VisibleRegion" && map != null && map.VisibleRegion != null)
            {
                ignoreInternalUpdate = true;
                Center = new Location(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude);
                Radius = map.VisibleRegion.Radius.Meters;
                ignoreInternalUpdate = false;
            }
        }
    }
}
