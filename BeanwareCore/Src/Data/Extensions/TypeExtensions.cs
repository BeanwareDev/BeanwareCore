using System;

namespace BeanwareCore.Src.Data.Extensions
{
    /// <summary> Extension methods for System.Type </summary>
    public static class TypeExtensions
    {
        // Methods
        /// <summary>Returns the name of the type including resolved generic parameters</summary>
        public static string ResolvedName(this Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var properType = $"{type.Name.Split('`')[0]}";
            var genericArgs = type.GetGenericArguments();
            var res = properType == nameof(Nullable) ? "" : $"{properType}<";

            for (int i = 0; i < genericArgs.Length; i++)
            {
                if (i > 0)
                    res = $"{res}, ";
                res = $"{res}{genericArgs[i].ResolvedName()}";
            }

            if (properType == nameof(Nullable))
                res = $"{res}?";
            else
                res = $"{res}>";

            return res;
        }
    }
}
