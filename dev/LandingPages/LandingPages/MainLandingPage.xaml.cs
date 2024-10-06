using System.Collections.ObjectModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public sealed partial class MainLandingPage : ItemsPageBase
{
    public double HeaderFontSize
    {
        get => (double)GetValue(HeaderFontSizeProperty);
        set => SetValue(HeaderFontSizeProperty, value);
    }

    public double HeaderSubtitleFontSize
    {
        get => (double)GetValue(HeaderSubtitleFontSizeProperty);
        set => SetValue(HeaderSubtitleFontSizeProperty, value);
    }

    public string NewGroupText
    {
        get => (string)GetValue(NewGroupTextProperty);
        set => SetValue(NewGroupTextProperty, value);
    }

    public string UpdatedGroupText
    {
        get => (string)GetValue(UpdatedGroupTextProperty);
        set => SetValue(UpdatedGroupTextProperty, value);
    }

    public string PreviewGroupText
    {
        get => (string)GetValue(PreviewGroupTextProperty);
        set => SetValue(PreviewGroupTextProperty, value);
    }

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public string HeaderSubtitleText
    {
        get => (string)GetValue(HeaderSubtitleTextProperty);
        set => SetValue(HeaderSubtitleTextProperty, value);
    }

    public object HeaderContent
    {
        get => (object)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public double HeaderImageHeight
    {
        get => (double)GetValue(HeaderImageHeightProperty);
        set => SetValue(HeaderImageHeightProperty, value);
    }

    public Thickness HeaderMargin
    {
        get => (Thickness)GetValue(HeaderMarginProperty);
        set => SetValue(HeaderMarginProperty, value);
    }

    public object FooterContent
    {
        get => (object)GetValue(FooterContentProperty);
        set => SetValue(FooterContentProperty, value);
    }

    public double FooterHeight
    {
        get => (double)GetValue(FooterHeightProperty);
        set => SetValue(FooterHeightProperty, value);
    }

    public Thickness FooterMargin
    {
        get => (Thickness)GetValue(FooterMarginProperty);
        set => SetValue(FooterMarginProperty, value);
    }

    public Thickness HeaderContentMargin
    {
        get { return (Thickness)GetValue(HeaderContentMarginProperty); }
        set { SetValue(HeaderContentMarginProperty, value); }
    }

    public static readonly DependencyProperty HeaderContentMarginProperty = DependencyProperty.Register(nameof(HeaderContentMargin), typeof(Thickness), typeof(MainLandingPage), new PropertyMetadata(new Thickness(36,100,0,0)));

    public static readonly DependencyProperty HeaderSubtitleFontSizeProperty = DependencyProperty.Register("HeaderSubtitleFontSize", typeof(double), typeof(MainLandingPage), new PropertyMetadata(18.0));
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register("HeaderFontSize", typeof(double), typeof(MainLandingPage), new PropertyMetadata(40.0));
    public static readonly DependencyProperty PreviewGroupTextProperty = DependencyProperty.Register("PreviewGroupText", typeof(string), typeof(MainLandingPage), new PropertyMetadata("Preview"));
    public static readonly DependencyProperty UpdatedGroupTextProperty = DependencyProperty.Register("UpdatedGroupText", typeof(string), typeof(MainLandingPage), new PropertyMetadata("Recently updated"));
    public static readonly DependencyProperty NewGroupTextProperty = DependencyProperty.Register("NewGroupText", typeof(string), typeof(MainLandingPage), new PropertyMetadata("Recently added"));

    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(MainLandingPage), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty HeaderSubtitleTextProperty = DependencyProperty.Register("HeaderSubtitleText", typeof(string), typeof(MainLandingPage), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(object), typeof(MainLandingPage), new PropertyMetadata(null));
    public static readonly DependencyProperty HeaderImageHeightProperty = DependencyProperty.Register("HeaderImageHeight", typeof(double), typeof(MainLandingPage), new PropertyMetadata(396.0));
    public static readonly DependencyProperty HeaderMarginProperty = DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(MainLandingPage), new PropertyMetadata(new Thickness(-24, 0, -24, 0)));
    public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register("FooterContent", typeof(object), typeof(MainLandingPage), new PropertyMetadata(null));
    public static readonly DependencyProperty FooterHeightProperty = DependencyProperty.Register("FooterHeight", typeof(double), typeof(MainLandingPage), new PropertyMetadata(200.0));
    public static readonly DependencyProperty FooterMarginProperty = DependencyProperty.Register("FooterMargin", typeof(Thickness), typeof(MainLandingPage), new PropertyMetadata(new Thickness(16, 34, 48, 0)));

    public MainLandingPage()
    {
        this.InitializeComponent();
        Loaded -= MainLandingPage_Loaded;
        Loaded += MainLandingPage_Loaded;
    }

    private void MainLandingPage_Loaded(object sender, RoutedEventArgs e)
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

        foreach (var group in dataSource.Groups.Where(g => !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => i.BadgeString != null && !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddItemsRecursively(item, allItems);
            }
        }

        Items = allItems;
        GetCollectionViewSource().Source = FormatData();
    }
    public async Task GetDataAsync(string jsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType, autoIncludedInBuild);

        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => i.BadgeString != null && !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddItemsRecursively(item, allItems);
            }
        }

        Items = allItems;
        GetCollectionViewSource().Source = FormatData();
    }

    private void AddItemsRecursively(DataItem currentItem, List<DataItem> allItems)
    {
        allItems.Add(currentItem);

        // Recursively check for nested items in the current item
        foreach (var nestedItem in currentItem.Items.Where(i => i.BadgeString != null && !i.HideItem))
        {
            AddItemsRecursively(nestedItem, allItems);
        }
    }

    private void GetLocalized(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        var allItems = new List<DataItem>();

        // Gather all items from all groups, including nested items
        foreach (var group in dataSource.Groups.Where(g => !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => i.BadgeString != null && !i.HideItem))
            {
                AddLocalizedItemsRecursively(item, allItems, resourceManager, resourceContext);
            }
        }

        Items = allItems;
        GetCollectionViewSource().Source = FormatData();
    }
    private async Task GetLocalizedAsync(string jsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType, bool autoIncludedInBuild)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType, autoIncludedInBuild);

        var allItems = new List<DataItem>();

        // Gather all items from all groups, including nested items
        foreach (var group in dataSource.Groups.Where(g => !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => i.BadgeString != null && !i.HideItem))
            {
                AddLocalizedItemsRecursively(item, allItems, resourceManager, resourceContext);
            }
        }

        Items = allItems;
        GetCollectionViewSource().Source = FormatData();
    }

    private void AddLocalizedItemsRecursively(DataItem currentItem, List<DataItem> allItems, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        currentItem.Title = Helper.GetLocalizedText(currentItem.Title, currentItem.UsexUid, resourceManager, resourceContext);
        currentItem.SecondaryTitle = Helper.GetLocalizedText(currentItem.SecondaryTitle, currentItem.UsexUid, resourceManager, resourceContext);
        currentItem.Subtitle = Helper.GetLocalizedText(currentItem.Subtitle, currentItem.UsexUid, resourceManager, resourceContext);
        currentItem.Description = Helper.GetLocalizedText(currentItem.Description, currentItem.UsexUid, resourceManager, resourceContext);

        allItems.Add(currentItem);

        // Recursively process nested items
        foreach (var nestedItem in currentItem.Items.Where(i => i.BadgeString != null && !i.HideItem))
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

    public CollectionViewSource GetCollectionViewSource()
    {
        return itemsCVS;
    }

    public ObservableCollection<GroupInfoList> FormatData()
    {
        // Flatten the items list to include nested items
        var allItems = new List<DataItem>();
        foreach (var item in this.Items)
        {
            AddFormatDataItemsRecursively(item, allItems);
        }

        // Group the flattened items by BadgeString
        var query = from item in allItems
                    group item by item.BadgeString into g
                    orderby g.Key
                    select new GroupInfoList(g) { Key = g.Key };

        ObservableCollection<GroupInfoList> groupList = new(query);

        if (groupList.Any())
        {
            // Move "Preview" to the back of the list
            foreach (var item in groupList?.ToList())
            {
                if (item?.Key.ToString() == "Preview")
                {
                    groupList?.Remove(item);
                    groupList?.Insert(groupList.Count, item);
                }
            }

            // Update group titles based on the key
            foreach (var item in groupList)
            {
                switch (item.Key.ToString())
                {
                    case "New":
                        item.Title = NewGroupText;
                        break;
                    case "Updated":
                        item.Title = UpdatedGroupText;
                        break;
                    case "Preview":
                        item.Title = PreviewGroupText;
                        break;
                }
            }

            return groupList;
        }
        return null;
    }

    private void AddFormatDataItemsRecursively(DataItem currentItem, List<DataItem> allItems)
    {
        // Add the current item to the result list
        allItems.Add(currentItem);

        // Recursively check for nested items in the current item
        foreach (var nestedItem in currentItem.Items.Where(i => !i.HideItem))
        {
            AddFormatDataItemsRecursively(nestedItem, allItems);
        }
    }

    protected override bool GetIsNarrowLayoutState()
    {
        return LayoutVisualStates.CurrentState == NarrowLayout;
    }
}
