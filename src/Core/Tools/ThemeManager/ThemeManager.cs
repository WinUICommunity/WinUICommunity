namespace WinUICommunity;

public partial class ThemeManager
{
    private Window currentWindow { get; set; }
    private SystemBackdrop currentSystemBackdrop { get; set; }
    private Dictionary<Window, SystemBackdrop> currentSystemBackdropDic = new();

    public delegate void ActualThemeChangedEventHandler(FrameworkElement sender, object args);
    public event ActualThemeChangedEventHandler ActualThemeChanged;

    public ThemeOptions ThemeOptions { get; set; }

    private bool useBuiltInSettings { get; set; } = true;

    /// <summary>
    /// Gets the current ThemeManager instance.
    /// </summary>
    /// <remarks>
    /// This property returns the current ThemeManager instance, which is used to manage the application's theme and UI settings. The ThemeManager is a singleton class, meaning that only one instance of it can exist at any given time. The Instance property provides access to the current instance of the ThemeManager, which can be used to change themes or perform other operations related to the application's visual style and appearance.
    /// </remarks>
    /// <returns>The current ThemeManager instance.</returns>

    private static ThemeManager instance;
    public static ThemeManager Instance => instance;

    /// <summary>
    /// Gets the current actual theme of the app based on the requested theme of the
    /// root element, or if that value is Default, the requested theme of the Application.
    /// </summary>
    public ElementTheme ActualTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    if (rootElement.RequestedTheme != ElementTheme.Default)
                    {
                        return rootElement.RequestedTheme;
                    }
                }
            }

            if (currentWindow != null && currentWindow.Content is FrameworkElement element)
            {
                if (element.RequestedTheme != ElementTheme.Default)
                {
                    return element.RequestedTheme;
                }
            }
            return ApplicationHelper.GetEnum<ElementTheme>(Application.Current.RequestedTheme.ToString());
        }
    }

    /// <summary>
    /// Gets or sets (with LocalSettings persistence) the RequestedTheme of the root element.
    /// </summary>
    public ElementTheme RootTheme
    {
        get
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    return rootElement.RequestedTheme;
                }
            }
            return currentWindow != null && currentWindow.Content is FrameworkElement element ? element.RequestedTheme : ElementTheme.Default;
        }
        set
        {
            foreach (Window window in WindowHelper.ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = value;
                }
            }

            if (currentWindow != null && currentWindow.Content is FrameworkElement element)
            {
                element.RequestedTheme = value;
            }
            if (this.useBuiltInSettings)
            {
                Settings.ElementTheme = value;
                Settings?.Save();
            }
        }
    }

    public SystemBackdrop GetSystemBackdrop(BackdropType backdropType)
    {
        switch (backdropType)
        {
            case BackdropType.None:
                return null;
            case BackdropType.Mica:
                return new MicaBackdrop();
            case BackdropType.MicaAlt:
                return new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
            case BackdropType.DesktopAcrylic:
                return new DesktopAcrylicBackdrop();
            default:
                return null;
        }
    }
    private SystemBackdrop GetCurrentSystemBackdropFromLocalConfig(BackdropType backdropType, bool ForceBackdrop)
    {
        BackdropType currentBackdrop = this.useBuiltInSettings ? Settings.BackdropType : backdropType;
        return ForceBackdrop ? GetSystemBackdrop(backdropType) : GetSystemBackdrop(currentBackdrop);
    }

    private ElementTheme GetCurrentThemeFromLocalConfig(ElementTheme theme, bool forceTheme)
    {
        var currentTheme = Settings.ElementTheme;
        return forceTheme ? theme : currentTheme;
    }

    /// <summary>
    /// Sets the system backdrop type for Windows.
    /// </summary>
    /// <param name="backdropType">The type of backdrop to set.</param>
    /// <remarks>
    /// This method changes the system backdrop type for Windows. The "backdropType" parameter specifies the type of backdrop to set, such as "Acrylic" or "Mica". The backdrop type can affect the visual appearance of the application. 
    /// </remarks>
    public void SetCurrentSystemBackdrop(BackdropType backdropType)
    {
        var systemBackdrop = GetSystemBackdrop(backdropType);

        foreach (var key in currentSystemBackdropDic.Keys)
        {
            if (Settings.BackdropType != backdropType)
            {
                key.SystemBackdrop = systemBackdrop;
                currentSystemBackdropDic[key] = systemBackdrop;
            }

            SetBackdropFallBackColorForWindows10(key);
        }

        if (currentSystemBackdrop != null)
        {
            if (Settings.BackdropType != backdropType)
            {
                currentWindow.SystemBackdrop = systemBackdrop;
            }

            SetBackdropFallBackColorForWindows10(currentWindow);
        }

        if (this.useBuiltInSettings && Settings.BackdropType != backdropType)
        {
            Settings.BackdropType = backdropType;
            Settings?.Save();
        }
    }

    /// <summary>
    /// Retrieves the current system backdrop type for the specified window. (WindowHelper.TrackWindow)
    /// </summary>
    /// <param name="activeWindow">The window to retrieve the backdrop type for.</param>
    /// <returns>The current system backdrop type for the specified window.</returns>
    /// <remarks>
    /// This method retrieves the current system backdrop type for the specified window. The backdrop type can affect the visual appearance of the application, depending on the system settings and the app's design. The method returns the current backdrop type as a "BackdropType" enumeration, such as "Acrylic" or "Mica". 
    /// </remarks>
    /// <returns>SystemBackdrop</returns>
    public SystemBackdrop GetCurrentSystemBackdrop(Window activeWindow)
    {
        var currentWindow = currentSystemBackdropDic.FirstOrDefault(x => x.Key == activeWindow);
        return currentWindow.Value != null ? currentWindow.Key.SystemBackdrop : null;
    }

    /// <summary>
    /// Retrieves the current system backdrop type.
    /// </summary>
    /// <returns>The current system backdrop type.</returns>
    /// <remarks>
    /// This method retrieves the current system backdrop type, which can affect the visual appearance of the application, depending on the system settings and the app's design. The method returns the current backdrop type as a "BackdropType" enumeration, such as "Acrylic" or "Mica". 
    /// </remarks>
    /// <returns>SystemBackdrop</returns>
    public SystemBackdrop GetCurrentSystemBackdrop()
    {
        return currentSystemBackdrop != null ? currentWindow.SystemBackdrop : null;
    }

    private void SetBackdropFallBackColorForWindows10(Window window)
    {
        if (OSVersionHelper.IsWindows10_1809_OrGreater && !OSVersionHelper.IsWindows11_22000_OrGreater && ThemeOptions != null && ThemeOptions.BackdropFallBackColorForWindows10 != null)
        {
            var content = window.Content;
            if (content != null)
            {
                var element = window.Content as FrameworkElement;
                dynamic panel = (dynamic)element;
                panel.Background = ThemeOptions.BackdropFallBackColorForWindows10;
            }
        }
    }
    private void OnActualThemeChanged(FrameworkElement sender, object args)
    {
        WindowHelper.SetPreferredAppMode(sender.ActualTheme);
        UpdateSystemCaptionButtonColors();
        ActualThemeChanged?.Invoke(sender, args);
    }

    /// <summary>
    /// Determines whether the current theme is a dark theme.
    /// </summary>
    /// <returns>True if the current theme is a dark theme, otherwise false.</returns>
    /// <remarks>
    /// This method determines whether the current theme is a dark theme. It checks the system settings and returns true if the current theme is set to a dark theme, otherwise false. This can be used to customize the user interface based on the current theme, such as adjusting the color scheme or selecting different icons or images. 
    /// </remarks>
    public bool IsDarkTheme()
    {
        return RootTheme == ElementTheme.Default
            ? Application.Current.RequestedTheme == ApplicationTheme.Dark
            : RootTheme == ElementTheme.Dark;
    }

    public void UpdateSystemCaptionButtonForWindow(Window window)
    {
        var res = Application.Current.Resources;

        if (this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme == null && this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme == null)
        {
            res["WindowCaptionBackground"] = Colors.Transparent;
            res["WindowCaptionBackgroundDisabled"] = Colors.Transparent;
        }
        else
        {
            if (IsDarkTheme())
            {
                res["WindowCaptionBackground"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonBackgroundColor;
                res["WindowCaptionBackgroundDisabled"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonInactiveBackgroundColor;
                res["WindowCaptionForeground"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonForegroundColor;
                res["WindowCaptionForegroundDisabled"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonInactiveForegroundColor;
            }
            else
            {
                res["WindowCaptionBackground"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonBackgroundColor;
                res["WindowCaptionBackgroundDisabled"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonInactiveBackgroundColor;
                res["WindowCaptionForeground"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonForegroundColor;
                res["WindowCaptionForegroundDisabled"] = this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonInactiveForegroundColor;
            }
        }

        WindowHelper.ActivateWindowAgain(window);
    }

    public void UpdateSystemCaptionButtonForAppWindow(Window window)
    {
        var titleBar = window.AppWindow.TitleBar;
        if (this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme == null && this.ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme == null)
        {
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            if (IsDarkTheme())
            {
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            }
            else
            {
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            }
        }
        else
        {
            if (IsDarkTheme())
            {
                titleBar.BackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.BackgroundColor;
                titleBar.ForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ForegroundColor;
                titleBar.ButtonForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonForegroundColor;
                titleBar.ButtonBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonBackgroundColor;
                titleBar.ButtonHoverBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonHoverBackgroundColor;
                titleBar.ButtonHoverForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonHoverForegroundColor;
                titleBar.ButtonInactiveBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonInactiveBackgroundColor;
                titleBar.ButtonInactiveForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonInactiveForegroundColor;
                titleBar.ButtonPressedBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonPressedBackgroundColor;
                titleBar.ButtonPressedForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForDarkTheme?.ButtonPressedForegroundColor;
            }
            else
            {
                titleBar.BackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.BackgroundColor;
                titleBar.ForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ForegroundColor;
                titleBar.ButtonForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonForegroundColor;
                titleBar.ButtonBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonBackgroundColor;
                titleBar.ButtonHoverBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonHoverBackgroundColor;
                titleBar.ButtonHoverForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonHoverForegroundColor;
                titleBar.ButtonInactiveBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonInactiveBackgroundColor;
                titleBar.ButtonInactiveForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonInactiveForegroundColor;
                titleBar.ButtonPressedBackgroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonPressedBackgroundColor;
                titleBar.ButtonPressedForegroundColor = ThemeOptions?.TitleBarCustomization?.CaptionButtonsColorForLightTheme?.ButtonPressedForegroundColor;
            }
        }
    }

    public void ResetCaptionButtonColors(Window window)
    {
        var res = Application.Current.Resources;

        window.AppWindow.TitleBar.BackgroundColor = null;
        window.AppWindow.TitleBar.ButtonBackgroundColor = null;
        window.AppWindow.TitleBar.ButtonInactiveBackgroundColor = null;
        window.AppWindow.TitleBar.ButtonHoverBackgroundColor = null;
        window.AppWindow.TitleBar.ButtonPressedBackgroundColor = null;
        window.AppWindow.TitleBar.ForegroundColor = null;
        window.AppWindow.TitleBar.ButtonForegroundColor = null;
        window.AppWindow.TitleBar.ButtonInactiveForegroundColor = null;
        window.AppWindow.TitleBar.ButtonHoverForegroundColor = null;
        window.AppWindow.TitleBar.ButtonPressedForegroundColor = null;
        res["WindowCaptionBackground"] = res["SystemControlBackgroundBaseLowBrush"];
        res["WindowCaptionBackgroundDisabled"] = res["SystemControlBackgroundBaseLowBrush"];
        res["WindowCaptionForeground"] = res["SystemControlForegroundBaseHighBrush"];
        res["WindowCaptionForegroundDisabled"] = res["SystemControlDisabledBaseMediumLowBrush"];
        WindowHelper.ActivateWindowAgain(window);
    }

    public void UpdateSystemCaptionButton(Window window)
    {
        if (this.ThemeOptions?.TitleBarCustomization != null)
        {
            switch (this.ThemeOptions.TitleBarCustomization.TitleBarType)
            {
                case TitleBarType.None:
                    ResetCaptionButtonColors(window);
                    break;
                case TitleBarType.Window:
                    UpdateSystemCaptionButtonForWindow(window);
                    break;
                case TitleBarType.AppWindow:
                    UpdateSystemCaptionButtonForAppWindow(window);
                    break;
            }
        }
    }

    /// <summary>
    /// Updates the system caption button colors, only if you are using a AppWindow.TitleBar and TitleBar.ExtendsContentIntoTitleBar = true
    /// </summary>
    /// <remarks>
    /// This method updates the system caption button colors, which are the colors used for the minimize, maximize, and close buttons on the window caption bar. The method uses the current system color scheme to update the colors, ensuring that they match the user's preferences. This can be useful for applications that use custom caption buttons or need to ensure that the caption buttons are visible and accessible to users. 
    /// </remarks>
    private void UpdateSystemCaptionButtonColors()
    {
        foreach (Window window in WindowHelper.ActiveWindows)
        {
            UpdateSystemCaptionButton(window);
        }

        if (currentWindow != null)
        {
            UpdateSystemCaptionButton(currentWindow);
        }
    }

    /// <summary>
    /// Changes the application theme to the specified element theme.
    /// </summary>
    /// <param name="elementTheme">The element theme to change the application theme to.</param>
    /// <remarks>
    /// This method changes the application theme to the specified element theme, such as "Dark" or "Light". It sets the requested theme as the current theme for the application's UI, including the window background, foreground, and other visual elements. This can be useful for customizing the user interface based on the user's preferences or the application's requirements. 
    /// </remarks>
    public void SetCurrentTheme(ElementTheme elementTheme)
    {
        if (RootTheme != elementTheme)
        {
            RootTheme = elementTheme;
        }
    }

    public ElementTheme GetCurrentTheme()
    {
        return RootTheme;
    }

    public BackdropType GetBackdropType(SystemBackdrop systemBackdrop)
    {
        var backdropType = systemBackdrop?.GetType();
        if (backdropType == typeof(MicaBackdrop))
        {
            var mica = (MicaBackdrop)systemBackdrop;
            return mica.Kind == Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt ? BackdropType.MicaAlt : BackdropType.Mica;
        }
        else
        {
            return backdropType == typeof(DesktopAcrylicBackdrop) ? BackdropType.DesktopAcrylic : BackdropType.None;
        }
    }

    #region RadioButtons and ComboBox
    /// <summary>
    /// Event handler for when the selection is changed in a combobox.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <remarks>
    /// This method is an event handler for when the selection is changed in a combobox. It takes the sender object as a parameter and performs the required actions based on the selected item in the combobox.
    /// </remarks>
    public void OnThemeComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedTheme = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            var currentTheme = ApplicationHelper.GetEnum<ElementTheme>(selectedTheme);
            if (RootTheme != currentTheme)
            {
                RootTheme = currentTheme;
            }
        }
    }

    /// <summary>
    /// Sets the default item for the specified combobox.
    /// </summary>
    /// <param name="themeComboBox">The combobox to set the default item for.</param>
    /// <remarks>
    /// This method sets the default item for the specified combobox. It is typically used to ensure that one of the items in the combobox is selected by default when the combo box is displayed to the user.
    /// </remarks>
    public void SetThemeComboBoxDefaultItem(ComboBox themeComboBox)
    {
        var currentTheme = RootTheme.ToString();
        var item = themeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme);
        if ((ComboBoxItem)themeComboBox.SelectedItem != item)
        {
            themeComboBox.SelectedItem = item;
        }
    }

    /// <summary>
    /// Event handler for when the selection is changed in a combobox.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <remarks>
    /// This method is an event handler for when the selection is changed in a combobox. It takes the sender object as a parameter and performs the required actions based on the selected item in the combobox.
    /// </remarks>
    public void OnBackdropComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedBackdrop = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            var backdrop = ApplicationHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetCurrentSystemBackdrop(backdrop);
        }
    }

    /// <summary>
    /// Sets the default item for the specified combobox.
    /// </summary>
    /// <param name="backdropComboBox">The combobox to set the default item for.</param>
    /// <remarks>
    /// This method sets the default item for the specified combobox. It is typically used to ensure that one of the items in the combobox is selected by default when the combo box is displayed to the user.
    /// </remarks>
    public void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox)
    {
        var currentBackdrop = GetBackdropType(GetCurrentSystemBackdrop()).ToString();

        var item = backdropComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop);
        if ((ComboBoxItem)backdropComboBox.SelectedItem != item)
        {
            backdropComboBox.SelectedItem = item;
        }
    }

    /// <summary>
    /// Event handler for when a radio button is checked.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <remarks>
    /// This method is an event handler for when a radio button is checked. It takes the sender object as a parameter and performs the required actions based on the checked state of the radio button. The specific actions performed by this method depend on the requirements of the application and the UI design, and can include updating the user interface, setting application preferences, or performing other operations.
    /// </remarks>
    public void OnThemeRadioButtonChecked(object sender)
    {
        var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            var currentTheme = ApplicationHelper.GetEnum<ElementTheme>(selectedTheme);
            if (RootTheme != currentTheme)
            {
                RootTheme = currentTheme;
            }
        }
    }

    /// <summary>
    /// Sets the default radio button item for the specified panel.
    /// </summary>
    /// <param name="ThemePanel">The panel to set the default radio button item for. StackPanel/Grid</param>
    /// <remarks>
    /// This method sets the default radio button item for the specified panel. It is typically used to ensure that one of the radio buttons in the panel is selected by default when the panel is displayed to the user. 
    /// </remarks>
    public void SetThemeRadioButtonDefaultItem(Panel ThemePanel)
    {
        var currentTheme = RootTheme.ToString();
        ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
    }

    /// <summary>
    /// Event handler for when a radio button is checked.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <remarks>
    /// This method is an event handler for when a radio button is checked. It takes the sender object as a parameter and performs the required actions based on the checked state of the radio button. The specific actions performed by this method depend on the requirements of the application and the UI design, and can include updating the user interface, setting application preferences, or performing other operations.
    /// </remarks>
    public void OnBackdropRadioButtonChecked(object sender)
    {
        var selectedBackdrop = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            var backdrop = ApplicationHelper.GetEnum<BackdropType>(selectedBackdrop);
            SetCurrentSystemBackdrop(backdrop);
        }
    }

    /// <summary>
    /// Sets the default radio button item for the specified panel.
    /// </summary>
    /// <param name="BackdropPanel">The panel to set the default radio button item for. StackPanel/Grid</param>
    /// <remarks>
    /// This method sets the default radio button item for the specified panel. It is typically used to ensure that one of the radio buttons in the panel is selected by default when the panel is displayed to the user. 
    /// </remarks>
    public void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel)
    {
        var currentBackdrop = GetBackdropType(GetCurrentSystemBackdrop()).ToString();

        BackdropPanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop).IsChecked = true;
    }
    #endregion
}
