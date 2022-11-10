using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Compunet.SudokuSolver.Extensions.Generic
{
    public static class ImmutableListExtensions
    {
        private static readonly Random rnd = new();

        public static ImmutableList<T> Shuffle<T>(this IImmutableList<T> source)
        {
            List<T> tmp = source.ToList();

            int count = source.Count;

            while (count > 1)
            {
                count--;
                int k = rnd.Next(count + 1);
                (tmp[count], tmp[k]) = (tmp[k], tmp[count]);
            }

            return tmp.ToImmutableList();
        }

    }
}
