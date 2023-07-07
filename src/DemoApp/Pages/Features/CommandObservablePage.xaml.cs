using Microsoft.UI.Xaml.Controls;
using WinUICommunity.DemoApp.Pages.Features;

namespace WinUICommunity.DemoApp.Pages;

public sealed partial class CommandObservablePage : Page
{
    public CommandObservableViewModel Vm { get; set; }

    public CommandObservablePage()
    {
        this.InitializeComponent();
        Vm = new CommandObservableViewModel();
        DataContext = Vm;
    }
}
