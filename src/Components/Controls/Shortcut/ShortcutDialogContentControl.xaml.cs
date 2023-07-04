using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace WinUICommunity;

public sealed partial class ShortcutDialogContentControl : UserControl
{
    public ShortcutDialogContentControl()
    {
        InitializeComponent();
    }

#pragma warning disable CA2227 // Collection properties should be read only
    public List<object> Keys
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get { return (List<object>) GetValue(KeysProperty); }
        set { SetValue(KeysProperty, value); }
    }

    public static readonly DependencyProperty KeysProperty = DependencyProperty.Register("Keys", typeof(List<object>), typeof(SettingsPageControl), new PropertyMetadata(default(string)));

    public bool IsError
    {
        get => (bool) GetValue(IsErrorProperty);
        set => SetValue(IsErrorProperty, value);
    }

    public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register("IsError", typeof(bool), typeof(ShortcutDialogContentControl), new PropertyMetadata(false));
}
