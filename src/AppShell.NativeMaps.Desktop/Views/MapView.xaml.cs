using Microsoft.Maps.MapControl.WPF;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        public static readonly DependencyProperty LatitudeProperty = DependencyProperty.Register("Latitude", typeof(double), typeof(MapView), new PropertyMetadata(0.0, LatitudeChanged));
        public static readonly DependencyProperty LongitudeProperty = DependencyProperty.Register("Longitude", typeof(double), typeof(MapView), new PropertyMetadata(0.0, LongitudeChanged));

        public string ApiKey { get { return (string)GetValue(ApiKeyProperty); } set { SetValue(ApiKeyProperty, value); } }
        public double ZoomLevel { get { return (double)GetValue(ZoomLevelProperty); } set { SetValue(ZoomLevelProperty, value); } }
        public double Latitude { get { return (double)GetValue(LatitudeProperty); } set { SetValue(LatitudeProperty, value); } }
        public double Longitude { get { return (double)GetValue(LongitudeProperty); } set { SetValue(LongitudeProperty, value); } }

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

        public static void LatitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mapView = d as MapView;

            if (e.NewValue != null)
                mapView.Map.Center = new Location(Convert.ToDouble(e.NewValue, CultureInfo.InvariantCulture), mapView.Longitude);
        }

        public static void LongitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView mapView = d as MapView;

            if (e.NewValue != null)
                mapView.Map.Center = new Location(mapView.Latitude, Convert.ToDouble(e.NewValue, CultureInfo.InvariantCulture));
        }      

        public MapView()
        {
            InitializeComponent();

            SetBinding(ApiKeyProperty, new Binding("ApiKey"));
            SetBinding(ZoomLevelProperty, new Binding("ZoomLevel"));
            SetBinding(LatitudeProperty, new Binding("Latitude"));
            SetBinding(LongitudeProperty, new Binding("Longitude"));   
        }
    }
}
