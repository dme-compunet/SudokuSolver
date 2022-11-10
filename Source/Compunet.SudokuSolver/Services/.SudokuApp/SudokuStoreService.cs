using Compunet.SudokuSolver.Core;
using Compunet.SudokuSolver.Utilities;
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
        private readonly BehaviorSubject<SolutionStatus> mStoreStatusSubject;
        private readonly BehaviorSubject<IImmutableStack<IBoard>> mUndoStackSubject;
        private readonly BehaviorSubject<IImmutableStack<IBoard>> mRedoStackSubject;

        private readonly SemaphoreLocker locker = new();


        //private IObservable<bool> IsSolving => mBusyStatusSubject.Select(status => status == SolutionStatus.Solving);

        public IObservable<IBoard> Board => mBoardSubject.AsObservable();
        public IObservable<SolutionStatus> StoreStatus => mStoreStatusSubject.AsObservable().DistinctUntilChanged();

        public IObservable<bool> IsBusy => mStoreStatusSubject.Select(status => status == SolutionStatus.Solving);
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

            mStoreStatusSubject = new(SolutionStatus.Default);

            mUndoStackSubject = new(ImmutableStack<IBoard>.Empty);
            mRedoStackSubject = new(ImmutableStack<IBoard>.Empty);

            CanSolve = mBoardSubject.Select(board =>
            {
                return !(board.IsEmpty
                         || board.IsFull
                         || board.HasWarning);
            });
        }

        public async void DispatchUndo()
        {
            var currentBoard = mBoardSubject.Value;
            var undoStack = mUndoStackSubject.Value;
            var redoStack = mRedoStackSubject.Value;

            await locker.LockAsync(() =>
            {
                if (undoStack.IsEmpty)
                    return;

                var previousBoard = undoStack.Peek();

                mBoardSubject.OnNext(previousBoard);
                mUndoStackSubject.OnNext(undoStack.Pop());
                mRedoStackSubject.OnNext(redoStack.Push(currentBoard));
                mStoreStatusSubject.OnNext(SolutionStatus.Default);
            });
        }

        public async void DispatchRedo()
        {
            var currentBoard = mBoardSubject.Value;
            var undoStack = mUndoStackSubject.Value;
            var redoStack = mRedoStackSubject.Value;

            await locker.LockAsync(() =>
            {
                if (redoStack.IsEmpty)
                    return;

                var previousBoard = redoStack.Peek();

                mBoardSubject.OnNext(previousBoard);
                mUndoStackSubject.OnNext(undoStack.Push(currentBoard));
                mRedoStackSubject.OnNext(redoStack.Pop());
                mStoreStatusSubject.OnNext(SolutionStatus.Default);
            });
        }

        public async void DispatchReset()
        {
            await locker.LockAsync(() => NextBoard(SudokuBoardCreator.CreateEmpty()));
        }

        public async void DispatchNext(Cell cell, Value value)
        {
            var current = mBoardSubject.Value;
            var next = SudokuBoardCreator.UpdateBoard(current, cell, value);

            await locker.LockAsync(() =>
            {
                if (next.Equals(current))
                    return;

                NextBoard(next);
            });
        }

        public async void DispatchTrySolve()
        {
            await locker.LockAsync(async () =>
            {
                mStoreStatusSubject.OnNext(SolutionStatus.Solving);

                await Task.Delay(100);

                var currentBoard = mBoardSubject.Value;

                var solution = await SudokuBoardGenerator.TrySolveState(currentBoard);

                if (solution is null)
                    mStoreStatusSubject.OnNext(SolutionStatus.FailedOrTimeout);
                else
                {
                    mStoreStatusSubject.OnNext(SolutionStatus.Completed);
                    NextBoard(solution, setDefualtStatus: false);
                }
            });
        }

        public async void DispatchCreatePuzzle(SudokuPuzzleLevel level)
        {
            await locker.LockAsync(async () =>
            {
                var board = await SudokuBoardGenerator.GeneratePuzzleState(level);
                NextBoard(board);
            });
        }

        private void NextBoard(IBoard board, bool setDefualtStatus = true, bool addToUndoStack = default, bool addToRedoStack = default)
        {
            var currentBoard = mBoardSubject.Value;
            var undoStack = mUndoStackSubject.Value;
            var status = mStoreStatusSubject.Value;
            //var redoStack = mRedoStackSubject.Value;

            mBoardSubject.OnNext(board);

            if (setDefualtStatus)
                mStoreStatusSubject.OnNext(SolutionStatus.Default);

            mUndoStackSubject.OnNext(undoStack.Push(currentBoard));
        }
    }
}
