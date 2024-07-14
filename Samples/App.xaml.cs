using WinUICommunityGallery.Pages;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinUICommunity;

namespace WinUICommunityGallery;

public partial class App : Application
{
    public static Window currentWindow = Window.Current;

    public IJsonNavigationViewService JsonNavigationViewService { get; set; }
    public IThemeService ThemeService { get; set; }
    public new static App Current => (App)Application.Current;
    public string Title { get; set; } = "WinUICommunity Gallery App";
    public string Version { get; set; } = AssemblyInfoHelper.GetAppInfo().Version;
    public App()
    {
        this.InitializeComponent();
        JsonNavigationViewService = new JsonNavigationViewService();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        currentWindow = new Window();
        currentWindow.AppWindow.Title = currentWindow.Title = Title;
        currentWindow.AppWindow.SetIcon("Assets/icon.ico");
        if (currentWindow.Content is not Frame rootFrame)
        {
            currentWindow.Content = rootFrame = new Frame();
        }

        rootFrame.Navigate(typeof(MainPage), args.Arguments);
        ThemeService = new ThemeService(currentWindow);
        //ThemeService.ConfigTintColor();
        //ThemeService.ConfigFallbackColor();

        // Ensure the current window is active
        currentWindow.Activate();
    }
}
