namespace LocalizationManager.Avalonia;
public static class ApplicationExtension
{
    public static AvaloniaLocator UseLocalizationManager(this AvaloniaLocator resolver, Func<ILocalizationProvider> configDelegate)
    {
        LocalizationManagerExtensions.Default = LocalizationManagerExtensions.Make(configDelegate);
        return resolver.BindToSelf(LocalizationManagerExtensions.Default);
    }
}
