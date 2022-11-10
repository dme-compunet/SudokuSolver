using System.Windows;

namespace Compunet.SudokuSolver.Controls
{
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
            var appWindow = System.Windows.Application.Current.MainWindow;
            Owner = appWindow;

            Loaded += (_, _) => WindowChrome.SetOverlayMode(appWindow, true);
            Closed += (_, _) => WindowChrome.SetOverlayMode(appWindow, false);

            //if (appWindow is IWindowOverlayMode window)
            //{
            //    Loaded += (_, _) => window.SetOverlay();
            //    Closed += (_, _) => window.CancelOverlay();
            //}
        }
    }
}
