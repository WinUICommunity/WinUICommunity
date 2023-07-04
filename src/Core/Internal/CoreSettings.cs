using Nucs.JsonSettings;
using Nucs.JsonSettings.Modulation;

namespace WinUICommunity;
internal class CoreSettings : JsonSettings, IVersionable
{
    [EnforcedVersion("4.0.0.1")]
    public Version Version { get; set; } = new Version(4, 0, 0, 1);
    public override string FileName { get; set; }
    public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;
    public BackdropType BackdropType { get; set; } = BackdropType.None;
    public bool IsFirstRun { get; set; } = true;
}
