using BeanwareCore.Src.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeanwareCore.Src.Ctrl.Utility
{
    /// <summary>Utility class providing randomization methods</summary>
    public static class BRandom
    {
        // Variables
        private static readonly Random RNG = new Random();

        // Methods
        /// <summary> Returns a random integer value between the inclusive mininum and exclusive maximum </summary>
        public static int Integer(int inclusiveMin, int exclusiveMax)
        {
            return RNG.Next(inclusiveMin, exclusiveMax);
        }

        /// <summary> Returns a random float value between the inclusive mininum and inclusive maximum </summary>
        public static float Float(float min, float max)
        {
            var randomValue = (float) RNG.NextDouble();
            return BMath.Map(randomValue, 0, 1, min, max);
        }

        /// <summary> Returns a random item from the given collection </summary>
        public static T Item<T>(IEnumerable<T> collection)
        {
            if (collection == null || collection.Count() == 0)
                return default;

            return collection.ElementAt(Index(collection));
        }

        /// <summary> Returns the given amount of random, non-duplicate items from the collection or all available the amount is larger or equal to the the collection size </summary>
        public static List<T> Items<T>(IEnumerable<T> collection, int amount)
        {
            if (amount >= collection.Count())
                return collection.ToList();

            var result = new List<T>();
            var tmpList = collection.ToList();
            tmpList.Shuffle();

            while (result.Count < amount)
                result.Add(tmpList.Pop());

            return result;
        }

        /// <summary> Returns a random number that can be used as an index for the given collection </summary>
        public static int Index<T>(IEnumerable<T> collection)
        {
            if (collection == null || collection.Count() == 0)
                return -1;

            return Integer(0, (collection.Count() + 1) * 2) % collection.Count();
        }

        /// <summary> Returns a random value from the given enum </summary>
        public static T Enum<T>() where T : Enum
        {
            return (T)(object)Index(System.Enum.GetNames(typeof(T)));
        }

        /// <summary> Returns true or false given the passed percentage like 50 for an even chance </summary>
        public static bool Percentage(int chance)
        {
            if (chance <= 0)
                return false;

            return Integer(0, 101) <= chance;
        }

        /// <summary> Returns true or false evenly distributed with a 50/50 chance </summary>
        public static bool Coinflip()
        {
            return Integer(0, 100) % 2 == 0;
        }
    }
}
