using System;

namespace Compunet.SudokuSolver.Core
{
    public interface IBoardCallValue : IEquatable<IBoardCallValue>
    {
        Cell Cell { get; }
        Value Value { get; }
        bool IsWrong { get; }
    }
}
