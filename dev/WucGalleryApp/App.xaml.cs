﻿namespace WucGalleryApp;

public partial class App : Application
{
    public static Window MainWindow = Window.Current;
    public IServiceProvider Services { get; }
    public new static App Current => (App)Application.Current;
    public IJsonNavigationViewService GetJsonNavigationViewService => GetService<IJsonNavigationViewService>();
    public IThemeService GetThemeService => GetService<IThemeService>();

    public static T GetService<T>() where T : class
    {
        if ((App.Current as App)!.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public App()
    {
        Services = ConfigureServices();
        this.InitializeComponent();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IJsonNavigationViewService>(factory =>
        {
            var json = new JsonNavigationViewService();
            json.ConfigDefaultPage(typeof(HomeLandingPage));
            json.ConfigSettingsPage(typeof(SettingsPage));
            json.ConfigSectionPage(typeof(DemoSectionPage));
            return json;
        });

        services.AddTransient<MainViewModel>();
        services.AddTransient<GeneralSettingViewModel>();
        services.AddTransient<AppUpdateSettingViewModel>();
        services.AddTransient<AboutUsSettingViewModel>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new Window();

        if (MainWindow.Content is not Frame rootFrame)
        {
            MainWindow.Content = rootFrame = new Frame();
        }

        if (GetThemeService != null)
        {
            GetThemeService.AutoInitialize(MainWindow);
        }

        rootFrame.Navigate(typeof(MainPage));

        MainWindow.Title = MainWindow.AppWindow.Title = ProcessInfoHelper.ProductNameAndVersion;
        MainWindow.AppWindow.SetIcon("Assets/icon.ico");

        MainWindow.Activate();
    }
}

