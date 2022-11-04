using System;
using System.Collections.Generic;

namespace Compunet.SudokuSolver.Core
{
    public interface IBoard : IEquatable<IBoard>
    {
        bool IsEmpty { get; }
        bool IsFull { get; }
        bool HasWarning { get; }
        IEnumerable<IBoardCallValue> Values { get; }
        Value GetValue(Cell cell);
    }
}
