using WinRT.Interop;

namespace WinUICommunity;
public class WndProcHelper
{
    private nint Handle { get; set; }
    private NativeValues.WNDPROC newMainWindowWndProc = null;
    private nint oldMainWindowWndProc = IntPtr.Zero;

    private NativeValues.WNDPROC newInputNonClientPointerSourceWndProc = null;
    private nint oldInputNonClientPointerSourceWndProc = IntPtr.Zero;

    public WndProcHelper(Window window)
    {
        Handle = WindowNative.GetWindowHandle(window);
    }

    public WndProcHelper(nint hwnd)
    {
        Handle = hwnd;
    }

    public nint CallWindowProc(nint hWnd, NativeValues.WindowMessage Msg, nint wParam, nint lParam)
    {
        return NativeMethods.CallWindowProc(oldMainWindowWndProc, hWnd, Msg, wParam, lParam);
    }

    public nint CallInputNonClientPointerSourceWindowProc(nint hWnd, NativeValues.WindowMessage Msg, nint wParam, nint lParam)
    {
        return NativeMethods.CallWindowProc(oldInputNonClientPointerSourceWndProc, hWnd, Msg, wParam, lParam);
    }
    public void RegisterWndProc(NativeValues.WNDPROC wndProc)
    {
        newMainWindowWndProc = wndProc;
        oldMainWindowWndProc = NativeMethods.SetWindowLongAuto(Handle, (int)NativeValues.WindowLongIndexFlags.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newMainWindowWndProc));
    }

    public void RegisterInputNonClientPointerSourceWndProc(NativeValues.WNDPROC wndProc)
    {
        var inputNonClientPointerSourceHandle = NativeMethods.FindWindowEx(Handle, IntPtr.Zero, "InputNonClientPointerSource", null);

        if (inputNonClientPointerSourceHandle != IntPtr.Zero)
        {
            var style = NativeMethods.GetWindowLongAuto(Handle, (int)NativeValues.WindowLongIndexFlags.GWL_STYLE);
            NativeMethods.SetWindowLongAuto(Handle, (int)NativeValues.WindowLongIndexFlags.GWL_STYLE, (IntPtr)(style & ~(int)NativeValues.WindowStyle.WS_SYSMENU));

            newInputNonClientPointerSourceWndProc = wndProc;
            oldInputNonClientPointerSourceWndProc = NativeMethods.SetWindowLongAuto(inputNonClientPointerSourceHandle, (int)NativeValues.WindowLongIndexFlags.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newInputNonClientPointerSourceWndProc));
        }
    }
}
