using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Compunet.SudokuSolver.Controls
{
    public static class BorderExtensions
    {
        public static readonly DependencyProperty ClipToCornerRadiusProperty =
            DependencyProperty.RegisterAttached("ClipToCornerRadius",
                                                typeof(bool),
                                                typeof(BorderExtensions),
                                                new PropertyMetadata(false, OnClipToCornerRadiusChanged));

        private class ClipGeometrySelectorConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                double width = (double)values[0];
                double height = (double)values[1];
                double radius = ((CornerRadius)values[2]).TopLeft;

                Size size = new(width, height);
                Rect rect = new(size);
                RectangleGeometry clip = new(rect, radius, radius);

                return clip;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                return Array.Empty<object>();
            }
        }

        private static void OnClipToCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Border border = (Border)d;
            bool value = (bool)e.NewValue;

            if (value == false)
            {
                border.SetValue(UIElement.ClipProperty, null);
                return;
            }

            var binding = new MultiBinding();

            binding.Bindings.Add(new Binding
            {
                Source = border,
                Path = new PropertyPath("ActualWidth")
            });

            binding.Bindings.Add(new Binding
            {
                Source = border,
                Path = new PropertyPath("ActualHeight")
            });

            binding.Bindings.Add(new Binding
            {
                Source = border,
                Path = new PropertyPath("CornerRadius")
            });

            binding.Converter = new ClipGeometrySelectorConverter();
            border.SetBinding(UIElement.ClipProperty, binding);
        }

        public static bool GetClipToCornerRadius(Border border)
        {
            return (bool)border.GetValue(ClipToCornerRadiusProperty);
        }

        public static void SetClipToCornerRadius(Border border, bool value)
        {
            border.SetValue(ClipToCornerRadiusProperty, value);
        }
    }
}
