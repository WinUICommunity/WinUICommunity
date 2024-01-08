using Microsoft.UI.Xaml.Markup;

namespace WinUICommunity;

[TemplatePart(Name = nameof(PART_FooterExpander), Type = typeof(Expander))]
[ContentProperty(Name = nameof(Content))]
public partial class OptionsPageControl : Control
{
    private string PART_FooterExpander = "PART_FooterExpander";
    private Expander _footerExpander;

    public OptionsPageControl()
    {
        DefaultStyleKey = typeof(OptionsPageControl);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _footerExpander = GetTemplateChild(PART_FooterExpander) as Expander;
    }

    private static void OnPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (OptionsPageControl)d;
        if (ctl != null)
        {
            if (e.NewValue == null)
            {
                ctl.PaneVisibility = Visibility.Collapsed;
            }
            else
            {
                ctl.PaneVisibility = Visibility.Visible;
            }
        }
    }

    private static void OnIsFooterExpanderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (OptionsPageControl)d;
        if (ctl != null && ctl._footerExpander != null)
        {
            ctl._footerExpander.IsExpanded = (bool)e.NewValue;
        }
    }
}
