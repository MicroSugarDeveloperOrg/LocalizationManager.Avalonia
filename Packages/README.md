# LocalizationManager.Wpf

## Develop
* please use visaulstudio 2022 or greater or rider
* use .net7 runtime (version 7.0.100 or greater)(if you want to use others, please modify the version in the global.json)
* please setup workloads include Android, iOS, Wasm 
* please open the long path support in Windows OS(https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry)

## How to use

1. Add [LocalizationManager.Wpf][nuget] nuget package to your project:

       dotnet add package LocalizationManager.Wpf

2. Edit `App.axaml.cs` file:
    > If you install 1.0.0-* version or higher, use this:
   ```
     public App()
     {       
        // if you use xml language  please MakeXmlFileProvider and set xml path and filename
        LocalizationManagerBuilder.Initialize(() =>
        {
            var appDirectory = AppContext.BaseDirectory;
            //var path = Path.Combine(appDirectory, "Assets", "Languages");
            //var appDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(appDirectory, "Assets", "Languages");
            return LocalizationProviderExtensions.MakeXmlFileProvider(path, "language");
        });

        // Or
    
        // if you use resource language use this, please MakeResourceProvider and set the resourcemanagers 
        LocalizationManagerBuilder.Initialize(() =>
        {
            return LocalizationProviderExtensions.MakeResourceProvider(LanguageResourceHelper.LanguageResourceManager);
        });
     }
   ```
3. You can custom the resource provider
   > If you use other language files or methods, you can implement the interface -- `ILocalizationProvider` 

   > and set for LocalizationManagerBuilder.Initialize

## Demo
You can always download demo executable to play around with LocalizationManager
  > https://github.com/AvaloniaDeveloperOrg/LocalizationManager.Avalonia

## Version compatibility

for net5.0 net6.0 net7.0 wpf
 



