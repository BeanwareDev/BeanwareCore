using System.Collections.Generic;
using System.Linq;

namespace BeanwareCore.Src.Data.Extensions
{
    public static class IListExtensions
    {
        /// <summary> Shuffles the list </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = BRandom.Integer(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary> Returns and removes the first item from the list </summary>
        public static T Pop<T>(this IList<T> list)
        {
            if (!list.Any())
                return default;

            var item = list.First();
            list.Remove(item);
            return item;
        }

        /// <summary> Returns and removes a random item from the list </summary>
        public static T PopRandom<T>(this IList<T> list)
        {
            if (!list.Any())
                return default;

            var item = list[BRandom.Index(list)];
            list.Remove(item);
            return item;
        }
    }
}
