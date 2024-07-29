using WinUICommunityGallery.Pages;

namespace WinUICommunityGallery;

public partial class App : Application
{
    public static Window CurrentWindow = Window.Current;

    public IJsonNavigationViewService JsonNavigationViewService { get; set; }
    public IThemeService ThemeService { get; set; }
    public new static App Current => (App)Application.Current;
    public App()
    {
        this.InitializeComponent();
        JsonNavigationViewService = new JsonNavigationViewService();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        CurrentWindow = new Window();
        CurrentWindow.AppWindow.Title = CurrentWindow.Title = ProcessInfoHelper.GetProductName();
        CurrentWindow.AppWindow.SetIcon("Assets/icon.ico");
        if (CurrentWindow.Content is not Frame rootFrame)
        {
            CurrentWindow.Content = rootFrame = new Frame();
        }

        rootFrame.Navigate(typeof(MainPage), args.Arguments);
        ThemeService = new ThemeService(CurrentWindow);
        //ThemeService.ConfigTintColor();
        //ThemeService.ConfigFallbackColor();

        // Ensure the current window is active
        CurrentWindow.Activate();
    }
}
