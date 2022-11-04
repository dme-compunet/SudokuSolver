using System.Windows;

namespace Compunet.SudokuSolver.Controls
{
    public static class WindowChrome
    {
        public static readonly DependencyProperty OverlayModeProperty =
            DependencyProperty.RegisterAttached("OverlayMode",
                                                typeof(bool),
                                                typeof(WindowChrome),
                                                new PropertyMetadata(false));

        public static bool GetOverlayMode(Window window)
        {
            return (bool)window.GetValue(OverlayModeProperty);
        }

        public static void SetOverlayMode(Window window, bool value)
        {
            window.SetValue(OverlayModeProperty, value);
        }
    }
}
