using Compunet.SudokuSolver.Mvvm.Base;
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

        public AsyncBindable(Task<T> task) : this(task.ToObservable()) { }

        public T? Get() => Value;

        private void OnNext(T value)
        {
            Value = value;
            OnPropertyChanged(nameof(Value));
        }
    }
}
