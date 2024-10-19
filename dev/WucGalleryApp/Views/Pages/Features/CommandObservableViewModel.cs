using System.Windows.Input;

namespace WucGalleryApp.Views.Features;

public class CommandObservableViewModel : Observable
{
    private string myText;
    public string MyText
    {
        get { return myText; }
        set { Set(ref myText, value); }
    }

    private ICommand itemInvokedCommand;

    public ICommand ItemInvokedCommand => itemInvokedCommand ?? (itemInvokedCommand = new WinUICommunity.RelayCommand<string>(OnItemInvoked));

    public void OnItemInvoked(string arg)
    {
        MyText = "Clicked!";
    }
}
