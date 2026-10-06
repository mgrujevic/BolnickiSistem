using System.Windows.Input;

namespace BolnickiSistem.ViewModels;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _izvrsavanje;
    private readonly Func<object?, bool>? _mozeDaSeIzvrsi;

    public RelayCommand(
        Action<object?> izvrsavanje,
        Func<object?, bool>? mozeDaSeIzvrsi = null)
    {
        _izvrsavanje = izvrsavanje;
        _mozeDaSeIzvrsi = mozeDaSeIzvrsi;
    }

    public bool CanExecute(object? parameter)
    {
        return _mozeDaSeIzvrsi == null ||
               _mozeDaSeIzvrsi(parameter);
    }

    public void Execute(object? parameter)
    {
        _izvrsavanje(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}