using Compunet.Platform;
using Compunet.SudokuSolver.Mvvm;

namespace Compunet.SudokuSolver.Controls
{
    public abstract class BaseWindow : BasePlatformWindow
    {
        public BaseWindow()
        {
            DataContext = new WindowViewModel(this);
        }
    }
}
