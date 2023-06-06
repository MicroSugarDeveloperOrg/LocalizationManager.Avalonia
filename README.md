# LocalizationManager.Avalonia

## Develop
* please use visaulstudio 2022 or greater or rider
* use .net7 runtime (version 7.0.100 or greater)(if you want to use others, please modify the version in the global.json)
* please setup workloads include Android, iOS, Wasm 
* please open the long path support in Windows OS(https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry)

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
   > If you use other language files or methods, you can implement the interface -- `ILocalizationProvider` 

   > and set for UseLocalizationManager

## Demo
You can always download demo executable to play around with LocalizationManager
  > https://github.com/AvaloniaDeveloperOrg/LocalizationManager.Avalonia

## Version compatibility

| LocalizationManager.Avalonia Version | Avalonia Version |
|:-------------------------------------|:-----------------|
| 0.1.0-preview6.x                     | 11.0-preview6    |
| 0.1.0-preview7.x                     | 11.0-preview7    |
| 0.1.0-preview8.x                     | 11.0-preview8    |
| 0.1.0-rc1.x                          | 11.0-rc1.x       |

**NOTE**

LocalizationManager.Avalonia is moving forward together with Avalonia preview versions now. So new feature/fixes are not backported to previous preview versions. If you need a feature/fix for outdated avalonia preview version, please raise an issue so we can do that for you. 

## Credits

[Avalonia](https://github.com/AvaloniaUI/Avalonia)



