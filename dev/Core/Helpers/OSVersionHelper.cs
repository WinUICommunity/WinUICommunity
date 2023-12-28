namespace WinUICommunity;
public static class OSVersionHelper
{
    public static readonly Version OSVersion = GetOSVersion();

    /// <summary>
    /// Windows NT
    /// </summary>
    public static bool IsWindowsNT { get; } = Environment.OSVersion.Platform == PlatformID.Win32NT;

    /// <summary>
    /// Windows 10 Redstone5 Version 1809 Build 17763 (October 2018 Update)
    /// </summary>
    public static bool IsWindows10_1809 { get; } = IsWindowsNT && OSVersion == new Version(10, 0, 17763, OSVersion.Revision);

    /// <summary>
    /// Windows 10 Redstone5 Version 1809 Build 17763 Or Greater (October 2018 Update)
    /// </summary>
    public static bool IsWindows10_1809_OrGreater { get; } = IsWindowsNT && OSVersion >= new Version(10, 0, 17763, OSVersion.Revision);

    /// <summary>
    ///     Windows 11 Build 22000
    /// </summary>
    public static bool IsWindows11_22000 { get; } = IsWindowsNT && OSVersion == new Version(10, 0, 22000, OSVersion.Revision);

    /// <summary>
    ///     Windows 11 Build 22000 Or Greater
    /// </summary>
    public static bool IsWindows11_22000_OrGreater { get; } = IsWindowsNT && OSVersion >= new Version(10, 0, 22000, OSVersion.Revision);

    /// <summary>
    ///     Windows 11 Build 22621
    /// </summary>
    public static bool IsWindows11_22621 { get; } = IsWindowsNT && OSVersion == new Version(10, 0, 22621, OSVersion.Revision);

    /// <summary>
    ///     Windows 11 Build 22621 Or Greater
    /// </summary>
    public static bool IsWindows11_22621_OrGreater { get; } = IsWindowsNT && OSVersion >= new Version(10, 0, 22621, OSVersion.Revision);

    /// <summary>
    ///     Windows 11 Build 22631
    /// </summary>
    public static bool IsWindows11_22631 { get; } = IsWindowsNT && OSVersion == new Version(10, 0, 22631, OSVersion.Revision);

    /// <summary>
    ///     Windows 11 Build 22631 Or Greater
    /// </summary>
    public static bool IsWindows11_22631_OrGreater { get; } = IsWindowsNT && OSVersion >= new Version(10, 0, 22631, OSVersion.Revision);

    public static Version GetOSVersion()
    {
        var osv = new NativeValues.RTL_OSVERSIONINFOEX();
        NativeMethods.RtlGetVersion(out osv);
        return new Version((int)osv.dwMajorVersion, (int)osv.dwMinorVersion, (int)osv.dwBuildNumber, (int)osv.dwRevision);
    }

    public static bool IsEqualOrGreater(Version version)
    {
        return IsWindowsNT && OSVersion >= new Version(version.Major, version.Minor, version.Build, version.Revision);
    }
}
