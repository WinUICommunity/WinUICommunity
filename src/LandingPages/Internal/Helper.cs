using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
internal static class Helper
{
    internal static string GetLocalizedText(string input, bool usexUid, ILocalizer localizer, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        try
        {
            if (usexUid)
            {
                if (localizer == null && resourceManager != null && resourceContext != null)
                {
                    var candidate = resourceManager.MainResourceMap.TryGetValue($"Resources/{input}", resourceContext);
                    return candidate != null ? candidate.ValueAsString : input;
                }
                else if (resourceManager == null && localizer != null)
                {
                    return localizer.GetLocalizedString(input);
                }
            }
        }
        catch (Exception)
        {
            return input;
        }

        return input;
    }
}
