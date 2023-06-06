using LocalizationManager.wpf.Converters;
using System.Windows.Data;

namespace LocalizationManager.wpf;

[ContentProperty(nameof(Token))]
public class LocalizedBinding : MarkupExtension
{
    public LocalizedBinding() : base()
    {
        _proxy = new DependencyObject();
    }

    public LocalizedBinding(BindingBase binding) : this()
    {
        Token = binding;
    }

    private readonly DependencyObject _proxy;

    public static readonly DependencyProperty TokenProperty =
        DependencyProperty.RegisterAttached("Token", typeof(object), typeof(LocalizedBinding), new PropertyMetadata(default));

    public static readonly DependencyProperty CategoryProperty =
        DependencyProperty.RegisterAttached("Category", typeof(object), typeof(LocalizedBinding), new PropertyMetadata(default));

    public static readonly DependencyProperty ArgumentsProperty =
        DependencyProperty.RegisterAttached("Arguments", typeof(object), typeof(LocalizedBinding), new PropertyMetadata(default));

    public object Token
    {
        get { return (object)_proxy.GetValue(TokenProperty); }
        set { _proxy.SetValue(TokenProperty, value); }
    }

    public object? Category
    {
        get => (object?)_proxy.GetValue(CategoryProperty);
        set => _proxy.SetValue(CategoryProperty, value);
    }

    public object? Arguments
    {
        get => (object?)_proxy.GetValue(ArgumentsProperty);
        set => _proxy.SetValue(ArgumentsProperty, value);
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)) return this;
        if (provideValueTarget.TargetObject.GetType().FullName == "System.Windows.SharedDp") return this;
        if (!(provideValueTarget.TargetObject is DependencyObject targetObject)) return this;
        if (!(provideValueTarget.TargetProperty is DependencyProperty targetProperty)) return this;

        var localizationManager = LocalizationManagerExtensions.Default;
        if (localizationManager is null)
            return this;

        MultiBinding multiBinding = new()
        {
            Converter = new MultiPropertyConverters(),
            ConverterParameter = localizationManager,
        };

        switch (Token)
        {
            case Binding tokenBinding:
                multiBinding.Bindings.Add(tokenBinding);
                break;
            default:
                multiBinding.Bindings.Add(new Binding()
                {
                    Source = Token,
                });
                break;
        }

        switch (Category)
        {
            case Binding categoryBinding:
                multiBinding.Bindings.Add(categoryBinding);
                break;
            default:
                multiBinding.Bindings.Add(new Binding()
                {
                    Source = Category,
                });
                break;
        }

        switch (Arguments)
        {
            case Binding argumentBinding:
                multiBinding.Bindings.Add(argumentBinding);
                break;
            default:
                multiBinding.Bindings.Add(new Binding()
                {
                    Source = Arguments,
                });
                break;
        }

        multiBinding.Bindings.Add(new Binding()
        {
            Path = new PropertyPath(nameof(ILocalizationChanged.CurrentCulture)),
            Mode = BindingMode.OneWay,
            Source = localizationManager
        });


        return multiBinding.ProvideValue(serviceProvider);
    }
}
