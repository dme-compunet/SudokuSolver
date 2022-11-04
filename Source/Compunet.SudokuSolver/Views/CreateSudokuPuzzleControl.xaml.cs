using Compunet.SudokuSolver.Mvvm;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Views
{
    public partial class CreateSudokuPuzzleControl : ContentControl
    {
        public CreateSudokuPuzzleControl(CreatePuzzleDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
