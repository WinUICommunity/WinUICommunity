namespace WinUICommunity;
public partial class JsonNavigationViewService : IJsonNavigationViewService
{
    public async void ConfigJson(string jsonFilePath, bool autoIncludedInBuild = false, PathType pathType = PathType.Relative)
    {
        JsonFilePath = jsonFilePath;
        _pathType = pathType;
        _autoIncludedInBuild = autoIncludedInBuild;
        DataSource = new DataSource();

        await AddNavigationMenuItems();
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
        _pageService.GetPages(MenuItems);
        _pageService.SetDefaultPage(_defaultPage);
        _pageService.SetSettingsPage(_settingsPage);
        _pageService.SetSectionPage(_sectionPage);

        if (_pageService.GetPageType(_pageService.DefaultPageKey) != null)
        {
            NavigateTo(_pageService.DefaultPageKey);
        }
    }
}
