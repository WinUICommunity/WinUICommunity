using DemoApp;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WindowUI.DemoApp.Pages;

public sealed partial class PresenterPage : Page
{
    AppWindow m_AppWindow;
    AppWindowPresenter m_Presenter;
    public PresenterPage()
    {
        this.InitializeComponent();
        m_AppWindow = App.currentWindow.AppWindow;
        m_Presenter = m_AppWindow.Presenter;

        m_AppWindow.Changed += AppWindow_Changed;
    }

    private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
    {
        if (args.DidPresenterChange
            && AppWindowTitleBar.IsCustomizationSupported())
        {
            switch (sender.Presenter.Kind)
            {
                case AppWindowPresenterKind.CompactOverlay:
                    // Compact overlay - hide custom title bar
                    // and use the default system title bar instead.
                    sender.TitleBar.ResetToDefault();
                    break;

                case AppWindowPresenterKind.FullScreen:
                    // Full screen - hide the custom title bar
                    // and the default system title bar.
                    sender.TitleBar.ExtendsContentIntoTitleBar = true;
                    break;

                case AppWindowPresenterKind.Overlapped:
                    // Normal - hide the system title bar
                    // and use the custom title bar instead.
                    sender.TitleBar.ExtendsContentIntoTitleBar = true;
                    break;

                default:
                    // Use the default system title bar.
                    sender.TitleBar.ResetToDefault();
                    break;
            }
        }
    }

    private void SwitchPresenter(object sender, RoutedEventArgs e)
    {
        if (m_AppWindow != null)
        {
            AppWindowPresenterKind newPresenterKind;
            switch ((sender as Button).Name)
            {
                case "CompactoverlaytBtn":
                    newPresenterKind = AppWindowPresenterKind.CompactOverlay;
                    break;

                case "FullscreenBtn":
                    newPresenterKind = AppWindowPresenterKind.FullScreen;
                    break;

                case "OverlappedBtn":
                    newPresenterKind = AppWindowPresenterKind.Overlapped;
                    break;

                default:
                    newPresenterKind = AppWindowPresenterKind.Default;
                    break;
            }

            // If the same presenter button was pressed as the
            // mode we're in, toggle the window back to Default.
            if (newPresenterKind == m_AppWindow.Presenter.Kind)
            {
                m_AppWindow.SetPresenter(AppWindowPresenterKind.Default);
            }
            else
            {
                // Else request a presenter of the selected kind
                // to be created and applied to the window.
                m_AppWindow.SetPresenter(newPresenterKind);
            }
        }
    }

    private void IsAlwaysOnTop_Checked(object sender, RoutedEventArgs e)
    {
        ((OverlappedPresenter)m_Presenter).IsAlwaysOnTop = IsAlwaysOnTopCheckBox.IsChecked.Value;
    }
    private void IsResizable_Checked(object sender, RoutedEventArgs e)
    {
        ((OverlappedPresenter)m_Presenter).IsResizable = IsResizableCheckBox.IsChecked.Value;
    }
    private void IsMinimizable_Checked(object sender, RoutedEventArgs e)
    {
        ((OverlappedPresenter)m_Presenter).IsMinimizable = IsMinimizableCheckBox.IsChecked.Value;
    }
    private void IsMaximizable_Checked(object sender, RoutedEventArgs e)
    {
        ((OverlappedPresenter)m_Presenter).IsMaximizable = IsMaximizableCheckBox.IsChecked.Value;
    }
    private void IsModal_Checked(object sender, RoutedEventArgs e)
    {
        ((OverlappedPresenter)m_Presenter).IsModal = IsModalCheckBox.IsChecked.Value;
    }

    private void btnKind_Click(object sender, RoutedEventArgs e)
    {
        txtKind.Text = ((OverlappedPresenter)m_Presenter).Kind.ToString();
    }

    private void btnState_Click(object sender, RoutedEventArgs e)
    {
        txtState.Text = ((OverlappedPresenter)m_Presenter).State.ToString();
    }
}
