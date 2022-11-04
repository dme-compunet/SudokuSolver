using System.Windows.Input;

namespace Compunet.SudokuSolver.Windows
{
    public class RedoKeyBinding : KeyBinding
    {
        public RedoKeyBinding()
        {
            Key = Key.Z;
            Modifiers = ModifierKeys.Control | ModifierKeys.Shift;
        }
    }
}
