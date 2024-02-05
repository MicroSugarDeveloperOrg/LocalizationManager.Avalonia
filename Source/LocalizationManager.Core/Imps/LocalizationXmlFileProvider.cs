namespace LocalizationManager.Core.Imps;
internal class LocalizationXmlFileProvider : ILocalizationProvider
{
    public LocalizationXmlFileProvider(Func<string, CultureInfo> provider, string baseDirectory, string baseName)
        : this(provider, (baseDirectory, baseName))
    {

    }

    public LocalizationXmlFileProvider(Func<string, CultureInfo> provider, params (string baseDirectory, string baseName)[] parameters)
    {
        _provider = provider;
        LoadResource(parameters);
    }

    readonly ConcurrentDictionary<CultureInfo, Dictionary<string, string>> _mapResources = new();

    Func<string, CultureInfo> _provider;

    List<CultureInfo> _languages = new();
    IEnumerable<CultureInfo>? ILocalizationLanguageMap.LanguageMaps
    {
        get => _languages;
        set => _languages = new(value);
    }

    public string? Category
    {
        get;
        set;
    }

    bool ILocalizationProvider.AddResource(string resourceDirectory, string baseName, Type? usingResourceSet)
    {
        LoadResources(resourceDirectory, baseName);
        return true;
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture)
    {
        if (token is null)
            return string.Empty;

        _mapResources.TryGetValue(culture, out var mapValues);
        if (mapValues is null)
            _mapResources.TryGetValue(new CultureInfo(culture.TwoLetterISOLanguageName), out mapValues);

        if (mapValues is null)
            mapValues = _mapResources.FirstOrDefault().Value;

        if (mapValues is null)
            return string.Empty;

        mapValues.TryGetValue(token, out var value);
        return value;
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture, params object[] arguments)
    {
        var value = ((ILocalizationProvider)this).GetString(token, culture);
        return string.Format(value, arguments);
    }

    public void Dispose()
    {
        foreach (var item in _mapResources)
            item.Value.Clear();
    }

    void LoadResource(params (string baseDirectory, string baseName)[] parameters)
    {
        foreach (var parameter in parameters)
            LoadResources(parameter.baseDirectory, parameter.baseName);
    }

    void LoadResources(string baseDirectory, string baseName)
    {
        if (string.IsNullOrWhiteSpace(baseDirectory) || string.IsNullOrWhiteSpace(baseName))
            return;

        var directory = new DirectoryInfo(baseDirectory);
        var allFiles = directory.GetFiles();
        var files = allFiles.Where(file => file.Name.Contains(baseName))?.ToArray();

        if (files is null || files.Length <= 0)
            return;

        foreach (var file in files)
        {
            using var readStream = file.OpenRead();
            if (readStream is null)
                continue;

            var cultureInfo = GetCultureInfoFromFileName(Path.GetFileNameWithoutExtension(file.Name));
            if (cultureInfo is null)
                continue;

            if (!_languages.Contains(cultureInfo))
                _languages.Add(cultureInfo);

            var document = XElement.Load(readStream);
            var mapValues = new Dictionary<string, string>();
            LoadLanguage(document, mapValues);

            _mapResources.AddOrUpdate(cultureInfo, new Dictionary<string, string>(mapValues), (@key, @old) =>
            {
                foreach (var keyValue in mapValues)
                    @old[keyValue.Key] = keyValue.Value;
                return old;
            });
        }
    }

    void LoadLanguage(XElement node, Dictionary<string, string> mapValues)
    {
        if (node is null)
            return;

        if (mapValues is null)
            return;

        if (node.HasElements)
        {
            foreach (var item in node.Elements())
                LoadLanguage(item, mapValues);
        }
        else
        {
            var name = node.Name.LocalName;
            mapValues[name] = node.Value;
        }

        return;
    }

    CultureInfo? GetCultureInfoFromFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return default;

        var cultureInfo = _provider?.Invoke(fileName);
        if (cultureInfo is null)
            return new CultureInfo("en");

        return cultureInfo;
    }
}
