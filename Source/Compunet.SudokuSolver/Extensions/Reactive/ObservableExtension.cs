using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Compunet.SudokuSolver.Extensions.Reactive
{
    public static class ObservableExtensions
    {
        public static IObservable<bool> SwitchValue(this IObservable<bool> source)
        {
            return source.Select(value => !value);
        }

        public static IObservable<bool> Or(this IObservable<bool> source1, IObservable<bool> source2)
        {
            return Observable.CombineLatest(
                source1,
                source2,
                (s1, s2) => s1 || s2);
        }
    }

    public static class ObserverExtensions
    {
        public static IDisposable Subscribe(this IObservable<Unit> source, Action onNext)
        {
            return source.Subscribe(_ => onNext());
        }
    }
}
