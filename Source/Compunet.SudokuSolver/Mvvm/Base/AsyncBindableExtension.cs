using System;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Mvvm
{
    public static class AsyncBindableExtension
    {
        public static AsyncBindable<T> ToBindable<T>(this IObservable<T> observable) => new(observable);
        public static AsyncBindable<T> ToBindable<T>(this Task<T> task) => new(task);
    }
}
