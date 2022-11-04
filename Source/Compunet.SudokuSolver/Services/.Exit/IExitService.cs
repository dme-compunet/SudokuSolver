using System;

namespace Compunet.SudokuSolver.Services
{
    public interface IExitService
    {
        event Action Exiting;
        void OnExit();
    }
}
