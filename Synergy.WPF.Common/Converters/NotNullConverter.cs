﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
	public class NotNullConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is not null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
