namespace LocalizationManager.Core.Imps;
internal class LocalizationXmlFileProvider : ILocalizationProvider
{
    public LocalizationXmlFileProvider(string baseDirectory, string baseName)
    {
        _baseDirectory = baseDirectory;
        _baseName = baseName;
        _mapResources = new();
        LoadResources();
    }

    readonly string _baseDirectory;
    readonly string _baseName;
    readonly Dictionary<string, Dictionary<string, string>> _mapResources;

    IEnumerable<CultureInfo>? ILocalizationLanguageMap.LanguageMaps => throw new NotImplementedException();

    void LoadResources()
    {
        var directory = new DirectoryInfo(_baseDirectory);
        var allFiles = directory.GetFiles();
        var files = allFiles.Where(file => file.Name.Contains(_baseName))?.ToArray();

        if (files is null || files.Length <= 0)
            return;

        foreach (var file in files)
        {
            using var readStream = file.OpenRead();
            if (readStream is null)
                continue;
            var document = XElement.Load(readStream);
            var mapValues = new Dictionary<string, string>();
            LoadLanguage(document, mapValues);
            _mapResources[Path.GetFileNameWithoutExtension(file.Name)] = mapValues;
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

    string ILocalizationProvider.GetString(string token, CultureInfo culture)
    {
        if (token is null)
            return string.Empty;

        var mapValues = _mapResources.Where(kv => kv.Key.Contains(culture.TwoLetterISOLanguageName)).FirstOrDefault().Value;
        if (mapValues is null)
        {
            mapValues = _mapResources.Where(kv =>
            {
                if (kv.Key == _baseName)
                    return true;

                return false;
            }).FirstOrDefault().Value;
        }

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
}
