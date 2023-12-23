using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace WinUICommunity.DemoApp.Pages;

public sealed partial class GrowlPage : Page
{
    public GrowlPage()
    {
        this.InitializeComponent();
    }

    private void btnGrowl_Click(object sender, RoutedEventArgs e)
    {
        Growl.Info("Hello");
        Growl.Info("Hello", "Info");
        Growl.Info2("Hello");
        Growl.Info2("Hello", "Info2");
        Growl.Warning("Hello");
        Growl.Warning("Hello", "Warning");
        Growl.Error("Hello");
        Growl.Error("Hello", "Error");
        Growl.Success("Hello");
        Growl.Success("Hello", "Success");
        Growl.Ask("Hello", (s,e) =>
        {
            Growl.Info("Clicked");
            return true;
        });
    }

    private void btnGrowlInfo_Click(object sender, RoutedEventArgs e)
    {
        Growl.Info(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Info with GrowlInfo",
        });
        Growl.Info(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Info with GrowlInfo",
            UseBlueColorForInfo = true,
        });
        Growl.Warning(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Warning with GrowlInfo"
        });
        Growl.Error(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Error with GrowlInfo"
        });
        Growl.Success(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Success with GrowlInfo"
        });
    }

    private void btnGrowlToken_Click(object sender, RoutedEventArgs e)
    {
        Growl.InfoWithToken("Hello", "Test");
        Growl.InfoWithToken("Hello", "Info", "Test");
        Growl.Info2WithToken("Hello", "Test");
        Growl.Info2WithToken("Hello", "Info2", "Test");
        Growl.WarningWithToken("Hello", "Test");
        Growl.WarningWithToken("Hello", "Warning", "Test");
        Growl.ErrorWithToken("Hello", "Test");
        Growl.ErrorWithToken("Hello", "Error", "Test");
        Growl.SuccessWithToken("Hello", "Test");
        Growl.SuccessWithToken("Hello", "Success", "Test");
        Growl.AskWithToken("Hello", "Test", (s, e) =>
        {
            Growl.Info("Clicked");
            return true;
        });
    }

    private void btnGrowlInfoToken_Click(object sender, RoutedEventArgs e)
    {
        Growl.Info(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Info with GrowlInfo",
            Token = "Test"
        });
        Growl.Info(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Info with GrowlInfo",
            UseBlueColorForInfo = true,
            Token = "Test"
        });
        Growl.Warning(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Warning with GrowlInfo",
            Token = "Test"
        });
        Growl.Error(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Error with GrowlInfo",
            Token = "Test"
        });
        Growl.Success(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Success with GrowlInfo",
            Token = "Test"
        });
    }

    private void btnGrowlGlobal_Click(object sender, RoutedEventArgs e)
    {
        Growl.InfoGlobal("Hello");
        Growl.InfoGlobal("Hello", "Info");
        Growl.Info2Global("Hello");
        Growl.Info2Global("Hello", "Info2");
        Growl.WarningGlobal("Hello");
        Growl.WarningGlobal("Hello", "Warning");
        Growl.ErrorGlobal("Hello");
        Growl.ErrorGlobal("Hello", "Error");
        Growl.SuccessGlobal("Hello");
        Growl.SuccessGlobal("Hello", "Success");
        Growl.AskGlobal("Hello", (s, e) =>
        {
            Growl.Info("Clicked");
            return true;
        });
    }

    private void btnGrowlInfoGlobal_Click(object sender, RoutedEventArgs e)
    {
        Growl.InfoGlobal(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Info with GrowlInfo"
        });
        Growl.InfoGlobal(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Info with GrowlInfo",
            UseBlueColorForInfo = true
        });
        Growl.WarningGlobal(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Warning with GrowlInfo"
        });
        Growl.ErrorGlobal(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Error with GrowlInfo"
        });
        Growl.SuccessGlobal(new GrowlInfo
        {
            ShowDateTime = true,
            StaysOpen = true,
            IsClosable = false,
            Title = "Hello",
            Message = "Success with GrowlInfo"
        });
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        Growl.Clear();
        Growl.Clear("Test");
        Growl.ClearGlobal();
    }
}
