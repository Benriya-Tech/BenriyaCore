using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Benriya.Utils.Extensions
{
    public static class StringExtension
    {
        public static string TrimAndReduce(this string str,string replace = " ")
        {
            return ConvertWhitespaces(str,replace).Trim();
        }
        public static string ConvertWhitespaces(this string value,string replace = " ")
        {
            return Regex.Replace(value, @"\s+", replace);
        }
        public static string RemoveFromLast(this string str, int len = 1)
        {
            return str.Remove(str.Length - len);
        }
        public static string RemoveFromFirst(this string str, int len = 1)
        {
            return str.Remove(0 - len);
        }
        public static bool isNOEOW(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static bool IsBase64String(this string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
        public static bool IsValidJson(this string str)
        {
            str = str.Trim();
            if ((str.StartsWith("{") && str.EndsWith("}")) || //For object
                (str.StartsWith("[") && str.EndsWith("]"))) //For array
            {
                try
                {
                    //parse the input into a JObject
                    var jObject = JObject.Parse(str);

                    foreach (var jo in jObject)
                    {
                        string name = jo.Key;
                        JToken value = jo.Value;

                        //if the element has a missing value, it will be Undefined - this is invalid
                        if (value.Type == JTokenType.Undefined)
                        {
                            return false;
                        }
                    }
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public static bool IsGuid(this string str)
        {
            return Guid.TryParse(str, out _);
        }

        public static bool isNumberic(this string str)
        {
            return int.TryParse(str, out _);
        }
        public static bool isDecimal(this string str)
        {
            return decimal.TryParse(str, out _);
        }
        public static bool StartsWithAny(this string str, IEnumerable<string> items)
        {
            return items.Any(i => str.StartsWith(i));
        }

        public static int ToInterger(this string str)
        {
            return int.Parse(str, NumberStyles.AllowLeadingSign);
        }
        public static Guid ToGuid(this string str)
        {
            return Guid.Parse(str);
        }
        public static bool ContainsAny(this string input, IEnumerable<string> containsKeywords, StringComparison comparisonType)
        {
            return containsKeywords.Any(keyword => input.IndexOf(keyword, comparisonType) >= 0);
        }
    }
}
