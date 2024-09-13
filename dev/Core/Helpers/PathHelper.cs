namespace WinUICommunity;
public partial class PathHelper
{
    private static readonly int MAX_PATH = 255;

    public static async Task<string> GetFilePath(string filePath, PathType pathType = PathType.Relative)
    {
        var file = await FileHelper.GetStorageFile(filePath, pathType);

        return file.Path;
    }

    /// <summary>
    /// Return Full Path to Assets folder even if your app is SelfContained/SingleFile
    /// Normal Mode: App/bin/Assets
    /// SelfContained/SingleFile: Temp/net/xx/Assets
    /// </summary>
    /// <returns></returns>
    public static string GetFullPathToAsset(string assetName)
    {
        return Path.Combine(AppContext.BaseDirectory, "Assets", assetName);
    }

    /// <summary>
    /// Return Full Path to Exe even if your app is SelfContained/SingleFile
    /// Normal Mode: App/bin/App.exe
    /// SelfContained/SingleFile: Temp/net/xx/App.exe
    /// </summary>
    /// <returns></returns>
    public static string GetFullPathToExe()
    {
        return Path.Combine(AppContext.BaseDirectory, ProcessInfoHelper.GetProcess().MainModule.ModuleName);
    }

    /// <summary>
    /// Return ApplicationData Folder Path for Packaged and UnPackaged Mode Automatically
    /// </summary>
    /// <param name="forceUnpackagedMode"></param>
    /// <returns></returns>
    public static string GetAppDataFolderPath(bool forceUnpackagedMode = false)
    {
        var unpackaged = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if (forceUnpackagedMode)
            return unpackaged;

        return PackageHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : unpackaged;
    }

    /// <summary>
    /// Use P/Invoke for getting Full Path To Exe
    /// Normal Mode: App/bin/App.exe
    /// SelfContained/SingleFile: App/bin/App.exe
    /// </summary>
    /// <returns></returns>
    public static string GetExecutablePathNative()
    {
        var sb = new StringBuilder(MAX_PATH);
        NativeMethods.GetModuleFileName(IntPtr.Zero, sb, MAX_PATH);
        return sb.ToString();
    }
}
