using System.Windows;

namespace Compunet.SudokuSolver.Behaviors
{
    public class SystemWindowButtonMinimaizeBehavior : SystemWindowButtonBehavior
    {
        protected override void OnButtonClick(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }
}
