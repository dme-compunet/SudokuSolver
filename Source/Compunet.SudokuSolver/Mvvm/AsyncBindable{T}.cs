using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Mvvm
{
    public class AsyncBindable<T> : BindableBase
    {
        public T? Value { get; set; }

        public AsyncBindable(IObservable<T> observable)
        {
            observable.Subscribe(OnNext);
        }

        public AsyncBindable(Task<T> task) : this(task.ToObservable())
        {

        }

        public T? Get() => Value;

        private void OnNext(T value)
        {
            Value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public static class AsyncBindableExtension
    {
        public static AsyncBindable<T> ToBindable<T>(this IObservable<T> observable) => new(observable);
        public static AsyncBindable<T> ToBindable<T>(this Task<T> task) => new(task);
    }
}
