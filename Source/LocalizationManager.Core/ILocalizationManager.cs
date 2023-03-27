namespace LocalizationManager;
public interface ILocalizationManager : ILocalizationChanged, IDisposable, IAsyncDisposable
{
    bool SetProvider(ILocalizationProvider localizationProvider);

    string GetValue(string token);
    string GetValue(string token, params object[] arguments);

    string this[string token] { get; }
    string this[string token, params object[] arguments] { get; }
}
