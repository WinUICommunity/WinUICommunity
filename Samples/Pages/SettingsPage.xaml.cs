using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.System;

namespace WinUICommunityGallery.Pages;
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        this.InitializeComponent();
        Loaded += SettingsPage_Loaded;
    }

    private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
    {
        App.Current.ThemeService.SetThemeComboBoxDefaultItem(CmbTheme);
        App.Current.ThemeService.SetBackdropComboBoxDefaultItem(CmbBackdrop);
    }

    private void cmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.Current.ThemeService.OnThemeComboBoxSelectionChanged(sender);
    }

    private void cmbBackdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.Current.ThemeService.OnBackdropComboBoxSelectionChanged(sender);
    }

    private async void SettingsCard_Click(object sender, RoutedEventArgs e)
    {
        _ = await Launcher.LaunchUriAsync(new Uri("ms-settings:colors"));
    }

    private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {
        App.Current.ThemeService.SetBackdropTintColor(args.NewColor);
    }
}
