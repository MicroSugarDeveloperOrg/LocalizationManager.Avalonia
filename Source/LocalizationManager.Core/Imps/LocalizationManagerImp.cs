using LocalizationManager.Core.Mvvm;

namespace LocalizationManager.Core.Imps;
internal class LocalizationManagerImp : BindableBase, ILocalizationManager
{
    public LocalizationManagerImp(ILocalizationProvider provider)
    {
        DefaultCulture = CultureInfo.CurrentCulture;
        _provider = provider;
    }

    public LocalizationManagerImp(Func<ILocalizationProvider> providerDelegate) 
        :this(providerDelegate.Invoke())
    {

    }


    ILocalizationProvider? _provider;
    CultureInfo _currentCulture = CultureInfo.CurrentCulture;

    public bool SetProvider(ILocalizationProvider localizationProvider)
    {
        _provider = localizationProvider;
        return true;
    }

    public string this[string token] => GetValue(token);

    public string this[string token, params object[] arguments] => GetValue(token, arguments);

    public CultureInfo DefaultCulture { get; }

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            CultureInfo.CurrentCulture = value;
            CultureInfo.CurrentUICulture = value;
            CultureInfo.DefaultThreadCurrentCulture = value;
            CultureInfo.DefaultThreadCurrentUICulture = value;
            SetProperty(ref _currentCulture, value);
        }
    }

    public string GetValue(string token) => _provider?.GetString(token, CurrentCulture) ?? string.Empty;

    public string GetValue(string token, params object[] arguments) => _provider?.GetString(token, CurrentCulture, arguments) ?? string.Empty;

    public void Dispose()
    {
        _provider?.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        _provider?.Dispose();
        return new ValueTask();
    }

   
}
