using Nucs.JsonSettings;
using Nucs.JsonSettings.Modulation;

namespace WinUICommunity;
internal class CoreSettings : JsonSettings, IVersionable
{
    [EnforcedVersion("7.0.0.0")]
    public Version Version { get; set; } = new Version(7, 0, 0, 0);
    public override string FileName { get; set; }
    public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;
    public BackdropType BackdropType { get; set; } = BackdropType.None;
    public Color BackdropTintColor { get; set; }
    public Color BackdropFallBackColor { get; set; }

    public bool IsThemeFirstRun { get; set; } = true;
    public bool IsBackdropFirstRun { get; set; } = true;
    public bool IsBackdropTintColorFirstRun { get; set; } = true;
    public bool IsBackdropFallBackColorFirstRun { get; set; } = true;
}
