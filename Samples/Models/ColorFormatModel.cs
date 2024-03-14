using WinUICommunity;

namespace WinUICommunityGallery.Models;

public class ColorFormatModel : Observable
{
    private string _name;
    private string _example;
    private bool _isShown;
    private bool _canMoveUp = true;
    private bool _canMoveDown = true;

    public ColorFormatModel(string name, string example, bool isShown)
    {
        Name = name;
        Example = example;
        IsShown = isShown;
    }

    public string Name
    {
        get { return _name; }
        set { Set(ref _name, value); }
    }

    public string Example
    {
        get { return _example; }
        set { Set(ref _example, value); }
    }

    public bool IsShown
    {
        get { return _isShown; }
        set { Set(ref _isShown, value); }
    }

    public bool CanMoveUp
    {
        get { return _canMoveUp; }
        set { Set(ref _canMoveUp, value); }
    }

    public bool CanMoveDown
    {
        get { return _canMoveDown; }
        set { Set(ref _canMoveDown, value); }
    }
}
