using WinRT.Interop;

namespace WinUICommunity;
public sealed partial class WindowMessageMonitor : IDisposable
{
    private IntPtr _hwnd = IntPtr.Zero;
    private delegate IntPtr WinProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    private Windows.Win32.UI.Shell.SUBCLASSPROC? callback;
    private readonly object _lockObject = new object();

    public WindowMessageMonitor(Window window) : this(WindowNative.GetWindowHandle(window))
    {
    }

    public WindowMessageMonitor(IntPtr hwnd)
    {
        _hwnd = hwnd;
    }

    ~WindowMessageMonitor()
    {
        Dispose(false);
    }

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (_NativeMessage != null)
            Unsubscribe();
    }

    private event EventHandler<WindowMessageEventArgs> _NativeMessage;

    public event EventHandler<WindowMessageEventArgs> WindowMessageReceived
    {
        add
        {
            if (_NativeMessage is null)
            {
                Subscribe();
            }
            _NativeMessage += value;
        }
        remove
        {
            _NativeMessage -= value;
            if (_NativeMessage is null)
            {
                Unsubscribe();
            }
        }
    }

    private LRESULT NewWindowProc(HWND hWnd, uint uMsg, WPARAM wParam, LPARAM lParam, nuint uIdSubclass, nuint dwRefData)
    {
        var handler = _NativeMessage;
        if (handler != null)
        {
            var args = new WindowMessageEventArgs(hWnd, uMsg, wParam.Value, lParam);
            handler.Invoke(this, args);
            if (args.Handled)
                return new LRESULT((int)args.Result);
        }
        return PInvoke.DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }

    private void Subscribe()
    {
        lock (_lockObject)
            if (callback == null)
            {
                callback = new Windows.Win32.UI.Shell.SUBCLASSPROC(NewWindowProc);
                bool ok = PInvoke.SetWindowSubclass(new HWND(_hwnd), callback, 101, 0);
            }
    }

    private void Unsubscribe()
    {
        lock (_lockObject)
            if (callback != null)
            {
                PInvoke.RemoveWindowSubclass(new HWND(_hwnd), callback, 101);
                callback = null;
            }
    }
}
