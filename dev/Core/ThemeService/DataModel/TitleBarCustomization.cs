namespace WinUICommunity;

[Obsolete("This Class will be removed after WASDK v1.6 released")]
public class TitleBarCustomization
{
    public TitleBarWindowType TitleBarWindowType { get; set; } = TitleBarWindowType.None;

    public TitleBarButtons LightTitleBarButtons { get; set; }
    public TitleBarButtons DarkTitleBarButtons { get; set; }
}
