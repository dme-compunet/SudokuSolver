using System;

namespace Compunet.SudokuSolver.Extensions.Generic
{
    public static class ArrayExtensions
    {
        private static readonly Random rnd = new();

        public static void Shuffle(this Array source)
        {
            int count = source.Length;

            while (count > 1)
            {
                count--;
                int k = rnd.Next(count + 1);

                var value = source.GetValue(k);
                source.SetValue(source.GetValue(count), k);
                source.SetValue(value, count);
            }
        }
    }
}
