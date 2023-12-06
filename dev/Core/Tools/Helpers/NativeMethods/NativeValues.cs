using System.Diagnostics;

namespace WinUICommunity;
public static class NativeValues
{
    /// <summary>
    /// Places the window at the top of the Z order.
    /// </summary>
    public static readonly IntPtr HWND_TOP_IntPtr = new(0);

    public delegate IntPtr WNDPROC(IntPtr hWnd, NativeValues.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    public static class ExternDll
    {
        public const string
            User32 = "user32.dll",
            Gdi32 = "gdi32.dll",
            GdiPlus = "gdiplus.dll",
            Kernel32 = "kernel32.dll",
            Shell32 = "shell32.dll",
            MsImg = "msimg32.dll",
            NTdll = "ntdll.dll",
            DwmApi = "dwmapi.dll",
            UxTheme = "uxtheme.dll";
    }

    public const int
        BITSPIXEL = 12,
        PLANES = 14,
        BI_RGB = 0,
        DIB_RGB_COLORS = 0,
        E_FAIL = unchecked((int)0x80004005),
        HWND_TOP = 0,
        GWL_EXSTYLE = -20,
        NIF_MESSAGE = 0x00000001,
        NIF_ICON = 0x00000002,
        NIF_TIP = 0x00000004,
        NIF_INFO = 0x00000010,
        NIM_ADD = 0x00000000,
        NIM_MODIFY = 0x00000001,
        NIM_DELETE = 0x00000002,
        NIIF_NONE = 0x00000000,
        NIIF_INFO = 0x00000001,
        NIIF_WARNING = 0x00000002,
        NIIF_ERROR = 0x00000003,
        WM_ACTIVATE = 0x0006,
        WA_ACTIVE = 0x01,
        WA_INACTIVE = 0x00,
        WM_QUIT = 0x0012,
        WM_GETMINMAXINFO = 0x0024,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_SETICON = 0x0080,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCHITTEST = 0x0084,
        WM_NCACTIVATE = 0x0086,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCUAHDRAWCAPTION = 0x00AE,
        WM_NCUAHDRAWFRAME = 0x00AF,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSCOMMAND = 0x112,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONUP = 0x0205,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_CLIPBOARDUPDATE = 0x031D,
        WM_USER = 0x0400,
        WS_VISIBLE = 0x10000000,
        WS_EX_LAYOUTLTR = 0x00000000,
        WS_EX_LAYOUTRTL = 0x00400000,
        MF_BYCOMMAND = 0x00000000,
        MF_BYPOSITION = 0x400,
        MF_GRAYED = 0x00000001,
        MF_SEPARATOR = 0x800,
        TB_GETBUTTON = WM_USER + 23,
        TB_BUTTONCOUNT = WM_USER + 24,
        TB_GETITEMRECT = WM_USER + 29,
        VERTRES = 10,
        DESKTOPVERTRES = 117,
        LOGPIXELSX = 88,
        LOGPIXELSY = 90,
        SC_CLOSE = 0xF060,
        SC_SIZE = 0xF000,
        SC_MOVE = 0xF010,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_RESTORE = 0xF120,
        SRCCOPY = 0x00CC0020,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    [Conditional("CodeGeneration")]
    public class FriendlyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyAttribute"/> class.
        /// </summary>
        /// <param name="flags">The flags that describe this parameter's semantics.</param>
        public FriendlyAttribute(FriendlyFlags flags)
        {
            Flags = flags;
        }

        /// <summary>
        /// Gets the flags that describe this parameter's semantics.
        /// </summary>
        public FriendlyFlags Flags { get; }

        /// <summary>
        /// Gets or sets the 0-based index to the parameter that takes the length of the array given by this array parameter.
        /// An overload will be generated that drops this parameter and sets it automatically based on the length of the array provided to the parameter this attribute is applied to.
        /// </summary>
        /// <value>-1 is the default and indicates that no automated parameter removal should be generated.</value>
        /// <remarks>
        /// Only applicable when <see cref="Flags"/> includes <see cref="FriendlyFlags.Array"/>.
        /// </remarks>
        public int ArrayLengthParameter { get; set; } = -1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RTL_OSVERSIONINFOEX
    {
        public RTL_OSVERSIONINFOEX(uint dwMajorVersion, uint dwMinorVersion, uint dwBuildNumber, uint dwRevision, uint dwPlatformId, string szCSDVersion) : this()
        {
            this.dwMajorVersion = dwMajorVersion;
            this.dwMinorVersion = dwMinorVersion;
            this.dwBuildNumber = dwBuildNumber;
            this.dwRevision = dwRevision;
            this.dwPlatformId = dwPlatformId;
            this.szCSDVersion = szCSDVersion;
        }
        public readonly uint dwOSVersionInfoSize { get; init; } = (uint)Marshal.SizeOf<RTL_OSVERSIONINFOEX>();

        public uint dwMajorVersion;
        public uint dwMinorVersion;
        public uint dwBuildNumber;
        public uint dwRevision;
        public uint dwPlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szCSDVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    public struct POINT
    {
        /// <summary>
        ///  The x-coordinate of the point.
        /// </summary>
        public int x;

        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public int y;

#if !UAP10_0
        public static implicit operator System.Drawing.Point(POINT point)
        {
            return new(point.x, point.y);
        }

        public static implicit operator POINT(System.Drawing.Point point)
        {
            return new() { x = point.X, y = point.Y };
        }
#endif
    }

    [Flags]
    public enum FriendlyFlags
    {
        /// <summary>
        /// The pointer is to the first element in an array.
        /// </summary>
        Array = 0x1,

        /// <summary>
        /// The value is at least partially initialized by the caller.
        /// </summary>
        In = 0x2
    }

    [Flags]
    public enum WindowStyle : uint
    {
        WS_SYSMENU = 0x80000
    }

    [Flags]
    public enum WindowLongIndexFlags : int
    {
        GWL_WNDPROC = -4,
        GWL_STYLE = -16
    }

    [Flags]
    public enum SetWindowPosFlags : uint
    {
        /// <summary>
        ///     Retains the current position (ignores X and Y parameters).
        /// </summary>
        SWP_NOMOVE = 0x0002
    }

    public enum SystemCommand
    {
        SC_MOUSEMENU = 0xF090,
        SC_KEYMENU = 0xF100
    }

    public enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    public enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    };

    public enum WindowMessage : int
    {
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_SYSCOMMAND = 0x0112,
        WM_SYSMENU = 0x0313,
        WM_GETMINMAXINFO = 0x0024
    }

    [Flags]
    public enum DWMWINDOWATTRIBUTE : uint
    {
        DWMWA_WINDOW_CORNER_PREFERENCE = 33,
        DWMWA_BORDER_COLOR,
        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS
    }
}
