using Compunet.SudokuSolver.Application;
using Compunet.SudokuSolver.Extensions.Reactive;
using Compunet.SudokuSolver.Mvvm.Base;
using Compunet.SudokuSolver.Mvvm.Commands;
using Compunet.SudokuSolver.Print;
using Compunet.SudokuSolver.Services;
using System;
using System.Reactive.Linq;
using System.Windows;

namespace Compunet.SudokuSolver.Mvvm
{
    public class AppSidebarViewModel : BindableBase
    {
        public AsyncBindable<bool> IsBusy { get; set; }

        public IRxCommand<Window> CompressCommand { get; set; } = new RxCommand<Window>();
        public IRxCommand NextThemeCommand { get; set; } = new RxCommand();
        public IRxCommand NextLangCommand { get; set; } = new RxCommand();
        public IRxCommand CreatePuzzleCommand { get; set; } = new RxCommand();
        public IRxCommand PrintCommand { get; set; } = new RxCommand();

        public AppSidebarViewModel(ISudokuStoreService store, IApplicationResourceManager manager, ICreatePuzzleDialogService puzzleDialog, IPrintService printService)
        {
            IsBusy = store.IsBusy.ToBindable();

            CompressCommand.Subscribe(window =>
            {
                window.WindowState = WindowState.Normal;
                window.Width = window.MinWidth;
            });

            //NextThemeCommand.Subscribe(async () => await theme.NextTheme());
            NextThemeCommand.Subscribe(async () => await manager.NextTheme());
            NextLangCommand.Subscribe(async () => await manager.NextLanguage());

            CreatePuzzleCommand.Subscribe(() =>
            {
                var level = puzzleDialog.AskLevel();

                if (level != null)
                {
                    store.DispatchCreatePuzzle(level.Value);
                }
            });

            PrintCommand.Subscribe(async () => printService.PrintBoard(await store.Board.FirstAsync()));
        }
    }
}
