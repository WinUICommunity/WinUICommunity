using Microsoft.UI.Xaml.Controls;
using WinUIGallery.Pages.Features;

namespace WinUIGallery.Pages;

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
