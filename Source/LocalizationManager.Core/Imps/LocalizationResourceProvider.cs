using System.Resources;

namespace LocalizationManager.Core.Imps;
internal class LocalizationResourceProvider : ILocalizationProvider, ILocalizationResourceProvider
{
    public LocalizationResourceProvider(ResourceManager resourceManager) 
    {
        ((ILocalizationResourceProvider)this).AddResource(resourceManager);
    }

    public LocalizationResourceProvider(IEnumerable<ResourceManager> resourceManagers)
    {
        _resourceManagers = resourceManagers.ToList();
    }

    public LocalizationResourceProvider(string resourceDirectory, string baseName, Type? usingResourceSet = null)
        : this(LoadResources(resourceDirectory, baseName, usingResourceSet))
    {
    }

    readonly List<ResourceManager> _resourceManagers = new();

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

    IEnumerable<CultureInfo>? ILocalizationLanguageMap.LanguageMaps
    {
        get;
        set;
    }

    public string? Category { get; set; }

    bool ILocalizationProvider.AddResource(string resourceDirectory, string baseName, Type? usingResourceSet) => ((ILocalizationResourceProvider)this).AddResource(LoadResources(resourceDirectory, baseName, usingResourceSet));

    bool ILocalizationResourceProvider.AddResource(ResourceManager resourceManager)
    {
        if (resourceManager is null)
            return false;

     
        _resourceManagers.Add(resourceManager);
        return true;
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(token))
            return string.Empty;

        try
        {
            var value = _resourceManagers.Select(item => item.GetString(token, culture)).FirstOrDefault();
            return value;
        }
        catch (Exception)
        {
            var value = _resourceManagers.Select(item => item.GetString(token)).FirstOrDefault();
            return value;
        }
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture, params object[] arguments) => string.Format(((ILocalizationProvider)this).GetString(token, culture), arguments);

    public void Dispose()
    {
        _resourceManagers.All(item =>
        {
            item.ReleaseAllResources();
            return true;
        });

        _resourceManagers.Clear();
    }


}
