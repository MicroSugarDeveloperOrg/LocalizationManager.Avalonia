namespace LocalizationManager;
public interface ILocalizationProvider : IDisposable
{
    string GetString(string token, CultureInfo culture);
    string GetString(string token, CultureInfo culture, params object[] arguments);
}
