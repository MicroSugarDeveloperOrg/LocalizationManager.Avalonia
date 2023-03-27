namespace LocalizationManager.Avalonia.Extensions;
internal static class Extensions
{
    public static T? GetService<T>(this IServiceProvider sp) => (T?)sp?.GetService(typeof(T));
}
