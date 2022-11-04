using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace Compunet.SudokuSolver.Views
{
    public partial class SudokuControl : UserControl
    {
        public SudokuControl()
        {
            InitializeComponent();
#if DEBUG
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = IoC.Simple.GetRequiredService<SudokuViewModel>();
            }
#else
            DataContext = IoC.Simple.GetRequiredService<SudokuViewModel>();
#endif
        }
    }
}
