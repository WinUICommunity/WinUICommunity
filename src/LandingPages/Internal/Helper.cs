namespace WinUICommunity;
internal static class Helper
{
    internal static string GetLocalizedText(string input, bool usexUid, ILocalizer localizer)
    {
        if (usexUid)
        {
            return !string.IsNullOrEmpty(input) ? localizer.GetLocalizedString(input) : input;
        }
        else
        {
            return input;
        }
    }
}
