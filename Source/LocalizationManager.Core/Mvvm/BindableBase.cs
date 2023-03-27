using LocalizationManager.Core.Weak;

namespace LocalizationManager.Core.Mvvm;
public abstract class BindableBase : INotifyPropertyChanging, INotifyPropertyChanged
{
    private WeakEventManager _propertyChangingManager = new();
    private WeakEventManager _propertyChangedManager = new();

    event PropertyChangingEventHandler? INotifyPropertyChanging.PropertyChanging
    {
        add  => _propertyChangingManager.AddEventHandler(value);
        remove => _propertyChangingManager.RemoveEventHandler(value);
    }

    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add => _propertyChangedManager.AddEventHandler(value);
        remove => _propertyChangedManager.RemoveEventHandler(value);
    }

    protected bool RaisePropertyChanging([CallerMemberName] string? propertyName = default)
    {
        _propertyChangingManager.HandleEvent(this, new PropertyChangingEventArgs(propertyName), nameof(INotifyPropertyChanging.PropertyChanging));
        return true;
    }

    protected bool RaisePropertyChanged([CallerMemberName] string? propertyName = default)
    {
        _propertyChangedManager.HandleEvent(this, new PropertyChangedEventArgs(propertyName), nameof(INotifyPropertyChanged.PropertyChanged));
        return true;
    }

    protected bool SetProperty<T>(ref T refField, T newValue, [CallerMemberName] string? propertyName = default)
    {
        if (EqualityComparer<T>.Default.Equals(refField, newValue))
            return false;

        RaisePropertyChanging(propertyName);
        refField = newValue;
        RaisePropertyChanged(propertyName);
        return true;
    }

}
