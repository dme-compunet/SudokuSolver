namespace Compunet.SudokuSolver.Core
{
    public interface IEditableBoard : IBoard
    {
        void SetValue(Cell cell, Value value);
        void SetWrong(Cell cell, bool wrong);
    }
}
