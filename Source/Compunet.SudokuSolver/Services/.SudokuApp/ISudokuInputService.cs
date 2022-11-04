using Compunet.SudokuSolver.Core;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Services
{
    public interface ISudokuInputService
    {
        Value? MapValue(Key key);
        (Cell cell, Value value)? TryCreateInput(int cell, Key key);
    }
}
