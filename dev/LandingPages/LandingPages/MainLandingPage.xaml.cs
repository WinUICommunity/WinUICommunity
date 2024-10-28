using System.Collections.ObjectModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public sealed partial class MainLandingPage : ItemsPageBase
{
    internal static MainLandingPage Instance { get; private set; }

    public static readonly DependencyProperty PreviewGroupTextProperty = DependencyProperty.Register(nameof(PreviewGroupText), typeof(string), typeof(MainLandingPage), new PropertyMetadata("Preview"));
    public static readonly DependencyProperty UpdatedGroupTextProperty = DependencyProperty.Register(nameof(UpdatedGroupText), typeof(string), typeof(MainLandingPage), new PropertyMetadata("Recently updated"));
    public static readonly DependencyProperty NewGroupTextProperty = DependencyProperty.Register(nameof(NewGroupText), typeof(string), typeof(MainLandingPage), new PropertyMetadata("Recently added"));
    public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(nameof(HeaderContent), typeof(object), typeof(MainLandingPage), new PropertyMetadata(null));
    public static readonly DependencyProperty HeaderMarginProperty = DependencyProperty.Register(nameof(HeaderMargin), typeof(Thickness), typeof(MainLandingPage), new PropertyMetadata(new Thickness(-24, 0, -24, 0)));
    public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register(nameof(FooterContent), typeof(object), typeof(MainLandingPage), new PropertyMetadata(null));
    public static readonly DependencyProperty FooterHeightProperty = DependencyProperty.Register(nameof(FooterHeight), typeof(double), typeof(MainLandingPage), new PropertyMetadata(200.0));
    public static readonly DependencyProperty FooterMarginProperty = DependencyProperty.Register(nameof(FooterMargin), typeof(Thickness), typeof(MainLandingPage), new PropertyMetadata(new Thickness(16, 34, 48, 0)));
    public static readonly DependencyProperty UseFullScreenHeaderImageProperty = DependencyProperty.Register(nameof(UseFullScreenHeaderImage), typeof(bool), typeof(MainLandingPage), new PropertyMetadata(false, OnFullScreenHeaderImageChanged));
    public static readonly DependencyProperty HeaderImageHeightProperty = DependencyProperty.Register(nameof(HeaderImageHeight), typeof(double), typeof(MainLandingPage), new PropertyMetadata(300.0, OnHeaderImageHeightChanged));

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
   
    public object HeaderContent
    {
        get => (object)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
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

    public double HeaderImageHeight
    {
        get => (double)GetValue(HeaderImageHeightProperty);
        set => SetValue(HeaderImageHeightProperty, value);
    }
    private static void OnHeaderImageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (MainLandingPage)d;
        if (ctl != null)
        {
            ctl.ToggleFullScreen(ctl.UseFullScreenHeaderImage);
        }
    }
    public bool UseFullScreenHeaderImage
    {
        get { return (bool)GetValue(UseFullScreenHeaderImageProperty); }
        set { SetValue(UseFullScreenHeaderImageProperty, value); }
    }
    private static void OnFullScreenHeaderImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (MainLandingPage)d;
        if (ctl != null)
        {
            ctl.ToggleFullScreen((bool)e.NewValue);
        }
    }

    private void ToggleFullScreen(bool value)
    {
        if (HomePageHeaderImage != null)
        {
            if (value)
            {
                HomePageHeaderImage.Height = double.NaN;
            }
            else
            {
                HomePageHeaderImage.Height = HeaderImageHeight;
            }
        }
    }
    
    public MainLandingPage()
    {
        this.InitializeComponent();
        Instance = this;

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
        GetData(dataSource, new ResourceManager());
    }

    public void GetData(DataSource dataSource, ResourceManager resourceManager)
    {
        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => i.BadgeString != null && !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddLocalizedItemsRecursively(item, allItems, resourceManager);
            }
        }

        Items = allItems;
        GetCollectionViewSource().Source = FormatData();
    }
    public async Task GetDataAsync(string jsonFilePath, PathType pathType = PathType.Relative)
    {
        await GetDataAsync(jsonFilePath, new ResourceManager(), pathType);
    }
    public async Task GetDataAsync(string jsonFilePath, ResourceManager resourceManager, PathType pathType = PathType.Relative)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType);

        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => i.BadgeString != null && !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddLocalizedItemsRecursively(item, allItems, resourceManager);
            }
        }

        Items = allItems;
        GetCollectionViewSource().Source = FormatData();
    }

    private void AddLocalizedItemsRecursively(DataItem currentItem, List<DataItem> allItems, ResourceManager resourceManager)
    {
        currentItem.Title = Helper.GetLocalizedText(currentItem.Title, currentItem.UsexUid, resourceManager);
        currentItem.SecondaryTitle = Helper.GetLocalizedText(currentItem.SecondaryTitle, currentItem.UsexUid, resourceManager);
        currentItem.Subtitle = Helper.GetLocalizedText(currentItem.Subtitle, currentItem.UsexUid, resourceManager);
        currentItem.Description = Helper.GetLocalizedText(currentItem.Description, currentItem.UsexUid, resourceManager);

        allItems.Add(currentItem);

        // Recursively process nested items
        foreach (var nestedItem in currentItem.Items.Where(i => i.BadgeString != null && !i.HideItem))
        {
            AddLocalizedItemsRecursively(nestedItem, allItems, resourceManager);
        }
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
