using System;
using Microsoft.UI.Xaml.Controls;
using WinUICommunity;

namespace WinUICommunityGallery.Pages;

public sealed partial class PinBoxPage : Page
{
    public PinBoxPage()
    {
        this.InitializeComponent();
        Loaded += PinBoxPage_Loaded;
    }

    private void PinBoxPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        numberBox.Value = 4;
        slider.Value = 12;
        pinBox.PasswordLength = Convert.ToInt32(numberBox.Value);
        pinBox.ItemSpacing = slider.Value;
    }

    public Orientation GetOrientation(int index)
    {
        return (Orientation)index;
    }

    public PasswordRevealMode GetPasswordRevealMode(int index)
    {
        return (PasswordRevealMode)index;
    }

    public PinBoxFocusMode GetFocusMode(int index)
    {
        return (PinBoxFocusMode)index;
    }

    private void slider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        pinBox.ItemSpacing = e.NewValue;
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        pinBox.PasswordLength = Convert.ToInt32(args.NewValue);
    }
}
