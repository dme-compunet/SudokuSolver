using System.Windows;

namespace Compunet.SudokuSolver.Behaviors
{
    public class SystemWindowButtonMaximaizeBehavior : SystemWindowButtonBehavior
    {
        protected override void OnButtonClick(Window window)
        {
            window.WindowState = window.WindowState
                                 == WindowState.Maximized
                                 ? WindowState.Normal
                                 : WindowState.Maximized;
        }
    }


}
