using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity;
public sealed partial class PageHeader : UserControl
{
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

    public Visibility PageHeaderVisibility
    {
        get => (Visibility)GetValue(PageHeaderVisibilityProperty);
        set => SetValue(PageHeaderVisibilityProperty, value);
    }

    public static readonly DependencyProperty DocumentationDropDownTextProperty =
        DependencyProperty.Register("DocumentationDropDownText", typeof(string), typeof(PageHeader), new PropertyMetadata("Documentation"));
    public static readonly DependencyProperty DocumentationDropDownIconElementProperty =
        DependencyProperty.Register("DocumentationDropDownIconElement", typeof(IconElement), typeof(PageHeader), new PropertyMetadata(new FontIcon { Glyph = "\xE130" }));
    public static readonly DependencyProperty HeaderLeftContentProperty =
        DependencyProperty.Register("HeaderLeftContent", typeof(object), typeof(PageHeader), new PropertyMetadata(null));
    public static readonly DependencyProperty HeaderRightContentProperty =
        DependencyProperty.Register("HeaderRightContent", typeof(object), typeof(PageHeader), new PropertyMetadata(null));
    public static readonly DependencyProperty PageHeaderVisibilityProperty =
        DependencyProperty.Register("PageHeaderVisibility", typeof(Visibility), typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

    public ControlInfoDataItem Item
    {
        get => _item;
        set => _item = value;
    }

    private ControlInfoDataItem _item;

    public PageHeader()
    {
        this.InitializeComponent();
    }
}
