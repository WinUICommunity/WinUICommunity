namespace WinUICommunity;
public static class VersionHelper
{
    /// <summary>
    /// <AssemblyVersion>1.0.0.0</AssemblyVersion>
    /// </summary>
    /// <returns></returns>
    public static string GetAssemblyVersion()
    {
        return Assembly.GetEntryAssembly().GetName().Version.ToString();
    }

    /// <summary>
    /// <FileVersion>1.0.0.0</FileVersion>
    /// </summary>
    /// <returns></returns>
    public static string GetFileVersion()
    {
        return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
    }

    /// <summary>
    /// <Version>1.0.0.0-xyz</Version>
    /// </summary>
    /// <returns></returns>
    public static string GetVersion()
    {
        return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
