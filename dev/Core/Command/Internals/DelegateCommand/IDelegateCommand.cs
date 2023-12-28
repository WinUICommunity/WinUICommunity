using System.Diagnostics.CodeAnalysis;

namespace WinUICommunity;

public interface IDelegateCommand : ICommand
{
    [SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "This method raise an existing event")]
    void RaiseCanExecuteChanged();
}
