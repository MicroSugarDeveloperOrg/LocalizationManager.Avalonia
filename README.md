# LocalizationManager.Avalonia

## How to use

1. Add [LocalizationManager.Avalonia][nuget] nuget package to your project:

       dotnet add package LocalizationManager.Avalonia

2. Edit `App.axaml.cs` file:
    > If you install 1.0.0-* version or higher, use this:
   ```
     public override void RegisterServices()
     {
        base.RegisterServices();
        
        // if you use xml language  please MakeXmlFileProvider and set xml path and filename
        AvaloniaLocator.CurrentMutable.UseLocalizationManager(() =>
        {
            var appDirectory = AppContext.BaseDirectory;
            //var path = Path.Combine(appDirectory, "Assets", "Languages");
            //var appDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(appDirectory, "Assets", "Languages");
            return LocalizationProviderExtensions.MakeXmlFileProvider(path, "language");
        });

        // Or
    
        // if you use resource language use this, please MakeResourceProvider and set the resourcemanagers 
        AvaloniaLocator.CurrentMutable.UseLocalizationManager(() =>
        {
            return LocalizationProviderExtensions.MakeResourceProvider(LanguageResourceHelper.LanguageResourceManager);
        });
     }
   ```
3. You can custom the resource provider
   > If you use other language file, you can implement the interface -- `ILocalizationProvider` 

