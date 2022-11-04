using Compunet.SudokuSolver.Core;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Services
{
    public class SudokuInputService : ISudokuInputService
    {
        public Value? MapValue(Key key)
        {
            return key switch
            {
                Key.Delete => Value.Zero,
                Key.NumPad0 => Value.Zero,
                Key.D0 => Value.Zero,
                Key.NumPad1 => 1,
                Key.NumPad2 => 2,
                Key.NumPad3 => 3,
                Key.NumPad4 => 4,
                Key.NumPad5 => 5,
                Key.NumPad6 => 6,
                Key.NumPad7 => 7,
                Key.NumPad8 => 8,
                Key.NumPad9 => 9,
                Key.D1 => 1,
                Key.D2 => 2,
                Key.D3 => 3,
                Key.D4 => 4,
                Key.D5 => 5,
                Key.D6 => 6,
                Key.D7 => 7,
                Key.D8 => 8,
                Key.D9 => 9,
                _ => null
            };
        }

        public (Cell cell, Value value)? TryCreateInput(int cell, Key key)
        {
            if (cell < 0 || cell > 81)
                return null;

            var value = MapValue(key);

            if (value is null)
                return null;

            return (new Cell(cell), value.Value);
        }
    }
}
