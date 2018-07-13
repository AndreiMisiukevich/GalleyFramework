using System;
using System.Linq;
using System.Collections.Generic;

namespace GalleyFramework.Extensions
{
    public static class StringExtensions
    {
        public static string Crop(this string str, int maxChars)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            var src = str.Trim();
            return src.Length > maxChars
                      ? $"{src.Substring(0, maxChars - 2)}..."
                      : src;

        }

        public static bool StartIgnore(this string str, string other)
        => str.StartsWith(other, StringComparison.OrdinalIgnoreCase);

        public static bool StartWithAny(this string str, params string[] others)
        => others?.Any(x => str.StartIgnore(x)) ?? false;

		public static string FirstToUpper(this string input)
		=> string.IsNullOrEmpty(input)
                     ? input 
                     : char.ToUpper(input[0]) + input.Substring(1);

		public static string AsPattern(this string pattern, params object[] param)
		=> string.Format(pattern, param);

        public static string TailTrunc(this string str, int count)
        => string.IsNullOrEmpty(str)
                 ? str
                 : str.Substring(0, str.Length - count);

		public static string HeadTrunc(this string str, int count)
        => string.IsNullOrEmpty(str)
		 ? str
		 : str.Substring(count, str.Length - count);

        public static KeyValuePair<string, string> Pair(this string key, object value)
        => new KeyValuePair<string, string>(key, value.ToString());
    }
}