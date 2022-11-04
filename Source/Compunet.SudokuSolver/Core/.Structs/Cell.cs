using System;
using System.Collections.Immutable;
using System.Linq;

namespace Compunet.SudokuSolver.Core
{
    public struct Cell : IEquatable<Cell>
    {
        #region All

        public static ImmutableList<Cell> AllRange { get; }
            = Enumerable.Range(0, 9 * 9)
                        .Select(index => new Cell(index))
                        .ToImmutableList();

        //public static ImmutableArray<Cell> AllRange { get; }
        //    = Enumerable.Range(0, 9 * 9)
        //                .Select(index => new Cell(index))
        //                .ToImmutableArray();

        #endregion

        //public static readonly Cell First = new(0);
        //public static readonly Cell Last = new(80);

        public int Row => Index / 9;
        public int Column => Index % 9;
        public int Square => Column / 3 + (Row / 3 * 3);
        public int Index { get; }

        public bool IsFirst => Index == 0;
        public bool IsLast => Index == 80;

        public Cell(int index)
        {
            if (index < 0
                || index > 80)
                throw new ArgumentOutOfRangeException(nameof(index));

            Index = index;
        }

        public Cell(int row, int column) : this(row * 9 + column) { }

        #region Back / Next

        public Cell Next()
        {
            if (IsLast)
                throw new InvalidOperationException("current is last");

            if (Column == 8)
                return new Cell(Row + 1, 0);

            return new Cell(Row, Column + 1);
        }

        public Cell Back()
        {
            if (IsFirst)
                throw new InvalidOperationException("current is first");

            if (Column == 0)
                return new Cell(Row - 1, 8);

            return new Cell(Row, Column - 1);
        }

        public static Cell operator ++(Cell pointer) => pointer.Next();

        public static Cell operator --(Cell pointer) => pointer.Back();

        #endregion

        #region Equals

        public static bool operator ==(Cell a, Cell b) => a.Equals(b);

        public static bool operator !=(Cell a, Cell b) => !a.Equals(b);

        public override bool Equals(object? obj)
        {
            return obj is Cell other
                   && Equals(other);
        }

        public bool Equals(Cell other)
        {
            return Row == other.Row
                   && Column == other.Column;
        }

        #endregion

        #region Overriden

        public override int GetHashCode()
        {
            return Row.GetHashCode()
                   ^ Column.GetHashCode();
        }

        public override string ToString()
        {
            return $"Row: {Row}, Column: {Column}, Square: {Square}";
        }

        #endregion
    }
}
