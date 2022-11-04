using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Controls;
using Compunet.SudokuSolver.Core;
using Compunet.SudokuSolver.Mvvm;
using Compunet.SudokuSolver.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive.Linq;

namespace Compunet.SudokuSolver.Services
{
    //public interface IDialogService
    //{
    //    TResolte? Ask<TResolte, TView, TViewModel>()
    //        where TView : FrameworkElement, new()
    //        where TViewModel : BindableDialogBase<TResolte>;

    //    SudokuPuzzleLevel? AskSudokuPuzzleLevel();

    //}

    //public class DialogService : IDialogService
    //{
    //    public TResolte? Ask<TResolte, TView, TViewModel>()
    //        where TView : FrameworkElement, new()
    //        where TViewModel : BindableDialogBase<TResolte>
    //    {
    //        var vm = IoC.Simple.GetRequiredService<TViewModel>();
    //        var win = new DialogWindow
    //        {
    //            Content = new TView
    //            {
    //                DataContext = vm
    //            }
    //        };

    //        vm.OnEvent += (value) => win.DialogResult = value;

    //        if (win.ShowDialog() == false)
    //            return default;

    //        return vm.Result;
    //    }

    //    public SudokuPuzzleLevel? AskSudokuPuzzleLevel()
    //    {
    //        return Ask<SudokuPuzzleLevel?, CreateSudokuPuzzleContent, CreateSudokuPuzzleViewModel>();
    //    }
    //}

    public interface ICreateSudokuPuzzleDialogService
    {
        SudokuPuzzleLevel? AskLevel();
    }

    public class CreateSudokuPuzzleDialogService : ICreateSudokuPuzzleDialogService
    {
        public SudokuPuzzleLevel? AskLevel()
        {
            var viewModel = IoC.Simple.GetRequiredService<CreateSudokuPuzzleViewModel>();
            var content = new CreateSudokuPuzzleControl(viewModel);
            SudokuPuzzleLevel? result = null;

            var win = new DialogWindow
            {
                Content = content
            };

            viewModel.Result.Subscribe(r =>
            {
                result = r;
                win.DialogResult = result != null;
            });

            if (win.ShowDialog() == false)
                return null;

            //return viewModel.Result;
            return result;
        }
    }
}
