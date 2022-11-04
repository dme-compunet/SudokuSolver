using Compunet.SudokuSolver.Core;
using Compunet.SudokuSolver.Extensions.Reactive;
using Compunet.SudokuSolver.Mvvm.Commands;
using Compunet.SudokuSolver.Services;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Mvvm
{
    public class SudokuViewModel : BindableBase
    {
        private readonly ISudokuStoreService mStore;
        private readonly ISudokuInputService mInput;

        public int SelectedIndex { get; set; } = -1;
        public AsyncBindable<IBoard> Board { get; set; }

        public AsyncBindable<bool> IsBusy { get; set; }
        public AsyncBindable<bool> CanSolve { get; set; }
        public AsyncBindable<string> SolveButtonText { get; set; }

        public AsyncBindable<bool> CanUndo { get; set; }
        public AsyncBindable<bool> CanRedo { get; set; }
        public AsyncBindable<bool> CanReset { get; set; }

        public IRxCommand UndoCommand { get; set; } = new RxCommand();
        public IRxCommand RedoCommand { get; set; } = new RxCommand();
        public IRxCommand ResetCommand { get; set; } = new RxCommand();
        public IRxCommand SolveCommand { get; set; } = new RxCommand();

        public RxCommand<Key> KeyDownCommand { get; set; } = new();

        public SudokuViewModel(ISudokuStoreService sudokuStore, ISudokuInputService inputService)
        {
            mInput = inputService;
            mStore = sudokuStore;

            Board = mStore.Board.ToBindable();
            IsBusy = mStore.IsBusy.ToBindable();

            CanUndo = mStore.CanUndo.ToBindable();
            CanRedo = mStore.CanRedo.ToBindable();
            CanReset = mStore.CanReset.ToBindable();
            CanSolve = mStore.CanSolve.ToBindable();

            SolveButtonText = mStore.StoreStatus.Select(status => status switch
            {
                SolutionStatus.Solving => "פותר...",
                SolutionStatus.FailedOrTimeout => "אופס... משהו בעייתי בלוח הזה",
                SolutionStatus.Completed => "הצליח",
                SolutionStatus.Default => "נסה לפתור את זה",
                _ => "???"
            }).ToBindable();

            KeyDownCommand.Subscribe(onNext: key =>
            {
                var input = mInput.TryCreateInput(SelectedIndex, key);

                if (input != null)
                {
                    var cell = input.Value.cell;
                    var value = input.Value.value;

                    mStore.DispatchNext(cell, value);
                }
            });

            UndoCommand.Subscribe(mStore.DispatchUndo);
            RedoCommand.Subscribe(mStore.DispatchRedo);
            ResetCommand.Subscribe(mStore.DispatchReset);
            SolveCommand.Subscribe(mStore.DispatchTrySolve);
        }
    }
}
