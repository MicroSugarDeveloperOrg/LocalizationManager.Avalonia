using LocalizationManager.Avalonia.Reactive;
using System.Reactive;

namespace LocalizationManager.Avalonia;

public class LocalizedBinding : SubjectedObject<string>, IBinding
{
    static LocalizedBinding()
    {
        TokenProperty.Changed.AddClassHandler<LocalizedBinding, string>((s, e) =>
        {
            if (s is null)
                return;

            var localizationManager = s._localizationManager;
            if (localizationManager is null)
                return;

            s.OnNext(localizationManager[e.NewValue.Value]);
        });
    }

    public LocalizedBinding() : base(string.Empty)
    {

    }

    public LocalizedBinding(IBinding binding) : this()
    {
        var subscription = this.Bind(TokenProperty, binding);
    }

    public static readonly StyledProperty<string> TokenProperty =
            AvaloniaProperty.Register<LocalizedBinding, string>(nameof(Token));

    public static readonly StyledProperty<string?> StringFormatProperty =
            AvaloniaProperty.Register<LocalizedBinding, string?>(nameof(StringFormat));

    [Content]
    [MarkupExtensionDefaultOption]
    public string Token
    {
        get => GetValue<string>(TokenProperty);
        set => SetValue(TokenProperty, value);
    }

    public string? StringFormat
    {
        get => GetValue<string?>(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
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
    public IBinding? ProvideValue(IServiceProvider serviceProvider)
    {
        var localizationManager = LocalizationManagerExtensions.Default;
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
