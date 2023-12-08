using System.Runtime.Versioning;

namespace WindowUI;
public static partial class NativeMethods
{
    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "FindWindowExW", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern IntPtr GetActiveWindow();

    [DllImport(NativeValues.ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

    [DllImport(NativeValues.ExternDll.Kernel32, CharSet = CharSet.Unicode)]
    public static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

    [DllImport(NativeValues.ExternDll.Kernel32, CharSet = CharSet.Unicode)]
    public static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

    [DllImport(NativeValues.ExternDll.UxTheme, EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern void FlushMenuThemes();

    [DllImport(NativeValues.ExternDll.UxTheme, EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int SetPreferredAppMode(NativeValues.PreferredAppMode preferredAppMode);

    [DllImport(NativeValues.ExternDll.User32, SetLastError = true)]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);

    [DllImport(NativeValues.ExternDll.NTdll)]
    public static extern int RtlGetVersion(out NativeValues.RTL_OSVERSIONINFOEX lpVersionInformation);

    [DllImport(NativeValues.ExternDll.Kernel32)]
    public static extern uint GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

    [DllImport("CoreMessaging.dll")]
    public static extern int CreateDispatcherQueueController([In] NativeValues.DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern int GetDpiForWindow(IntPtr hwnd);

    [DllImport(NativeValues.ExternDll.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, NativeValues.SetWindowPosFlags uFlags);

    [DllImport("Shcore.dll", SetLastError = true)]
    public static extern int GetDpiForMonitor(IntPtr hmonitor, NativeValues.Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    [DllImport(NativeValues.ExternDll.Kernel32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, [Optional] StringBuilder packageFullName);


    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, NativeValues.WNDPROC newProc);

    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, NativeValues.WNDPROC newProc);

    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, NativeValues.WNDPROC newProc)
    {
        return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, newProc) : new IntPtr(SetWindowLong32(hWnd, nIndex, newProc));
    }

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "SetWindowLongW", SetLastError = false)]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "SetWindowLongPtrW", SetLastError = false)]
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    public static IntPtr SetWindowLongAuto(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size is 8)
        {
            return SetWindowLongPtr(hWnd, nIndex, dwNewLong);
        }
        else
        {
            return SetWindowLong(hWnd, nIndex, dwNewLong);
        }
    }


    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "GetWindowLongW", SetLastError = false)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport(NativeValues.ExternDll.User32, EntryPoint = "GetWindowLongPtrW", SetLastError = false)]
    public static extern int GetWindowLongPtr(IntPtr hWnd, int nIndex);

    public static int GetWindowLongAuto(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size is 8)
        {
            return GetWindowLongPtr(hWnd, nIndex);
        }
        else
        {
            return GetWindowLong(hWnd, nIndex);
        }
    }

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, NativeValues.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    [DllImport(NativeValues.ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern bool GetCursorPos(out NativeValues.POINT pt);

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern bool EnableMenuItem(IntPtr hMenu, int UIDEnabledItem, int uEnable);

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIDNewItem, string lpNewItem);

    [DllImport(NativeValues.ExternDll.User32, ExactSpelling = true, EntryPoint = "DestroyMenu", CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.None)]
    public static extern bool IntDestroyMenu(HandleRef hMenu);

    [DllImport(NativeValues.ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

    [DllImport(NativeValues.ExternDll.User32)]
    public static extern IntPtr GetWindow(IntPtr hwnd, int nCmd);

    [DllImport(NativeValues.ExternDll.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyWindow(IntPtr hwnd);

    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, NativeValues.DWMWINDOWATTRIBUTE dwAttribute, ref uint pvAttribute, uint cbAttribute);

}
