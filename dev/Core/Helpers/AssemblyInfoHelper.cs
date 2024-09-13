namespace WinUICommunity;
public partial class AssemblyInfoHelper
{
    internal static T GetAssemblyCustomAttribute<T>(Assembly assembly) where T : Attribute
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

    public static T GetCurrentAssemblyCustomAttribute<T>() where T : Attribute
    {
        return GetAssemblyCustomAttribute<T>(null);
    }

    public static T GetCurrentAssemblyCustomAttribute<T>(Assembly assembly) where T : Attribute
    {
        return GetAssemblyCustomAttribute<T>(assembly);
    }

    internal static string GetVersionInfoBase(VersionType versionType, Assembly assembly)
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
    public static string GetVersion(VersionType versionType = VersionType.AssemblyInformationalVersion)
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
    public static string GetVersion(VersionType versionType, Assembly assembly)
    {
        return GetVersionInfoBase(versionType, assembly);
    }

    internal static string GetAppNameInfoBase(NameType nameType, Assembly assembly)
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
    public static string GetAppName(NameType nameType = NameType.EntryAssemblyVersion)
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
    public static string GetAppName(NameType nameType, Assembly assembly)
    {
        return GetAppNameInfoBase(nameType, assembly);
    }

    internal static (string Name, string Version, string NameAndVersion) GetAppInfoBase(NameType nameType, VersionType versionType, Assembly assembly)
    {
        var name = GetAppName(nameType, assembly);
        var version = GetVersion(versionType, assembly);
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
    public static (string Name, string Version, string NameAndVersion) GetAppDetails(NameType nameType = NameType.EntryAssemblyVersion, VersionType versionType = VersionType.AssemblyInformationalVersion)
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
    public static (string Name, string Version, string NameAndVersion) GetAppDetails(NameType nameType, VersionType versionType, Assembly assembly)
    {
        return GetAppInfoBase(nameType, versionType, assembly);
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
