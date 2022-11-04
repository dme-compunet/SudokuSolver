using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace Compunet.SudokuSolver.Behaviors
{
    public class SidebarCollapseBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty HostActualWidthProperty =
            DependencyProperty.Register("HostActualWidth",
                                        typeof(double),
                                        typeof(SidebarCollapseBehavior),
                                        new PropertyMetadata(0.0, OnHostActualWidthChanged));

        private static void OnHostActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public double HostActualWidth
        {
            get => (double)GetValue(HostActualWidthProperty);
            set => SetValue(HostActualWidthProperty, value);
        }



        protected override void OnAttached()
        {

        }
    }
}
