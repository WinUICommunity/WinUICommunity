using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.System;
using WinUICommunity;

namespace DemoApp.Pages;
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
        TintBox.Fill = new SolidColorBrush(args.NewColor);
        App.Current.ThemeService.SetBackdropTintColor(args.NewColor);
    }

    private void ColorPalette_ItemClick(object sender, ItemClickEventArgs e)
    {
        var color = e.ClickedItem as ColorPaletteItem;
        if (color != null)
        {
            if (color.Hex.Contains("#000000"))
            {
                App.Current.ThemeService.ResetBackdropProperties();
            }
            else
            {
                App.Current.ThemeService.SetBackdropTintColor(color.Color);
            }

            TintBox.Fill = new SolidColorBrush(color.Color);
            TintOpacitySlider.Value = (double)App.Current.ThemeService.GetBackdropTintOpacity();
        }
    }

    private double GetTintOpacity()
    {
        var value = App.Current.ThemeService.GetBackdropTintOpacity();
        return (double)value;
    }

    private void TintOpacitySlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        if (TintBox == null || TintBox.Fill == null)
        {
            return;
        }
        if ((float)e.NewValue != App.Current.ThemeService.GetBackdropTintOpacity())
        {
            // Changing the TintOpacity alone is not enough and currently the changes are not effect, that's why we reset and adjust it again.
            App.Current.ThemeService.ResetBackdropProperties();

            App.Current.ThemeService.SetBackdropTintOpacity((float)e.NewValue);
            App.Current.ThemeService.SetBackdropTintColor(((SolidColorBrush)TintBox.Fill).Color);
        }
    }
}
