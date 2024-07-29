using System.Diagnostics;

namespace WinUICommunity;
public static class ProcessInfoHelper
{
    private static readonly FileVersionInfo _fileVersionInfo;
    static ProcessInfoHelper()
    {
        using var process = Process.GetCurrentProcess();
        _fileVersionInfo = process.MainModule.FileVersionInfo;
    }
    public static string GetVersionString => GetVersion()?.ToString();
    public static Version GetVersion()
    {
        return new Version(_fileVersionInfo.FileMajorPart, _fileVersionInfo.FileMinorPart, _fileVersionInfo.FileBuildPart, _fileVersionInfo.FilePrivatePart);
    }
    public static FileVersionInfo GetFileVersionInfo()
    {
        return _fileVersionInfo;
    }

    public static string GetProductName()
    {
        return _fileVersionInfo.ProductName;
    }

    public static string GetProductNameAndVersion()
    {
        return $"{_fileVersionInfo.ProductName} v{GetVersion().ToString()}";
    }
}
