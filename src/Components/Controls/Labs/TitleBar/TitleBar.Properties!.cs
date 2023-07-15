using Microsoft.UI.Windowing;

namespace WinUICommunity;
public partial class TitleBar : Control
{
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

    public static readonly DependencyProperty HasTitleBarProperty =
        DependencyProperty.Register("HasTitleBar", typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnHasTitleBarChanged));

    public static readonly DependencyProperty IsResizableProperty =
        DependencyProperty.Register("IsResizable", typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnIsResizableChanged));

    public static readonly DependencyProperty IsMaximizableProperty =
        DependencyProperty.Register("IsMaximizable", typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnIsMaximizableChanged));

    public static readonly DependencyProperty IsMinimizableProperty =
        DependencyProperty.Register("IsMinimizable", typeof(bool), typeof(TitleBar), new PropertyMetadata(true, OnIsMinimizableChanged));

    public static readonly DependencyProperty IsAlwaysOnTopProperty =
        DependencyProperty.Register("IsAlwaysOnTop", typeof(bool), typeof(TitleBar), new PropertyMetadata(false, OnIsAlwaysOnTopChanged));

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
