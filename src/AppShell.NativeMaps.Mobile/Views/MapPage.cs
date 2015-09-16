using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppShell.NativeMaps.Mobile
{
    [View(typeof(MapViewModel))]
    public class MapPage : ContentPage
    {
        private static Dictionary<int, double> ZoomLevelMapping = new Dictionary<int, double>();

        static MapPage()
        {
            double startRadius = 78271.52;

            for (int i = 1; i < 20; i++)
            {
                ZoomLevelMapping.Add(i, startRadius);
                startRadius /= 2.0;
            }
        }

        public static readonly BindableProperty ZoomLevelProperty = BindableProperty.Create<MapPage, double>(d => d.ZoomLevel, 10.0, propertyChanged: ZoomLevelPropertyChanged);
        public static readonly BindableProperty LatitudeProperty = BindableProperty.Create<MapPage, double>(d => d.Latitude, 0.0, propertyChanged: LatitudePropertyChanged);
        public static readonly BindableProperty LongitudeProperty = BindableProperty.Create<MapPage, double>(d => d.Longitude, 0.0, propertyChanged: LongitudePropertyChanged);

        public double ZoomLevel { get { return (double)GetValue(ZoomLevelProperty); } set { SetValue(ZoomLevelProperty, value); } }
        public double Latitude { get { return (double)GetValue(LatitudeProperty); } set { SetValue(LatitudeProperty, value); } }
        public double Longitude { get { return (double)GetValue(LongitudeProperty); } set { SetValue(LongitudeProperty, value); } }
        public double Radius
        {
            get
            {
                int zoomLevel = (int)ZoomLevel;
                if (ZoomLevelMapping.ContainsKey(zoomLevel))
                    return ZoomLevelMapping[zoomLevel] * 200;

                return 5000;
            }
        }

        public static void ZoomLevelPropertyChanged(BindableObject d, double oldValue, double newValue)
        {
            MapPage mapPage = d as MapPage;
            mapPage.map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(mapPage.Latitude, mapPage.Longitude), new Distance(mapPage.Radius)));
        }

        public static void LatitudePropertyChanged(BindableObject d, double oldValue, double newValue)
        {
            MapPage mapPage = d as MapPage;
            mapPage.map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(mapPage.Latitude, mapPage.Longitude), new Distance(mapPage.Radius)));
        }

        public static void LongitudePropertyChanged(BindableObject d, double oldValue, double newValue)
        {
            MapPage mapPage = d as MapPage;
            mapPage.map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(mapPage.Latitude, mapPage.Longitude), new Distance(mapPage.Radius)));
        }

        private Map map;

        public MapPage()
        {
            map = new Map();

            Content = map;

            SetBinding(ZoomLevelProperty, new Binding("ZoomLevel"));
            SetBinding(LatitudeProperty, new Binding("Latitude"));
            SetBinding(LongitudeProperty, new Binding("Longitude"));
        }
    }
}
