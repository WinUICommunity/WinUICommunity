namespace WinUICommunity;

public partial class ThemeManager
{
    private Window builderWindow = null;
    private ThemeOptions builderThemeOptions = null;

    /// <summary>
    /// Sets the Window to be used by the ThemeManager during initialization.
    /// </summary>
    /// <param name="window">The Window to use.</param>
    /// <returns>The ThemeManager instance.</returns>
    /// <remarks>
    /// This method sets the Window to be used by the ThemeManager during initialization. If this method is not called, the ThemeManager will use the application's activeWindow (if one exists) as the default Window. also you should use BuildWithoutWindow instead of Build Method.
    /// </remarks>
    public ThemeManager UseWindow(Window window)
    {
        this.builderWindow = window;
        return this;
    }

    public ThemeManager UseThemeOptions(ThemeOptions themeOptions)
    {
        this.builderThemeOptions = themeOptions;
        return this;
    }

    /// <summary>
    /// Builds the ThemeManager instance using the specified settings without Window.
    /// </summary>
    /// <returns>The ThemeManager instance.</returns>
    public ThemeManager BuildWithoutWindow()
    {
        builderWindow = null;
        return InternalBuild();
    }

    /// <summary>
    /// Builds the ThemeManager instance using the specified settings.
    /// </summary>
    /// <returns>The ThemeManager instance.</returns>
    /// <remarks>
    /// This method builds the ThemeManager instance using the specified settings. If no settings were specified, the ThemeManager will use the default settings for the system.
    /// </remarks>
    public ThemeManager Build()
    {
        return InternalBuild();
    }

    private ThemeManager InternalBuild()
    {
        if (builderWindow == null && builderThemeOptions == null)
        {
            return new ThemeManager(null, null);
        }
        else if (builderWindow == null && builderThemeOptions != null)
        {
            return new ThemeManager(builderThemeOptions);
        }
        else if (builderWindow != null && builderThemeOptions == null)
        {
            return new ThemeManager(builderWindow);
        }
        else if (builderWindow != null && builderThemeOptions != null)
        {
            return new ThemeManager(builderWindow, builderThemeOptions);
        }
        return this;
    }
}
