using System;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Mvvm.Commands
{
    public interface IRxCommand<T> : ICommand, IObservable<T>
    {
        void WithCanExecute(IObservable<bool> canExecuteObservable);
        void Execute(T parameter);
    }

}
