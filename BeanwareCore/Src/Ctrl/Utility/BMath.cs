using System;

namespace BeanwareCore.Src.Ctrl.Utility
{
    /// <summary>Provides utility math methods</summary>
    public static class BMath
    {
        /// <summary> Maps the passed value from the original range to the new one </summary>
        /// <param name="value">The value to be mapped to the target range</param>
        /// <param name="fromMin">The minimum value of the original range</param>
        /// <param name="fromMax">The maximum value of the original range</param>
        /// <param name="toMin">The minimum value of the target range</param>
        /// <param name="toMax">The maximum value of the target range</param>
        public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            if (fromMax - fromMin == 0)
                throw new ArgumentException($"{nameof(BMath)}.{nameof(Map)}() can not be calculated if the origin range is zero");
            return (((value - fromMin) * (toMax - toMin)) / (fromMax - fromMin)) + toMin;
        }

        /// <summary> Returns a Mod b. This differs to the operator % in that negative numbers will give positive results. This can be useful for wrapping the index of an array </summary>
        public static int Mod(int a, int b)
        {
            int c = a % b;
            if ((c < 0 && b > 0) || (c > 0 && b < 0))
                c += b;
            return c;
        }
    }
}
