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
        Items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem));
    }
    public void GetLocalizedData(DataSource dataSource)
    {
        GetLocalized(dataSource, null, null);
    }

    public void GetLocalizedData(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        GetLocalized(dataSource, resourceManager, resourceContext);
    }

    private void GetLocalized(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        var items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem)).ToList();
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Title = Helper.GetLocalizedText(items[i].Title, items[i].UsexUid, resourceManager, resourceContext);
            items[i].SecondaryTitle = Helper.GetLocalizedText(items[i].SecondaryTitle, items[i].UsexUid, resourceManager, resourceContext);
            items[i].Subtitle = Helper.GetLocalizedText(items[i].Subtitle, items[i].UsexUid, resourceManager, resourceContext);
            items[i].Description = Helper.GetLocalizedText(items[i].Description, items[i].UsexUid, resourceManager, resourceContext);
        }

        Items = items;
    }

    public async void GetDataAsync(string JsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(JsonFilePath, pathType, autoIncludedInBuild);
        Items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem));
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        await GetLocalizedAsync(JsonFilePath, null, null, pathType, autoIncludedInBuild);
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
       await GetLocalizedAsync(JsonFilePath, resourceManager, resourceContext, pathType, autoIncludedInBuild);
    }

    private async Task GetLocalizedAsync(string JsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType, bool autoIncludedInBuild)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(JsonFilePath, pathType, autoIncludedInBuild);
        var items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem)).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Title = Helper.GetLocalizedText(items[i].Title, items[i].UsexUid, resourceManager, resourceContext);
            items[i].SecondaryTitle = Helper.GetLocalizedText(items[i].SecondaryTitle, items[i].UsexUid, resourceManager, resourceContext);
            items[i].Subtitle = Helper.GetLocalizedText(items[i].Subtitle, items[i].UsexUid, resourceManager, resourceContext);
            items[i].Description = Helper.GetLocalizedText(items[i].Description, items[i].UsexUid, resourceManager, resourceContext);
        }

        Items = items;
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
