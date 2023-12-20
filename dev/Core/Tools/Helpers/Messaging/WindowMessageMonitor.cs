using WinRT.Interop;

namespace WinUICommunity;
public sealed class WindowMessageMonitor : IDisposable
{
    private nint _hwnd = IntPtr.Zero;
    private delegate nint WinProc(nint hWnd, uint msg, nint wParam, nint lParam);
    private NativeValues.SUBCLASSPROC callback;
    private readonly object _lockObject = new object();

    public WindowMessageMonitor(Window window) : this(WindowNative.GetWindowHandle(window))
    {
    }

    public WindowMessageMonitor(nint hwnd)
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

    private nint NewWindowProc(nint hWnd, uint uMsg, nuint wParam, nint lParam, nuint uIdSubclass, nuint dwRefData)
    {
        var handler = _NativeMessage;
        if (handler != null)
        {
            var args = new WindowMessageEventArgs(hWnd, uMsg, wParam, lParam);
            handler.Invoke(this, args);
            if (args.Handled)
                return new IntPtr(args.Result);
        }
        return NativeMethods.DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }

    private void Subscribe()
    {
        lock (_lockObject)
            if (callback == null)
            {
                callback = new NativeValues.SUBCLASSPROC(NewWindowProc);
                var ok = NativeMethods.SetWindowSubclass(new IntPtr((int)_hwnd), callback, 101, 0);
            }
    }

    private void Unsubscribe()
    {
        lock (_lockObject)
            if (callback != null)
            {
                NativeMethods.RemoveWindowSubclass(new IntPtr(_hwnd), callback, 101);
                callback = null;
            }
    }
}
