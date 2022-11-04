using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compunet.SudokuSolver.Core
{
    public static class SudokuBoardCreator
    {
        #region Editable Board

        private class EditableBoardCellValue : IEditableBoardCallValue
        {
            public Cell Cell { get; }
            public Value Value { get; private set; }
            public bool IsWrong { get; private set; }

            public EditableBoardCellValue(IBoardCallValue other)
            {
                Cell = other.Cell;
                Value = other.Value;
                IsWrong = other.IsWrong;
            }

            public EditableBoardCellValue(Cell cell)
            {
                Cell = cell;
                Value = Value.Zero;
                IsWrong = false;
            }

            public void ChangeValue(Value value)
            {
                Value = value;
            }

            public void ChangeWarning(bool value)
            {
                IsWrong = value;
            }

            public bool Equals(IBoardCallValue? other)
            {
                if (other is null)
                    return false;

                return Cell == other.Cell
                       && Value == other.Value;
            }
        }

        private class EditableBoard : IEditableBoard
        {
            public static EditableBoard Empty = new();

            private readonly EditableBoardCellValue[] mValues;

            public IEnumerable<IBoardCallValue> Values => mValues.AsEnumerable();

            public bool IsEmpty => !mValues.Any(value => !value.Value.IsZero);
            public bool IsFull => !mValues.Any(value => value.Value.IsZero);
            public bool HasWarning => mValues.Any(value => value.IsWrong);

            public EditableBoard(IBoard other)
            {
                var range = other.Values.Select(value =>
                {
                    var cellvalue = new EditableBoardCellValue(value);
                    return cellvalue;
                });

                mValues = range.ToArray();
            }

            public EditableBoard()
            {
                var range = Enumerable.Range(0, 81).Select(index =>
                {
                    var cell = new Cell(index);
                    return new EditableBoardCellValue(cell);
                });

                mValues = range.ToArray();
            }

            public void SetValue(Cell cell, Value value)
            {
                mValues[cell.Index].ChangeValue(value);
            }

            public void SetWrong(Cell cell, bool value)
            {
                mValues[cell.Index].ChangeWarning(value);
            }

            public bool Equals(IBoard? other)
            {
                if (other is null)
                    return false;

                return Values.SequenceEqual(other.Values);
            }

            public Value GetValue(Cell cell)
            {
                return mValues[cell.Index].Value;
            }
        }

        #endregion

        #region Empty

        public static IBoard CreateEmpty()
        {
            return new EditableBoard();
        }

        public static IEditableBoard CreateEditableEmpty()
        {
            return new EditableBoard();
        }

        #endregion

        public static IBoard CreateFromText(string? text)
        {
            var board = CreateEditableEmpty();

            if (text?.Length != 81)
                return board;

            for (int i = 0; i < 81; i++)
            {
                string part = text[i].ToString();

                if (int.TryParse(part, out int number) == false
                    || number > 9
                    || number < 1)
                    continue;

                Value value = new(number);
                board.SetValue(new Cell(i), value);
            }

            board.SetupWrongCells();
            return board;
        }

        public static string TextFromBoard(IBoard board)
        {
            StringBuilder builder = new(81);

            for (int i = 0; i < 81; i++)
            {
                Cell cell = new(i);
                Value value = board.GetValue(cell);
                builder.Append(value.ToString());
            }

            return builder.ToString();
        }

        public static IBoard UpdateBoard(IBoard board, Cell cell, Value value)
        {
            var clone = new EditableBoard(board);
            clone.SetValue(cell, value);
            clone.SetupWrongCells();
            return clone;
        }

        public static IEditableBoard CreateEditable(IBoard board)
        {
            return new EditableBoard(board);
        }

        public static IBoard ConvertFromText(string text)
        {
            IEditableBoard board = CreateEditableEmpty();

            return board;
        }
    }
}
