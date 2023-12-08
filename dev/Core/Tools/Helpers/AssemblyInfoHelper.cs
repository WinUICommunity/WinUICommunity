namespace WindowUI;
public partial class AssemblyInfoHelper
{
    internal static T GetCustomAttributeFromAssembly<T>(Assembly assembly) where T : Attribute
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
        return GetCustomAttributeFromAssembly<T>(null);
    }

    public static T GetCustomAttribute<T>(Assembly assembly) where T : Attribute
    {
        return GetCustomAttributeFromAssembly<T>(assembly);
    }

    internal static string GetVersionInfoBase(VersionType versionType, Assembly assembly = null)
    {
        if (assembly == null)
        {
            assembly = Assembly.GetEntryAssembly();
        }

        switch (versionType)
        {
            case VersionType.EntryAssemblyVersion:
                return assembly.GetName().Version.ToString();
            case VersionType.CurrentAssemblyVersion:
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
    /// EntryAssemblyVersion: Assembly.GetEntryAssembly
    /// CurrentAssemblyVersion: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="versionType"></param>
    /// <returns></returns>
    public static string GetAssemblyVersion(VersionType versionType = VersionType.AssemblyInformationalVersion)
    {
        return GetVersionInfoBase(versionType, null);
    }

    /// <summary>
    /// <AssemblyVersion>1.0.0.0</AssemblyVersion>
    /// <FileVersion>1.0.0.0</FileVersion>
    /// <Version>1.0.0.0-xyz</Version>
    /// EntryAssemblyVersion: Assembly.GetEntryAssembly
    /// CurrentAssemblyVersion: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="versionType"></param>
    /// <param name="assembly">Default is GetEntryAssembly</param>
    /// <returns></returns>
    public static string GetAssemblyVersion(VersionType versionType, Assembly assembly)
    {
        return GetVersionInfoBase(versionType, assembly);
    }

    internal static string GetAppNameInfoBase(NameType nameType, Assembly assembly = null)
    {
        if (assembly == null)
        {
            assembly = Assembly.GetEntryAssembly();
        }

        switch (nameType)
        {
            case NameType.EntryAssemblyVersion:
                return assembly.GetName().Name;
            case NameType.CurrentAssemblyVersion:
                return Application.Current.GetType().Assembly.GetName().Name;
            default: return null;
        }
    }

    /// <summary>
    /// EntryAssemblyVersion: Assembly.GetEntryAssembly
    /// CurrentAssemblyVersion: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <returns></returns>
    public static string GetAppName(NameType nameType = NameType.CurrentAssemblyVersion)
    {
        return GetAppNameInfoBase(nameType, null);
    }

    /// <summary>
    /// EntryAssemblyVersion: Assembly.GetEntryAssembly
    /// CurrentAssemblyVersion: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static string GetAppName(NameType nameType = NameType.CurrentAssemblyVersion, Assembly assembly = null)
    {
        return GetAppNameInfoBase(nameType, assembly);
    }

    internal static (string Name, string Version, string NameAndVersion) GetAppInfoBase(NameType nameType, VersionType versionType, Assembly assembly)
    {
        var name = GetAppName(nameType, assembly);
        var version = GetAssemblyVersion(versionType, assembly);
        var nameAndVersion = $"{name} v{version}";
        return (name, version, nameAndVersion);
    }

    /// <summary>
    /// EntryAssemblyVersion: Assembly.GetEntryAssembly
    /// CurrentAssemblyVersion: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="versionType"></param>
    /// <returns></returns>
    public static (string Name, string Version, string NameAndVersion) GetAppInfo(NameType nameType = NameType.CurrentAssemblyVersion, VersionType versionType = VersionType.AssemblyInformationalVersion)
    {
        return GetAppInfoBase(nameType, versionType, null);
    }

    /// <summary>
    /// EntryAssemblyVersion: Assembly.GetEntryAssembly
    /// CurrentAssemblyVersion: Application.Current.GetType().Assembly
    /// </summary>
    /// <param name="nameType"></param>
    /// <param name="versionType"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static (string Name, string Version, string NameAndVersion) GetAppInfo(NameType nameType, VersionType versionType, Assembly assembly)
    {
        return GetAppInfoBase(nameType, versionType, assembly);
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
    EntryAssemblyVersion = 0, // Assembly.GetEntryAssembly
    CurrentAssemblyVersion = 1, // Application.Current.GetType().Assembly
    AssemblyFileVersion = 2,
    AssemblyInformationalVersion = 3,
}

public enum NameType
{
    EntryAssemblyVersion = 0, // Assembly.GetEntryAssembly
    CurrentAssemblyVersion = 1, // Application.Current.GetType().Assembly
}
