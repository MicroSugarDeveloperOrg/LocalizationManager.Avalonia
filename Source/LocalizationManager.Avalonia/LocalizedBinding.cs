using Avalonia.Threading;

namespace LocalizationManager.Avalonia;

public class LocalizedBinding : AvaloniaObject
{
    public LocalizedBinding()
    {
        TokenProperty.Changed.AddClassHandler<LocalizedBinding, string>((s, e) =>
        {
            if (s is null)
                return;

            var localizationManager = AvaloniaLocator.Current.GetService<ILocalizationManager>();
            if (localizationManager is null)
                return;

            s.Subject?.OnNext(localizationManager[e.NewValue.Value]);
        });
    }

    public LocalizedBinding(IBinding binding) :base()
    {
        var subscription = this.Bind(TokenProperty, binding);
        //Subject?.OnNext()
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
        var localizationManager = AvaloniaLocator.Current.GetService<ILocalizationManager>();
        if (localizationManager is null)
            return AvaloniaProperty.UnsetValue;

        localizationManager.PropertyChanged += (s, e) =>
        {
            Subject?.OnNext(localizationManager[Token]);
        };

        Subject = new("");
        var binding = new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"Subject^",
            Source = this,
            StringFormat = StringFormat
        };

        //post to next property
        Dispatcher.UIThread.Post(() => Subject.OnNext(localizationManager[Token]));
        return binding;
    }
}
