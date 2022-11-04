using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Compunet.SudokuSolver.Controls
{
    public static class ButtonChrome
    {
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.RegisterAttached("HoverBackground",
                                                typeof(Brush),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(null));

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.RegisterAttached("PressedBackground",
                                                typeof(Brush),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(null));

        public static readonly DependencyProperty HoverBackgroundOpacityProperty =
            DependencyProperty.RegisterAttached("HoverBackgroundOpacity",
                                                typeof(double),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(1.0));

        public static readonly DependencyProperty BubbleBackgroundProperty =
            DependencyProperty.RegisterAttached("BubbleBackground",
                                                typeof(Brush),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(null));

        public static readonly DependencyProperty BubbleBackgroundOpacityProperty =
            DependencyProperty.RegisterAttached("BubbleBackgroundOpacity",
                                                typeof(double),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(1.0));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius",
                                                typeof(CornerRadius),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(new CornerRadius(10)));

        public static readonly DependencyProperty DropShadowProperty =
            DependencyProperty.RegisterAttached("DropShadow",
                                                typeof(bool),
                                                typeof(ButtonChrome),
                                                new PropertyMetadata(false));

        public static Brush GetHoverBackground(Button button) => (Brush)button.GetValue(HoverBackgroundProperty);
        public static void SetHoverBackground(Button button, Brush value) => button.SetValue(HoverBackgroundProperty, value);

        public static double GetHoverBackgroundOpacity(Button button) => (double)button.GetValue(HoverBackgroundOpacityProperty);
        public static void SetHoverBackgroundOpacity(Button button, double value) => button.SetValue(HoverBackgroundOpacityProperty, value);

        public static Brush GetPressedBackground(Button button) => (Brush)button.GetValue(PressedBackgroundProperty);
        public static void SetPressedBackground(Button button, Brush value) => button.SetValue(PressedBackgroundProperty, value);

        public static Brush GetBubbleBackground(Button button) => (Brush)button.GetValue(BubbleBackgroundProperty);
        public static void SetBubbleBackground(Button button, Brush value) => button.SetValue(BubbleBackgroundProperty, value);

        public static double GetBubbleBackgroundOpacity(Button button) => (double)button.GetValue(BubbleBackgroundOpacityProperty);
        public static void SetBubbleBackgroundOpacity(Button button, double value) => button.SetValue(BubbleBackgroundOpacityProperty, value);

        public static CornerRadius GetCornerRadius(Button button) => (CornerRadius)button.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(Button button, CornerRadius value) => button.SetValue(CornerRadiusProperty, value);

        public static bool GetDropShadow(Button button) => (bool)button.GetValue(DropShadowProperty);
        public static void SetDropShadow(Button button, bool value) => button.SetValue(DropShadowProperty, value);
    }
}
