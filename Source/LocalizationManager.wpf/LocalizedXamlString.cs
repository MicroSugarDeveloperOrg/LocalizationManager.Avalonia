using System.Windows.Data;

namespace LocalizationManager.wpf;

[ContentProperty(nameof(Token))]
public class LocalizedXamlString : MarkupExtension
{
    public LocalizedXamlString() : base()
    {
    }

    public LocalizedXamlString(string token) : this()
    {
        Token = token;
    }

    public LocalizedXamlString(string token, object[] arguments) : this(token)
    {
        Arguments = arguments;
    }

    public LocalizedXamlString(string token, string category, object[] arguments) : this(token, arguments)
    {
        Category = category;
    }

    ~LocalizedXamlString()
    {

    }

    public string Token { get; set; } = string.Empty;

    public string? Category { get; set; }
    public object[]? Arguments { get; set; }

    public string? StringFormat { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        var localizationManager = LocalizationManagerExtensions.Default;
        if (localizationManager is null)
            return default;

        var value = SetLanguageValue(localizationManager)?.ProvideValue(serviceProvider);
        return value;
    }

    void LanguageChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not ILocalizationManager localizationManager)
            return;

        SetLanguageValue(localizationManager);
    }

    BindingBase? SetLanguageValue(ILocalizationManager localizationManager)
    {
        if (localizationManager is null)
            return default;

        Binding binding;
        if (Arguments is not null && !string.IsNullOrWhiteSpace(Category))
        {
            binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{Token},{Category},{Arguments}]"),
                Source = localizationManager,
                StringFormat = StringFormat
            };
        }
        else if (Arguments is not null)
        {
            binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{Token},{Arguments}]"),
                Source = localizationManager,
                StringFormat = StringFormat
            };
        }
        else if (!string.IsNullOrWhiteSpace(Category))
        {
            binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{Token},{Category}]"),
                Source = localizationManager,
                StringFormat = StringFormat
            };
        }
        else
        {
            binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath($"[{Token}]"),
                Source = localizationManager,
                StringFormat = StringFormat
            };
        }

        return binding;
    }


}
