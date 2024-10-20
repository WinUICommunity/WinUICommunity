using System.Collections.ObjectModel;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public partial class JsonNavigationViewService : PageServiceEx, IJsonNavigationViewService
{
    private void ConfigJsonBase(string jsonFilePath, bool orderRootItems, bool orderByDescending, PathType pathType)
    {
        JsonFilePath = jsonFilePath;
        _pathType = pathType;
        DataSource = new DataSource();

        AddNavigationMenuItems(orderRootItems, orderByDescending);
    }
    public void ConfigJson(string jsonFilePath)
    {
        ConfigJsonBase(jsonFilePath, true, false, PathType.Relative);
    }

    public void ConfigJson(string jsonFilePath, bool orderRootItems)
    {
        ConfigJsonBase(jsonFilePath, orderRootItems, false, PathType.Relative);
    }
    public void ConfigJson(string jsonFilePath, bool orderRootItems, bool orderByDescending)
    {
        ConfigJsonBase(jsonFilePath, orderRootItems, orderByDescending, PathType.Relative);
    }

    public void ConfigJson(string jsonFilePath, PathType pathType)
    {
        ConfigJsonBase(jsonFilePath, true, false, pathType);
    }

    public void ConfigJson(string jsonFilePath, bool orderRootItems, bool orderByDescending, PathType pathType)
    {
        ConfigJsonBase(jsonFilePath, orderRootItems, orderByDescending, pathType);
    }
    public void ConfigAutoSuggestBox(AutoSuggestBox autoSuggestBox, bool useItemTemplate = true, string autoSuggestBoxNotFoundString = null, string autoSuggestBoxNotFoundImagePath = null)
    {
        _autoSuggestBox = autoSuggestBox;

        if (_autoSuggestBox != null)
        {
            _autoSuggestBoxNotFoundString = autoSuggestBoxNotFoundString;
            _autoSuggestBoxNotFoundImagePath = autoSuggestBoxNotFoundImagePath;

            if (string.IsNullOrEmpty(_autoSuggestBoxNotFoundString))
            {
                _autoSuggestBoxNotFoundString = "No result found";
            }

            if (string.IsNullOrEmpty(_autoSuggestBoxNotFoundImagePath))
            {
                _autoSuggestBoxNotFoundImagePath = "ms-appx:///Assets/icon.png";
            }

            _autoSuggestBox.TextChanged += _autoSuggestBox_TextChanged;
            _autoSuggestBox.QuerySubmitted += _autoSuggestBox_QuerySubmitted;

            if (useItemTemplate)
            {
                CreateAutoSuggestBoxItemTemplate();
            }
        }
    }

    private void CreateAutoSuggestBoxItemTemplate()
    {
        _autoSuggestBox.Resources.MergedDictionaries.AddIfNotExists(new InternalAutoSuggestBoxItemTemplate());
        _autoSuggestBox.ItemTemplate = _autoSuggestBox.Resources["InternalAutoSuggestBoxItemTemplate"] as DataTemplate;
    }

    public void ConfigDefaultPage(Type DefaultPage)
    {
        _defaultPage = DefaultPage;
    }

    public void ConfigSettingsPage(Type SettingsPage)
    {
        _settingsPage = SettingsPage;
    }

    public void ConfigSettingsPage(Type SettingsPage, IconElement icon)
    {
        _settingsPage = SettingsPage;
        var settingItem = (NavigationViewItem)SettingsItem;
        if (settingItem != null)
        {
            settingItem.Icon = icon;
        }
    }
    public void ConfigSectionPage(Type sectionPage)
    {
        _sectionPage = sectionPage;
    }

    public void ConfigFontFamilyForGlyph(string fontFamily)
    {
        _fontFamilyForGlyph = fontFamily;
    }

    private void ConfigPages()
    {
        _pageKeyToTypeMap = _navigationPageDictionary;
        SetDefaultPage(_defaultPage);
        SetSettingsPage(_settingsPage);
        SetSectionPage(_sectionPage);

        if (GetPageType(DefaultPageKey) != null)
        {
            NavigateTo(DefaultPageKey);
        }
    }

    public void ConfigLocalizer(ResourceManager resourceManager, ResourceContext resourceContext)
    {
        ResourceManager = resourceManager;
        ResourceContext = resourceContext;
    }

    public void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar, Dictionary<Type, BreadcrumbPageConfig> pageDictionary)
    {
        _mainBreadcrumb = breadcrumbBar;
        _useBreadcrumbBar = false;

        if (_mainBreadcrumb != null)
        {
            _mainBreadcrumb.NavigationView = _navigationView;
            _mainBreadcrumb.InternalFrame = Frame;
            _mainBreadcrumb.PageDictionary = pageDictionary;
            _mainBreadcrumb.Visibility = Visibility.Collapsed;
            _mainBreadcrumb.Initialize();

            _mainBreadcrumb.BreadCrumbs = new ObservableCollection<NavigationBreadcrumb>();
            _useBreadcrumbBar = true;
            _mainBreadcrumb.ItemClicked -= MainBreadcrumb_ItemClicked;
            _mainBreadcrumb.ItemClicked += MainBreadcrumb_ItemClicked;
            _mainBreadcrumb.ChangeBreadcrumbVisibility(false);
        }
    }

    public void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar, Dictionary<Type, BreadcrumbPageConfig> pageDictionary, BreadcrumbNavigatorHeaderVisibilityOptions headerVisibilityOptions)
    {
        breadcrumbBar.HeaderVisibilityOptions = headerVisibilityOptions;
        ConfigBreadcrumbBar(breadcrumbBar, pageDictionary);
    }

    public void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar, Dictionary<Type, BreadcrumbPageConfig> pageDictionary, bool allowDuplication)
    {
        _allowDuplication = allowDuplication;
        ConfigBreadcrumbBar(breadcrumbBar, pageDictionary);
    }

    public void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar, Dictionary<Type, BreadcrumbPageConfig> pageDictionary, BreadcrumbNavigatorHeaderVisibilityOptions headerVisibilityOptions, bool allowDuplication)
    {
        breadcrumbBar.HeaderVisibilityOptions = headerVisibilityOptions;
        ConfigBreadcrumbBar(breadcrumbBar, pageDictionary, allowDuplication);
    }
}
