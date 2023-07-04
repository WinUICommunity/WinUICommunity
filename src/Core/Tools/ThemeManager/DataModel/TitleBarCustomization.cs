namespace WinUICommunity;

public class TitleBarCustomization
{
    public TitleBarType TitleBarType { get; set; } = TitleBarType.None;

    public CaptionButtons CaptionButtonsColorForLightTheme { get; set; }
    public CaptionButtons CaptionButtonsColorForDarkTheme { get; set; }
}
