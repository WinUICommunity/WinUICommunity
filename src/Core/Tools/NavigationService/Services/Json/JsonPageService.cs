namespace WinUICommunity;

public class JsonPageService : PageService
{
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

    private void AddNavigationItemToDictionary(NavigationViewItem navigationItem)
    {
        if (navigationItem.DataContext is DataItem dataItem)
        {
            if (dataItem.IncludedInBuild)
            {
                _pageKeyToTypeMap[dataItem.UniqueId] = ApplicationHelper.GetPageType(dataItem.UniqueId, dataItem.ApiNamespace);
            }
        }
        else if (navigationItem.DataContext is DataGroup dataGroup)
        {
            _pageKeyToTypeMap[dataGroup.UniqueId] = ApplicationHelper.GetPageType(dataGroup.UniqueId, dataGroup.ApiNamespace);
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
}
