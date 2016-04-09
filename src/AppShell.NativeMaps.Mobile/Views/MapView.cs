using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppShell.NativeMaps.Mobile
{
    [View(typeof(MapViewModel))]
    public class MapView : View
    {
        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create("ZoomLevel", typeof(double), typeof(MapView), 10.0, BindingMode.TwoWay);
        public static readonly BindableProperty CenterProperty = BindableProperty.Create("Center", typeof(Location), typeof(MapView), null, BindingMode.TwoWay);
        public static readonly BindableProperty MapTypeProperty = BindableProperty.Create("MapType", typeof(MapType), typeof(MapView), MapType.Roads);
        public static readonly BindableProperty MarkersProperty = BindableProperty.Create("Markers", typeof(IEnumerable<Marker>), typeof(MapView), null);
        public static readonly BindableProperty TileOverlaysProperty = BindableProperty.Create("TileOverlays", typeof(IEnumerable<TileOverlay>), typeof(MapView), null);
        public static readonly BindableProperty SelectedMarkerProperty = BindableProperty.Create("SelectedMarker", typeof(Marker), typeof(MapView), null, BindingMode.TwoWay);
        public static readonly BindableProperty NavigationDestinationProperty = BindableProperty.Create("NavigationDestination", typeof(Location), typeof(MapView), null);

        public double ZoomLevel { get { return (double)GetValue(ZoomLevelProperty); } set { SetValue(ZoomLevelProperty, value); } }
        public Location Center { get { return (Location)GetValue(CenterProperty); } set { SetValue(CenterProperty, value); } }
        public MapType MapType { get { return (MapType)GetValue(MapTypeProperty); } set { SetValue(MapTypeProperty, value); } }
        public IEnumerable<Marker> Markers { get { return (IEnumerable<Marker>)GetValue(MarkersProperty); } set { SetValue(MarkersProperty, value); } }
        public IEnumerable<TileOverlay> TileOverlays { get { return (IEnumerable<TileOverlay>)GetValue(TileOverlaysProperty); } set { SetValue(TileOverlaysProperty, value); } }
        public Marker SelectedMarker { get { return (Marker)GetValue(SelectedMarkerProperty); } set { SetValue(SelectedMarkerProperty, value); } }
        public Location NavigationDestination { get { return (Location)GetValue(NavigationDestinationProperty); } set { SetValue(NavigationDestinationProperty, value); } }
        public double Radius
        {
            get { return 16000000 / Math.Pow(2, ZoomLevel); }
            set { ZoomLevel = Math.Round(Math.Log(16000000 / value) / Math.Log(2), 2); }
        }

        public MapView()
        {
            SetBinding(ZoomLevelProperty, new Binding("ZoomLevel"));
            SetBinding(CenterProperty, new Binding("Center"));
            SetBinding(MapTypeProperty, new Binding("MapType"));
            SetBinding(MarkersProperty, new Binding("Markers"));
            SetBinding(TileOverlaysProperty, new Binding("TileOverlays"));
            SetBinding(SelectedMarkerProperty, new Binding("SelectedMarker"));
            SetBinding(NavigationDestinationProperty, new Binding("NavigationDestination"));
        }
    }
}
