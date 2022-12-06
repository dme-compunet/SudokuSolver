using Compunet.SudokuSolver.Mvvm.Base;
using Compunet.SudokuSolver.Mvvm.Commands;
using System.Windows;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Mvvm
{
    public class WindowViewModel : BindableBase
    {
        private readonly Window mWindow;

        public ICommand CloseCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }

        public WindowViewModel(Window window)
        {
            mWindow = window;

            CloseCommand = new RelayCommand(mWindow.Close);

            MaximizeCommand = new RelayCommand(() =>
            {
                mWindow.WindowState = window.WindowState == WindowState.Maximized
                                      ? WindowState.Normal
                                      : WindowState.Maximized;
            });

            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
        }
    }
}
