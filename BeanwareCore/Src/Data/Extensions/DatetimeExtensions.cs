using BeanwareCore.Src.Data.Config;
using System;

namespace BeanwareCore.Src.Data.Extensions
{
    /// <summary>
    /// Extension methods for DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns the date in the given DateFormat
        /// </summary>
        /// <param name="dateTime">The DateTime object to represent</param>
        /// <param name="dateFormat">The format to write the date in</param>
        /// <param name="seperator">The symbol to use as a seperator between the date values</param>
        /// <returns>The string representation of the date in the given date format</returns>
        public static string ToFormatBasedString(this DateTime dateTime, DateFormat dateFormat, string seperator = ".")
        {
            switch (dateFormat)
            {
                case DateFormat.DayMonthYear:
                    return dateTime.ToString($"dd{seperator}MM{seperator}yyyy");
                case DateFormat.MonthDayYear:
                    return dateTime.ToString($"MM{seperator}dd{seperator}yyyy");
                case DateFormat.YearDayMonth:
                    return dateTime.ToString($"yyyy{seperator}dd{seperator}MM");
                case DateFormat.YearMonthDay:
                    return dateTime.ToString($"yyyy{seperator}MM{seperator}dd");
                default:
                    return dateTime.ToString();
            }
        }
    }
}
