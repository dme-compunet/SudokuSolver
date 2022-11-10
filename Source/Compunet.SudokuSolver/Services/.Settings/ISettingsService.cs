using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Services
{
    public interface ISettingsService
    {
        string? SudokuBoard { get; set; }
        string? CurrentTheme { get; set; }
        string? CurrentLanguage { get; set; }

        Task Load();
        Task Save();
    }
}
