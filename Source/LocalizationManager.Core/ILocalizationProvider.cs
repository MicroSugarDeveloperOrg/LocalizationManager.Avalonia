namespace LocalizationManager;
public interface ILocalizationProvider : ILocalizationLanguageMap, IDisposable
{
    string? Category { get; }
    bool AddResource(string resourceDirectory, string baseName, Type? usingResourceSet = null);
    string GetString(string token, CultureInfo culture);
    string GetString(string token, CultureInfo culture, params object[] arguments);
}
