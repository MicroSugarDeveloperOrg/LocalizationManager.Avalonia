using LocalizationManager.args;
using LocalizationManager.Core.Mvvm; 


namespace LocalizationManager.Core.Imps;

internal class LocalizationManagerImp : BindableBase, ILocalizationManager
{
    public LocalizationManagerImp(params ILocalizationProvider[] providers)
    {
        DefaultCulture = CultureInfo.CurrentCulture;
        _currentCulture = DefaultCulture;

        _mapProviders = new();
        if (providers is null)
            return;

        foreach (var provider in providers)
            AddProvider(provider);
    }

    public LocalizationManagerImp(Func<ILocalizationProvider[]>? providerDelegate) 
        :this(providerDelegate?.Invoke()!)
    {

    }

    const string __innerDefaultCatagory__ = nameof(__innerDefaultCatagory__);

    CultureInfo _currentCulture = CultureInfo.CurrentCulture;
    ConcurrentDictionary<string, ConcurrentBag<ILocalizationProvider>> _mapProviders;

    event EventHandler<LanguageChangedEventArgs>? _LanguageChanged;

    event EventHandler<LanguageChangedEventArgs>? ILocalizationChanged.LanguageChanged
    {
        add => _LanguageChanged += value;
        remove => _LanguageChanged -= value;
    }

    List<CultureInfo> _languageMaps = new();
    public IEnumerable<CultureInfo>? LanguageMaps
    {
        get => _languageMaps;
        set => _languageMaps = new(value);
    }

    public bool AddProvider(ILocalizationProvider localizationProvider)
    {
        if (localizationProvider is null)
            return false;

        var category = string.IsNullOrWhiteSpace(localizationProvider.Category) ? __innerDefaultCatagory__ : localizationProvider.Category!;
        var bag = _mapProviders.AddOrUpdate(category, new ConcurrentBag<ILocalizationProvider>() 
        {
            localizationProvider
        }, (@key, @old) => 
        {
            if (!@old.Contains(localizationProvider))
                @old.Add(localizationProvider);
            return @old;
        });

        var languageMaps = localizationProvider.LanguageMaps;
        if (languageMaps is not null)
        {
            foreach (var language in languageMaps)
            {
                if (!_languageMaps.Contains(language))
                    _languageMaps.Add(language);
            }
        }

        return true;
    }

    public string this[string token] => GetValue(token);
    public string this[string token, string category] => GetValue(token, category);
    public string this[string token, params object[] arguments] => GetValue(token, arguments);
    public string this[string token, string category, params object[] arguments] => GetValue(token, category, arguments);

    public CultureInfo DefaultCulture { get; }

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (SetProperty(ref _currentCulture, value, (oldValue, newValue) =>
            {
                CultureInfo.CurrentCulture = newValue;
                CultureInfo.CurrentUICulture = newValue;
                CultureInfo.DefaultThreadCurrentCulture = newValue;
                CultureInfo.DefaultThreadCurrentUICulture = newValue;
                return true;
            }, propertyName:null))
                _LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(nameof(CurrentCulture), value));
        }
    }


    public string GetValue(string token) => GetValue(token, __innerDefaultCatagory__);

    public string GetValue(string token, params object[] arguments) => GetValue(token, __innerDefaultCatagory__, arguments);

    public string GetValue(string token, string category)
    {
        var innerCategory = string.IsNullOrWhiteSpace(category) ? __innerDefaultCatagory__ : category!;
        _mapProviders.TryGetValue(innerCategory, out var providerBags);
        if (providerBags == null)
            return string.Empty;

        foreach (var provider in providerBags)
        {
            if (provider is null)
                continue;

            return provider.GetString(token, CurrentCulture);
        }

        return string.Empty;
    }

    public string GetValue(string token, string category, params object[] arguments)
    {
        var innerCategory = string.IsNullOrWhiteSpace(category) ? __innerDefaultCatagory__ : category!;
        _mapProviders.TryGetValue(innerCategory, out var providerBags);
        if (providerBags == null)
            return string.Empty;

        foreach (var provider in providerBags)
        {
            if (provider is null)
                continue;

            return provider.GetString(token, CurrentCulture, arguments);
        }

        return string.Empty;
    }
        
       
    public void Dispose()
    {
        foreach (var providerBags in _mapProviders)
        {
            foreach (var provider in providerBags.Value)
                provider?.Dispose();
        }

        _mapProviders.Clear();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return new ValueTask();
    }
}
