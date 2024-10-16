﻿using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
internal static partial class Helper
{
    internal static string GetLocalizedText(string input, bool usexUid, ResourceManager resourceManager, ResourceContext resourceContext)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        try
        {
            if (usexUid)
            {
                if (resourceManager != null && resourceContext != null)
                {
                    var candidate = resourceManager.MainResourceMap.TryGetValue($"Resources/{input}", resourceContext);
                    return candidate != null ? candidate.ValueAsString : input;
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
