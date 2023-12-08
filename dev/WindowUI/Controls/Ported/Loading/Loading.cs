namespace WindowUI;

/// <summary>
/// Loading control allows to show an loading animation with some xaml in it.
/// </summary>
[TemplateVisualState(Name = "LoadingIn", GroupName = "CommonStates")]
[TemplateVisualState(Name = "LoadingOut", GroupName = "CommonStates")]
public partial class Loading : ContentControl
{
    private FrameworkElement? _presenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="Loading"/> class.
    /// </summary>
    public Loading()
    {
        DefaultStyleKey = typeof(Loading);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Update();
    }

    private void Update()
    {
        VisualStateManager.GoToState(this, IsLoading ? "LoadingIn" : "LoadingOut", true);
    }
}
