using System.Diagnostics;

namespace WinUICommunity;
public static class NativeMethods
{
    public const int WM_ACTIVATE = 0x0006;
    public const int WA_ACTIVE = 0x01;
    public const int WA_INACTIVE = 0x00;

    public const int GWL_EXSTYLE = -20;
    public const int WS_EX_LAYOUTRTL = 0x00400000;
    public const int WS_EX_LAYOUTLTR = 0x00000000;

    /// <summary>
    /// Places the window at the top of the Z order.
    /// </summary>
    public static readonly IntPtr HWND_TOP = new(0);

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

    [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern void FlushMenuThemes();

    [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);

    public delegate IntPtr WinProc(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("NTdll.dll")]
    public static extern int RtlGetVersion(out RTL_OSVERSIONINFOEX lpVersionInformation);

    [DllImport("kernel32.dll")]
    public static extern uint GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

    [DllImport("CoreMessaging.dll")]
    public static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    [DllImport("User32.dll")]
    public static extern int GetDpiForWindow(IntPtr hwnd);

    [DllImport("User32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport("Shcore.dll", SetLastError = true)]
    public static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, [Optional] StringBuilder packageFullName);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong32(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc);

    public static IntPtr SetWindowLongPtr(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc)
    {
        return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, newProc) : new IntPtr(SetWindowLong32(hWnd, nIndex, newProc));
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// An attribute applied to native pointer parameters to indicate its semantics
    /// such that a friendly overload of the method can be generated with the right signature.
    /// </summary>
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

    public enum WindowMessage : int
    {
        WM_GETMINMAXINFO = 0x0024,
    }

    [Flags]
    public enum WindowLongIndexFlags : int
    {
        GWL_WNDPROC = -4,
    }

    public enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
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
    public enum SetWindowPosFlags : uint
    {
        /// <summary>
        ///     Retains the current position (ignores X and Y parameters).
        /// </summary>
        SWP_NOMOVE = 0x0002
    }

    public enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    };
}
