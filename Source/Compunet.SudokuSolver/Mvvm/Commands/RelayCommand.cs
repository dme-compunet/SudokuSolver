using System;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Mvvm.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> mExecute;
        private readonly Predicate<T>? mCanExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute)
        {
            mExecute = execute ?? throw new ArgumentNullException(nameof(execute));
            mCanExecute = canExecute;
        }

        //public bool CanExecute(T? parameter)
        //{
        //    return mCanExecute is null || mCanExecute((T)parameter!);
        //}

        //public void Execute(T? parameter)
        //{
        //    mExecute((T)parameter!);
        //}

        public bool CanExecute(object? parameter)
        {
            return mCanExecute is null || mCanExecute((T)parameter!);
        }

        public void Execute(object? parameter)
        {
            mExecute((T)parameter!);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action mExecute;
        private readonly Func<bool>? mCanExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            mExecute = execute ?? throw new ArgumentNullException(nameof(execute));
            mCanExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return mCanExecute is null || mCanExecute();
        }

        public void Execute(object? parameter)
        {
            mExecute();
        }
    }
}
