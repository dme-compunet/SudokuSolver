using System.Windows.Input;

namespace Compunet.SudokuSolver.Windows
{
    public class UndoKeyBinding : KeyBinding
    {
        public UndoKeyBinding()
        {
            Key = Key.Z;
            Modifiers = ModifierKeys.Control;
        }
    }
}
