using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Compunet.SudokuSolver.Mvvm.Commands
{
    public class RxCommand<T> : IRxCommand<T>
    {
        protected readonly Subject<T> mSubject = new();

        private bool mCanExecute = true;
        private IDisposable? mCanExecuteSubscription;

        public event EventHandler? CanExecuteChanged;

        public IObservable<T> ExecuteObservable => mSubject.AsObservable();

        public bool CanExecute(object? parameter)
        {
            return mCanExecute;
        }

        public void WithCanExecute(IObservable<bool> canExecuteObservable)
        {
            mCanExecuteSubscription?.Dispose();

            mCanExecuteSubscription =
                canExecuteObservable.Subscribe(value =>
                {
                    mCanExecute = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });
        }

        public virtual void Execute(object? parameter)
        {
            if (parameter is null)
            {
                Debugger.Break();
                return;
            }

            //mSubject.OnNext((T)parameter);
            Execute((T)parameter);
        }

        public void Execute(T parameter)
        {
            if (mCanExecute)
                mSubject.OnNext(parameter);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return mSubject
                .AsObservable()
                .Subscribe(observer);
        }
    }
}
