using LocalizationManager.Sample.Language;
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
            return LocalizationProviderExtensions.MakeResourceProvider(LanguageResourceHelper.LanguageResourceManager);
        });

    }

}
