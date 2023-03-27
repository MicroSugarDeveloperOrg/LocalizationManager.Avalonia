using System.Xml;

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

            using var reader = XmlReader.Create(readStream);
            var document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.Load(reader);

            var mapValues = new Dictionary<string, string>();
            LoadLanguage(document.ParentNode, mapValues);
            _mapResources[file.Name] = mapValues;
        }
    }

    void LoadLanguage(XmlNode node, Dictionary<string, string> mapValues)
    {
        if (node is null)
            return;

        if (mapValues is null)
            return;

        if (node.HasChildNodes)
        {
            foreach (XmlNode item in node.ChildNodes)
                LoadLanguage(item, mapValues);
        }
        else
            mapValues[node.Name] = node.Value;

        return;
    }

    string ILocalizationProvider.GetString(string token, CultureInfo culture)
    {
        var mapValues = _mapResources.Select(kv =>
        {
            if (kv.Key.Contains(culture.TwoLetterISOLanguageName))
                return kv.Value;

            return null;
        }).FirstOrDefault();

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
