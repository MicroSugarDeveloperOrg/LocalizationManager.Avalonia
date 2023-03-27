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

    string ILocalizationProvider.GetString(string token, CultureInfo culture) => _resourceManager.GetString(token, culture);
    string ILocalizationProvider.GetString(string token, CultureInfo culture, params object[] arguments) => string.Format(_resourceManager.GetString(token, culture), arguments);

    public void Dispose()
    {
        _resourceManager.ReleaseAllResources(); 
    }
}
