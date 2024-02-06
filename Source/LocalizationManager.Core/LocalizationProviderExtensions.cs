using LocalizationManager.Core.Imps;

namespace LocalizationManager;

public class LocalizationProviderExtensions
{
    public static ILocalizationProvider MakeXmlFileProvider(string? category, Func<string, CultureInfo> provider, string baseDirectory, string baseName)
    {
        if (provider is null) throw new ArgumentNullException(nameof(provider));
        if (string.IsNullOrWhiteSpace(baseName)) throw new ArgumentNullException(nameof(baseName));
        if (string.IsNullOrWhiteSpace(baseDirectory)) throw new ArgumentNullException(nameof(baseDirectory));

        if (!Directory.Exists(baseDirectory)) throw new ArgumentNullException(nameof(baseDirectory), $"The baseDirectory:{baseDirectory} is not exists!");
        return new LocalizationXmlFileProvider(category, provider, baseDirectory, baseName);
    }

    public static ILocalizationProvider MakeXmlFilesProvider(string? category, Func<string, CultureInfo> provider, params (string baseDirectory, string baseName)[] parameters)
    {
        if (provider is null) throw new ArgumentNullException(nameof(provider));
        return new LocalizationXmlFileProvider(category, provider, parameters);
    }

    public static ILocalizationProvider MakeResourceProvider(string? category, ResourceManager resourceManager)
    {
        if (resourceManager is null) throw new ArgumentNullException(nameof(resourceManager));
        resourceManager.GetString(string.Empty);
        return new LocalizationResourceProvider(category, resourceManager);
    }

    public static ILocalizationProvider MakeResourceProvider(string? category, IEnumerable<ResourceManager> resourceManagers)
    {
        if (resourceManagers is null) throw new ArgumentNullException(nameof(resourceManagers));
        return new LocalizationResourceProvider(category, resourceManagers);
    }

    public static ILocalizationProvider MakeResourceProvider(string? category, string resourceDirectory, string baseName, Type? usingResourceSet = null)
    {
        if (string.IsNullOrWhiteSpace(baseName)) throw new ArgumentNullException(nameof(baseName));
        if (string.IsNullOrWhiteSpace(resourceDirectory)) throw new ArgumentNullException(nameof(resourceDirectory));

        if (!Directory.Exists(resourceDirectory)) throw new ArgumentNullException(nameof(resourceDirectory), $"The resourceDirectory:{resourceDirectory} is not exists!");
        return new LocalizationResourceProvider(category, resourceDirectory, baseName, usingResourceSet);
    }

}
