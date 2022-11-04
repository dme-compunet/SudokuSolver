using System;
using System.Reactive;

namespace Compunet.SudokuSolver.Mvvm.Commands
{
    public interface IRxCommand : IRxCommand<Unit>, IObservable<Unit>
    {
        new void WithCanExecute(IObservable<bool> canExecuteObservable);
    }
}
