using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LocalizationManager.Avalonia;
using LocalizationManager.Sample.ViewModels;
using LocalizationManager.Sample.Views;
using LocalizationManager.Sample.Language;
using System.Globalization;

namespace LocalizationManager.Sample;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();

        // Use xml language 
        LocalizationManagerBuilder.Initialize(() =>
        {
            var appDirectory = AppContext.BaseDirectory;
            //var path = Path.Combine(appDirectory, "Assets", "Languages");
            //var appDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(appDirectory, "Assets", "Languages");
            return LocalizationProviderExtensions.MakeXmlFileProvider(path, "language");
        });

        // Use Resoucece language
        LocalizationManagerBuilder.Initialize(() =>
        {
            return LocalizationProviderExtensions.MakeResourceProvider(LanguageResourceHelper.LanguageResourceManager);
        });

    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}