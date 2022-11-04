using Compunet.SudokuSolver.Core;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Controls
{
    public partial class SudokuListBox : ListBox
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius),
                                        typeof(CornerRadius),
                                        typeof(SudokuListBox),
                                        new PropertyMetadata(new CornerRadius(15)));

        //public static readonly DependencyProperty SudokuStateProperty =
        //    DependencyProperty.Register(nameof(SudokuState),
        //                                typeof(ISudokuState),
        //                                typeof(SudokuListBox),
        //                                new PropertyMetadata(null, OnStateChanged));

        public static readonly DependencyProperty BoradProperty =
            DependencyProperty.Register(nameof(Board),
                                        typeof(IBoard),
                                        typeof(SudokuListBox),
                                        new PropertyMetadata(null, OnBoardChanged));

        private readonly SudokuListBoxItem[] items;

        private static void OnBoardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IBoard board)
            {
                (d as SudokuListBox)?.UpdateBoard(board);
            }
        }

        //public ISudokuState SudokuState
        //{
        //    get => (ISudokuState)GetValue(SudokuStateProperty);
        //    set => SetValue(SudokuStateProperty, value);
        //}

        public IBoard Board
        {
            get => (IBoard)GetValue(BoradProperty);
            set => SetValue(BoradProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public SudokuListBox()
        {
            InitializeComponent();

            ItemsSource = items = Cell.AllRange.Select(p => new SudokuListBoxItem(p)).ToArray();
        }

        private void UpdateBoard(IBoard board)
        {
            for (int i = 0; i < 81; i++)
            {
                items[i].Value = board.Values.ElementAt(i).Value;
                items[i].IsWrong = board.Values.ElementAt(i).IsWrong;
            }
        }
    }
}
