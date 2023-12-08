using Microsoft.UI.Xaml.Controls;
using WindowUI.DemoApp.Pages.Features;

namespace WindowUI.DemoApp.Pages;

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
