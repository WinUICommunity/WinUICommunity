﻿namespace WinUICommunity;

public class PageServiceEx : IPageServiceEx
{
    public Dictionary<string, Type> _pageKeyToTypeMap;

    public string DefaultPageKey { get; set; } = nameof(DefaultPageKey);
    public string SettingsPageKey { get; set; } = nameof(SettingsPageKey);

    public virtual Type GetPageType(string key)
    {
        if (key == null)
            return null;

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
        if (pageType != null)
        {
            _pageKeyToTypeMap.TryAdd(DefaultPageKey, pageType);
        }
    }

    public virtual void SetSettingsPage(Type pageType)
    {
        if (pageType != null)
        {
            _pageKeyToTypeMap.TryAdd(SettingsPageKey, pageType);
        }
    }
}
