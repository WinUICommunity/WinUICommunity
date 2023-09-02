using System.Web;

namespace WinUICommunity;
public static partial class ApplicationHelper
{
    public static void EnableSound(ElementSoundPlayerState elementSoundPlayerState = ElementSoundPlayerState.On, bool withSpatial = false)
    {
        ElementSoundPlayer.State = elementSoundPlayerState;

        ElementSoundPlayer.SpatialAudioMode = !withSpatial ? ElementSpatialAudioMode.Off : ElementSpatialAudioMode.On;
    }

    public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
    {
        return !typeof(TEnum).GetTypeInfo().IsEnum
            ? throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.")
            : (TEnum)Enum.Parse(typeof(TEnum), text);
    }

    public static int GetThemeIndex(ElementTheme elementTheme)
    {
        return elementTheme switch
        {
            ElementTheme.Default => 0,
            ElementTheme.Light => 1,
            ElementTheme.Dark => 2,
            _ => 0,
        };
    }

    public static ElementTheme GetElementThemeEnum(int themeIndex)
    {
        return themeIndex switch
        {
            0 => ElementTheme.Default,
            1 => ElementTheme.Light,
            2 => ElementTheme.Dark,
            _ => ElementTheme.Default,
        };
    }

    public static bool IsNetworkAvailable()
    {
        return NetworkInformation.GetInternetConnectionProfile()?.NetworkAdapter != null;
    }

    public static Geometry GetGeometry(string key)
    {
        return (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), (string)Application.Current.Resources[key]);
    }

    public static Color GetColorFromHex(string hexaColor)
    {
        return
            Color.FromArgb(
              Convert.ToByte(hexaColor.Substring(1, 2), 16),
                Convert.ToByte(hexaColor.Substring(3, 2), 16),
                Convert.ToByte(hexaColor.Substring(5, 2), 16),
                Convert.ToByte(hexaColor.Substring(7, 2), 16)
            );
    }

    /// <summary>
    /// Get Glyph string
    /// </summary>
    /// <param name="key">Example: EA6A</param>
    /// <returns></returns>
    public static string GetGlyph(string key)
    {
        int codePoint = int.Parse(key, System.Globalization.NumberStyles.HexNumber);
        return char.ConvertFromUtf32(codePoint);
    }

    public static double GetScaleAdjustment(Window window)
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        var result = NativeMethods.GetDpiForMonitor(hMonitor, NativeMethods.Monitor_DPI_Type.MDT_Default, out var dpiX, out var _);
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    public static void SetApplicationLayoutRTL(Window window)
    {
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        int exstyle = NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE, exstyle | NativeMethods.WS_EX_LAYOUTRTL);
    }

    public static void SetApplicationLayoutLTR(Window window)
    {
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        int exstyle = NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE, exstyle | NativeMethods.WS_EX_LAYOUTLTR);
    }

    public static string GetDecodedStringFromHtml(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var decoded = HttpUtility.HtmlDecode(text);
        var result = decoded != text;
        return result ? decoded : text;
    }

    public static string GetFileSize(long size)
    {
        string[] sizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        const string formatTemplate = "{0}{1:0.#} {2}";

        if (size == 0)
        {
            return string.Format(formatTemplate, null, 0, sizeSuffixes[0]);
        }

        var absSize = Math.Abs((double)size);
        var fpPower = Math.Log(absSize, 1000);
        var intPower = (int)fpPower;
        var iUnit = intPower >= sizeSuffixes.Length
            ? sizeSuffixes.Length - 1
            : intPower;
        var normSize = absSize / Math.Pow(1000, iUnit);

        return string.Format(
            formatTemplate,
            size < 0 ? "-" : null, normSize, sizeSuffixes[iUnit]);
    }
}

