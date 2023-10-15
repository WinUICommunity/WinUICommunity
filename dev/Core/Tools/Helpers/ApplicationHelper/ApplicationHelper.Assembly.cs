namespace WinUICommunity;
public partial class ApplicationHelper
{
    internal static T GetCustomAttributeBase<T>(Assembly assembly) where T : Attribute
    {
        if (assembly == null)
        {
            assembly = Assembly.GetEntryAssembly();
        }
        var attr = assembly.GetCustomAttribute<T>();
        if (attr != null)
        {
            return attr;
        }
        else
        {
            // Throw an exception or return a default value
            throw new ArgumentException("Attribute not found");
        }
    }

    public static T GetCustomAttribute<T>() where T : Attribute
    {
        return GetCustomAttributeBase<T>(null);
    }

    public static T GetCustomAttribute<T>(Assembly assembly) where T : Attribute
    {
        return GetCustomAttributeBase<T>(assembly);
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
            case VersionType.AssemblyFileVersion:
                return assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            case VersionType.AssemblyInformationalVersion:
                return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            default: return null;
        }
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
    public static string GetAppVersion(VersionType versionType = VersionType.AssemblyInformationalVersion)
    {
        return GetAppVersionBase(versionType, null);
    }

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
    /// <returns></returns>
    public static string GetAppName(NameType nameType = NameType.AssemblyVersion2)
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
    public static string GetAppName(NameType nameType = NameType.AssemblyVersion2, Assembly assembly = null)
    {
        return GetAppNameBase(nameType, assembly);
    }

    internal static (string Name, string Version, string NameAndVersion) GetAppNameAndVersionBase(NameType nameType, VersionType versionType, Assembly assembly)
    {
        var name = GetAppName(nameType, assembly);
        var version = GetAppVersion(versionType, assembly);
        var nameAndVersion = $"{name} v{version}";
        return (name, version, nameAndVersion);
    }

    /// <summary>
    /// AssemblyVersion: Assembly.GetEntryAssembly
    /// AssemblyVersion2: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="versionType"></param>
    /// <returns></returns>
    public static (string Name, string Version, string NameAndVersion) GetAppNameAndVersion(NameType nameType = NameType.AssemblyVersion2, VersionType versionType = VersionType.AssemblyInformationalVersion)
    {
        return GetAppNameAndVersionBase(nameType, versionType, null);
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
    AssemblyFileVersion = 2,
    AssemblyInformationalVersion = 3,
}

public enum NameType
{
    AssemblyVersion = 0, // Assembly.GetEntryAssembly
    AssemblyVersion2 = 1, // Application.Current.GetType().Assembly
}
