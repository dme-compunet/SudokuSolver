using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Compunet.SudokuSolver.Mvvm.Converters
{
    public abstract class ConverterExtension<T> : MarkupExtension, IValueConverter
    {
        private static readonly Dictionary<Type, object?> caches = new();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((T)value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            var type = GetType();

            if (caches.TryGetValue(type, out var cache))
                return cache;

            object? obj = Activator.CreateInstance(type);
            caches.Add(type, obj);

            return obj;
        }

        protected abstract object Convert(T value, Type TargetType, object parameter, CultureInfo culture);
    }
}
