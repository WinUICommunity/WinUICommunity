namespace WindowUI;

public class JsonPageService : PageService
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
                    _pageKeyToTypeMap[GenerateUniqueId(dataItem.UniqueId, dataItem.Parameter?.ToString())] = NavigationServiceHelper.GetPageType(dataItem.UniqueId, dataItem.ApiNamespace);
                }
                else
                {
                    _pageKeyToTypeMap[GenerateUniqueId(dataItem.UniqueId, dataItem.Parameter?.ToString())] = NavigationServiceHelper.GetPageType(dataItem.SectionId, dataItem.ApiNamespace);
                }
            }
        }
        else if (navigationItem.DataContext is DataGroup dataGroup && !string.IsNullOrEmpty(dataGroup.UniqueId))
        {
            if (string.IsNullOrEmpty(dataGroup.SectionId))
            {
                _pageKeyToTypeMap[GenerateUniqueId(dataGroup.UniqueId, null)] = NavigationServiceHelper.GetPageType(dataGroup.UniqueId, dataGroup.ApiNamespace);
            }
            else
            {
                _pageKeyToTypeMap[GenerateUniqueId(dataGroup.UniqueId, null)] = NavigationServiceHelper.GetPageType(dataGroup.SectionId, dataGroup.ApiNamespace);
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
