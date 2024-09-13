namespace WinUICommunity;
internal static partial class WindowsCompositionHelper
{
    private static WindowsSystemDispatcherQueueHelper dispatcherQueueHelper;

    private static Windows.UI.Composition.Compositor compositor;
    private static object locker = new object();

    public static Windows.UI.Composition.Compositor Compositor => EnsureCompositor();

    private static Windows.UI.Composition.Compositor EnsureCompositor()
    {
        if (compositor == null)
        {
            lock (locker)
            {
                if (compositor == null)
                {
                    dispatcherQueueHelper = new WindowsSystemDispatcherQueueHelper();
                    dispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();

                    compositor = new Windows.UI.Composition.Compositor();
                }
            }
        }

        return compositor;
    }

    private static DispatcherQueueController InitializeCoreDispatcher()
    {
        var options = new NativeValues.DispatcherQueueOptions2();
        options.apartmentType = NativeValues.DISPATCHERQUEUE_THREAD_APARTMENTTYPE.DQTAT_COM_STA;
        options.threadType = NativeValues.DISPATCHERQUEUE_THREAD_TYPE.DQTYPE_THREAD_CURRENT;
        options.dwSize = Marshal.SizeOf<NativeValues.DispatcherQueueOptions>();

        NativeMethods.CreateDispatcherQueueController(options, out var raw);

        return DispatcherQueueController.FromAbi(raw);
    }


    [Guid("AF86E2E0-B12D-4c6a-9C5A-D7AA65101E90")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IInspectable
    {
        void GetIids();
        int GetRuntimeClassName([Out, MarshalAs(UnmanagedType.HString)] out string name);
        void GetTrustLevel();
    }

    [ComImport]
    [Guid("29E691FA-4567-4DCA-B319-D0F207EB6807")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICompositorDesktopInterop
    {
        void CreateDesktopWindowTarget(nint hwndTarget, bool isTopmost, out nint test);
    }
}
