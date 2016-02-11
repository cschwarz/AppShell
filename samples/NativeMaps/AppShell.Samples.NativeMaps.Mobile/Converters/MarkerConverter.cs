using AppShell.NativeMaps;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppShell.Samples.NativeMaps.Mobile.Converters
{
    public class MarkerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Marker marker = value as Marker;

            if (marker == null)
                return "None";

            return string.Format("{0:0.0000}/{1:0.0000}", marker.Center.Latitude, marker.Center.Longitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
