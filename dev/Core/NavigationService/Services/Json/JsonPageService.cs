namespace WinUICommunity;

public partial class JsonPageService : PageServiceEx
{
    public string SectionPageKey { get; set; } = nameof(SectionPageKey);

    public void GetPages(IList<object>? menuItems)
    {
        _pageKeyToTypeMap = new Dictionary<string, Type>();
        if (menuItems != null)
        {
            foreach (var item in menuItems)
            {
                if (item is NavigationViewItem navigationItem)
                {
                    AddNavigationItemToDictionary(navigationItem);
                }
            }
        }
    }

    public void SetSectionPage(Type pageType)
    {
        if (pageType != null)
        {
            _pageKeyToTypeMap.TryAdd(SectionPageKey, pageType);
        }
    }

    private void AddNavigationItemToDictionary(NavigationViewItem navigationItem)
    {
        if (navigationItem.DataContext is DataItem dataItem)
        {
            if (dataItem.IncludedInBuild && !string.IsNullOrEmpty(dataItem.UniqueId))
            {
                if (string.IsNullOrEmpty(dataItem.SectionId))
                {
                    _pageKeyToTypeMap[GenerateUniqueId(dataItem.UniqueId, dataItem.Parameter?.ToString())] = Type.GetType(dataItem.UniqueId);
                }
                else
                {
                    _pageKeyToTypeMap[GenerateUniqueId(dataItem.UniqueId, dataItem.Parameter?.ToString())] = Type.GetType(dataItem.SectionId);
                }
            }
        }
        else if (navigationItem.DataContext is DataGroup dataGroup && !string.IsNullOrEmpty(dataGroup.UniqueId))
        {
            if (string.IsNullOrEmpty(dataGroup.SectionId))
            {
                _pageKeyToTypeMap[GenerateUniqueId(dataGroup.UniqueId, null)] = Type.GetType(dataGroup.UniqueId);
            }
            else
            {
                _pageKeyToTypeMap[GenerateUniqueId(dataGroup.UniqueId, null)] = Type.GetType(dataGroup.SectionId);
            }
        }

        // Check for nested NavigationViewItems
        if (navigationItem.MenuItems != null)
        {
            foreach (var subItem in navigationItem.MenuItems)
            {
                if (subItem is NavigationViewItem subNavigationItem)
                {
                    AddNavigationItemToDictionary(subNavigationItem);
                }
            }
        }
    }

    private string GenerateUniqueId(string uniqueId, string parameter)
    {
        return uniqueId + parameter;
    }
}
