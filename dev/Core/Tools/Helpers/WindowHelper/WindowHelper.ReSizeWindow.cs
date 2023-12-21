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

    private WindowMessageMonitor monitor;
    private Window Window;
    public WindowHelper(Window window)
    {
        this.Window = window;
    }

    public void RegisterWindowMinMax()
    {
        monitor = new WindowMessageMonitor(Window);
        monitor.WindowMessageReceived -= Monitor_WindowMessageReceived;
        monitor.WindowMessageReceived += Monitor_WindowMessageReceived;
    }
    private void Monitor_WindowMessageReceived(object? sender, WindowMessageEventArgs e)
    {
        if (e.MessageType == NativeValues.WindowMessage.WM_GETMINMAXINFO)
        {
            var dpi = NativeMethods.GetDpiForWindow(WindowNative.GetWindowHandle(Window));
            var scalingFactor = (float)dpi / 96;

            var minMaxInfo = Marshal.PtrToStructure<NativeValues.MINMAXINFO>(e.Message.LParam);
            minMaxInfo.ptMinTrackSize.x = (int)(MinWindowWidth * scalingFactor);
            minMaxInfo.ptMaxTrackSize.x = (int)(MaxWindowWidth * scalingFactor);
            minMaxInfo.ptMinTrackSize.y = (int)(MinWindowHeight * scalingFactor);
            minMaxInfo.ptMaxTrackSize.y = (int)(MaxWindowHeight * scalingFactor);

            Marshal.StructureToPtr(minMaxInfo, e.Message.LParam, true);
        }
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

    public static void MoveAndResizeCenterScreen(Window window, int width, int height)
    {
        var displayArea = DisplayArea.GetFromWindowId(window.AppWindow.Id, DisplayAreaFallback.Nearest);

        int screenWidth = displayArea.WorkArea.Width;
        int screenHeight = displayArea.WorkArea.Height;

        int positionX = (screenWidth - width) / 2;
        int positionY = (screenHeight - height) / 2;

        MoveAndResize(window, width, height, positionX, positionY);
    }

    public static void MoveAndResize(Window window, double x, double y, double width, double height)
    {
        var scale = NativeMethods.GetDpiForWindow(WindowNative.GetWindowHandle(window)) / 96f;
        window.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32((int)x, (int)y, (int)(width * scale), (int)(height * scale)));
    }

    public static void Move(Window window, int x, int y)
    {
        window.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(x, y, window.AppWindow.Size.Width, window.AppWindow.Size.Height));
    }
}
