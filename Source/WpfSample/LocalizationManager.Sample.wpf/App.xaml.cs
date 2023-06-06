using System.IO;
using System.Windows;

namespace LocalizationManager.Sample.wpf;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        LocalizationManagerBuilder.Initialize(() =>
        {
            var appDirectory = AppContext.BaseDirectory;
            var path = Path.Combine(appDirectory, "Resource", "Languages");
            return LocalizationProviderExtensions.MakeXmlFilesProvider(path);
        });

    }

}
