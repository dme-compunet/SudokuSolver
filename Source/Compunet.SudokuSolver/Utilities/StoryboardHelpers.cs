using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Compunet.SudokuSolver.Utilities
{
    public static class StoryboardHelpers
    {
        //private static readonly TimeSpan def = TimeSpan.FromSeconds(.3);

        public static void AddScaleInAnimation(this Storyboard storyboard, TimeSpan duration)
        {
            DoubleAnimation x_animation = new()
            {
                To = 1.0,
                Duration = duration,
                DecelerationRatio = .7
            };

            Storyboard.SetTargetProperty(x_animation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));

            DoubleAnimation y_animation = new()
            {
                To = 1.0,
                Duration = duration,
                DecelerationRatio = .7
            };

            Storyboard.SetTargetProperty(y_animation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(x_animation);
            storyboard.Children.Add(y_animation);
        }

        public static void AddFadeInAnimation(this Storyboard storyboard, TimeSpan duration, double to = 1.0, double? from = null)
        {
            DoubleAnimation animation = new()
            {
                To = to,
                From = from,
                Duration = duration / 3
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);
        }

        public static void AddFadeOutAnimation(this Storyboard storyboard, TimeSpan duration, double to = .0, double? from = null)
        {
            DoubleAnimation animation = new()
            {
                To = to,
                From = from,
                Duration = duration
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);
        }
    }

}
