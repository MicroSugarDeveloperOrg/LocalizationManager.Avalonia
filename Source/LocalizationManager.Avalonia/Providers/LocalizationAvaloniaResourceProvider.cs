namespace LocalizationManager.Avalonia.Providers;
internal class LocalizationAvaloniaResourceProvider : ILocalizationProvider
{
    IEnumerable<CultureInfo>? ILocalizationLanguageMap.LanguageMaps { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    string? ILocalizationProvider.Category => throw new NotImplementedException();

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    bool ILocalizationProvider.AddResource(string resourceDirectory, string baseName, Type? usingResourceSet)
    {
        throw new NotImplementedException();
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture, params object[] arguments)
    {
        throw new NotImplementedException();
    }

}
