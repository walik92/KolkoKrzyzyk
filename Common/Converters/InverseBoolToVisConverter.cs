using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
    public class InverseBoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var flag = false;

            if (value is bool)
            {
                flag = (bool) value;
            }

            if (parameter != null)
            {
                if (bool.Parse((string) parameter))
                {
                    flag = !flag;
                }
            }

            if (flag)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}