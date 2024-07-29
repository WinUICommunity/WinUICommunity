using System.Diagnostics;
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
        ReActivateWindow(hwnd);
    }
    public static void ReActivateWindow(IntPtr windowHandle)
    {
        var activeWindow = NativeMethods.GetActiveWindow();
        if (windowHandle == activeWindow)
        {
            NativeMethods.SendMessage(windowHandle, NativeValues.WM_ACTIVATE, NativeValues.WA_INACTIVE, IntPtr.Zero);
            NativeMethods.SendMessage(windowHandle, NativeValues.WM_ACTIVATE, NativeValues.WA_ACTIVE, IntPtr.Zero);
        }
        else
        {
            NativeMethods.SendMessage(windowHandle, NativeValues.WM_ACTIVATE, NativeValues.WA_ACTIVE, IntPtr.Zero);
            NativeMethods.SendMessage(windowHandle, NativeValues.WM_ACTIVATE, NativeValues.WA_INACTIVE, IntPtr.Zero);
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
        return (window, null);
    }

    public static void SetWindowCornerRadius(Window window, NativeValues.DWM_WINDOW_CORNER_PREFERENCE cornerPreference)
    {
        SetWindowCornerRadius(WindowNative.GetWindowHandle(window), cornerPreference);
    }

    public static void SetWindowCornerRadius(IntPtr hwnd, NativeValues.DWM_WINDOW_CORNER_PREFERENCE cornerPreference)
    {
        if (OSVersionHelper.IsWindows11_22000_OrGreater)
        {
            var attribute = NativeValues.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = (uint)cornerPreference;
            NativeMethods.DwmSetWindowAttribute(hwnd, attribute, ref preference, sizeof(uint));
        }
    }

    public static IReadOnlyList<Win32Window> GetTopLevelWindows()
    {
        var list = new List<Win32Window>();
        NativeMethods.EnumWindows((h, l) =>
        {
            list.Add(new Win32Window(h));
            return true;
        }, IntPtr.Zero);
        return list.AsReadOnly();
    }

    public static IReadOnlyList<Win32Window> GetProcessWindowList()
    {
        var process = Process.GetCurrentProcess();
        var list = new List<Win32Window>();
        NativeMethods.EnumWindows((h, l) =>
        {
            var window = new Win32Window(h);
            if (window.ProcessId == process.Id)
            {
                list.Add(window);
            }
            return true;
        }, IntPtr.Zero);
        return list.AsReadOnly();
    }

    public static string GetWindowText(IntPtr hwnd)
    {
        var sb = new StringBuilder(1024);
        NativeMethods.GetWindowText(hwnd, sb, sb.Capacity - 1);
        return sb.ToString();
    }

    public static string GetClassName(IntPtr hwnd)
    {
        var sb = new StringBuilder(256);
        NativeMethods.GetClassName(hwnd, sb, sb.Capacity - 1);
        return sb.ToString();
    }

    public static AppWindow GetCurrentAppWindow()
    {
        var tops = GetProcessWindowList();

        var firstWinUI3 = tops.FirstOrDefault(w => w.ClassName == "WinUIDesktopWin32WindowClass");

        var windowId = Win32Interop.GetWindowIdFromWindow(firstWinUI3.Handle);

        return AppWindow.GetFromWindowId(windowId);
    }

    /// <summary>
    /// Use XamlRoot
    /// </summary>
    /// <param name="uIElement"></param>
    /// <returns></returns>
    public static AppWindow GetAppWindow(UIElement uIElement)
    {
        if (uIElement == null)
        {
            return null;
        }

        return AppWindow.GetFromWindowId(uIElement.XamlRoot.ContentIslandEnvironment.AppWindowId);
    }

    /// <summary>
    /// Use Microsoft.UI.Composition.Visual
    /// </summary>
    /// <param name="uIElement"></param>
    /// <returns></returns>
    public static AppWindow GetAppWindow2(UIElement uIElement)
    {
        if (uIElement == null)
        {
            return null;
        }

        Microsoft.UI.Composition.Visual visual = Microsoft.UI.Xaml.Hosting.ElementCompositionPreview.GetElementVisual(uIElement);
        var ci = Microsoft.UI.Content.ContentIsland.FindAllForCompositor(visual.Compositor);
        if (ci[0] != null)
        {
            return AppWindow.GetFromWindowId(ci[0].Environment.AppWindowId);
        }

        return null;
    }

    /// <summary>
    /// Use XamlRoot
    /// </summary>
    /// <param name="uIElement"></param>
    /// <returns></returns>
    public static IntPtr GetWindowHandle(UIElement uIElement)
    {
        if (uIElement == null)
        {
            return IntPtr.Zero;
        }

        return Win32Interop.GetWindowFromWindowId(uIElement.XamlRoot.ContentIslandEnvironment.AppWindowId);
    }

    /// <summary>
    /// Use Microsoft.UI.Composition.Visual
    /// </summary>
    /// <param name="uIElement"></param>
    /// <returns></returns>
    public static IntPtr GetWindowHandle2(UIElement uIElement)
    {
        if (uIElement == null)
        {
            return IntPtr.Zero;
        }

        Microsoft.UI.Composition.Visual visual = Microsoft.UI.Xaml.Hosting.ElementCompositionPreview.GetElementVisual(uIElement);
        var ci = Microsoft.UI.Content.ContentIsland.FindAllForCompositor(visual.Compositor);
        if (ci[0] != null)
        {
            return Win32Interop.GetWindowFromWindowId(ci[0].Environment.AppWindowId);

        }

        return IntPtr.Zero;
    }
}
