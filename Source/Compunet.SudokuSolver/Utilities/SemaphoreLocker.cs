using System;
using System.Threading;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Utilities
{
    public class SemaphoreLocker
    {
        private readonly SemaphoreSlim locker = new(1, 1);

        public async Task LockAsync(Action work)
        {
            await locker.WaitAsync();

            try
            {
                work();
            }
            finally
            {
                locker.Release();
            }
        }

        public async Task LockAsync(Func<Task> work)
        {
            await locker.WaitAsync();

            try
            {
                await work();
            }
            finally
            {
                locker.Release();
            }
        }

        public async Task<T> LcokAsync<T>(Func<T> work)
        {
            await locker.WaitAsync();

            try
            {
                return work();
            }
            finally
            {
                locker.Release();
            }
        }

        public async Task<T> LcokAsync<T>(Func<Task<T>> work)
        {
            await locker.WaitAsync();

            try
            {
                return await work();
            }
            finally
            {
                locker.Release();
            }
        }

        public void SubscribeOnLocker<T>(IObservable<T> observable, Action<T> onNext)
        {
            observable.Subscribe(onNext: async value =>
            {
                await LockAsync(() => onNext(value));
            });
        }

        public void SubscribeOnLocker<T>(IObservable<T> observable, Func<T, Task> onNext)
        {
            observable.Subscribe(onNext: async value =>
            {
                await LockAsync(async () => await onNext(value));
            });
        }
    }
}
