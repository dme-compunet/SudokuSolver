using Compunet.SudokuSolver.Extensions.Generic;
using System;
using System.Linq;

namespace Compunet.SudokuSolver.Core
{
    public struct Value : IEquatable<Value>
    {
        public static readonly Value Zero = new();
        public bool IsZero => value == 0;

        private readonly int value;

        public Value(int value)
        {
            //if (value < 0 || value > 9)
            //    throw new ArgumentOutOfRangeException(nameof(value));

            this.value = value % 10;
        }

        public int GetValue() => value;

        public static implicit operator Value(int value) => new(value);

        public static Value operator ++(Value value) => new(value.value + 1);
        public static Value operator --(Value value) => new(value.value - 1);

        public static Value[] GenerateRandomRange()
        {
            var arr = Enumerable.Range(1, 9)
                                 .Select(index => new Value(index))
                                 .ToArray();

            arr.Shuffle();

            return arr;
        }

        public string ToDisplayString()
        {
            return value == 0 ? string.Empty : ToString();
        }

        #region Equals

        public static bool operator ==(Value a, Value b) => a.Equals(b);
        public static bool operator !=(Value a, Value b) => !a.Equals(b);

        public override bool Equals(object? obj) => obj is Value other && Equals(other);
        public bool Equals(Value other) => value == other.value;

        #endregion

        public override int GetHashCode() => value.GetHashCode();
        public override string ToString() => value.ToString();
    }


}
