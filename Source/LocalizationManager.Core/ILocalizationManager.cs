﻿namespace LocalizationManager;
public interface ILocalizationManager : ILocalizationLanguageMap, ILocalizationChanged, IDisposable, IAsyncDisposable
{
    bool AddProvider(ILocalizationProvider localizationProvider);

    string GetValue(string token);
    string GetValue(string token, params object[] arguments);

    string GetValue(string token, string category);
    string GetValue(string token, string category, params object[] arguments);

    string this[string token] { get; }
    string this[string token, string category] { get; }
    string this[string token, params object[] arguments] { get; }
    string this[string token, string category, params object[] arguments] { get; }
}
