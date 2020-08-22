using System;
using System.Collections.Generic;
using System.Linq;

namespace BeanwareCore.Src.Data.Extensions
{
    /// <summary> Extensions Methods for IEnumerables </summary>
    public static class IEnumerableExtensions
    {
        /// <summary> 
        /// Returns all elements from the collection that have the max value of the selector.
        /// Make sure the selector selects a proper comparable type like a string, int, float etc
        /// </summary>
        /// <param name="collection">The source collection</param>
        /// <param name="valueSelector">The method used to select the value to compare from each source element</param>
        /// <returns>All items with the maximum value</returns>
        public static IEnumerable<TItem> MaxBy<TItem, TValue>(this IEnumerable<TItem> collection, Func<TItem, TValue> valueSelector)
        {
            var maxValue = collection.Max(valueSelector);
            return collection.Where(item => valueSelector(item).Equals(maxValue));
        }

        /// <summary>
        /// Returns all elements from the collection that have the min value of the selector.
        /// Make sure the selector selects a proper comparable type like a string, int, float etc
        /// </summary>
        /// <param name="collection">The source collection</param>
        /// <param name="valueSelector">The method used to select the value to compare from each source element</param>
        /// <returns>All items with the minimum value</returns>
        public static IEnumerable<TItem> MinBy<TItem, TValue>(this IEnumerable<TItem> collection, Func<TItem, TValue> valueSelector)
        {
            var maxValue = collection.Min(valueSelector);
            return collection.Where(item => valueSelector(item).Equals(maxValue));
        }
    }
}
