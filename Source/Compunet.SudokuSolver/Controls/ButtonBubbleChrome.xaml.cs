using Compunet.SudokuSolver.Utilities;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Compunet.SudokuSolver.Controls
{
    [TemplatePart(Name = PART_bubble_area, Type = typeof(Canvas))]
    public partial class ButtonBubbleChrome : Control
    {
        private const string PART_bubble_area = nameof(PART_bubble_area);

        private BubbleEntry? current_Bubble;

        #region Nested

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

        private class BubbleEntry
        {
            private readonly TimeSpan in_time = TimeSpan.FromSeconds(.2);
            private readonly TimeSpan out_time = TimeSpan.FromSeconds(.25);

            public Canvas ParentElement { get; }
            public Border BubbleElement { get; }

            public BubbleEntry(Canvas parent, Border bubble)
            {
                ParentElement = parent;
                BubbleElement = bubble;
            }

            public void AnimateBubble()
            {
                BubbleElement.RenderTransformOrigin = new Point(.5, .5);
                BubbleElement.RenderTransform = new ScaleTransform(.0, .0);

                Storyboard storyboard = new();
                storyboard.AddScaleInAnimation(in_time);
                //storyboard.AddFadeInAnimation(in_time / 3, to: .3, from: .0);
                storyboard.AddFadeInAnimation(in_time / 3, to: 1.0, from: .0);

                ParentElement.Children.Add(BubbleElement);
                storyboard.Begin(BubbleElement);
            }

            public void OutWithFade()
            {
                Storyboard storyboard = new();
                storyboard.AddFadeOutAnimation(out_time);

                //storyboard.Children.Add(animation);
                storyboard.Completed += (_, _) => ParentElement.Children.Remove(BubbleElement);

                storyboard.Begin(BubbleElement);
            }
        }

        #endregion

        #region Dependency Properties Fields

        private static readonly DependencyProperty RenderPressedProperty =
            DependencyProperty.Register("RenderPressed",
                                        typeof(bool),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(false, OnRenderPressedChanged));

        public static readonly DependencyProperty TargetButtonProperty =
            DependencyProperty.Register(nameof(TargetButton),
                                        typeof(Button),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(null, OnTargetButtonChanged));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius),
                                        typeof(CornerRadius),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(nameof(HoverBackground),
                                        typeof(Brush),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(null));

        public static readonly DependencyProperty HoverBackgroundOpacityProperty =
            DependencyProperty.Register(nameof(HoverBackgroundOpacity),
                                        typeof(double),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(1.0));


        public static readonly DependencyProperty BubbleBackgroundProperty =
            DependencyProperty.Register(nameof(BubbleBackground),
                                        typeof(Brush),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(null));

        public static readonly DependencyProperty BubbleBackgroundOpacityProperty =
            DependencyProperty.Register(nameof(BubbleBackgroundOpacity),
                                        typeof(double),
                                        typeof(ButtonBubbleChrome),
                                        new PropertyMetadata(1.0));

        public Button TargetButton
        {
            get => (Button)GetValue(TargetButtonProperty);
            set => SetValue(TargetButtonProperty, value);
        }

        public Brush HoverBackground
        {
            get => (Brush)GetValue(HoverBackgroundProperty);
            set => SetValue(HoverBackgroundProperty, value);
        }

        public double HoverBackgroundOpacity
        {
            get => (double)GetValue(HoverBackgroundOpacityProperty);
            set => SetValue(HoverBackgroundOpacityProperty, value);
        }

        public Brush BubbleBackground
        {
            get => (Brush)GetValue(BubbleBackgroundProperty);
            set => SetValue(BubbleBackgroundProperty, value);
        }

        public double BubbleBackgroundOpacity
        {
            get => (double)GetValue(BubbleBackgroundOpacityProperty);
            set => SetValue(BubbleBackgroundOpacityProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region Dependencies Callbacks

        private static void OnRenderPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ButtonBubbleChrome chrome ||
                e.NewValue is not bool value)
                return;

            //Button? parent = VisualTreeHelperEx.GetParent<Button>(d);

            if (d.GetValue(TargetButtonProperty) is not Button parent)
                return;

            if (value == false)
            {
                chrome.TryRemoveBubble();
                return;
            }

            if (d.GetValue(BubbleBackgroundProperty) is null)
                return;

            // makesure it's not a key pressed
            bool mouse_captured = Mouse.Captured == parent;

            if (mouse_captured)
                chrome.AddBubble(Mouse.GetPosition(chrome));
        }

        private static void OnTargetButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not Button button
                || d is not ButtonBubbleChrome chrome)
                return;

            chrome.SetBinding(RenderPressedProperty, new Binding
            {
                Source = button,
                Path = new PropertyPath("IsPressed")
            });
        }

        #endregion

        public ButtonBubbleChrome()
        {
            InitializeComponent();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //AddBubble(Mouse.GetPosition(this));
            base.OnMouseDown(e);
        }

        public override void OnApplyTemplate()
        {
            SetupBubbleClipAreaUsingBinding();
            base.OnApplyTemplate();
        }

        #region Bubble - Add / Remove

        private void AddBubble(Point mousePoint)
        {
            Canvas canvas = (Canvas)GetTemplateChild(PART_bubble_area);
            Border Bubble = CreateBubbleElement(mousePoint);

            current_Bubble = new(canvas, Bubble);
            current_Bubble.AnimateBubble();
        }

        private void TryRemoveBubble()
        {
            current_Bubble?.OutWithFade();
        }

        #endregion

        #region Private helpers

        private void SetupBubbleClipAreaUsingBinding()
        {
            Canvas canvas = (Canvas)GetTemplateChild(PART_bubble_area);

            var binding = new MultiBinding();

            binding.Bindings.Add(new Binding
            {
                Source = this,
                Path = new PropertyPath("ActualWidth")
            });

            binding.Bindings.Add(new Binding
            {
                Source = this,
                Path = new PropertyPath("ActualHeight")
            });

            binding.Bindings.Add(new Binding
            {
                Source = this,
                Path = new PropertyPath("CornerRadius")
            });

            binding.Converter = new ClipGeometrySelectorConverter();

            canvas.SetBinding(ClipProperty, binding);
        }

        private Border CreateBubbleElement(Point mouse)
        {
            double current_radius = GetCornerRadiusActualSize(CornerRadius);
            double current_width = ActualWidth;
            double current_height = ActualHeight;

            double max_hor_line = Math.Max(mouse.X, current_width - mouse.X);
            double max_ver_line = Math.Max(mouse.Y, current_height - mouse.Y);
            double max_line = Math.Max(max_hor_line, max_ver_line);

            // compuet needed radius of bubble
            double radius = Math.Sqrt(max_hor_line * max_hor_line + max_ver_line * max_ver_line); ;

            // corner radius reducing 
            radius -= Math.Sqrt(current_radius * current_radius * 2) - current_radius;

            // fix for round button
            if (radius < max_line)
                radius = max_line;

            double size = radius * 2;

            Border Bubble = new()
            {
                Width = size,
                Height = size,
                CornerRadius = new CornerRadius(radius),
                Background = BubbleBackground,
                RenderTransformOrigin = new Point(.5, .5),
                RenderTransform = new ScaleTransform(.0, .0)
            };

            Canvas.SetTop(Bubble, mouse.Y - radius);
            Canvas.SetLeft(Bubble, mouse.X - radius);

            return Bubble;
        }

        private double GetCornerRadiusActualSize(CornerRadius cornerRadius)
        {
            double max = MathEx.Max(cornerRadius.TopRight,
                                    cornerRadius.TopLeft,
                                    cornerRadius.BottomRight,
                                    cornerRadius.BottomLeft);

            double min_size = Math.Min(ActualWidth, ActualHeight);

            if (max > min_size / 2)
                return min_size / 2;

            return max;
        }

        #endregion
    }
}
