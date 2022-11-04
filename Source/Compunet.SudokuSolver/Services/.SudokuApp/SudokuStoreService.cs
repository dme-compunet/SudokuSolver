using Compunet.SudokuSolver.Core;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Services
{
    public class SudokuStoreService : ISudokuStoreService
    {
        private readonly BehaviorSubject<IBoard> mBoardSubject;
        private readonly BehaviorSubject<SolutionStatus> mBusyStatusSubject;
        private readonly BehaviorSubject<IImmutableStack<IBoard>> mUndoStackSubject;
        private readonly BehaviorSubject<IImmutableStack<IBoard>> mRedoStackSubject;

        //private IObservable<bool> IsSolving => mBusyStatusSubject.Select(status => status == SolutionStatus.Solving);

        public IObservable<IBoard> Board => mBoardSubject.AsObservable();
        public IObservable<SolutionStatus> StoreStatus => mBusyStatusSubject.AsObservable().DistinctUntilChanged();

        public IObservable<bool> IsBusy => mBusyStatusSubject.Select(status => status == SolutionStatus.Solving);
        public IObservable<bool> CanSolve { get; }

        public IObservable<bool> CanUndo => mUndoStackSubject.Select(stack => stack.IsEmpty == false);
        public IObservable<bool> CanRedo => mRedoStackSubject.Select(stack => stack.IsEmpty == false);
        public IObservable<bool> CanReset => mBoardSubject.Select(board => board.IsEmpty == false);

        public SudokuStoreService(ISettingsService settings, IExitService exit)
        {
            //mBoardSubject = new(SudokuBoardCreator.CreateEmpty());
            mBoardSubject = new(SudokuBoardCreator.CreateFromText(settings.SudokuBoard));
            exit.Exiting += () =>
            {
                var text = SudokuBoardCreator.TextFromBoard(mBoardSubject.Value);
                settings.SudokuBoard = text;
            };

            mBusyStatusSubject = new(SolutionStatus.Default);

            mUndoStackSubject = new(ImmutableStack<IBoard>.Empty);
            mRedoStackSubject = new(ImmutableStack<IBoard>.Empty);

            CanSolve = mBoardSubject.Select(board =>
            {
                return !(board.IsEmpty
                         || board.IsFull
                         || board.HasWarning);
            });
        }

        public void DispatchUndo()
        {
            var currentBoard = mBoardSubject.Value;
            var undoStack = mUndoStackSubject.Value;
            var redoStack = mRedoStackSubject.Value;

            if (undoStack.IsEmpty)
                return;

            var previousBoard = undoStack.Peek();

            mBoardSubject.OnNext(previousBoard);
            mUndoStackSubject.OnNext(undoStack.Pop());
            mRedoStackSubject.OnNext(redoStack.Push(currentBoard));
            mBusyStatusSubject.OnNext(SolutionStatus.Default);
        }

        public void DispatchRedo()
        {
            var currentBoard = mBoardSubject.Value;
            var undoStack = mUndoStackSubject.Value;
            var redoStack = mRedoStackSubject.Value;

            if (redoStack.IsEmpty)
                return;

            var previousBoard = redoStack.Peek();

            mBoardSubject.OnNext(previousBoard);
            mUndoStackSubject.OnNext(undoStack.Push(currentBoard));
            mRedoStackSubject.OnNext(redoStack.Pop());
            mBusyStatusSubject.OnNext(SolutionStatus.Default);
        }

        public void DispatchReset()
        {
            NextBoard(SudokuBoardCreator.CreateEmpty());
        }

        public void DispatchNext(Cell cell, Value value)
        {
            var current = mBoardSubject.Value;
            var next = SudokuBoardCreator.UpdateBoard(current, cell, value);

            if (next.Equals(current))
                return;

            NextBoard(next);
        }

        public async void DispatchTrySolve()
        {
            mBusyStatusSubject.OnNext(SolutionStatus.Solving);

            await Task.Delay(100);

            var currentBoard = mBoardSubject.Value;

            var solution = await SudokuBoardGenerator.TrySolveState(currentBoard);

            if (solution is null)
                mBusyStatusSubject.OnNext(SolutionStatus.FailedOrTimeout);
            else
            {
                mBusyStatusSubject.OnNext(SolutionStatus.Completed);
                NextBoard(solution);
            }
        }

        private void NextBoard(IBoard board, bool addToUndoStack = default, bool addToRedoStack = default)
        {
            var currentBoard = mBoardSubject.Value;
            var undoStack = mUndoStackSubject.Value;
            //var redoStack = mRedoStackSubject.Value;

            mBoardSubject.OnNext(board);
            mBusyStatusSubject.OnNext(SolutionStatus.Default);
            mUndoStackSubject.OnNext(undoStack.Push(currentBoard));
        }

        public async void DispatchCreatePuzzle(SudokuPuzzleLevel level)
        {
            var board = await SudokuBoardGenerator.GeneratePuzzleState(level);
            NextBoard(board);
        }
    }

    [Flags]
    public enum SudokuStoreStatus
    {
        Default = 0,
        BoardEmpty = 1,
        BoardFull = 2,
        BoardHasWarning = 4,
        IsBusy = 8,
        SolutionFiledOrTimeout = 16,
        SolutionComplete = 32,
    }
}
