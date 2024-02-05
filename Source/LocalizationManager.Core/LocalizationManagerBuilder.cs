namespace LocalizationManager;
public static class LocalizationManagerBuilder
{
    public static void Build(Func<ILocalizationProvider[]> configDelegate)
    {
        LocalizationManagerExtensions.Default = LocalizationManagerExtensions.Make(configDelegate);
    }

    public static void Build(Func<ILocalizationProvider> configDelegate)
    {
        LocalizationManagerExtensions.Default = LocalizationManagerExtensions.Make(configDelegate);
    }
}
