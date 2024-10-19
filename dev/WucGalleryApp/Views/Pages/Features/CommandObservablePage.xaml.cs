using WucGalleryApp.Views.Features;

namespace WucGalleryApp.Views;

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
