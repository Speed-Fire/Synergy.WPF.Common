using System;
using System.Globalization;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
	internal class BoolAndConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var span = values.AsSpan();

			foreach (var item in span)
			{
				if (item is bool b && b == false)
					return false;
			}

			return true;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
