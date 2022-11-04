using System;
using System.Globalization;

namespace Compunet.SudokuSolver.Mvvm.Converters
{
    public class BooleanConvertConverter : ConverterExtension<bool>
    {
        protected override object Convert(bool value, Type TargetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}
