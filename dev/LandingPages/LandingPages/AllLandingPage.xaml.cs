using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public sealed partial class AllLandingPage : ItemsPageBase
{
    internal static AllLandingPage Instance { get; private set; }
    public double HeaderFontSize
    {
        get => (double)GetValue(HeaderFontSizeProperty);
        set => SetValue(HeaderFontSizeProperty, value);
    }
    
    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public double HeaderImageHeight
    {
        get => (double)GetValue(HeaderImageHeightProperty);
        set => SetValue(HeaderImageHeightProperty, value);
    }

    public CornerRadius HeaderGridCornerRadius
    {
        get => (CornerRadius)GetValue(HeaderGridCornerRadiusProperty);
        set => SetValue(HeaderGridCornerRadiusProperty, value);
    }
    public double OverlayOpacity
    {
        get { return (double)GetValue(OverlayOpacityProperty); }
        set { SetValue(OverlayOpacityProperty, value); }
    }

    public Stretch Stretch
    {
        get { return (Stretch)GetValue(StretchProperty); }
        set { SetValue(StretchProperty, value); }
    }
    public string NormalizedCenterPoint
    {
        get { return (string)GetValue(NormalizedCenterPointProperty); }
        set { SetValue(NormalizedCenterPointProperty, value); }
    }

    public static readonly DependencyProperty NormalizedCenterPointProperty = DependencyProperty.Register(nameof(NormalizedCenterPoint), typeof(string), typeof(AllLandingPage), new PropertyMetadata("0.5"));
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(AllLandingPage), new PropertyMetadata(Stretch.UniformToFill));
    public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(nameof(OverlayOpacity), typeof(double), typeof(AllLandingPage), new PropertyMetadata(0.5));
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(AllLandingPage), new PropertyMetadata(28.0));
    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(AllLandingPage), new PropertyMetadata("All"));
    public static readonly DependencyProperty HeaderGridCornerRadiusProperty = DependencyProperty.Register(nameof(HeaderGridCornerRadius), typeof(CornerRadius), typeof(AllLandingPage), new PropertyMetadata(new CornerRadius(8,0,0,0)));
    public static readonly DependencyProperty HeaderImageHeightProperty = DependencyProperty.Register(nameof(HeaderImageHeight), typeof(double), typeof(AllLandingPage), new PropertyMetadata(200.0));
    
    public AllLandingPage()
    {
        this.InitializeComponent();
        Instance = this;
        Loaded -= AllLandingPage_Loaded;
        Loaded += AllLandingPage_Loaded;
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

    public async Task GetDataAsync(string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType, autoIncludedInBuild);

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
    private async Task GetLocalizedAsync(string jsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType, bool autoIncludedInBuild)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType, autoIncludedInBuild);

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

    public async void GetLocalizedDataAsync(string JsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        await GetLocalizedAsync(JsonFilePath, null, null, pathType, autoIncludedInBuild);
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
       await GetLocalizedAsync(JsonFilePath, resourceManager, resourceContext, pathType, autoIncludedInBuild);
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
