using DemoApp;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity.DemoApp.Pages;

public sealed partial class ThemeManagerPage : Page
{
    public ThemeManagerPage()
    {
        this.InitializeComponent();
    }

    private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        App.Current.ThemeManager.OnThemeRadioButtonChecked(sender);
    }
    private void OnBackdropRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        App.Current.ThemeManager.OnBackdropRadioButtonChecked(sender);
    }
    private void cmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.Current.ThemeManager.OnThemeComboBoxSelectionChanged(sender);
    }
    private void cmbBackdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.Current.ThemeManager.OnBackdropComboBoxSelectionChanged(sender);
    }
    private void SettingsPageControl_Loaded(object sender, RoutedEventArgs e)
    {
        App.Current.ThemeManager.SetThemeRadioButtonDefaultItem(themePanel);
        App.Current.ThemeManager.SetBackdropRadioButtonDefaultItem(backdropPanel);
        App.Current.ThemeManager.SetThemeComboBoxDefaultItem(cmbTheme);
        App.Current.ThemeManager.SetBackdropComboBoxDefaultItem(cmbBackdrop);
    }
}
