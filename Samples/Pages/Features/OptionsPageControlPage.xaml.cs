using System;
using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity.DemoApp.Pages;
public sealed partial class OptionsPageControlPage : Page
{
    public OptionsPageControlPage()
    {
        this.InitializeComponent();
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        OptionsPage.OptionsBarMinWidth = 48.0;
    }
}
