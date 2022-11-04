using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Controls;
using Compunet.SudokuSolver.Core;
using Compunet.SudokuSolver.Mvvm;
using Compunet.SudokuSolver.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Compunet.SudokuSolver.Services
{
    public class CreatePuzzleDialogService : ICreatePuzzleDialogService
    {
        public SudokuPuzzleLevel? AskLevel()
        {
            var viewModel = IoC.Simple.GetRequiredService<CreatePuzzleDialogViewModel>();
            var content = new CreateSudokuPuzzleControl(viewModel);
            SudokuPuzzleLevel? result = null;

            var win = new DialogWindow
            {
                Content = content
            };

            //viewModel.Result.Subscribe(
            //    onNext: r =>
            //    {
            //        result = r;
            //    }, 
            //    onCompleted: () =>
            //    {
            //        win.DialogResult = result != null;
            //    });

            viewModel.Result.Subscribe(value =>
            {
                win.DialogResult = (result = value) is not null;
            });

            if (win.ShowDialog() == false)
                return null;

            return result;
        }
    }
}
