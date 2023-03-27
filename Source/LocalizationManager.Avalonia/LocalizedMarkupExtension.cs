using LocalizationManager.Avalonia.Extensions;

namespace LocalizationManager.Avalonia;

public class LocalizedMarkupExtension : MarkupExtension
{
    public LocalizedMarkupExtension()
    {

    }

    public LocalizedMarkupExtension(string token)
    {
        Token = token;
    }

    public LocalizedMarkupExtension(string token, string format)
    {
        Token = token;
        StringFormat = format;
    }

    [Content]
    [MarkupExtensionDefaultOption]
    public string Token { get; set; } = string.Empty;

    public string? StringFormat { get; set; } 

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var LocalizationManager = LocalizationManagerExtensions.Default;
        //var LocalizationManager = serviceProvider.GetService<ILocalizationManager>();
        if (LocalizationManager is null)
            return AvaloniaProperty.UnsetValue;

        var binding = new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Token}]",
            Source = LocalizationManager,
            StringFormat = StringFormat
        };
        return binding;
    }
}
