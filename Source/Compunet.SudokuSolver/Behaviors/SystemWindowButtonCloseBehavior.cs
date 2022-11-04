using System.Windows;

namespace Compunet.SudokuSolver.Behaviors
{
    public class SystemWindowButtonCloseBehavior : SystemWindowButtonBehavior
    {
        protected override void OnButtonClick(Window window)
        {
            window?.Close();
        }
    }
}
