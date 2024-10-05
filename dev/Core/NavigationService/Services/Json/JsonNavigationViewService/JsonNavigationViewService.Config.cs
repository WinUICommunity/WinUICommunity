using System.Collections.ObjectModel;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public partial class JsonNavigationViewService : IJsonNavigationViewService
{
    private void ConfigJsonBase(string jsonFilePath, bool orderRootItems, bool orderByDescending, bool autoIncludedInBuild, PathType pathType)
    {
        JsonFilePath = jsonFilePath;
        _pathType = pathType;
        _autoIncludedInBuild = autoIncludedInBuild;
        DataSource = new DataSource();

        AddNavigationMenuItems(orderRootItems, orderByDescending);
    }
    public void ConfigJson(string jsonFilePath)
    {
        ConfigJsonBase(jsonFilePath, true, false, false, PathType.Relative);
    }

    public void ConfigJson(string jsonFilePath, bool orderRootItems)
    {
        ConfigJsonBase(jsonFilePath, orderRootItems, false, false, PathType.Relative);
    }
    public void ConfigJson(string jsonFilePath, bool orderRootItems, bool orderByDescending)
    {
        ConfigJsonBase(jsonFilePath, orderRootItems, orderByDescending, false, PathType.Relative);
    }

    public void ConfigJson(string jsonFilePath, PathType pathType)
    {
        ConfigJsonBase(jsonFilePath, true, false, false, pathType);
    }

    public void ConfigJson(string jsonFilePath, bool autoIncludedInBuild, PathType pathType)
    {
        ConfigJsonBase(jsonFilePath, true, false, autoIncludedInBuild, pathType);
    }

    public void ConfigJson(string jsonFilePath, bool orderRootItems, bool orderByDescending, bool autoIncludedInBuild, PathType pathType)
    {
        ConfigJsonBase(jsonFilePath, orderRootItems, orderByDescending, autoIncludedInBuild, pathType);
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
                _autoSuggestBoxNotFoundImagePath = "Assets/autoSuggestBoxNotFound.png";
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
        string dataTemplateXaml = @"
    <DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
        <Grid ColumnSpacing='12'>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width='16' />
                <ColumnDefinition Width='*' />
            </Grid.ColumnDefinitions>
            <Image Source='{Binding ImagePath}' />
            <TextBlock Grid.Column='1' Text='{Binding Title}' />
        </Grid>
    </DataTemplate>";

        var dataTemplate = XamlReader.Load(dataTemplateXaml) as DataTemplate;
        _autoSuggestBox.ItemTemplate = dataTemplate;
    }

    public void ConfigDefaultPage(Type DefaultPage)
    {
        _defaultPage = DefaultPage;
    }

    public void ConfigSettingsPage(Type SettingsPage)
    {
        _settingsPage = SettingsPage;
    }

    private void ConfigPages()
    {
        _pageService.GetPages(_menuItemsWithFooterMenuItems);
        _pageService.SetDefaultPage(_defaultPage);
        _pageService.SetSettingsPage(_settingsPage);
        _pageService.SetSectionPage(_sectionPage);

        if (_pageService.GetPageType(_pageService.DefaultPageKey) != null)
        {
            NavigateTo(_pageService.DefaultPageKey);
        }
    }

    public void ConfigLocalizer(ResourceManager resourceManager, ResourceContext resourceContext)
    {
        ResourceManager = resourceManager;
        ResourceContext = resourceContext;
    }

    public void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar, bool disableNavigationViewNavigator = true)
    {
        _navigationView.AlwaysShowHeader = false;
        _disableNavigationViewNavigator = disableNavigationViewNavigator;
        _mainBreadcrumb = breadcrumbBar;
        _useBreadcrumbBar = false;
        if (_mainBreadcrumb != null)
        {
            _mainBreadcrumb.BreadCrumbs = new ObservableCollection<NavigationBreadcrumb>();
            _useBreadcrumbBar = true;
            _mainBreadcrumb.ItemClicked -= MainBreadcrumb_ItemClicked;
            _mainBreadcrumb.ItemClicked += MainBreadcrumb_ItemClicked;
        }
    }
    public void ConfigBreadcrumbBar(BreadcrumbNavigator breadcrumbBar, bool disableNavigationViewNavigator, bool allowDuplication)
    {
        _allowDuplication = allowDuplication;
        ConfigBreadcrumbBar(breadcrumbBar, disableNavigationViewNavigator);
    }
}
