using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BeanwareCore.Src.Data.Extensions
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static class StringExtensions
    {
        // Methods
        /// <summary>Returns a camel case representation of the given string by removing all spaces</summary>
        public static string ToCamelCase(this string s)
        {
            StringBuilder sb = new StringBuilder();
            bool upper = true;
            char c;

            for (int i = 0; i < s.Length; i++)
            {
                c = s[i];

                if (char.IsWhiteSpace(c))
                    upper = true;
                else if (!upper)
                    sb.Append(c);
                else
                {
                    sb.Append(c.ToString().ToUpper());
                    upper = false;
                }
            }

            return sb.ToString();
        }
        
        /// <summary> 
        /// Returns the edit distance of the given string towards the target string limited to the max distance.
        /// Written by Joshua Honig on https://stackoverflow.com/questions/9453731/how-to-calculate-distance-similarity-measure-of-given-2-strings
        /// </summary>
        /// <param name="source">The source string of the comparison</param>
        /// <param name="target">The string to compare against</param>
        /// <param name="cutoffThreshhold">The max distance at which to stop</param>
        public static int DamerauLevenshteinDistance(this string source, string target, int cutoffThreshhold)
        {
            int length1 = source.Length;
            int length2 = target.Length;

            // Return trivial case - difference in string lengths exceeds threshhold
            if (Math.Abs(length1 - length2) > cutoffThreshhold) { return int.MaxValue; }

            // Ensure arrays [i] / length1 use shorter length 
            if (length1 > length2)
            {
                Swap(ref target, ref source);
                Swap(ref length1, ref length2);
            }

            int maxi = length1;
            int maxj = length2;

            int[] dCurrent = new int[maxi + 1];
            int[] dMinus1 = new int[maxi + 1];
            int[] dMinus2 = new int[maxi + 1];
            int[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (int j = 1; j <= maxj; j++)
            {

                // Rotate
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;

                // Initialize
                int minDistance = int.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {

                    int cost = source[im1] == target[jm1] ? 0 : 1;

                    int del = dCurrent[im1] + 1;
                    int ins = dMinus1[i] + 1;
                    int sub = dMinus1[im1] + cost;

                    //Fastest execution for min value of 3 integers
                    int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;
                }
                jm1++;
                if (minDistance > cutoffThreshhold) { return int.MaxValue; }
            }

            int result = dCurrent[maxi];
            return (result > cutoffThreshhold) ? int.MaxValue : result;
        }

        /// <summary> Returns the string with all characters changed to lower case and all spaces removed </summary>
        public static string Minimized(this string s)
        {
            return s.Spaceless().ToLower();
        }

        /// <summary> Returns the string with all whitespaces removed </summary>
        public static string Spaceless(this string s)
        {
            return Regex.Replace(s, @"\s+", "");
        }

        /// <summary> Returns the string with all whitespaces trimmed down to never have more than once successively </summary>
        public static string SingleSpaced(this string s)
        {
            return Regex.Replace(s, @"\s+", " ");
        }

        /// <summary> Returns true if the string has any whitespaces and false otherwise </summary>
        public static bool HasWhiteSpaces(this string s)
        {
            return Regex.IsMatch(s, @"\s");
        }

        /// <summary> Returns true if the given string would contain any illegal characters as a filename </summary>
        public static bool HasIllegalCharacters(this string s)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return s.Any(c => invalid.Contains(c));
        }

        // Utility Methods
        /// <summary> Used by DamerauLevenshteinDistance </summary>
        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}
