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
            ctl.HandlePivotItemVisibility(ctl.Xaml, ctl.XamlSource, ctl.PivotXAMLItem);
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
            ctl.HandlePivotItemVisibility(ctl.Xaml, ctl.XamlSource, ctl.PivotXAMLItem);
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
            ctl.HandlePivotItemVisibility(ctl.CSharp, ctl.CSharpSource, ctl.PivotCSharpItem);
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
            ctl.HandlePivotItemVisibility(ctl.CSharp, ctl.CSharpSource, ctl.PivotCSharpItem);
        }
    }
    private void AddOrRemovePivotItem(Visibility visibility, PivotItem pivotItem)
    {
        if (visibility == Visibility.Collapsed)
        {
            if (pivot.Items.Any(p => ((PivotItem)p).Equals(pivotItem)))
            {
                pivot.Items.Remove(pivotItem);
            }
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
            btnViewCode.Visibility = Visibility.Collapsed;
            FooterVisibility = Visibility.Collapsed;
            foreach (var item in pivot.Items)
            {
                pivot.Items.Remove(item);
            }
        }
        else
        {
            btnViewCode.Visibility = Visibility.Visible;
            FooterVisibility = Visibility.Visible;
        }
    }
    private void HandlePivotItemVisibility(string firstValue, string secondValue, PivotItem pivotItem)
    {
        if (string.IsNullOrEmpty(firstValue) && string.IsNullOrEmpty(secondValue))
        {
            pivotItem.Visibility = Visibility.Collapsed;
            AddOrRemovePivotItem(Visibility.Collapsed, pivotItem);
        }
        else
        {
            PivotXAMLItem.Visibility = Visibility.Visible;
            AddOrRemovePivotItem(Visibility.Visible, pivotItem);
        }

        HandleFooterVisibility();
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

        OnCSharpChanged(this, null);
        OnCSharpSourceChanged(this, null);
        OnXamlChanged(this, null);
        OnXamlSourceChanged(this, null);
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
