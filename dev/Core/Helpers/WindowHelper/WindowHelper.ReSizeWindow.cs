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
        monitor = new WindowMessageMonitor(this.Window);
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

    public static void SetWindowSize(AppWindow appWindow, int width, int height)
    {
        appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
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

    public static void Move(AppWindow appWindow, int x, int y)
    {
        appWindow.MoveAndResize(new Windows.Graphics.RectInt32(x, y, appWindow.Size.Width, appWindow.Size.Height));
    }
}
