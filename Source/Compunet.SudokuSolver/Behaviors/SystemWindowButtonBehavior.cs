using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Behaviors
{
    public abstract class SystemWindowButtonBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty TargetWindowProperty =
            DependencyProperty.Register(nameof(TargetWindow),
                                        typeof(Window),
                                        typeof(SystemWindowButtonBehavior),
                                        new PropertyMetadata(null));

        public Window TargetWindow
        {
            get => (Window)GetValue(TargetWindowProperty);
            set => SetValue(TargetWindowProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.Click += AssociatedObjectOnClick;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= AssociatedObjectOnClick;
            base.OnDetaching();
        }

        private void AssociatedObjectOnClick(object sender, RoutedEventArgs e)
        {
            OnButtonClick(TargetWindow);
        }

        protected virtual void OnButtonClick(Window window) { }
    }


}
