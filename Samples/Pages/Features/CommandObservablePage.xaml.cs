using Microsoft.UI.Xaml.Controls;
using WinUICommunityGallery.Pages.Features;

namespace WinUICommunityGallery.Pages;

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
