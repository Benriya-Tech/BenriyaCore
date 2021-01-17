//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace Benriya.Core.Extensions
//{
//    public static class StringExtension
//    {
//        public static string TrimAndReduce(this string str)
//        {
//            return ConvertWhitespacesToSingleSpace(str).Trim();
//        }
//        public static string ConvertWhitespacesToSingleSpace(this string value)
//        {
//            return Regex.Replace(value, @"\s+", " ");
//        }
//        public static string RemoveFromLast(this string value, int len = 1)
//        {
//            return value.Remove(value.Length - len);
//        }
//        public static bool isNOEOW(this string value)
//        {
//            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
//        }

//        public static bool isNumberic(this string value)
//        {
//            return int.TryParse(value, out _);
//        }
//        public static bool isDecimal(this string value)
//        {
//            return decimal.TryParse(value, out _);
//        }
//        public static bool StartsWithAny(this string s, IEnumerable<string> items)
//        {
//            return items.Any(i => s.StartsWith(i));
//        }
//    }
//}
