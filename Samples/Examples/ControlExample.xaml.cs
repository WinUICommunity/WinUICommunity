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
        DependencyProperty.Register("Xaml", typeof(string), typeof(ControlExample), new PropertyMetadata(null));

    public static readonly DependencyProperty XamlSourceProperty =
        DependencyProperty.Register("XamlSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null));

    public static readonly DependencyProperty CSharpProperty =
        DependencyProperty.Register("CSharp", typeof(string), typeof(ControlExample), new PropertyMetadata(null));

    public static readonly DependencyProperty CSharpSourceProperty =
        DependencyProperty.Register("CSharpSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null));

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

    public ControlExample()
    {
        this.InitializeComponent();
        Loaded += ControlExample_Loaded;
    }

    private void ControlExample_Loaded(object sender, RoutedEventArgs e)
    {
        OnDocPageChanged(this, null);
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

    private void SelectorBarItem_Loaded(object sender, RoutedEventArgs e)
    {
        var item = sender as SelectorBarItem;
        if (item.Tag.Equals("XAML"))
        {
            if (string.IsNullOrEmpty(Xaml) && string.IsNullOrEmpty(XamlSource))
            {
                item.Visibility = Visibility.Collapsed;
            }
        }
        else if (item.Tag.Equals("C#"))
        {
            if (string.IsNullOrEmpty(CSharp) && string.IsNullOrEmpty(CSharpSource))
            {
                item.Visibility = Visibility.Collapsed;
            }
        }

        var firstVisibileItem = selectorBarControl.Items.Where(x => x.Visibility == Visibility.Visible).FirstOrDefault();
        if (firstVisibileItem != null)
        {
            firstVisibileItem.IsSelected = true;
        }
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
