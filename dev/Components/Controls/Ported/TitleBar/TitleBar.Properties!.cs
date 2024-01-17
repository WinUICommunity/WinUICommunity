using Microsoft.UI.Windowing;

namespace WinUICommunity;
public partial class TitleBar : Control
{
    public Thickness PaneButtonMargin
    {
        get { return (Thickness)GetValue(PaneButtonMarginProperty); }
        set { SetValue(PaneButtonMarginProperty, value); }
    }

    public Thickness BackButtonMargin
    {
        get { return (Thickness)GetValue(BackButtonMarginProperty); }
        set { SetValue(BackButtonMarginProperty, value); }
    }

    public double BackButtonWidth
    {
        get { return (double)GetValue(BackButtonWidthProperty); }
        set { SetValue(BackButtonWidthProperty, value); }
    }

    public double PaneButtonWidth
    {
        get { return (double)GetValue(PaneButtonWidthProperty); }
        set { SetValue(PaneButtonWidthProperty, value); }
    }

    public Thickness FooterMargin
    {
        get { return (Thickness)GetValue(FooterMarginProperty); }
        set { SetValue(FooterMarginProperty, value); }
    }

    public bool HasTitleBar
    {
        get { return (bool)GetValue(HasTitleBarProperty); }
        set { SetValue(HasTitleBarProperty, value); }
    }

    public bool IsResizable
    {
        get { return (bool)GetValue(IsResizableProperty); }
        set { SetValue(IsResizableProperty, value); }
    }

    public bool IsMaximizable
    {
        get { return (bool)GetValue(IsMaximizableProperty); }
        set { SetValue(IsMaximizableProperty, value); }
    }

    public bool IsMinimizable
    {
        get { return (bool)GetValue(IsMinimizableProperty); }
        set { SetValue(IsMinimizableProperty, value); }
    }

    public bool IsAlwaysOnTop
    {
        get { return (bool)GetValue(IsAlwaysOnTopProperty); }
        set { SetValue(IsAlwaysOnTopProperty, value); }
    }

    public string PaneButtonTooltipText
    {
        get { return (string)GetValue(PaneButtonTooltipTextProperty); }
        set { SetValue(PaneButtonTooltipTextProperty, value); }
    }

    public string BackButtonTooltipText
    {
        get { return (string)GetValue(BackButtonTooltipTextProperty); }
        set { SetValue(BackButtonTooltipTextProperty, value); }
    }

    public static readonly DependencyProperty PaneButtonMarginProperty =
    DependencyProperty.Register(nameof(PaneButtonMargin), typeof(Thickness), typeof(TitleBar), new PropertyMetadata(default(Thickness)));

    public static readonly DependencyProperty BackButtonMarginProperty =
        DependencyProperty.Register(nameof(BackButtonMargin), typeof(Thickness), typeof(TitleBar), new PropertyMetadata(default(Thickness)));

    public static readonly DependencyProperty BackButtonWidthProperty =
        DependencyProperty.Register(nameof(BackButtonWidth), typeof(double), typeof(TitleBar), new PropertyMetadata(double.NaN));

    public static readonly DependencyProperty PaneButtonWidthProperty =
        DependencyProperty.Register(nameof(PaneButtonWidth), typeof(double), typeof(TitleBar), new PropertyMetadata(double.NaN));

    public static readonly DependencyProperty FooterMarginProperty =
        DependencyProperty.Register(nameof(FooterMargin), typeof(Thickness), typeof(TitleBar), new PropertyMetadata(new Thickness(4, 0, 8, 0)));

    public static readonly DependencyProperty HasTitleBarProperty =
        DependencyProperty.Register(nameof(HasTitleBar), typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnHasTitleBarChanged));

    public static readonly DependencyProperty IsResizableProperty =
        DependencyProperty.Register(nameof(IsResizable), typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnIsResizableChanged));

    public static readonly DependencyProperty IsMaximizableProperty =
        DependencyProperty.Register(nameof(IsMaximizable), typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnIsMaximizableChanged));

    public static readonly DependencyProperty IsMinimizableProperty =
        DependencyProperty.Register(nameof(IsMinimizable), typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnIsMinimizableChanged));

    public static readonly DependencyProperty IsAlwaysOnTopProperty =
        DependencyProperty.Register(nameof(IsAlwaysOnTop), typeof(bool), typeof(TitleBar), new PropertyMetadata(false, OnIsAlwaysOnTopChanged));

    public static readonly DependencyProperty PaneButtonTooltipTextProperty =
        DependencyProperty.Register(nameof(PaneButtonTooltipText), typeof(string), typeof(TitleBar), new PropertyMetadata("Toggle menu"));

    public static readonly DependencyProperty BackButtonTooltipTextProperty =
        DependencyProperty.Register(nameof(BackButtonTooltipText), typeof(string), typeof(TitleBar), new PropertyMetadata("Back"));

    private static void OnHasTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var titleBar = (TitleBar)d;
        if (titleBar.Window != null)
        {
            if (titleBar.Window.AppWindow.Presenter is OverlappedPresenter presenter)
            {
                var value = (bool)e.NewValue;
                var hasBorder = presenter.HasBorder;
                if (value)
                {
                    hasBorder = true;
                }

                presenter.SetBorderAndTitleBar(hasBorder, value);
            }
        }
    }

    private static void OnIsResizableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var titleBar = (TitleBar)d;
        if (titleBar.Window != null)
        {
            if (titleBar.Window.AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = (bool)e.NewValue;
            }
        }
    }

    private static void OnIsMaximizableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var titleBar = (TitleBar)d;
        if (titleBar.Window != null)
        {
            if (titleBar.Window.AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsMaximizable = (bool)e.NewValue;
            }
        }
    }

    private static void OnIsMinimizableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var titleBar = (TitleBar)d;
        if (titleBar.Window != null)
        {
            if (titleBar.Window.AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsMinimizable = (bool)e.NewValue;
            }
        }
    }

    private static void OnIsAlwaysOnTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var titleBar = (TitleBar)d;
        if (titleBar.Window != null)
        {
            if (titleBar.Window.AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsAlwaysOnTop = (bool)e.NewValue;
            }
        }
    }

    private void ConfigPresenter()
    {
        if (Window.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsAlwaysOnTop = IsAlwaysOnTop;
            presenter.IsResizable = IsResizable;
            presenter.IsMaximizable = IsMaximizable;
            presenter.IsMinimizable = IsMinimizable;
            presenter.SetBorderAndTitleBar(true, HasTitleBar);
        }
    }
}
