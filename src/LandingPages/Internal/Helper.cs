using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
internal static class Helper
{
    internal static string GetLocalizedText(string input, bool usexUid, ILocalizer localizer, ResourceLoader resourceLoader)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        if (usexUid)
        {
            if (localizer == null && resourceLoader != null)
            {
                return resourceLoader.GetString(input);
            }
            else if (resourceLoader == null && localizer != null)
            {
                return localizer.GetLocalizedString(input);
            }
        }

        return input;
    }
}
