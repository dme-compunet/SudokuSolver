using Compunet.SudokuSolver.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Core
{
    public class BoardGeneratorEngine
    {
        private readonly TaskCompletionSource tcs = new();

        private readonly IEditableBoard mBoard;
        private readonly Value[] mValues;
        private readonly Cell[] mCells;

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(4); // default timeout

        public bool IsCompleted => Status == BoardGeneratorStatus.Completed;
        public BoardGeneratorStatus Status { get; private set; }
        public IBoard? Resolte { get; private set; }

        public BoardGeneratorEngine() : this(SudokuBoardCreator.CreateEmpty()) { }

        public BoardGeneratorEngine(IBoard board)
        {
            mBoard = SudokuBoardCreator.CreateEditable(board);
            mValues = Value.GenerateRandomRange();

            // setup empty cells
            mCells = board.Values.Where(cv => cv.Value.IsZero == true)
                                .Select(cv => cv.Cell)
                                .ToArray();
        }

        public Task StartAsync()
        {
            if (mCells.Length == 0)
                return Tasks.Empty;

            Status = BoardGeneratorStatus.Processing;

            bool cancel = false;

            Task.Run(async () =>
            {
                await Task.Delay(Timeout);
                cancel = true;
                SetResolteStatus(BoardGeneratorStatus.Timeout);
            });

            Task.Run(() => Start(ref cancel));

            return tcs.Task;
        }

        private void Start(ref bool cancelflag)
        {
            int index = 0;

            while (!cancelflag)
            {
                bool succes = TryRunValue(mCells[index]);

                if (succes)
                {
                    if (index == mCells.Length - 1)
                    {
                        SetResolteStatus(BoardGeneratorStatus.Completed);
                        break;
                    }
                    else
                        index++;
                }
                else
                {
                    if (index == 0)
                    {
                        SetResolteStatus(BoardGeneratorStatus.Failed);
                        break;
                    }
                    index--;
                }
            }
        }

        private bool TryRunValue(Cell cell)
        {
            Value currentValue = mBoard.GetValue(cell);

            int valueIndex = currentValue.IsZero
                             ? 0
                             : mValues.ToList().IndexOf(currentValue);

            while (valueIndex < 9)
            {
                var value = mValues[valueIndex];

                if (mBoard.CheckValue(cell, value))
                {
                    SetValue(cell, value);
                    return true;
                }
                valueIndex++;
            }

            SetValue(cell, Value.Zero);
            return false;
        }

        private void SetValue(Cell cell, Value value)
        {
            mBoard.SetValue(cell, value);
        }

        private void SetResolteStatus(BoardGeneratorStatus status)
        {
            if (status == BoardGeneratorStatus.Completed)
            {
                Resolte = mBoard;
            }

            Status = status;
            tcs.SetResult();
        }
    }
}
