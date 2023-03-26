using Avalonia.Data;

namespace LocalizationResourceManager.Avalonia;

public class LocalizedString : AvaloniaObject
{
    public LocalizedString(Func<string> generator) 
        : this(default!, generator)
    {

    }

    public LocalizedString(ILocalizationResourceManager localizationManager, Func<string> generator)
    {
        _generator = generator;
        //localizationManager.PropertyChanged += (sender, e) => ((INotifyPropertyChanged)this).PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Localized)));
    }

    private readonly Func<string> _generator;

    public string Localized => _generator.Invoke();

    public static implicit operator LocalizedString(Func<string> func) => new(func);
}
