using WinUICommunity;

namespace WinUICommunityGallery.Models;

public class ImageSize : Observable
{
    private int _id;
    private string _name;
    private int _fit;
    private double _height;
    private double _width;
    private int _unit;

    public int Id
    {
        get { return _id; }
        set { Set(ref _id, value); }
    }

    public string Name
    {
        get { return _name; }
        set { Set(ref _name, value); }
    }

    public int Fit
    {
        get { return _fit; }
        set { Set(ref _fit, value); }
    }

    public double Width
    {
        get { return _width; }
        set { Set(ref _width, value); }
    }

    public double Height
    {
        get { return _height; }
        set { Set(ref _height, value); }
    }

    public int Unit
    {
        get { return _unit; }
        set { Set(ref _unit, value); }
    }
}
