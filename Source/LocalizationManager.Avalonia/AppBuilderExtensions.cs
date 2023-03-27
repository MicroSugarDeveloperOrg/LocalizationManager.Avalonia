namespace LocalizationManager.Avalonia;
public static class AppBuilderExtensions
{
    public static AppBuilder UseLocalizationManager(this AppBuilder builder, Func<ILocalizationProvider> configDelegate)
    {
        builder.AfterPlatformServicesSetup(app =>
        {
            LocalizationManagerExtensions.Default = LocalizationManagerExtensions.Make(configDelegate);
            AvaloniaLocator.CurrentMutable.BindToSelf(LocalizationManagerExtensions.Default);
        });

        return builder;
    }
}
