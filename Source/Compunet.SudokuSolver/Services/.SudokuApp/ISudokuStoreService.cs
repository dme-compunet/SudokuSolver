using Compunet.SudokuSolver.Core;
using System;

namespace Compunet.SudokuSolver.Services
{
    public interface ISudokuStoreService
    {
        IObservable<IBoard> Board { get; }
        IObservable<SolutionStatus> StoreStatus { get; }

        IObservable<bool> IsBusy { get; }
        IObservable<bool> CanSolve { get; }

        IObservable<bool> CanUndo { get; }
        IObservable<bool> CanRedo { get; }
        IObservable<bool> CanReset { get; }

        void DispatchUndo();
        void DispatchRedo();
        void DispatchReset();

        void DispatchNext(Cell cell, Value value);
        void DispatchTrySolve();
        void DispatchCreatePuzzle(SudokuPuzzleLevel level);
    }
}
