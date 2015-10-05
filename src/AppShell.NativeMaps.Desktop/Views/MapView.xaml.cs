using Microsoft.Maps.MapControl.WPF;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Maps = Microsoft.Maps.MapControl.WPF;

namespace AppShell.NativeMaps.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    [View(typeof(MapViewModel))]
    public partial class MapView : UserControl
    {
        public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register("ApiKey", typeof(string), typeof(MapView), new PropertyMetadata(string.Empty, ApiKeyPropertyChanged));
        public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(MapView), new PropertyMetadata(10.0, ZoomLevelChanged));
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Location), typeof(MapView), new PropertyMetadata(null, CenterChanged));
        
        public string ApiKey { get { return (string)GetValue(ApiKeyProperty); } set { SetValue(ApiKeyProperty, value); } }
        public double ZoomLevel { get { return (double)GetValue(ZoomLevelProperty); } set { SetValue(ZoomLevelProperty, value); } }
        public Location Center { get { return (Location)GetValue(CenterProperty); } set { SetValue(CenterProperty, value); } }
        
        public static void ApiKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mapView = d as MapView;

            if (e.NewValue != null)
                mapView.Map.CredentialsProvider = new ApplicationIdCredentialsProvider((string)e.NewValue);
        }

        public static void ZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mapView = d as MapView;

            if (e.NewValue != null)
                mapView.Map.ZoomLevel = Convert.ToDouble(e.NewValue, CultureInfo.InvariantCulture);
        }

        public static void CenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mapView = d as MapView;

            if (e.NewValue != null)
            {
                Location location = e.NewValue as Location;
                mapView.Map.Center = new Maps.Location(location.Latitude, location.Longitude);
            }
        }
        
        public MapView()
        {
            InitializeComponent();

            Map.ViewChangeEnd += Map_ViewChangeEnd;

            SetBinding(ApiKeyProperty, new Binding("ApiKey"));
            SetBinding(ZoomLevelProperty, new Binding("ZoomLevel") { Mode = BindingMode.TwoWay });
            SetBinding(CenterProperty, new Binding("Center") { Mode = BindingMode.TwoWay });
        }

        private void Map_ViewChangeEnd(object sender, MapEventArgs e)
        {
            Center = new Location(Map.Center.Latitude, Map.Center.Longitude);
            ZoomLevel = Map.ZoomLevel;
        }
    }
}
