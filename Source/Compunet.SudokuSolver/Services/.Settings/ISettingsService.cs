using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Services
{
    public interface ISettingsService
    {
        int ActivatedTheme { get; set; }
        string? SudokuBoard { get; set; }

        Task Load();
        Task Save();
    }
}
