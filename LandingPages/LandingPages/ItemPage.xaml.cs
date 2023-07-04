using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUICommunity;

public sealed partial class ItemPage : Page
{
    public Visibility PageHeaderVisibility
    {
        get => (Visibility)GetValue(PageHeaderVisibilityProperty);
        set => SetValue(PageHeaderVisibilityProperty, value);
    }
    public string DocumentationDropDownText
    {
        get => (string)GetValue(DocumentationDropDownTextProperty);
        set => SetValue(DocumentationDropDownTextProperty, value);
    }
    public IconElement DocumentationDropDownIconElement
    {
        get => (IconElement)GetValue(DocumentationDropDownIconElementProperty);
        set => SetValue(DocumentationDropDownIconElementProperty, value);
    }
    public object HeaderLeftContent
    {
        get => (object)GetValue(HeaderLeftContentProperty);
        set => SetValue(HeaderLeftContentProperty, value);
    }
    public object HeaderRightContent
    {
        get => (object)GetValue(HeaderRightContentProperty);
        set => SetValue(HeaderRightContentProperty, value);
    }

    public static readonly DependencyProperty PageHeaderVisibilityProperty =
        DependencyProperty.Register("PageHeaderVisibility", typeof(Visibility), typeof(ItemPage), new PropertyMetadata(Visibility.Visible));
    public static readonly DependencyProperty DocumentationDropDownTextProperty =
        DependencyProperty.Register("DocumentationDropDownText", typeof(string), typeof(ItemPage), new PropertyMetadata("Documentation"));
    public static readonly DependencyProperty DocumentationDropDownIconElementProperty =
        DependencyProperty.Register("DocumentationDropDownIconElement", typeof(IconElement), typeof(ItemPage), new PropertyMetadata(new FontIcon { Glyph = "\xE130" }));
    public static readonly DependencyProperty HeaderLeftContentProperty =
        DependencyProperty.Register("HeaderLeftContent", typeof(object), typeof(ItemPage), new PropertyMetadata(null));
    public static readonly DependencyProperty HeaderRightContentProperty =
        DependencyProperty.Register("HeaderRightContent", typeof(object), typeof(ItemPage), new PropertyMetadata(null));

    public ControlInfoDataItem Item
    {
        get => _item;
        set => _item = value;
    }

    private ControlInfoDataItem _item;

    public ItemPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        GetDataAsync(e);
        base.OnNavigatedTo(e);
    }

    public async void GetDataAsync(NavigationEventArgs e)
    {
        NavigationArgs args = (NavigationArgs)e.Parameter;
        var dataSource = new ControlInfoDataSource();
        await dataSource.GetGroupsAsync(args.JsonFilePath, args.PathType, args.IncludedInBuildMode);
        var item = await dataSource.GetItemAsync((String)args.Parameter, args.JsonFilePath, args.PathType, args.IncludedInBuildMode);

        if (item != null && !string.IsNullOrEmpty(item.UniqueId))
        {
            Item = item;

            // Load control page into frame.
            Assembly assembly = string.IsNullOrEmpty(item.ApiNamespace) ? Application.Current.GetType().Assembly : Assembly.Load(item.ApiNamespace);
            if (assembly is not null)
            {
                Type pageType = assembly.GetType(item.UniqueId);
                if (pageType != null)
                {
                    this.contentFrame.Navigate(pageType);
                    args.NavigationView.EnsureNavigationSelection(item?.UniqueId);
                }
            }
        }
    }
}
