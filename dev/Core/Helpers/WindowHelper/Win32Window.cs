namespace WinUICommunity;
public partial class Win32Window
{
    public Win32Window(IntPtr handle)
    {
        Handle = handle;
        ThreadId = NativeMethods.GetWindowThreadProcessId(handle, out var processId);
        ProcessId = processId;
    }

    public IntPtr Handle { get; }
    public int ThreadId { get; }
    public int ProcessId { get; }
    public string ClassName => WindowHelper.GetClassName(Handle);
    public string Text => WindowHelper.GetWindowText(Handle);
    public bool IsEnabled => NativeMethods.IsWindowEnabled(Handle);

    public override string ToString()
    {
        var s = ClassName;
        var text = Text;
        if (text != null)
        {
            s += " '" + text + "'";
        }
        return s;
    }
}
