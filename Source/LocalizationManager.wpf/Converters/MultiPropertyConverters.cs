using System.Globalization;
using System.Windows.Data;

namespace LocalizationManager.wpf.Converters;
internal class MultiPropertyConverters : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not ILocalizationManager localizationManager)
            return default;

        if (values is null)
            return default;

        if (values.Length < 4)
            return default;

        if (values[0] is null)
            return default;

        var tokenString = values[0].ToString();
        if (string.IsNullOrWhiteSpace(tokenString))
            return default;

        var category = values[1]?.ToString();
        var arguments = values[2] as object[];

        if (arguments is not null)
            return localizationManager.GetValue(tokenString, category, arguments);

        return localizationManager.GetValue(tokenString, category);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
