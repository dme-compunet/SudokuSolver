using Compunet.SudokuSolver.Core;

namespace Compunet.SudokuSolver.Services
{
    public interface ICreatePuzzleDialogService
    {
        SudokuPuzzleLevel? AskLevel();
    }
}
