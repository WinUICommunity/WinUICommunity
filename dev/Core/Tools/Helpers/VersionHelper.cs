using Windows.Foundation.Metadata;

namespace WinUICommunity;

[Deprecated("Please Use ApplicationHelper Class", DeprecationType.Deprecate, 5)]
public static class VersionHelper
{
    [Obsolete("Please Use ApplicationHelper.GetAppVersion method")]
    /// <summary>
    /// <AssemblyVersion>1.0.0.0</AssemblyVersion>
    /// </summary>
    /// <returns></returns>
    public static string GetAssemblyVersion()
    {
        return Assembly.GetEntryAssembly().GetName().Version.ToString();
    }

    [Obsolete("Please Use ApplicationHelper.GetAppVersion method")]
    /// <summary>
    /// <FileVersion>1.0.0.0</FileVersion>
    /// </summary>
    /// <returns></returns>
    public static string GetFileVersion()
    {
        return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
    }

    [Obsolete("Please Use ApplicationHelper.GetAppVersion method")]
    /// <summary>
    /// <Version>1.0.0.0-xyz</Version>
    /// </summary>
    /// <returns></returns>
    public static string GetVersion()
    {
        return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
