using System.Runtime.Versioning;
using static WinUICommunity.NativeValues;

namespace WinUICommunity;
public static partial class NativeMethods
{
    [DllImport(ExternDll.User32, EntryPoint = "FindWindowExW", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

    [DllImport(ExternDll.User32)]
    public static extern IntPtr GetActiveWindow();

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

    [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode)]
    public static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

    [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode)]
    public static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

    [DllImport(ExternDll.UxTheme, EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern void FlushMenuThemes();

    [DllImport(ExternDll.UxTheme, EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

    [DllImport(ExternDll.User32, SetLastError = true)]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);

    [DllImport(ExternDll.NTdll)]
    public static extern int RtlGetVersion(out RTL_OSVERSIONINFOEX lpVersionInformation);

    [DllImport(ExternDll.Kernel32)]
    public static extern uint GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

    [DllImport("CoreMessaging.dll")]
    public static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    [DllImport("coremessaging.dll", EntryPoint = "CreateDispatcherQueueController", CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateDispatcherQueueController(DispatcherQueueOptions2 options, out IntPtr dispatcherQueueController);

    [DllImport(ExternDll.User32)]
    public static extern int GetDpiForWindow(IntPtr hwnd);

    [DllImport(ExternDll.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport("Shcore.dll", SetLastError = true)]
    public static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    [DllImport(ExternDll.Kernel32, SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, [Optional] StringBuilder packageFullName);


    [DllImport(ExternDll.User32, EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, WNDPROC newProc);

    [DllImport(ExternDll.User32, EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, WNDPROC newProc);

    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, WNDPROC newProc)
    {
        return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, newProc) : new IntPtr(SetWindowLong32(hWnd, nIndex, newProc));
    }

    [DllImport(ExternDll.User32)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport(ExternDll.User32, EntryPoint = "SetWindowLongW", SetLastError = false)]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport(ExternDll.User32, EntryPoint = "SetWindowLongPtrW", SetLastError = false)]
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


    [DllImport(ExternDll.User32, EntryPoint = "GetWindowLongW", SetLastError = false)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport(ExternDll.User32, EntryPoint = "GetWindowLongPtrW", SetLastError = false)]
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

    [DllImport(ExternDll.User32)]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern bool GetCursorPos(out POINT pt);

    [DllImport(ExternDll.User32)]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport(ExternDll.User32)]
    public static extern bool EnableMenuItem(IntPtr hMenu, int UIDEnabledItem, int uEnable);

    [DllImport(ExternDll.User32)]
    public static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIDNewItem, string lpNewItem);

    [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "DestroyMenu", CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.None)]
    public static extern bool IntDestroyMenu(HandleRef hMenu);

    [DllImport(ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

    [DllImport(ExternDll.User32)]
    public static extern IntPtr GetWindow(IntPtr hwnd, int nCmd);

    [DllImport(ExternDll.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyWindow(IntPtr hwnd);

    [DllImport(ExternDll.DwmApi, CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, ref uint pvAttribute, uint cbAttribute);

    [DllImport(ExternDll.Gdi32)]
    public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

    [DllImport(ExternDll.DwmApi)]
    public static extern int DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND pBlurBehind);

    [DllImport(ExternDll.User32)]
    public static extern IntPtr BeginPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);

    [DllImport(ExternDll.User32)]
    public static extern bool FillRect(IntPtr hDC, [In] ref RECT lprc, nint hbr);

    [DllImport(ExternDll.Gdi32)]
    public static extern IntPtr GetStockObject(StockObjectType fnObject);

    [DllImport(ExternDll.DwmApi, SetLastError = true)]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

    [DllImport(ExternDll.User32, SetLastError = true)]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport(ExternDll.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport(ExternDll.Gdi32, SetLastError = true)]
    public static extern IntPtr CreateSolidBrush(uint crColor);

    [DllImport(ExternDll.Gdi32, SetLastError = true)]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("comctl32.dll", SetLastError = true)]
    public static extern IntPtr DefSubclassProc(IntPtr hWnd, uint uMsg, nuint wParam, nint lParam);

    [DllImport("comctl32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, nuint uIdSubclass, nuint dwRefData);

    [DllImport("comctl32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool RemoveWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, nuint uIdSubclass);
}
