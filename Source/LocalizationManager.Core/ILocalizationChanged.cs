namespace LocalizationManager;
public interface ILocalizationChanged : INotifyPropertyChanged
{
    CultureInfo DefaultCulture { get; }
    CultureInfo CurrentCulture { get; set; } 
}
