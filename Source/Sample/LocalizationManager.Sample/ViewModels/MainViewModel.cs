namespace LocalizationManager.Sample.ViewModels;

public class MainViewModel : ViewModelBase
{

    public MainViewModel()
    {
    }
    public string Greeting => "Welcome to Avalonia!";

    public string HelloWorld => nameof(HelloWorld);
}
