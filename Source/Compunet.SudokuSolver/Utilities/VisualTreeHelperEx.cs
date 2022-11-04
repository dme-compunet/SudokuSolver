using System.Windows;
using System.Windows.Media;

namespace Compunet.SudokuSolver.Utilities
{
    public static class VisualTreeHelperEx
    {
        public static T? GetParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject? element = child;

            while (element is not null)
            {
                if (element is T button)
                    return button;

                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                element = parent;
            }

            return null;
        }

        //public static T? GetParent<T>(FrameworkElement child)
        //    where T : FrameworkElement
        //{
        //    FrameworkElement? element = child;

        //    while (element is not null)
        //    {
        //        if (element is T button)
        //            return button;

        //        FrameworkElement? parent = element.Parent as FrameworkElement;

        //        if (parent is null)
        //            parent = element.TemplatedParent as FrameworkElement;

        //        element = parent;
        //    }

        //    return null;
        //}
    }

}
