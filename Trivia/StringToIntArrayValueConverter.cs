using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Trivia
{
    public class StringToIntArrayValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int[] intArray = value as int[];
            if (intArray == null) return string.Empty;
            return string.Join(", ", Array.ConvertAll(intArray, x => x.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userString = parameter as string;
            if (userString == null) return new int[0];
            int[] convertedInts = Array.ConvertAll(userString.Replace(" ", string.Empty).Split(','), x => int.Parse(x));
            return convertedInts;
        }
    }
}
