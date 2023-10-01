using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Synergy.WPF.Common.Converters
{
    public class LongStroryShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var maxLength = (int)parameter;

            var str = (string)value;

            if(string.IsNullOrEmpty(str) )
            {
                return str;
            }

            if(str.Length > maxLength)
            {
                return str.Substring(0, maxLength - 3) + "...";
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
