namespace WinUICommunity;

public static partial class WindowHelper
{
    /// <summary>
    /// allow the app to find the Window that contains an
    /// arbitrary UIElement (GetWindowForElement).  To do this, we keep track
    /// of all active Windows.  The app code must call WindowHelper.CreateWindow
    /// rather than "new Window" so we can keep track of all the relevant windows.
    /// </summary>
    public static List<Window> ActiveWindows => _activeWindows;

    private static List<Window> _activeWindows = new();

    /// <summary>
    /// Get WindowHandle for a Window
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static IntPtr GetWindowHandleForCurrentWindow(object target)
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(target);
        return hWnd;
    }

    /// <summary>
    /// Get WindowId from Window
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static WindowId GetWindowIdFromCurrentWindow(object target)
    {
        var wndId = Win32Interop.GetWindowIdFromWindow(GetWindowHandleForCurrentWindow(target));
        return wndId;
    }

    /// <summary>
    /// allow the app to find the Window that contains an
    /// arbitrary UIElement (GetWindowForElement).  To do this, we keep track
    /// of all active Windows.  The app code must call WindowHelper.CreateWindow
    /// rather than "new Window" so we can keep track of all the relevant windows.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Window GetWindowForElement(UIElement element)
    {
        if (element.XamlRoot != null)
        {
            foreach (var window in _activeWindows)
            {
                if (element.XamlRoot == window.Content.XamlRoot)
                {
                    return window;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Create a new Window
    /// </summary>
    /// <returns></returns>
    public static Window CreateWindow()
    {
        var newWindow = new Window();
        TrackWindow(newWindow);
        return newWindow;
    }

    /// <summary>
    /// track of all active Windows.  The app code must call WindowHelper.CreateWindow
    /// rather than "new Window" so we can keep track of all the relevant windows.
    /// </summary>
    /// <param name="window"></param>
    public static void TrackWindow(Window window)
    {
        window.Closed += (sender, args) =>
        {
            _activeWindows.Remove(window);
        };
        _activeWindows.Add(window);
    }

    public static void SwitchToThisWindow(object target)
    {
        if (target != null)
        {
            NativeMethods.SwitchToThisWindow(GetWindowHandleForCurrentWindow(target), true);
        }
    }

    public static void ActivateWindowAgain(Window window)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var activeWindow = NativeMethods.GetActiveWindow();
        if (hwnd == activeWindow)
        {
            NativeMethods.SendMessage(hwnd, NativeMethods.WM_ACTIVATE, NativeMethods.WA_INACTIVE, IntPtr.Zero);
            NativeMethods.SendMessage(hwnd, NativeMethods.WM_ACTIVATE, NativeMethods.WA_ACTIVE, IntPtr.Zero);
        }
        else
        {
            NativeMethods.SendMessage(hwnd, NativeMethods.WM_ACTIVATE, NativeMethods.WA_ACTIVE, IntPtr.Zero);
            NativeMethods.SendMessage(hwnd, NativeMethods.WM_ACTIVATE, NativeMethods.WA_INACTIVE, IntPtr.Zero);
        }
    }

    /// <summary>
    /// Sets the preferred app mode based on the specified element theme.
    /// </summary>
    /// <param name="theme">The element theme to set the preferred app mode to.</param>
    /// <remarks>
    /// This method sets the preferred app mode based on the specified element theme. If the "theme" parameter is set to "Dark", it sets the preferred app mode to "ForceDark", forcing the app to use a dark theme. If the "theme" parameter is set to "Light", it sets the preferred app mode to "ForceLight", forcing the app to use a light theme. Otherwise, it sets the preferred app mode to "Default", using the system default theme. After setting the preferred app mode, the method flushes the menu themes to ensure that any changes take effect immediately. 
    /// </remarks>
    public static void SetPreferredAppMode(ElementTheme theme)
    {
        if (theme == ElementTheme.Dark)
        {
            NativeMethods.SetPreferredAppMode(NativeMethods.PreferredAppMode.ForceDark);
        }
        else if (theme == ElementTheme.Light)
        {
            NativeMethods.SetPreferredAppMode(NativeMethods.PreferredAppMode.ForceLight);
        }
        else
        {
            NativeMethods.SetPreferredAppMode(NativeMethods.PreferredAppMode.Default);
        }
        NativeMethods.FlushMenuThemes();
    }
}
