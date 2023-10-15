using Nucs.JsonSettings;
using Nucs.JsonSettings.Modulation;

namespace WinUICommunity;
internal class CoreSettings : JsonSettings, IVersionable
{
    [EnforcedVersion("5.3.0.0")]
    public Version Version { get; set; } = new Version(5, 3, 0, 0);
    public override string FileName { get; set; }
    public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;
    public BackdropType BackdropType { get; set; } = BackdropType.None;

    public bool IsThemeFirstRun { get; set; } = true;
    public bool IsBackdropFirstRun { get; set; } = true;
}
