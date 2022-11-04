using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace Compunet.SudokuSolver.Behaviors
{
    public class InvokeCommandOnKeyDownBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command),
                                        typeof(ICommand),
                                        typeof(InvokeCommandOnKeyDownBehavior),
                                        new PropertyMetadata(null));


        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        protected override void OnAttached()
        {
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            Command?.Execute(e.Key);
        }
    }
}
