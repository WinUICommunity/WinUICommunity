namespace WinUICommunity;
public partial class ApplicationHelper
{
    /// <summary>
    /// <AssemblyVersion>1.0.0.0</AssemblyVersion>
    /// <FileVersion>1.0.0.0</FileVersion>
    /// <Version>1.0.0.0-xyz</Version>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="versionType"></param>
    /// <param name="assembly">Default is GetEntryAssembly</param>
    /// <returns></returns>
    public static string GetAppVersion(VersionType versionType, Assembly assembly)
    {
        return GetAppVersionBase(versionType, assembly);
    }

    /// <summary>
    /// <AssemblyVersion>1.0.0.0</AssemblyVersion>
    /// <FileVersion>1.0.0.0</FileVersion>
    /// <Version>1.0.0.0-xyz</Version>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="versionType"></param>
    /// <returns></returns>
    public static string GetAppVersion(VersionType versionType)
    {
        return GetAppVersionBase(versionType, null);
    }

    internal static string GetAppVersionBase(VersionType versionType, Assembly assembly = null)
    {
        if (assembly == null)
        {
            assembly = Assembly.GetEntryAssembly();
        }

        switch (versionType)
        {
            case VersionType.AssemblyVersion:
                return assembly.GetName().Version.ToString();
            case VersionType.AssemblyVersion2:
                return Application.Current.GetType().Assembly.GetName().Version.ToString();
            case VersionType.AssemblyFileVersionAttribute:
                return assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            case VersionType.AssemblyInformationalVersionAttribute:
                return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            default: return null;
        }
    }

    /// <summary>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <returns></returns>
    public static string GetAppName(NameType nameType)
    {
        return GetAppNameBase(nameType, null);
    }

    /// <summary>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static string GetAppName(NameType nameType, Assembly assembly)
    {
        return GetAppNameBase(nameType, assembly);
    }

    internal static string GetAppNameBase(NameType nameType, Assembly assembly = null)
    {
        if (assembly == null)
        {
            assembly = Assembly.GetEntryAssembly();
        }

        switch (nameType)
        {
            case NameType.AssemblyVersion:
                return assembly.GetName().Name;
            case NameType.AssemblyVersion2:
                return Application.Current.GetType().Assembly.GetName().Name;
            default: return null;
        }
    }


    /// <summary>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="versionType"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static (string Name, string Version, string NameAndVersion) GetAppNameAndVersion(NameType nameType, VersionType versionType, Assembly assembly)
    {
        return GetAppNameAndVersionBase(nameType, versionType, assembly);
    }

    /// <summary>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="versionType"></param>
    /// <returns></returns>
    public static (string Name, string Version, string NameAndVersion) GetAppNameAndVersion(NameType nameType, VersionType versionType)
    {
        return GetAppNameAndVersionBase(nameType, versionType, null);
    }

    internal static (string Name, string Version, string NameAndVersion) GetAppNameAndVersionBase(NameType nameType, VersionType versionType, Assembly assembly)
    {
        var name = GetAppName(nameType, assembly);
        var version = GetAppVersion(versionType, assembly);
        var nameAndVersion = $"{name} v{version}";
        return (name, version, nameAndVersion);
    }

    [Obsolete("Please Use ApplicationHelper.GetAppNameAndVersion method")]
    public static string GetProjectNameAndVersion()
    {
        return $"{GetProjectName()}V{GetProjectVersion()}";
    }

    [Obsolete("Please Use ApplicationHelper.GetAppName method")]
    public static string GetProjectName()
    {
        return Application.Current.GetType().Assembly.GetName().Name;
    }

    [Obsolete("Please Use ApplicationHelper.GetAppVersion method")]
    public static string GetProjectVersion()
    {
        return Application.Current.GetType().Assembly.GetName().Version.ToString();
    }
}

public enum VersionType
{
    AssemblyVersion = 0, // Assembly.GetEntryAssembly
    AssemblyVersion2 = 1, // Application.Current.GetType().Assembly
    AssemblyFileVersionAttribute = 2,
    AssemblyInformationalVersionAttribute = 3,
}

public enum NameType
{
    AssemblyVersion = 0, // Assembly.GetEntryAssembly
    AssemblyVersion2 = 1, // Application.Current.GetType().Assembly
}
