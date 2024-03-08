using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
    public class Bool2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null)
                return Visibility.Collapsed;

            if(parameter is not null && parameter is string str)
            {
                if(str.Equals("Invert"))
					return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
			}

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
