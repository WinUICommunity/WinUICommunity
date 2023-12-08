using WinRT.Interop;

namespace WindowUI;
public class WndProcHelper
{
    private IntPtr Handle { get; set; }
    private NativeValues.WNDPROC newMainWindowWndProc = null;
    private IntPtr oldMainWindowWndProc = IntPtr.Zero;

    private NativeValues.WNDPROC newInputNonClientPointerSourceWndProc = null;
    private IntPtr oldInputNonClientPointerSourceWndProc = IntPtr.Zero;

    public WndProcHelper(Window window)
    {
        Handle = WindowNative.GetWindowHandle(window);
    }
    
    public IntPtr CallWindowProc(IntPtr hWnd, NativeValues.WindowMessage Msg, IntPtr wParam, IntPtr lParam)
    {
        return NativeMethods.CallWindowProc(oldMainWindowWndProc, hWnd, Msg, wParam, lParam);
    }

    public IntPtr CallInputNonClientPointerSourceWindowProc(IntPtr hWnd, NativeValues.WindowMessage Msg, IntPtr wParam, IntPtr lParam)
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
        IntPtr inputNonClientPointerSourceHandle = NativeMethods.FindWindowEx(Handle, IntPtr.Zero, "InputNonClientPointerSource", null);

        if (inputNonClientPointerSourceHandle != IntPtr.Zero)
        {
            int style = NativeMethods.GetWindowLongAuto(Handle, (int)NativeValues.WindowLongIndexFlags.GWL_STYLE);
            NativeMethods.SetWindowLongAuto(Handle, (int)NativeValues.WindowLongIndexFlags.GWL_STYLE, (IntPtr)(style & ~(int) NativeValues.WindowStyle.WS_SYSMENU));

            newInputNonClientPointerSourceWndProc = wndProc;
            oldInputNonClientPointerSourceWndProc = NativeMethods.SetWindowLongAuto(inputNonClientPointerSourceHandle, (int)NativeValues.WindowLongIndexFlags.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newInputNonClientPointerSourceWndProc));
        }
    }
}
