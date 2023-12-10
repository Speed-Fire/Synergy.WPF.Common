using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
	public class IsGreaterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter is null && value is null)
				return false;

			if (value is null)
				return false;

			if(parameter is null)
				return true;

			return Comparer.Default.Compare(value, parameter) > 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
