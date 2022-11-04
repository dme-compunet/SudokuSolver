using System;

namespace Compunet.SudokuSolver.Services
{
    public class ExitService : IExitService
    {
        public event Action Exiting = delegate { };

        public void AddExitAction(Action action)
        {
            Exiting += action;
        }

        public void OnExit()
        {
            Exiting();
        }
    }
}
