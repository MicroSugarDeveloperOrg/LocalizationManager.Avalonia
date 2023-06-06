echo a = %1
echo b = %2

dotnet nuget push nuget\LocalizationManager.Avalonia.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json

dotnet nuget push nuget\LocalizationManager.wpf.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json
 