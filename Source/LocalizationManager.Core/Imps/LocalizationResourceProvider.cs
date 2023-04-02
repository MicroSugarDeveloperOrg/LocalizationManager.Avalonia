namespace LocalizationManager.Core.Imps;
internal class LocalizationResourceProvider : ILocalizationProvider
{
    public LocalizationResourceProvider(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    public LocalizationResourceProvider(string resourceDirectory, string baseName, Type? usingResourceSet = null)
        : this(LoadResources(resourceDirectory, baseName, usingResourceSet))
    {
    }

    readonly ResourceManager _resourceManager;

    static ResourceManager LoadResources(string resourceDirectory, string baseName, Type? usingResourceSet = null)
    {
        var resourceFileName = Path.Combine(resourceDirectory, $"{baseName}.resources");
        if (!File.Exists(resourceFileName))
        {
            using var writer = new ResourceWriter(resourceFileName);
            writer.AddResource(string.Empty, string.Empty);
        }
        var resourceManager = ResourceManager.CreateFileBasedResourceManager(baseName, resourceDirectory, usingResourceSet);
        return resourceManager;
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(token))
            return string.Empty;

        try
        {
            var value = _resourceManager.GetString(token, culture);
            return value;
        }
        catch (Exception)
        {
            var value = _resourceManager.GetString(token);
            return value;
        }
    }
    string ILocalizationProvider.GetString(string token, CultureInfo culture, params object[] arguments) => string.Format(((ILocalizationProvider)this).GetString(token, culture), arguments);

    public void Dispose()
    {
        _resourceManager.ReleaseAllResources(); 
    }
}
