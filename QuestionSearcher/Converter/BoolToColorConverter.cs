using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QuestionSearcher.Converter
{
  internal class BoolToColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool boolValue = (bool)value;
      return boolValue ? Brushes.Green : Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is SolidColorBrush solidColorBrush)
        return solidColorBrush == Brushes.Green;
      return false;
    }
  }
}