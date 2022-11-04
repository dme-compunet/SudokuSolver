using Compunet.SudokuSolver.Extensions.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Core
{
    public static class SudokuBoardGenerator
    {
        private static readonly Random random = new();

        public static async Task<IBoard?> TrySolveState(IBoard board)
        {
            BoardGeneratorEngine engine = new(board);
            await engine.StartAsync();

            if (engine.IsCompleted)
                return engine.Resolte;

            return null;
        }

        public static async Task<IBoard> GeneratePuzzleState(SudokuPuzzleLevel level)
        {
            BoardGeneratorEngine engine = new();
            await engine.StartAsync();
            var resolte = engine.Resolte;
            //var board = engine.Resolte;

            if (resolte is null)
            {
                Debugger.Break();
                return SudokuBoardCreator.CreateEmpty();
            }

            var board = SudokuBoardCreator.CreateEditable(resolte);

            int TakeRandomForRemove()
            {
                return level switch
                {
                    SudokuPuzzleLevel.Easy => random.Next(3, 8),
                    SudokuPuzzleLevel.Medium => random.Next(5, 9),
                    SudokuPuzzleLevel.Hard => random.Next(6, 9),
                    _ => 0
                };
            }

            var cells = Cell.AllRange.Shuffle();

            foreach (var square in cells.GroupBy(c => c.Square))
            {
                foreach (var cell in square.Take(TakeRandomForRemove()))
                {
                    board.SetValue(cell, Value.Zero);
                }
            }

            return board;
        }
    }
}
