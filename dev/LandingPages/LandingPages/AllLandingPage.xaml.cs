using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public sealed partial class AllLandingPage : ItemsPageBase
{
    internal static AllLandingPage Instance { get; private set; }

    public bool UseFullScreenHeaderImage
    {
        get { return (bool)GetValue(UseFullScreenHeaderImageProperty); }
        set { SetValue(UseFullScreenHeaderImageProperty, value); }
    }

    public static readonly DependencyProperty UseFullScreenHeaderImageProperty = DependencyProperty.Register(nameof(UseFullScreenHeaderImage), typeof(bool), typeof(AllLandingPage), new PropertyMetadata(false, OnFullScreenHeaderImageChanged));

    private static void OnFullScreenHeaderImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (AllLandingPage)d;
        if (ctl != null)
        {
            ctl.ToggleFullScreen((bool)e.NewValue);
        }
    }

    private void ToggleFullScreen(bool value)
    {
        if (HeaderPanel != null && itemGridView != null)
        {
            if (value)
            {
                Microsoft.UI.Xaml.Controls.Grid.SetRowSpan(HomePageHeaderImage, 3);
            }
            else
            {
                Microsoft.UI.Xaml.Controls.Grid.SetRowSpan(HomePageHeaderImage, 2);
            }
        }
    }
    
    public AllLandingPage()
    {
        this.InitializeComponent();
        Instance = this;
        Loaded -= AllLandingPage_Loaded;
        Loaded += AllLandingPage_Loaded;

        ToggleFullScreen(UseFullScreenHeaderImage);
    }

    internal void Navigate(object sender, RoutedEventArgs e)
    {
        if (JsonNavigationViewService != null)
        {
            var args = (ItemClickEventArgs)e;
            var item = (DataItem)args.ClickedItem;

            JsonNavigationViewService.NavigateTo(item.UniqueId + item.Parameter?.ToString(), item);
        }
    }

    private void AllLandingPage_Loaded(object sender, RoutedEventArgs e)
    {
        if (CanExecuteInternalCommand && JsonNavigationViewService != null)
        {
            GetData(JsonNavigationViewService.DataSource);
            OrderBy(i => i.Title);
        }
    }

    public void GetData(DataSource dataSource)
    {
        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddItemsRecursively(item, allItems);
            }
        }

        Items = allItems;
    }

    public async Task GetDataAsync(string jsonFilePath, PathType pathType = PathType.Relative)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType);

        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddItemsRecursively(item, allItems);
            }
        }

        Items = allItems;
    }

    private void AddItemsRecursively(DataItem currentItem, List<DataItem> allItems)
    {
        allItems.Add(currentItem);

        // Recursively check for nested items in the current item
        foreach (var nestedItem in currentItem.Items.Where(i => !i.HideItem))
        {
            AddItemsRecursively(nestedItem, allItems);
        }
    }
    private void GetLocalized(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        var allItems = new List<DataItem>();

        // Gather all items from all groups, including nested items
        foreach (var group in dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => !i.HideItem))
            {
                AddLocalizedItemsRecursively(item, allItems, resourceManager, resourceContext);
            }
        }

        Items = allItems;
    }
    private async Task GetLocalizedAsync(string jsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType);

        var allItems = new List<DataItem>();

        // Gather all items from all groups, including nested items
        foreach (var group in dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => !i.HideItem))
            {
                AddLocalizedItemsRecursively(item, allItems, resourceManager, resourceContext);
            }
        }

        Items = allItems;
    }

    private void AddLocalizedItemsRecursively(DataItem currentItem, List<DataItem> allItems, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        currentItem.Title = Helper.GetLocalizedText(currentItem.Title, currentItem.UsexUid, resourceManager, resourceContext);
        currentItem.SecondaryTitle = Helper.GetLocalizedText(currentItem.SecondaryTitle, currentItem.UsexUid, resourceManager, resourceContext);
        currentItem.Subtitle = Helper.GetLocalizedText(currentItem.Subtitle, currentItem.UsexUid, resourceManager, resourceContext);
        currentItem.Description = Helper.GetLocalizedText(currentItem.Description, currentItem.UsexUid, resourceManager, resourceContext);

        allItems.Add(currentItem);

        // Recursively process nested items
        foreach (var nestedItem in currentItem.Items.Where(i => !i.HideItem))
        {
            AddLocalizedItemsRecursively(nestedItem, allItems, resourceManager, resourceContext);
        }
    }

    public void GetLocalizedData(DataSource dataSource)
    {
        GetLocalized(dataSource, null, null);
    }

    public void GetLocalizedData(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        GetLocalized(dataSource, resourceManager, resourceContext);
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, PathType pathType = PathType.Relative)
    {
        await GetLocalizedAsync(JsonFilePath, null, null, pathType);
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType = PathType.Relative)
    {
       await GetLocalizedAsync(JsonFilePath, resourceManager, resourceContext, pathType);
    }

    public void OrderBy(Func<DataItem, object> orderby = null)
    {
        if (orderby != null)
        {
            Items = Items?.OrderBy(orderby);
        }
        else
        {
            Items = Items?.OrderBy(i => i.Title);
        }
    }

    public void OrderByDescending(Func<DataItem, object> orderByDescending = null)
    {
        if (orderByDescending != null)
        {
            Items = Items?.OrderByDescending(orderByDescending);
        }
        else
        {
            Items = Items?.OrderByDescending(i => i.Title);
        }
    }
}
