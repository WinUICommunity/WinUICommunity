
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace WinUICommunity;

public class AcrylicBackdropHelper
{
    public Window window;
    public WindowsSystemDispatcherQueueHelper m_wsdqHelper;
    public DesktopAcrylicController m_acrylicController;
    public SystemBackdropConfiguration m_configurationSource;

    public AcrylicBackdropHelper(Window window)
    {
        this.window = window;
    }

    public DesktopAcrylicKind GetDesktopAcrylicKind()
    {
        return m_acrylicController.Kind;
    }

    public bool TrySetAcrylicBackdrop(DesktopAcrylicKind acrylicKind)
    {
        if (DesktopAcrylicController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            m_configurationSource = new SystemBackdropConfiguration();
            window.Activated += Window_Activated;
            window.Closed += Window_Closed;
            ((FrameworkElement)window.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            m_acrylicController = new DesktopAcrylicController();
            m_acrylicController.Kind = acrylicKind;

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_acrylicController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_acrylicController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Acrylic is not supported on this system
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use this closed window.
        if (m_acrylicController != null)
        {
            m_acrylicController.Dispose();
            m_acrylicController = null;
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
