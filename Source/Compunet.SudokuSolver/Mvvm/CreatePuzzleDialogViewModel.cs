using Compunet.SudokuSolver.Core;
using Compunet.SudokuSolver.Extensions.Reactive;
using Compunet.SudokuSolver.Mvvm.Commands;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Compunet.SudokuSolver.Mvvm
{
    public class CreatePuzzleDialogViewModel : BindableBase
    {
        private readonly Subject<SudokuPuzzleLevel?> mResultSubject = new();

        public IRxCommand EasyCreateCommand { get; set; } = new RxCommand();
        public IRxCommand MediumCreateCommand { get; set; } = new RxCommand();
        public IRxCommand HardCreateCommand { get; set; } = new RxCommand();

        public IObservable<SudokuPuzzleLevel?> Result => mResultSubject.AsObservable();

        public CreatePuzzleDialogViewModel()
        {
            EasyCreateCommand.Subscribe(() => SetResult(SudokuPuzzleLevel.Easy));
            MediumCreateCommand.Subscribe(() => SetResult(SudokuPuzzleLevel.Medium));
            HardCreateCommand.Subscribe(() => SetResult(SudokuPuzzleLevel.Hard));
        }

        private void SetResult(SudokuPuzzleLevel result)
        {
            mResultSubject.OnNext(result);
            mResultSubject.OnCompleted();
        }
    }
}
