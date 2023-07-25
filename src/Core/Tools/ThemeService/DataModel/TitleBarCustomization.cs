namespace WinUICommunity;

public class TitleBarCustomization
{
    public TitleBarWindowType TitleBarType { get; set; } = TitleBarWindowType.None;

    public TitleBarButtons CaptionButtonsColorForLightTheme { get; set; }
    public TitleBarButtons CaptionButtonsColorForDarkTheme { get; set; }
}
