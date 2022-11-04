using System.Collections.Generic;

namespace Compunet.SudokuSolver.Core
{
    public static class BoardValidationExtensions
    {
        public static bool RowContains(this IBoard board, int rowindex, Value value, Cell? ignore = null)
        {
            for (int i = 0; i < 9; i++)
            {
                Cell cell = new(rowindex, i);

                if (cell == ignore)
                    continue;

                if (board.GetValue(cell) == value)
                    return true;
            }

            return false;
        }

        public static bool ColumnContains(this IBoard board, int columnindex, Value value, Cell? ignore = null)
        {
            for (int i = 0; i < 9; i++)
            {
                Cell cell = new(i, columnindex);

                if (cell == ignore)
                    continue;

                if (board.GetValue(cell) == value)
                    return true;
            }

            return false;
        }

        public static bool SquareContains(this IBoard board, int sequreindex, Value value, Cell? ignore = null)
        {
            int r = sequreindex / 3 * 3;
            int c = sequreindex % 3 * 3;

            for (int ri = 0; ri < 3; ri++)
            {
                for (int ci = 0; ci < 3; ci++)
                {
                    Cell cell = new(ri + r, ci + c);

                    if (cell == ignore)
                        continue;

                    if (board.GetValue(cell) == value)
                        return true;
                }
            }

            return false;
        }

        public static bool CheckValue(this IBoard board, Cell position, Value value)
        {
            return !(board.RowContains(position.Row, value)
                     || board.ColumnContains(position.Column, value)
                     || board.SquareContains(position.Square, value));
        }

        public static IEnumerable<Cell> GetWarns(this IBoard state)
        {
            foreach (var cell in Cell.AllRange)
            {
                Value value = state.GetValue(cell);

                if (value.IsZero)
                    continue;

                if (state.RowContains(cell.Row, value, cell))
                    yield return cell;

                if (state.ColumnContains(cell.Column, value, cell))
                    yield return cell;

                if (state.SquareContains(cell.Square, value, cell))
                    yield return cell;

            }
            yield break;
        }

        public static void SetupWrongCells(this IEditableBoard board)
        {
            // clean all old wrongs
            foreach (var cell in Cell.AllRange)
            {
                board.SetWrong(cell, false);
            }

            foreach (var cell in board.GetWarns())
            {
                board.SetWrong(cell, true);
            }
        }
    }
}
