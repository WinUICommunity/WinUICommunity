using System;
using Microsoft.UI.Xaml.Controls;

namespace WinUICommunity.DemoApp.Pages;
public sealed partial class ParticlePage : Page
{
    public ParticlePage()
    {
        this.InitializeComponent();
    }

    private void nbDensity_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        particle.Density = Convert.ToInt32(args.NewValue);
    }
}
