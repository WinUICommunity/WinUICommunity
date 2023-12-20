using WinRT.Interop;

namespace WinUICommunity;
public partial class WindowHelper
{
    /// <summary>
    /// Default is 900
    /// </summary>
    public static int MinWindowWidth { get; set; } = 900;
    /// <summary>
    /// Default is 1800
    /// </summary>
    public static int MaxWindowWidth { get; set; } = 1800;
    /// <summary>
    /// Default is 600
    /// </summary>
    public static int MinWindowHeight { get; set; } = 600;
    /// <summary>
    /// Default is 1600
    /// </summary>
    public static int MaxWindowHeight { get; set; } = 1600;

    private WndProcHelper wndProc;
    private Window Window;
    public WindowHelper(Window window)
    {
        this.Window = window;
    }

    public void RegisterWindowMinMax()
    {
        wndProc = new WndProcHelper(Window);
        wndProc.RegisterWndProc(WndProc);
    }

    private IntPtr WndProc(IntPtr hWnd, NativeValues.WindowMessage Msg, IntPtr wParam, IntPtr lParam)
    {
        switch (Msg)
        {
            case NativeValues.WindowMessage.WM_GETMINMAXINFO:
                var dpi = NativeMethods.GetDpiForWindow(hWnd);
                var scalingFactor = (float)dpi / 96;

                var minMaxInfo = Marshal.PtrToStructure<NativeValues.MINMAXINFO>(lParam);
                minMaxInfo.ptMinTrackSize.x = (int)(MinWindowWidth * scalingFactor);
                minMaxInfo.ptMaxTrackSize.x = (int)(MaxWindowWidth * scalingFactor);
                minMaxInfo.ptMinTrackSize.y = (int)(MinWindowHeight * scalingFactor);
                minMaxInfo.ptMaxTrackSize.y = (int)(MaxWindowHeight * scalingFactor);

                Marshal.StructureToPtr(minMaxInfo, lParam, true);
                break;

        }
        return wndProc.CallWindowProc(hWnd, Msg, wParam, lParam);
    }

    /// <summary>
    /// Set Window Width and Height
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void SetWindowSize(Window window, int width, int height)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        // Win32 uses pixels and WinUI 3 uses effective pixels, so you should apply the DPI scale factor
        var dpi = NativeMethods.GetDpiForWindow(hwnd);
        var scalingFactor = (float)dpi / 96;
        width = (int)(width * scalingFactor);
        height = (int)(height * scalingFactor);

        NativeMethods.SetWindowPos(hwnd, NativeValues.HWND_TOP_IntPtr, 0, 0, width, height, NativeValues.SetWindowPosFlags.SWP_NOMOVE);
    }

    private static void MoveAndResizeWindowBase(AppWindow appWindow, int width, int height, int x, int y)
    {
        var rightPosition = new Windows.Graphics.RectInt32
        {
            Height = height,
            Width = width,
            X = x,
            Y = y
        };

        appWindow.MoveAndResize(rightPosition);
    }
    public static void MoveAndResizeWindow(AppWindow appWindow, int width, int height, int x, int y)
    {
        MoveAndResizeWindowBase(appWindow, width, height, x, y);
    }
    public static void MoveAndResizeWindow(AppWindow appWindow, int width, int height)
    {
        var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Nearest);

        int screenWidth = displayArea.WorkArea.Width;
        int screenHeight = displayArea.WorkArea.Height;

        int positionX = (screenWidth - width) / 2;
        int positionY = (screenHeight - height) / 2;

        MoveAndResizeWindowBase(appWindow, width, height, positionX, positionY);
    }
}
