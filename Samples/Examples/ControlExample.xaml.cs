using System.Linq;
using DemoApp;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity.DemoApp.Examples;
public sealed partial class ControlExample : OptionsPageControl
{
    public static readonly DependencyProperty XamlProperty = DependencyProperty.Register("Xaml", typeof(string), typeof(ControlExample), new PropertyMetadata(null, OnXamlChanged));

    private static void OnXamlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            if (string.IsNullOrEmpty((string)e.NewValue) && string.IsNullOrEmpty(ctl.XamlSource))
            {
                ctl.PivotXAMLItem.Visibility = Visibility.Collapsed;
                ctl.AddOrRemovePivotItem(Visibility.Collapsed, ctl.PivotXAMLItem);
            }
            else
            {
                ctl.PivotXAMLItem.Visibility = Visibility.Visible;
                ctl.AddOrRemovePivotItem(Visibility.Visible, ctl.PivotXAMLItem);
            }

            ctl.HandleFooterVisibility();
        }
    }

    public string Xaml
    {
        get { return (string)GetValue(XamlProperty); }
        set { SetValue(XamlProperty, value); }
    }

    public static readonly DependencyProperty XamlSourceProperty = DependencyProperty.Register("XamlSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null, OnXamlSourceChanged));

    private static void OnXamlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            if (string.IsNullOrEmpty((string)e.NewValue) && string.IsNullOrEmpty(ctl.Xaml))
            {
                ctl.PivotXAMLItem.Visibility = Visibility.Collapsed;
                ctl.AddOrRemovePivotItem(Visibility.Collapsed, ctl.PivotXAMLItem);
            }
            else
            {
                ctl.PivotXAMLItem.Visibility = Visibility.Visible;
                ctl.AddOrRemovePivotItem(Visibility.Visible, ctl.PivotXAMLItem);
            }

            ctl.HandleFooterVisibility();
        }
    }

    public string XamlSource
    {
        get { return (string)GetValue(XamlSourceProperty); }
        set { SetValue(XamlSourceProperty, value); }
    }

    public static readonly DependencyProperty CSharpProperty = DependencyProperty.Register("CSharp", typeof(string), typeof(ControlExample), new PropertyMetadata(null, OnCSharpChanged));

    private static void OnCSharpChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            if (string.IsNullOrEmpty((string)e.NewValue) && string.IsNullOrEmpty(ctl.CSharpSource))
            {
                ctl.PivotCSharpItem.Visibility = Visibility.Collapsed;
                ctl.AddOrRemovePivotItem(Visibility.Collapsed, ctl.PivotCSharpItem);

            }
            else
            {
                ctl.PivotCSharpItem.Visibility = Visibility.Visible;
                ctl.AddOrRemovePivotItem(Visibility.Visible, ctl.PivotCSharpItem);
            }
            ctl.HandleFooterVisibility();
        }
    }

    public string CSharp
    {
        get { return (string)GetValue(CSharpProperty); }
        set { SetValue(CSharpProperty, value); }
    }

    public static readonly DependencyProperty CSharpSourceProperty = DependencyProperty.Register("CSharpSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null, OnCSharpSourceChanged));

    private static void OnCSharpSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            if (string.IsNullOrEmpty((string)e.NewValue) && string.IsNullOrEmpty(ctl.CSharp))
            {
                ctl.PivotCSharpItem.Visibility = Visibility.Collapsed;
                ctl.AddOrRemovePivotItem(Visibility.Collapsed, ctl.PivotCSharpItem);
            }
            else
            {
                ctl.PivotCSharpItem.Visibility = Visibility.Visible;
                ctl.AddOrRemovePivotItem(Visibility.Visible, ctl.PivotCSharpItem);
            }

            ctl.HandleFooterVisibility();
        }
    }
    private void AddOrRemovePivotItem(Visibility visibility, PivotItem pivotItem)
    {
        if (visibility == Visibility.Collapsed)
        {
            pivot.Items.Remove(pivot.Items.Single(p => ((PivotItem)p).Equals(pivotItem)));
        }
        else
        {
            if (!pivot.Items.Any(p => ((PivotItem)p).Equals(pivotItem)))
            {
                pivot.Items.Add(pivotItem);
            }
        }
    }
    private void HandleFooterVisibility()
    {
        if (string.IsNullOrEmpty(CSharpSource) && string.IsNullOrEmpty(CSharp) && string.IsNullOrEmpty(Xaml) && string.IsNullOrEmpty(XamlSource))
        {
            FooterVisibility = Visibility.Collapsed;
            foreach (var item in pivot.Items)
            {
                pivot.Items.Remove(item);
            }
        }
        else
        {
            FooterVisibility = Visibility.Visible;
        }
    }
    public string CSharpSource
    {
        get { return (string)GetValue(CSharpSourceProperty); }
        set { SetValue(CSharpSourceProperty, value); }
    }
    public ControlExample()
    {
        this.InitializeComponent();
        HandleFooterVisibility();
    }

    private void ViewCode_Click(object sender, RoutedEventArgs e)
    {
        IsFooterExpanded = !IsFooterExpanded;
    }
    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        if (ActualTheme == ElementTheme.Dark)
        {
            App.Current.ThemeService.SetCurrentThemeWithoutSave(ElementTheme.Light);
        }
        else
        {
            App.Current.ThemeService.SetCurrentThemeWithoutSave(ElementTheme.Dark);
        }
    }
}
