using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace WinUICommunity;
internal class MicaSystemBackdrop
{
    public Window window;
    public WindowsSystemDispatcherQueueHelper m_wsdqHelper;
    public MicaController micaController;
    public SystemBackdropConfiguration m_configurationSource;
    public MicaKind kind;
    public MicaSystemBackdrop(Window window, MicaKind kind)
    {
        this.window = window;
        this.kind = kind;
    }

    public void Disconnect()
    {
        window.Activated -= Window_Activated;
        window.Closed -= Window_Closed;
        ((FrameworkElement)window.Content).ActualThemeChanged -= Window_ThemeChanged;
        micaController.RemoveAllSystemBackdropTargets();
        micaController.ResetProperties();
        micaController.Dispose();
        micaController = null;
    }
    public bool TrySetMicaBackdrop()
    {
        if (MicaController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            m_configurationSource = new SystemBackdropConfiguration();
            window.Activated -= Window_Activated;
            window.Activated += Window_Activated;
            window.Closed -= Window_Closed;
            window.Closed += Window_Closed;
            ((FrameworkElement)window.Content).ActualThemeChanged -= Window_ThemeChanged;
            ((FrameworkElement)window.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            micaController = new MicaController();
            micaController.Kind = kind;

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            micaController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            micaController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use this closed window.
        if (micaController != null)
        {
            micaController.Dispose();
            micaController = null;
        }
        window.Activated -= Window_Activated;
        m_configurationSource = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (m_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)window.Content).ActualTheme)
        {
            case ElementTheme.Dark: m_configurationSource.Theme = SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: m_configurationSource.Theme = SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = SystemBackdropTheme.Default; break;
        }
    }
}
