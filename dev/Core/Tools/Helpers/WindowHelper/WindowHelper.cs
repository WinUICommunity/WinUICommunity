using WinRT.Interop;

namespace WinUICommunity;

public partial class WindowHelper
{
    public static void SwitchToThisWindow(object target)
    {
        if (target != null)
        {
            NativeMethods.SwitchToThisWindow(WindowNative.GetWindowHandle(target), true);
        }
    }

    public static void ReActivateWindow(Window window)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        var activeWindow = NativeMethods.GetActiveWindow();
        if (hwnd == activeWindow)
        {
            NativeMethods.SendMessage(hwnd, NativeValues.WM_ACTIVATE, NativeValues.WA_INACTIVE, IntPtr.Zero);
            NativeMethods.SendMessage(hwnd, NativeValues.WM_ACTIVATE, NativeValues.WA_ACTIVE, IntPtr.Zero);
        }
        else
        {
            NativeMethods.SendMessage(hwnd, NativeValues.WM_ACTIVATE, NativeValues.WA_ACTIVE, IntPtr.Zero);
            NativeMethods.SendMessage(hwnd, NativeValues.WM_ACTIVATE, NativeValues.WA_INACTIVE, IntPtr.Zero);
        }
    }

    public static (Window window, Frame rootFrame) CreateWindowWithFrame()
    {
        var window = new Window();

        if (window.Content is not Frame frame)
        {
            window.Content = frame = new Frame();
            return (window, frame);
        }
        return (null, null);
    }
}
