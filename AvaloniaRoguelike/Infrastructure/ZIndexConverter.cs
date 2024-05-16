using System;
using System.Globalization;

using Avalonia.Data.Converters;

using AvaloniaRoguelike.Model;

namespace AvaloniaRoguelike.Infrastructure;

public class ZIndexConverter : IValueConverter
{
    public static ZIndexConverter Instance { get; } = new ZIndexConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Player)
            return 2;
        if (value is Enemy)
            return 1;
        else 
            return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}