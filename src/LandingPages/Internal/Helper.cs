namespace WinUICommunity;
internal static class Helper
{
    internal static string GetLocalizedText(string input, bool usexUid, ILocalizer localizer)
    {
        if (usexUid)
        {
            return localizer.GetLocalizedString(input);
        }
        else
        {
            return input;
        }
    }
}
