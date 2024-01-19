namespace WinUICommunity;
public partial class ThemeService
{
    public void OnThemeComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedTheme = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            var currentTheme = GeneralHelper.GetEnum<ElementTheme>(selectedTheme);
            SetElementTheme(currentTheme);
        }
    }

    public void SetThemeComboBoxDefaultItem(ComboBox themeComboBox)
    {
        var currentTheme = RootTheme.ToString();
        var item = themeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme);
        if ((ComboBoxItem)themeComboBox.SelectedItem != item)
        {
            themeComboBox.SelectedItem = item;
        }
    }

    public void OnBackdropComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedBackdrop = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            var backdrop = GeneralHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetBackdropType(backdrop);
        }
    }

    public void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox)
    {
        var currentBackdrop = GetBackdropType(GetSystemBackdrop()).ToString();

        var item = backdropComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop);
        if ((ComboBoxItem)backdropComboBox.SelectedItem != item)
        {
            backdropComboBox.SelectedItem = item;
        }
    }

    public void OnThemeRadioButtonChecked(object sender)
    {
        var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            var currentTheme = GeneralHelper.GetEnum<ElementTheme>(selectedTheme);
            SetElementTheme(currentTheme);
        }
    }

    public void SetThemeRadioButtonDefaultItem(Panel ThemePanel)
    {
        var currentTheme = RootTheme.ToString();
        ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
    }

    public void OnBackdropRadioButtonChecked(object sender)
    {
        var selectedBackdrop = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            var backdrop = GeneralHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetBackdropType(backdrop);
        }
    }

    public void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel)
    {
        var currentBackdrop = GetBackdropType(GetSystemBackdrop()).ToString();

        BackdropPanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop).IsChecked = true;
    }
}
