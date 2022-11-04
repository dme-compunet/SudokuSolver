using System;
using System.Globalization;
using System.Windows;

namespace Compunet.SudokuSolver.Mvvm.Converters
{
    public class BooleanToVisibilityConverter : ConverterExtension<bool>
    {
        protected override object Convert(bool value, Type TargetType, object parameter, CultureInfo culture)
        {
            return !value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
