﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WindowUI;

public sealed partial class ShortcutWithTextLabelControl : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ShortcutWithTextLabelControl), new PropertyMetadata(default(string)));

#pragma warning disable CA2227 // Collection properties should be read only
    public List<object> Keys
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get => (List<object>)GetValue(KeysProperty);
        set => SetValue(KeysProperty, value);
    }

    public static readonly DependencyProperty KeysProperty = DependencyProperty.Register("Keys", typeof(List<object>), typeof(ShortcutWithTextLabelControl), new PropertyMetadata(default(string)));

    public ShortcutWithTextLabelControl()
    {
        this.InitializeComponent();
    }
}
