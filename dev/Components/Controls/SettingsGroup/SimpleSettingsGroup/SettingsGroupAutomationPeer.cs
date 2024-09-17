using Microsoft.UI.Xaml.Automation.Peers;

namespace WinUICommunity;

public partial class SimpleSettingsGroupAutomationPeer : FrameworkElementAutomationPeer
{
    public SimpleSettingsGroupAutomationPeer(SimpleSettingsGroup owner)
        : base(owner)
    {
    }

    protected override string GetNameCore()
    {
        var selectedSimpleSettingsGroup = (SimpleSettingsGroup)Owner;
        return selectedSimpleSettingsGroup.Header;
    }
}
