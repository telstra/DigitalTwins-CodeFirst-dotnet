using System;

namespace Telstra.Twins.Common
{
    public static class StringHelpers
    {
        public static string ToCamelCase(this string str) =>
            str != null && str.Length > 1 ? Char.ToLower(str[0]) + str.Substring(1) : str;
        public static string ToCapitalCase(this string str) =>
            str != null && str.Length > 1 ? Char.ToUpper(str[0]) + str.Substring(1) : str;
    }
}
