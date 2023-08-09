namespace WinUICommunity;

public partial class Localizer
{
    public class Options
    {
        public string DefaultLanguage { get; set; } = "en-US";

        public bool UseUidWhenLocalizedStringNotFound { get; set; } = false;

        public bool DisableDefaultLocalizationActions { get; set; } = false;
    }
}