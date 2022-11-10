using System.Windows;

namespace Compunet.SudokuSolver.Application
{
    public interface IApplicationResource
    {
        string Name { get; }
        ApplicationResourceCategory Category { get; }
        ResourceDictionary CreateResourceDictionary();
    }
}
