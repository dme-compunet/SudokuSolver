using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Views
{
    /// <summary>
    /// Interaction logic for AppSidebarControl.xaml
    /// </summary>
    public partial class AppSidebarControl : UserControl
    {
        public AppSidebarControl()
        {
            InitializeComponent();
#if DEBUG
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = IoC.Simple.GetRequiredService<AppSidebarViewModel>();
            }
#else
                DataContext = IoC.Simple.GetRequiredService<AppSidebarViewModel>();
#endif

        }
    }
}
