using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Services
{
    public interface IThemeService
    {
        int CurrentThemeIndex { get; }
        Task Load();
        Task NextTheme();
    }
}
