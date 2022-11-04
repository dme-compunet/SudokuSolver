using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Controls;
using Compunet.SudokuSolver.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Media.Effects;

namespace Compunet.SudokuSolver
{
    public partial class AppWindow : BaseWindow, IWindowOverlayMode
    {
        public AppWindow()
        {
            InitializeComponent();
            DataContext = IoC.Simple.GetRequiredService<AppWindowViewModel>();
        }

        public void SetOverlay()
        {
            OverlayBorder.Visibility = Visibility.Visible;
            OverlayBorder.Opacity = .5;
            Effect = new BlurEffect() { Radius = 10.0 };
        }

        public void CancelOverlay()
        {
            OverlayBorder.Opacity = 0;
            OverlayBorder.Visibility = Visibility.Collapsed;
            Effect = null;
        }
    }

    public interface IWindowOverlayMode
    {
        void SetOverlay();
        void CancelOverlay();
    }
}
