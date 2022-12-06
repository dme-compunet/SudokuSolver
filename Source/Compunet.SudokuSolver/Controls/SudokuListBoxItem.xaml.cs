using Compunet.SudokuSolver.Core;
using System.Windows;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Controls
{
    public partial class SudokuListBoxItem : ListBoxItem
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                                        typeof(Value),
                                        typeof(SudokuListBoxItem),
                                        new PropertyMetadata(Value.Zero, OnValueChanged));

        public static readonly DependencyProperty ValueTextProperty =
            DependencyProperty.Register(nameof(ValueText),
                                        typeof(string),
                                        typeof(SudokuListBoxItem),
                                        new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsWrongProperty =
            DependencyProperty.Register(nameof(IsWrong),
                                        typeof(bool),
                                        typeof(SudokuListBoxItem),
                                        new PropertyMetadata(false));

        public Value Value
        {
            get => (Value)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public string ValueText
        {
            get => (string)GetValue(ValueTextProperty);
            set => SetValue(ValueTextProperty, value);
        }

        public bool IsWrong
        {
            get => (bool)GetValue(IsWrongProperty);
            set => SetValue(IsWrongProperty, value);
        }

        public SudokuListBoxItem(Cell cell)
        {
            InitializeComponent();

            //SetResourceReference(BackgroundProperty, cell.Index % 2 == 1 ? "Primary200" : "Primary100");
            SetResourceReference(BackgroundProperty, cell.Index % 2 == 1 ? "Primary100" : "Primary50");

            Canvas.SetTop(this, cell.Row * 50.0);
            Canvas.SetLeft(this, cell.Column * 50.0);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Value value = (Value)e.NewValue;
            d.SetValue(ValueTextProperty, value.ToDisplayString());
        }
    }
}
