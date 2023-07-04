using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace WinUICommunity;

public class CheckBoxWithDescriptionControl : CheckBox
{
    private CheckBoxWithDescriptionControl _checkBoxSubTextControl;

    public CheckBoxWithDescriptionControl()
    {
        _checkBoxSubTextControl = (CheckBoxWithDescriptionControl) this;
        this.Loaded += CheckBoxSubTextControl_Loaded;
    }

    protected override void OnApplyTemplate()
    {
        Update();
        base.OnApplyTemplate();
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(Header))
        {
            AutomationProperties.SetName(this, Header);
        }
    }

    private void CheckBoxSubTextControl_Loaded(object sender, RoutedEventArgs e)
    {
        var panel = new StackPanel() { Orientation = Orientation.Vertical };

        // Add text box only if the description is not empty. Required for additional plugin options.
        if (!string.IsNullOrWhiteSpace(Description))
        {
            panel.Children.Add(new TextBlock() { Margin = new Thickness(0, 10, 0, 0), Text = Header, TextWrapping = TextWrapping.WrapWholeWords });
            panel.Children.Add(new IsEnabledTextBlock() { Style = (Style) Application.Current.Resources["SecondaryIsEnabledTextBlockStyle"], Text = Description });
        }
        else
        {
            panel.Children.Add(new TextBlock() { Margin = new Thickness(0, 0, 0, 0), Text = Header, TextWrapping = TextWrapping.WrapWholeWords });
        }

        _checkBoxSubTextControl.Content = panel;
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        "Header",
        typeof(string),
        typeof(CheckBoxWithDescriptionControl),
        new PropertyMetadata(default(string)));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        "Description",
        typeof(string),
        typeof(CheckBoxWithDescriptionControl),
        new PropertyMetadata(default(string)));

    [Localizable(true)]
    public string Header
    {
        get => (string) GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    [Localizable(true)]
    public string Description
    {
        get => (string) GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}
