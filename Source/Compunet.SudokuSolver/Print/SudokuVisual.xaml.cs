using Compunet.SudokuSolver.Core;
using System.Windows;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Print
{
    public partial class SudokuVisual : UserControl
    {
        public SudokuVisual(IBoard board)
        {
            InitializeComponent();
            FillBoard(board);
        }

        private void FillBoard(IBoard board)
        {
            foreach (IBoardCallValue cell in board.Values)
            {
                var tb = new TextBlock
                {
                    Text = cell.Value.ToDisplayString(),
                };

                Canvas.SetTop(tb, cell.Cell.Row * 30.0);
                Canvas.SetLeft(tb, cell.Cell.Column * 30.0);
                canvas.Children.Add(tb);
            }
        }
    }
}
