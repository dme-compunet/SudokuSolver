using Compunet.SudokuSolver.Extensions.Reactive;
using Compunet.SudokuSolver.Mvvm.Commands;
using Compunet.SudokuSolver.Services;
using System;
using System.Windows;

namespace Compunet.SudokuSolver.Mvvm
{
    public class AppWindowViewModel : BindableBase
    {
        private readonly ISudokuStoreService mStore;

        public AsyncBindable<bool> IsBusy { get; set; }

        public IRxCommand<Window> CompressCommand { get; set; } = new RxCommand<Window>();
        public IRxCommand NextThemeCommand { get; set; } = new RxCommand();
        public IRxCommand CreatePuzzleCommand { get; set; } = new RxCommand();

        public AppWindowViewModel(ISudokuStoreService store, IThemeService theme, ICreatePuzzleDialogService puzzleDialog)
        {
            mStore = store;
            IsBusy = mStore.IsBusy.ToBindable();

            CompressCommand.Subscribe(window =>
            {
                window.WindowState = WindowState.Normal;
                window.Width = window.MinWidth;
            });

            NextThemeCommand.Subscribe(async () => await theme.NextTheme());

            CreatePuzzleCommand.Subscribe(() =>
            {
                var level = puzzleDialog.AskLevel();

                if (level != null)
                {
                    store.DispatchCreatePuzzle(level.Value);
                }
            });
        }
    }
}
