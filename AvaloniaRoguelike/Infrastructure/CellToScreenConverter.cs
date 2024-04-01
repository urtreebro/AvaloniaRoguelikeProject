using Avalonia.Data.Converters;
using AvaloniaRoguelike.Model;
using System;
using System.Globalization;

namespace AvaloniaRoguelike.Infrastructure
{
    public class CellToScreenConverter : IValueConverter
    {
        public static CellToScreenConverter Instance { get; } = new CellToScreenConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * GameField.CellSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
