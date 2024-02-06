namespace LocalizationManager.args;
public class LanguageChangedEventArgs : PropertyChangedEventArgs
{
    public LanguageChangedEventArgs(string propertyName, CultureInfo cultureInfo)
        : base(propertyName)
    {
        CurrentCulture = cultureInfo;
    }

    public CultureInfo CurrentCulture { get; }
}
