using System;
using System.Globalization;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
	internal class Text2BooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is null || value is not string || string.IsNullOrEmpty((string)value))
				return false;

			return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
