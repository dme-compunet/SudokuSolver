using System;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Application
{
    public interface IApplicationResourceManager
    {
        Task Load();

        event Action<string> ThemeChanged;
        event Action<string> LanguageChanged;

        object GetResource(string resourceKey);
        string GetStringResource(string resourceKey);

        Task SetTheme(string themeName);
        Task NextTheme();

        Task SetLanguage(string langName);
        Task NextLanguage();
    }
}
