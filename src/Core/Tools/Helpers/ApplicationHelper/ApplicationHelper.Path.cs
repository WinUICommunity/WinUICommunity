namespace WinUICommunity;
public static partial class ApplicationHelper
{
    private static readonly int MAX_PATH = 255;
    private const uint APPMODEL_ERROR_NO_PACKAGE = 15700;
    public static bool IsPackaged { get; } = GetCurrentPackageName() != null;

    public static string GetCurrentPackageName()
    {
        var length = 0u;
        NativeMethods.GetCurrentPackageFullName(ref length);

        var result = new StringBuilder((int)length);
        var error = NativeMethods.GetCurrentPackageFullName(ref length, result);

        return error == APPMODEL_ERROR_NO_PACKAGE ? null : result.ToString();
    }

    public static Windows.ApplicationModel.Package GetPackageDetails()
    {
        return Windows.ApplicationModel.Package.Current;
    }
    public static Windows.ApplicationModel.PackageVersion GetPackageVersion()
    {
        return GetPackageDetails().Id.Version;
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

    public static string GetProjectNameAndVersion()
    {
        return $"{GetProjectName()}V{GetProjectVersion()}";
    }

    public static string GetProjectName()
    {
        return Application.Current.GetType().Assembly.GetName().Name;
    }

    public static string GetProjectVersion()
    {
        return Application.Current.GetType().Assembly.GetName().Version.ToString();
    }

    public static string GetLocalFolderPath()
    {
        return IsPackaged ? ApplicationData.Current.LocalFolder.Path : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }

    public static string GetExecutablePathNative()
    {
        var sb = new StringBuilder(MAX_PATH);
        NativeMethods.GetModuleFileName(IntPtr.Zero, sb, MAX_PATH);
        return sb.ToString();
    }
}
