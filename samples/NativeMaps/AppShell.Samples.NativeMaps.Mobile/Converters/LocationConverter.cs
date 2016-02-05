using AppShell.NativeMaps;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppShell.Samples.NativeMaps.Mobile.Converters
{
    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location location = value as Location;
            return string.Format("{0:0.0000}/{1:0.0000}", location.Latitude, location.Longitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
