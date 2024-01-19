using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace WinUICommunity;
internal sealed class StringToBrushConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not string strValue)
            return null;

        return new SolidColorBrush(GeneralHelper.GetColorFromHex(strValue));
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
