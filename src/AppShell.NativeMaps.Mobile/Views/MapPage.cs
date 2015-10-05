using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppShell.NativeMaps.Mobile
{
    [View(typeof(MapViewModel))]
    public class MapPage : ContentPage
    {
        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create<MapPage, double>(d => d.ZoomLevel, 10.0, BindingMode.TwoWay, propertyChanged: ZoomLevelPropertyChanged);
        public static readonly BindableProperty CenterProperty = BindableProperty.Create<MapPage, Location>(d => d.Center, null, BindingMode.TwoWay, propertyChanged: CenterPropertyChanged);
        
        public double ZoomLevel { get { return (double)GetValue(ZoomLevelProperty); } set { SetValue(ZoomLevelProperty, value); } }
        public Location Center { get { return (Location)GetValue(CenterProperty); } set { SetValue(CenterProperty, value); } }
        public double Radius
        {
            get { return 16000000 / Math.Pow(2, ZoomLevel); }
            set { ZoomLevel = Math.Round(Math.Log(16000000 / value) / Math.Log(2), 2); }
        }

        public static void ZoomLevelPropertyChanged(BindableObject d, double oldValue, double newValue)
        {
            if (oldValue == newValue)
                return;

            MapPage mapPage = d as MapPage;

            if (mapPage.map.VisibleRegion != null)
                mapPage.map.MoveToRegion(MapSpan.FromCenterAndRadius(mapPage.map.VisibleRegion.Center, new Distance(mapPage.Radius)));
        }

        public static void CenterPropertyChanged(BindableObject d, Location oldValue, Location newValue)
        {
            if (oldValue == newValue)
                return;

            MapPage mapPage = d as MapPage;

            if (newValue != null)
                mapPage.map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(newValue.Latitude, newValue.Longitude), new Distance(mapPage.Radius)));
        }
        
        private Map map;

        public MapPage()
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
                Center = new Location(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude);
                Radius = map.VisibleRegion.Radius.Meters;
            }
        }
    }
}
