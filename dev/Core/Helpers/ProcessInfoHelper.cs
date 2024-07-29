using System.Diagnostics;

namespace WinUICommunity;
public static class ProcessInfoHelper
{
    public static string GetVersionString => GetVersion()?.ToString();
    public static Version GetVersion()
    {
        using var process = Process.GetCurrentProcess();
        var ver = process.MainModule.FileVersionInfo;
        var version = new Version(ver.FileMajorPart, ver.FileMinorPart, ver.FileBuildPart, ver.FilePrivatePart);
        return version;
    }
    public static FileVersionInfo GetFileVersionInfo()
    {
        using var process = Process.GetCurrentProcess();
        return process.MainModule.FileVersionInfo;
    }

    public static string GetProductName()
    {
        using var process = Process.GetCurrentProcess();
        return process.MainModule.FileVersionInfo.ProductName;
    }
}
