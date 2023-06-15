using System;
using System.Globalization;
using System.Windows.Data;

namespace QuestionSearcher.Converter
{
  internal class ArrayToLengthConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is Array array)
      {
        return array.Length;
      }

      return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}