using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LocalizationManager.Sample.Language;
using LocalizationManager.Sample.ViewModels;
using LocalizationManager.Sample.Views;
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
        LocalizationManagerBuilder.Build(() =>
        {
            var appDirectory = AppContext.BaseDirectory;
            var path = Path.Combine(appDirectory, "Assets", "Languages");
            var provider = LocalizationProviderExtensions.MakeXmlFileProvider(default, name =>
            {
                if (string.IsNullOrWhiteSpace(name))
                    return CultureInfo.CurrentCulture;

                var index = name.IndexOf('-');
                if (index == -1)
                    return new CultureInfo("en-US");

                var stringName = name.Substring(index + 1);
                return new CultureInfo(stringName);
            }, path, "language");

            return provider;
        });

        // Use Resoucece language
        //LocalizationManagerBuilder.Build(() =>
        //{
        //    return LocalizationProviderExtensions.MakeResourceProvider(default, LanguageResourceHelper.LanguageResourceManager);
        //});
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