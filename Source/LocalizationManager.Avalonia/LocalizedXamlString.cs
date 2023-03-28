using LocalizationManager.Avalonia.Reactive;
using System.Reactive;

namespace LocalizationManager.Avalonia;

public class LocalizedXamlString : Subjected<string>, IBinding /*: MarkupExtension*/
{
    static LocalizedXamlString()
    {

    }

    public LocalizedXamlString() : base("")
    {
    }

    public LocalizedXamlString(string token) : this()
    {
        Token = token;
    }

    public LocalizedXamlString(string token, string format) : this(token)
    {
        StringFormat = format;
    }

    ~LocalizedXamlString()
    {

    }

    private ILocalizationManager? _localizationManager;
    protected ILocalizationManager? LocalizationManager
    {
        get => _localizationManager;
        set
        {
            _localizationManager = value;

            if (_localizationManager is not null)
                _localizationManager.PropertyChanged += (s,e)=> 
                {
                    if (_localizationManager is null)
                        return;

                    OnNext(_localizationManager[Token]);
                };
        }
    }

    [Content]
    [MarkupExtensionDefaultOption]
    public string Token { get; set; } = string.Empty;

    public string? StringFormat { get; set; }
 
    public IBinding? ProvideValue(IServiceProvider serviceProvider)
    {
        var localizationManager = AvaloniaLocator.Current.GetService<ILocalizationManager>();
        if (localizationManager is null)
            return default;

        LocalizationManager = localizationManager;
        OnNext(localizationManager[Token]);
        return this;
    }

    public InstancedBinding? Initiate(AvaloniaObject target, AvaloniaProperty? targetProperty, object? anchor = null, bool enableDataValidation = false)
    {
        var observer = Observer.Create<object?>(t =>
        {

        });

        return InstancedBinding.TwoWay(this, observer); ;
    }
}
