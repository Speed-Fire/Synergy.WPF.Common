using System;
using System.Globalization;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
	public class IsEqualConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(parameter is null && value is null)
				return true;

			if(parameter is null ^ value is null)
				return false;

			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class IsEqualMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var res = true;

			for(int i = 1; i < values.Length && res; i++)
			{
				res &= values[i - 1].Equals(values[i]);
			}

			return res;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
