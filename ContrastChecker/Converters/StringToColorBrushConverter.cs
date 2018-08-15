using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ContrastChecker.Helpers;

namespace ContrastChecker.Converters
{
    public class StringToColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string colorString))
            {
                return Color.FromArgb(0, 0, 0, 0);
            }

            if (ColorUtilities.TryParseColorString(colorString, out var color))
            {
                return new SolidColorBrush(color);
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush brush))
            {
                return Binding.DoNothing;
            }

            var c = brush.Color;

            return $"{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
        }
    }
}
