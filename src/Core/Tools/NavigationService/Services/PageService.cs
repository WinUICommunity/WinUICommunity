﻿namespace WinUICommunity;

public class PageService : IPageService
{
    public Dictionary<string, Type> _pageKeyToTypeMap;

    public string DefaultPageKey { get; set; } = nameof(DefaultPageKey);
    public string SettingsPageKey { get; set; } = nameof(SettingsPageKey);

    public virtual Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pageKeyToTypeMap)
        {
            if (!_pageKeyToTypeMap.TryGetValue(key, out pageType))
            {
                return pageType;
            }
        }

        return pageType;
    }

    public virtual void SetDefaultPage(Type pageType)
    {
        _pageKeyToTypeMap.TryAdd(DefaultPageKey, pageType);
    }

    public virtual void SetSettingsPage(Type pageType)
    {
        _pageKeyToTypeMap.TryAdd(SettingsPageKey, pageType);
    }
}