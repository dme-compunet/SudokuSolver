namespace Compunet.SudokuSolver.Core
{
    public interface IEditableBoardCallValue : IBoardCallValue
    {
        void ChangeValue(Value value);
        void ChangeWarning(bool value);
    }
}
