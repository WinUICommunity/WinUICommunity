using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.System;
using WinUICommunity;

namespace WinUICommunityGallery.Examples;
public sealed partial class ControlExample : OptionsPageControl
{
    public static readonly DependencyProperty DocTypeProperty =
        DependencyProperty.Register("DocType", typeof(DocType), typeof(ControlExample), new PropertyMetadata(DocType.Components));

    public static readonly DependencyProperty DocPageProperty =
        DependencyProperty.Register(nameof(DocPage), typeof(string), typeof(ControlExample), new PropertyMetadata(null, OnDocPageChanged));

    public static readonly DependencyProperty XamlProperty =
        DependencyProperty.Register("Xaml", typeof(string), typeof(ControlExample), new PropertyMetadata(null, OnXamlChanged));

    public static readonly DependencyProperty XamlSourceProperty =
        DependencyProperty.Register("XamlSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null, OnXamlSourceChanged));

    public static readonly DependencyProperty CSharpProperty =
        DependencyProperty.Register("CSharp", typeof(string), typeof(ControlExample), new PropertyMetadata(null, OnCSharpChanged));

    public static readonly DependencyProperty CSharpSourceProperty =
        DependencyProperty.Register("CSharpSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null, OnCSharpSourceChanged));

    public DocType DocType
    {
        get { return (DocType)GetValue(DocTypeProperty); }
        set { SetValue(DocTypeProperty, value); }
    }

    public string DocPage
    {
        get { return (string)GetValue(DocPageProperty); }
        set { SetValue(DocPageProperty, value); }
    }

    public string Xaml
    {
        get { return (string)GetValue(XamlProperty); }
        set { SetValue(XamlProperty, value); }
    }

    public string XamlSource
    {
        get { return (string)GetValue(XamlSourceProperty); }
        set { SetValue(XamlSourceProperty, value); }
    }

    public string CSharp
    {
        get { return (string)GetValue(CSharpProperty); }
        set { SetValue(CSharpProperty, value); }
    }

    public string CSharpSource
    {
        get { return (string)GetValue(CSharpSourceProperty); }
        set { SetValue(CSharpSourceProperty, value); }
    }
    private static void OnDocPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null && ctl.btnGoToDoc != null)
        {
            if (string.IsNullOrEmpty(ctl.DocPage))
            {
                ctl.btnGoToDoc.Visibility = Visibility.Collapsed;
            }
            else
            {
                ctl.btnGoToDoc.Visibility = Visibility.Visible;
            }
        }
    }

    private static void OnXamlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            ctl.HandlePivotItemVisibility(ctl.Xaml, ctl.XamlSource, ctl.PivotXAMLItem);
        }
    }

    private static void OnXamlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            ctl.HandlePivotItemVisibility(ctl.Xaml, ctl.XamlSource, ctl.PivotXAMLItem);
        }
    }

    private static void OnCSharpChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            ctl.HandlePivotItemVisibility(ctl.CSharp, ctl.CSharpSource, ctl.PivotCSharpItem);
        }
    }

    private static void OnCSharpSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (ControlExample)d;
        if (ctl != null)
        {
            ctl.HandlePivotItemVisibility(ctl.CSharp, ctl.CSharpSource, ctl.PivotCSharpItem);
        }
    }

    public ControlExample()
    {
        this.InitializeComponent();
        Loaded += ControlExample_Loaded;
    }

    private void ControlExample_Loaded(object sender, RoutedEventArgs e)
    {
        OnCSharpChanged(this, null);
        OnCSharpSourceChanged(this, null);
        OnXamlChanged(this, null);
        OnXamlSourceChanged(this, null);
        OnDocPageChanged(this, null);
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

    private void ViewCode_Click(object sender, RoutedEventArgs e)
    {
        IsFooterExpanded = !IsFooterExpanded;
    }
    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        if (ActualTheme == ElementTheme.Dark)
        {
            App.Current.ThemeService.SetElementThemeWithoutSave(ElementTheme.Light);
        }
        else
        {
            App.Current.ThemeService.SetElementThemeWithoutSave(ElementTheme.Dark);
        }
    }
    private async void GoToDoc_Click(object sender, RoutedEventArgs e)
    {
        string docTypeValue = null;
        switch (DocType)
        {
            case DocType.Core:
                docTypeValue = "winUICommunityCore";
                break;
            case DocType.Components:
                docTypeValue = "winUICommunityComponents";
                break;
            case DocType.Win2d:
                docTypeValue = "winUICommunityWin2d";
                break;
            case DocType.LandingPages:
                docTypeValue = "winUICommunityLandingPages";
                break;
            case DocType.ContextMenuExtensions:
                docTypeValue = "winUICommunityContextMenuExtensions";
                break;
        }

        await Launcher.LaunchUriAsync(new Uri($"https://ghost1372.github.io/{docTypeValue}/{DocPage}"));
    }
}
public enum DocType
{
    Core,
    Components,
    Win2d,
    LandingPages,
    ContextMenuExtensions
}
