using Microsoft.UI.Xaml.Data;

namespace WinUICommunity;
internal class WidthToHalfHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double width)
        {
            var height = width / 2;
            return height;
        }
        return 50.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
