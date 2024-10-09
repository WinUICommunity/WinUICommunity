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
    public bool FullScreenHeaderImage
    {
        get { return (bool)GetValue(FullScreenHeaderImageProperty); }
        set { SetValue(FullScreenHeaderImageProperty, value); }
    }

    public static readonly DependencyProperty FullScreenHeaderImageProperty = DependencyProperty.Register(nameof(FullScreenHeaderImage), typeof(bool), typeof(AllLandingPage), new PropertyMetadata(false, OnFullScreenHeaderImageChanged));

    private static void OnFullScreenHeaderImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (AllLandingPage)d;
        if (ctl != null && ctl.HeaderGrid != null && ctl.itemGridView != null)
        {
            if ((bool)e.NewValue)
            {
                ctl.HeaderGrid.Height = double.NaN;
                ctl.itemGridView.Padding = new Thickness(24,0,24,36);
            }
            else
            {
                ctl.HeaderGrid.Height = ctl.HeaderImageHeight;
                ctl.itemGridView.Padding = new Thickness(24, 0, 24, 72);
            }
        }
    }

    public Thickness GridViewPadding
    {
        get { return (Thickness)GetValue(GridViewPaddingProperty); }
        set { SetValue(GridViewPaddingProperty, value); }
    }
    public Brush HeaderTextForeground
    {
        get { return (Brush)GetValue(HeaderTextForegroundProperty); }
        set { SetValue(HeaderTextForegroundProperty, value); }
    }

    public VerticalAlignment GridViewVerticalAlignment
    {
        get { return (VerticalAlignment)GetValue(GridViewVerticalAlignmentProperty); }
        set { SetValue(GridViewVerticalAlignmentProperty, value); }
    }



    public VerticalAlignment HeaderTextVerticalAlignment
    {
        get { return (VerticalAlignment)GetValue(HeaderTextVerticalAlignmentProperty); }
        set { SetValue(HeaderTextVerticalAlignmentProperty, value); }
    }

    public static readonly DependencyProperty HeaderTextVerticalAlignmentProperty = DependencyProperty.Register(nameof(HeaderTextVerticalAlignment), typeof(VerticalAlignment), typeof(AllLandingPage), new PropertyMetadata(VerticalAlignment.Center));
    public static readonly DependencyProperty GridViewVerticalAlignmentProperty = DependencyProperty.Register(nameof(GridViewVerticalAlignment), typeof(VerticalAlignment), typeof(AllLandingPage), new PropertyMetadata(VerticalAlignment.Bottom));
    public static readonly DependencyProperty GridViewPaddingProperty = DependencyProperty.Register(nameof(GridViewPadding), typeof(Thickness), typeof(AllLandingPage), new PropertyMetadata(new Thickness(24,0,24,72)));
    public static readonly DependencyProperty NormalizedCenterPointProperty = DependencyProperty.Register(nameof(NormalizedCenterPoint), typeof(string), typeof(AllLandingPage), new PropertyMetadata("0.5"));
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(AllLandingPage), new PropertyMetadata(Stretch.UniformToFill));
    public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(nameof(OverlayOpacity), typeof(double), typeof(AllLandingPage), new PropertyMetadata(0.5));
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(AllLandingPage), new PropertyMetadata(28.0));
    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(AllLandingPage), new PropertyMetadata("All"));
    public static readonly DependencyProperty HeaderGridCornerRadiusProperty = DependencyProperty.Register(nameof(HeaderGridCornerRadius), typeof(CornerRadius), typeof(AllLandingPage), new PropertyMetadata(new CornerRadius(8,0,0,0)));
    public static readonly DependencyProperty HeaderImageHeightProperty = DependencyProperty.Register(nameof(HeaderImageHeight), typeof(double), typeof(AllLandingPage), new PropertyMetadata(400.0, OnHeaderImageHeightChanged));
    public static readonly DependencyProperty HeaderTextForegroundProperty = DependencyProperty.Register(nameof(HeaderTextForeground), typeof(Brush), typeof(AllLandingPage), new PropertyMetadata(null, OnHeaderTextForegroundChanged));

    private static void OnHeaderTextForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (AllLandingPage)d;
        if (ctl != null && ctl.smallHeaderText != null && e.NewValue != null)
        {
            ctl.smallHeaderText.Foreground = (Brush)e.NewValue;
        }
    }

    private static void OnHeaderImageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (AllLandingPage)d;
        if (ctl != null && ctl.HeaderGrid != null)
        {
            if (ctl.FullScreenHeaderImage)
            {
                ctl.HeaderGrid.Height = double.NaN;
            }
            else
            {
                ctl.HeaderGrid.Height = (double)e.NewValue;
            }
        }
    }

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
