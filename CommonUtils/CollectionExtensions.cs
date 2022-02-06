using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CommonUtils
{
    public static class CollectionExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static T Random<T>(this IEnumerable<T> items)
        {
            var all = items.ToList();
            return all[rng.Next(all.Count)];
        }
        
        

        public static void Move<T>(this IList<T> source, int n, IList<T> target)
        {
            for (var i = 0; i < n; i++)
            {
                target.Add(source[0]);
                source.RemoveAt(0);
            }
        }

        public static void Move<T>(this IList<T> source, T item, IList<T> target)
        {
            target.Add(item);
            source.Remove(item);
        }
    }
}