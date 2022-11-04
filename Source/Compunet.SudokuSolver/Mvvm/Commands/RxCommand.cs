using System.Reactive;

namespace Compunet.SudokuSolver.Mvvm.Commands
{
    public class RxCommand : RxCommand<Unit>, IRxCommand
    {
        public override void Execute(object? parameter)
        {
            Execute(Unit.Default);
            //if (CanExecute(parameter))
            //    mSubject.OnNext(Unit.Default);
        }
    }
}
