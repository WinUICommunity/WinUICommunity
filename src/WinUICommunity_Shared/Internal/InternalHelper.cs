using System.Runtime.InteropServices;
using System.Text;

namespace WinUICommunity;
internal class InternalHelper
{
    [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
    internal static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, [Optional] StringBuilder packageFullName);
    private const uint APPMODEL_ERROR_NO_PACKAGE = 15700;

    internal static bool IsPackaged { get; } = GetCurrentPackageName() != null;

    internal static string GetCurrentPackageName()
    {
        var length = 0u;
        GetCurrentPackageFullName(ref length);

        var result = new StringBuilder((int)length);
        var error = GetCurrentPackageFullName(ref length, result);

        return error == APPMODEL_ERROR_NO_PACKAGE ? null : result.ToString();
    }
}
