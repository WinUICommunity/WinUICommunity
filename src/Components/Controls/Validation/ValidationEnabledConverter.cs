using System;
using System.ComponentModel;
using Microsoft.UI.Xaml.Data;

namespace WinUICommunity;

public sealed class ValidationEnabledConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is INotifyDataErrorInfo)
            return true;

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
