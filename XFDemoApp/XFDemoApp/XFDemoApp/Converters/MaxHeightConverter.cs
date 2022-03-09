using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XFDemoApp.Converters
{
    /// <summary>
    /// Clamps the result between the value and the passed in parameter.
    /// If the parameter is omitted the value will be returned.
    /// </summary>
    public class MaxHeightConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double.TryParse(value?.ToString() ?? "0", out double numericValue);
            double.TryParse(parameter?.ToString() ?? "0", out double maxValue);

            return Math.Min(numericValue, Math.Max(numericValue, maxValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
