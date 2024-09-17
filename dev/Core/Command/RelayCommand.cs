namespace WinUICommunity;

public partial class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public event EventHandler CanExecuteChanged;

    public RelayCommand(Action execute)
        : this(execute, null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute();
    }

    public void Execute(object parameter)
    {
        _execute();
    }

    public void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "abstract T and abstract")]
public partial class RelayCommand<T> : ICommand
{
    private readonly Action<T> execute;

    private readonly Func<T, bool> canExecute;

    public event EventHandler CanExecuteChanged;

    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return canExecute == null || canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        execute((T)parameter);
    }

    public void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
