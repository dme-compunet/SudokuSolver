using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Utilities
{
    public static class Tasks
    {
        public static Task Empty => Task.CompletedTask;

        public static Task<T> AsTaskResult<T>(this T result)
        {
            return Task.FromResult(result);
        }
    }
}
