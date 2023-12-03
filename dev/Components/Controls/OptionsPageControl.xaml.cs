namespace WinUICommunity;

public sealed partial class OptionsPageControl : UserControl
{
    public static readonly DependencyProperty MainCardCornerRadiusProperty =
           DependencyProperty.Register(nameof(MainCardCornerRadius), typeof(CornerRadius), typeof(OptionsPageControl), new PropertyMetadata(new CornerRadius(8)));

    public static readonly DependencyProperty MainCardBorderThicknessProperty =
        DependencyProperty.Register(nameof(MainCardBorderThickness), typeof(Thickness), typeof(OptionsPageControl), new PropertyMetadata(new Thickness(1, 1, 1, 1)));

    public static readonly DependencyProperty PageContentProperty =
        DependencyProperty.Register(nameof(PageContent), typeof(UIElement), typeof(OptionsPageControl), new PropertyMetadata(null));

    public static readonly DependencyProperty PageFooterContentProperty =
        DependencyProperty.Register(nameof(PageFooterContent), typeof(UIElement), typeof(OptionsPageControl), new PropertyMetadata(null));

    public static readonly DependencyProperty OptionsPaneContentProperty =
        DependencyProperty.Register(nameof(OptionsPaneContent), typeof(UIElement), typeof(OptionsPageControl), new PropertyMetadata(null, OnOptionsPaneContentChanged));

    public static readonly DependencyProperty OptionsPaneMinWidthProperty =
       DependencyProperty.Register(nameof(OptionsPaneMinWidth), typeof(int), typeof(OptionsPageControl), new PropertyMetadata(286));

    public static readonly DependencyProperty OptionsPanePaddingProperty =
        DependencyProperty.Register(nameof(OptionsPanePadding), typeof(Thickness), typeof(OptionsPageControl), new PropertyMetadata(new Thickness(16)));

    public static readonly DependencyProperty OptionsPaneVisibilityProperty =
        DependencyProperty.Register(nameof(OptionsPaneVisibility), typeof(Visibility), typeof(OptionsPageControl), new PropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty OptionsBarContentProperty =
       DependencyProperty.Register(nameof(OptionsBarContent), typeof(UIElement), typeof(OptionsPageControl), new PropertyMetadata(null));

    public static readonly DependencyProperty OptionsBarFooterContentProperty =
       DependencyProperty.Register(nameof(OptionsBarFooterContent), typeof(UIElement), typeof(OptionsPageControl), new PropertyMetadata(null));

    public static readonly DependencyProperty OptionsBarHorizontalContentAlignmentProperty =
       DependencyProperty.Register(nameof(OptionsBarHorizontalContentAlignment), typeof(HorizontalAlignment), typeof(OptionsPageControl), new PropertyMetadata(HorizontalAlignment.Center));

    public static readonly DependencyProperty OptionsBarMinWidthProperty =
        DependencyProperty.Register(nameof(OptionsBarMinWidth), typeof(int), typeof(OptionsPageControl), new PropertyMetadata(32));

    public static readonly DependencyProperty IsPageFooterExpandedProperty =
        DependencyProperty.Register(nameof(IsPageFooterExpanded), typeof(bool), typeof(OptionsPageControl), new PropertyMetadata(false));

    public static readonly DependencyProperty IsPageFooterEnabledProperty =
        DependencyProperty.Register(nameof(IsPageFooterEnabled), typeof(bool), typeof(OptionsPageControl), new PropertyMetadata(true));

    public static readonly DependencyProperty IsOptionsBarEnabledProperty =
        DependencyProperty.Register(nameof(IsOptionsBarEnabled), typeof(bool), typeof(OptionsPageControl), new PropertyMetadata(true));

    public static readonly DependencyProperty PageFooterVisibilityProperty =
        DependencyProperty.Register(nameof(PageFooterVisibility), typeof(Visibility), typeof(OptionsPageControl), new PropertyMetadata(Visibility.Visible));

    public static readonly DependencyProperty PageFooterHeaderProperty =
        DependencyProperty.Register(nameof(PageFooterHeader), typeof(object), typeof(OptionsPageControl), new PropertyMetadata(null));

    public CornerRadius MainCardCornerRadius
    {
        get { return (CornerRadius)GetValue(MainCardCornerRadiusProperty); }
        set { SetValue(MainCardCornerRadiusProperty, value); }
    }

    public Thickness MainCardBorderThickness
    {
        get { return (Thickness)GetValue(MainCardBorderThicknessProperty); }
        set { SetValue(MainCardBorderThicknessProperty, value); }
    }

    public UIElement? PageContent
    {
        get => (UIElement?)GetValue(PageContentProperty);
        set => SetValue(PageContentProperty, value);
    }

    public UIElement? PageFooterContent
    {
        get => (UIElement?)GetValue(PageFooterContentProperty);
        set => SetValue(PageFooterContentProperty, value);
    }

    public UIElement? OptionsPaneContent
    {
        get => (UIElement?)GetValue(OptionsPaneContentProperty);
        set => SetValue(OptionsPaneContentProperty, value);
    }

    public int OptionsPaneMinWidth
    {
        get { return (int)GetValue(OptionsPaneMinWidthProperty); }
        set { SetValue(OptionsPaneMinWidthProperty, value); }
    }

    public Thickness OptionsPanePadding
    {
        get { return (Thickness)GetValue(OptionsPanePaddingProperty); }
        set { SetValue(OptionsPanePaddingProperty, value); }
    }

    public Visibility OptionsPaneVisibility
    {
        get { return (Visibility)GetValue(OptionsPaneVisibilityProperty); }
        set { SetValue(OptionsPaneVisibilityProperty, value); }
    }

    public UIElement? OptionsBarContent
    {
        get { return (UIElement?)GetValue(OptionsBarContentProperty); }
        set { SetValue(OptionsBarContentProperty, value); }
    }

    public UIElement? OptionsBarFooterContent
    {
        get { return (UIElement?)GetValue(OptionsBarFooterContentProperty); }
        set { SetValue(OptionsBarFooterContentProperty, value); }
    }

    public HorizontalAlignment OptionsBarHorizontalContentAlignment
    {
        get { return (HorizontalAlignment)GetValue(OptionsBarHorizontalContentAlignmentProperty); }
        set { SetValue(OptionsBarHorizontalContentAlignmentProperty, value); }
    }

    public int OptionsBarMinWidth
    {
        get { return (int)GetValue(OptionsBarMinWidthProperty); }
        set { SetValue(OptionsBarMinWidthProperty, value); }
    }

    public bool IsPageFooterExpanded
    {
        get { return (bool)GetValue(IsPageFooterExpandedProperty); }
        set { SetValue(IsPageFooterExpandedProperty, value); }
    }

    public bool IsPageFooterEnabled
    {
        get { return (bool)GetValue(IsPageFooterEnabledProperty); }
        set { SetValue(IsPageFooterEnabledProperty, value); }
    }

    public bool IsOptionsBarEnabled
    {
        get { return (bool)GetValue(IsOptionsBarEnabledProperty); }
        set { SetValue(IsOptionsBarEnabledProperty, value); }
    }

    public Visibility PageFooterVisibility
    {
        get { return (Visibility)GetValue(PageFooterVisibilityProperty); }
        set { SetValue(PageFooterVisibilityProperty, value); }
    }

    public object PageFooterHeader
    {
        get { return (object)GetValue(PageFooterHeaderProperty); }
        set { SetValue(PageFooterHeaderProperty, value); }
    }

    private static void OnOptionsPaneContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (OptionsPageControl)d;
        if (ctl != null)
        {
            if (e.NewValue == null)
            {
                ctl.OptionsPaneVisibility = Visibility.Collapsed;
            }
            else
            {
                ctl.OptionsPaneVisibility = Visibility.Visible;
            }
        }
    }
    public OptionsPageControl()
    {
        this.InitializeComponent();
    }
}
