namespace LocalizationManager.Avalonia;

public class LocalizedBinding : AvaloniaObject
{
    public LocalizedBinding()
    {

    }

    public static readonly StyledProperty<string> TokenProperty =
            AvaloniaProperty.Register<LocalizedBinding, string>(nameof(Token));

    [Content]
    [MarkupExtensionDefaultOption]
    public string Token
    {
        get => GetValue<string>(TokenProperty);
        set => SetValue(TokenProperty, value);
    }


    public string? StringFormat { get; set; }

    protected BehaviorSubject<string>? Subject { get; private set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var LocalizationManager = AvaloniaLocator.Current.GetService<ILocalizationManager>();
        if (LocalizationManager is null)
            return AvaloniaProperty.UnsetValue;

        LocalizationManager.PropertyChanged += (s, e) =>
        {
            Subject?.OnNext(LocalizationManager[Token]);
        };

        Subject = new(LocalizationManager[Token]);

        var binding = new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"Subject^",
            Source = this,
            StringFormat = StringFormat
        };

        return binding;
    }
}
