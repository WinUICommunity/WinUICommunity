namespace WinUICommunity;

public class ThemeOptions
{
    public BackdropType BackdropType { get; set; } = BackdropType.Mica;

    public Brush? BackdropFallBackColorForWindows10 { get; set; }
    public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;
    public bool ForceBackdrop { get; set; }
    public bool ForceElementTheme { get; set; }
    public TitleBarCustomization TitleBarCustomization { get; set; }
    public bool UseBuiltInSettings { get; set; } = true;
    public string BuiltInSettingsFileName { get; set; }
}
