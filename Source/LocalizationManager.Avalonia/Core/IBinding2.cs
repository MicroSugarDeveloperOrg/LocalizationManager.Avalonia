namespace LocalizationManager.Avalonia.Core;
internal interface IBinding2 : IBinding
{
    BindingExpressionBase Instance(
        AvaloniaObject target,
        AvaloniaProperty? targetProperty,
        object? anchor);
}

