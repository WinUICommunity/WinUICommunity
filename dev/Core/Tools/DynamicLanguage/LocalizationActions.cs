using Microsoft.UI.Xaml.Documents;

namespace WinUICommunity;

public static class LocalizationActions
{
    public record ActionArguments(DependencyObject DependencyObject, string Value);

    public record ActionItem(Type TargetType, Action<ActionArguments> Action);

    public static List<ActionItem> DefaultActions { get; } = new()
    {
        new ActionItem(typeof(Run), arguments =>
        {
            if (arguments.DependencyObject is Run target)
            {
                target.Text = arguments.Value;
            }
        }),
        new ActionItem(typeof(Span), arguments =>
        {
            if (arguments.DependencyObject is Span target)
            {
                target.Inlines.Clear();
                target.Inlines.Add(new Run() { Text = arguments.Value });
            }
        }),
        new ActionItem(typeof(Bold), arguments =>
        {
            if (arguments.DependencyObject is Hyperlink target)
            {
                target.Inlines.Clear();
                target.Inlines.Add(new Run() { Text = arguments.Value });
            }
        }),
        new ActionItem(typeof(Hyperlink), arguments =>
        {
            if (arguments.DependencyObject is Hyperlink target)
            {
                target.Inlines.Clear();
                target.Inlines.Add(new Run() { Text = arguments.Value });
            }
        }),
    };
}