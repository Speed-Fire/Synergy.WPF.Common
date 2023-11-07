using System;
using System.Globalization;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
	public class InvertBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not bool b)
				throw new ArgumentException("Value is not a boolean!");

			return !b;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture);
		}
	}
}
