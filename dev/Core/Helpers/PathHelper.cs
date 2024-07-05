namespace WinUICommunity;
public partial class PathHelper
{
    private static readonly int MAX_PATH = 255;

    public static async Task<string> GetFilePath(string filePath, PathType pathType = PathType.Relative)
    {
        var file = await FileHelper.GetStorageFile(filePath, pathType);

        return file.Path;
    }

    public static string GetFullPathToExe()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var pos = path.LastIndexOf("\\");
        return path.Substring(0, pos);
    }

    public static string GetFullPathToAsset(string assetName)
    {
        return GetFullPathToExe() + "\\Assets\\" + assetName;
    }

    /// <summary>
    /// Return ApplicationData Folder Path for Packaged and UnPackaged Mode Automatically
    /// </summary>
    /// <param name="forceUnpackagedMode"></param>
    /// <returns></returns>
    public static string GetApplicationDataFolderPath(bool forceUnpackagedMode = false)
    {
        var unpackaged = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if (forceUnpackagedMode)
            return unpackaged;

        return PackageHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : unpackaged;
    }

    public static string GetExecutablePathNative()
    {
        var sb = new StringBuilder(MAX_PATH);
        NativeMethods.GetModuleFileName(IntPtr.Zero, sb, MAX_PATH);
        return sb.ToString();
    }
}
