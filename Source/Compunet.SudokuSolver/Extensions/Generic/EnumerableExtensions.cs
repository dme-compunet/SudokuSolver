using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compunet.SudokuSolver.Extensions.Generic
{
    public static class EnumerableExtensions
    {
        public static int FirstIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            int index = 0;
            int found = -1;

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    found = index;
                    break;
                }

                index++;
            }

            return found;
        }
    }
}
