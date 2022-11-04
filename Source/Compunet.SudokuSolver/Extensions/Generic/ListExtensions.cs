using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Compunet.SudokuSolver.Extensions.Generic
{
    public static class ListExtensions
    {
        private static readonly Random rng = new();

        //public static void Shuffle<T>(this IList<T> source)
        //{
        //    int n = source.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rng.Next(n + 1);
        //        //T value = source[k];
        //        //source[k] = source[n];
        //        //source[n] = value;
        //        (source[n], source[k]) = (source[k], source[n]);
        //    }
        //}
    }

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

                //T value = source[k];
                var value = source.GetValue(k);

                //source[k] = source[n];
                source.SetValue(source.GetValue(count), k);

                //source[n] = value;
                source.SetValue(value, count);

                //(source[n], source[k]) = (source[k], source[n]);
            }
        }

    }
}
