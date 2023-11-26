using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public sealed partial class AllLandingPage : ItemsPageBase
{
    public double HeaderFontSize
    {
        get => (double)GetValue(HeaderFontSizeProperty);
        set => SetValue(HeaderFontSizeProperty, value);
    }
    public string HeaderImage
    {
        get => (string)GetValue(HeaderImageProperty);
        set => SetValue(HeaderImageProperty, value);
    }
    public string HeaderOverlayImage
    {
        get => (string)GetValue(HeaderOverlayImageProperty);
        set => SetValue(HeaderOverlayImageProperty, value);
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

    public ImageSource PlaceholderSource
    {
        get => (ImageSource)GetValue(PlaceholderSourceProperty);
        set => SetValue(PlaceholderSourceProperty, value);
    }
    public bool IsCacheEnabled
    {
        get => (bool)GetValue(IsCacheEnabledProperty);
        set => SetValue(IsCacheEnabledProperty, value);
    }
    public bool EnableLazyLoading
    {
        get => (bool)GetValue(EnableLazyLoadingProperty);
        set => SetValue(EnableLazyLoadingProperty, value);
    }
    public double LazyLoadingThreshold
    {
        get => (double)GetValue(LazyLoadingThresholdProperty);
        set => SetValue(LazyLoadingThresholdProperty, value);
    }

    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register("HeaderFontSize", typeof(double), typeof(AllLandingPage), new PropertyMetadata(28.0));
    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(AllLandingPage), new PropertyMetadata("All"));
    public static readonly DependencyProperty HeaderImageProperty = DependencyProperty.Register("HeaderImage", typeof(string), typeof(AllLandingPage), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty HeaderGridCornerRadiusProperty = DependencyProperty.Register("HeaderGridCornerRadius", typeof(CornerRadius), typeof(AllLandingPage), new PropertyMetadata(new CornerRadius(8,0,0,0)));
    public static readonly DependencyProperty HeaderOverlayImageProperty = DependencyProperty.Register("HeaderOverlayImage", typeof(string), typeof(AllLandingPage), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty HeaderImageHeightProperty = DependencyProperty.Register("HeaderImageHeight", typeof(double), typeof(AllLandingPage), new PropertyMetadata(200.0));
    public static readonly DependencyProperty PlaceholderSourceProperty = DependencyProperty.Register("PlaceholderSource", typeof(ImageSource), typeof(AllLandingPage), new PropertyMetadata(default(ImageSource)));
    public static readonly DependencyProperty IsCacheEnabledProperty = DependencyProperty.Register("IsCacheEnabled", typeof(bool), typeof(AllLandingPage), new PropertyMetadata(true));
    public static readonly DependencyProperty EnableLazyLoadingProperty = DependencyProperty.Register("EnableLazyLoading", typeof(bool), typeof(AllLandingPage), new PropertyMetadata(true));
    public static readonly DependencyProperty LazyLoadingThresholdProperty = DependencyProperty.Register("LazyLoadingThreshold", typeof(double), typeof(AllLandingPage), new PropertyMetadata(300.0));

    public AllLandingPage()
    {
        this.InitializeComponent();
    }

    public void GetData(DataSource dataSource)
    {
        Items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem));
    }

    public void GetLocalizedData(DataSource dataSource, ILocalizer localizer)
    {
        GetLocalized(dataSource, null, null, localizer);
    }

    public void GetLocalizedData(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        GetLocalized(dataSource, resourceManager, resourceContext, null);
    }

    private void GetLocalized(DataSource dataSource, ResourceManager resourceManager, ResourceContext resourceContext, ILocalizer localizer)
    {
        var items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem)).ToList();
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Title = Helper.GetLocalizedText(items[i].Title, items[i].UsexUid, localizer, resourceManager, resourceContext);
            items[i].SecondaryTitle = Helper.GetLocalizedText(items[i].SecondaryTitle, items[i].UsexUid, localizer, resourceManager, resourceContext);
            items[i].Subtitle = Helper.GetLocalizedText(items[i].Subtitle, items[i].UsexUid, localizer, resourceManager, resourceContext);
            items[i].Description = Helper.GetLocalizedText(items[i].Description, items[i].UsexUid, localizer, resourceManager, resourceContext);
        }

        Items = items;
    }

    public async void GetDataAsync(string JsonFilePath, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(JsonFilePath, pathType, autoIncludedInBuild);
        Items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem));
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, ILocalizer localizer, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
        await GetLocalizedAsync(JsonFilePath, null, null, localizer, pathType, autoIncludedInBuild);
    }

    public async void GetLocalizedDataAsync(string JsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, PathType pathType = PathType.Relative, bool autoIncludedInBuild = false)
    {
       await GetLocalizedAsync(JsonFilePath, resourceManager, resourceContext, null, pathType, autoIncludedInBuild);
    }

    private async Task GetLocalizedAsync(string JsonFilePath, ResourceManager resourceManager, ResourceContext resourceContext, ILocalizer localizer, PathType pathType, bool autoIncludedInBuild)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(JsonFilePath, pathType, autoIncludedInBuild);
        var items = dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup).SelectMany(g => g.Items.Where(i => !i.HideItem)).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Title = Helper.GetLocalizedText(items[i].Title, items[i].UsexUid, localizer, resourceManager, resourceContext);
            items[i].SecondaryTitle = Helper.GetLocalizedText(items[i].SecondaryTitle, items[i].UsexUid, localizer, resourceManager, resourceContext);
            items[i].Subtitle = Helper.GetLocalizedText(items[i].Subtitle, items[i].UsexUid, localizer, resourceManager, resourceContext);
            items[i].Description = Helper.GetLocalizedText(items[i].Description, items[i].UsexUid, localizer, resourceManager, resourceContext);
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
