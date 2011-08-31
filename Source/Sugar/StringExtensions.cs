﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Sugar
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
#if !CLIENT

        /// <summary>
        /// HTML Decodes this string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string HtmlDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        /// HTML Encodes this string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string HtmlEncode(this string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// URL encodes this string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// URL decodes this string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string UrlDecode(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }

#endif
        /// <summary>
        /// Determines whether the string starts with the specified value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <param name="isCaseSensitive">if set to <c>true</c> [is case sensitive].</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsWith(this string source, string value, bool isCaseSensitive)
        {
            StringComparison comparisonOption = isCaseSensitive ? StringComparison.CurrentCulture : StringComparison.InvariantCultureIgnoreCase;

            return source.StartsWith(value, comparisonOption);
        }

        /// <summary>
        /// Determines whether the string starts with the specified value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <param name="conparisonOption">The conparison option.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsWith(this string source, string value, StringComparison conparisonOption)
        {
            return source.IndexOf(value, 0, conparisonOption) == 0;
        }

        /// <summary>
        /// Substrings the after char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public static string SubstringAfterChar(this string value, string match)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(match))
            {
                if (value.Contains(match))
                {
                    result = value.Substring(value.IndexOf(match) + match.Length);
                }
            }

            return result;
        }

        public static string SubstringAfterLastChar(this string value, string match)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(match))
            {
                if (value.Contains(match))
                {
                    result = value.Substring(value.LastIndexOf(match) + match.Length);
                }
            }

            return result;
        }

        /// <summary>
        /// Substrings the before char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public static string SubstringBeforeChar(this string value, string match)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(match))
            {
                if (value.Contains(match))
                {
                    result = value.Substring(0, value.IndexOf(match));
                }
            }

            return result;
        }


        /// <summary>
        /// Substrings the before char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public static string SubstringBeforeLastChar(this string value, string match)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(match))
            {
                if (value.Contains(match))
                {
                    result = value.Substring(0, value.LastIndexOf(match));
                }
            }

            return result;
        }

        /// <summary>
        /// Keeps the specified characters in the given value, removed the rest.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="keepTheseCharacters">The keep these characters.</param>
        /// <returns></returns>
        public static string Keep(this string value, string keepTheseCharacters)
        {
            var result = string.Empty;

            if (value != null)
            {
                foreach (var @char in value.ToCharArray())
                {
                    if (!keepTheseCharacters.Contains(@char.ToString())) continue;

                    result += @char;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified value is numeric.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string value)
        {
            double temp;

            return double.TryParse(value, out temp);
        }

        public static string Repeat(this string value, int times)
        {
            var result = string.Empty;

            for (var i = 0; i < times; i++)
            {
                result += value;
            }

            return result;
        }

        public static string TrimTo(this string value, int length)
        {
            return TrimTo(value, length, string.Empty);
        }

        public static string TrimTo(this string value, int length, string overrunIndicator)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > length)
                {
                    result = value.Substring(0, length - overrunIndicator.Length) + overrunIndicator;
                }
            }

            return result;
        }

        /// <summary>
        /// Splits the specified value using the given seperator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seperator">The seperator.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static IList<string> Split(this string value, string seperator, StringSplitOptions options = StringSplitOptions.None)
        {
            var results = new List<string>();

            if (!string.IsNullOrWhiteSpace(value))
            {
                results.AddRange(value.Split(new[] { seperator }, options));
            }

            return results;
        }

        public static string Join(this IEnumerable<string> values, string seperator = "")
        {
            var sb = new StringBuilder();

            if (values != null)
            {
                foreach (var value in values)
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;

                    if (sb.Length > 0) sb.Append(seperator);

                    sb.Append(value);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified value contains any of the characters in the given input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified value contains any; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string value, string input)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(input) && !string.IsNullOrWhiteSpace(value))
            {
                foreach (var @char in input)
                {
                    result = value.Contains(@char.ToString());

                    if (result) break;                    
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the given string contains any numeric values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if  the given string contains any numeric values; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAnyNumeric(this string value)
        {
            return ContainsAny(value, "1234567890");
        }

        /// <summary>
        /// Prepends the specified value to the given string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="prependWith">The prepend with.</param>
        /// <returns></returns>
        public static string Prepend(this string value, string prependWith)
        {
            var result = prependWith;

            if (!string.IsNullOrWhiteSpace(value))
            {
                result = string.Concat(result, value);
            }

            return result;
        }

        /// <summary>
        /// Formats the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string FormatWith(this object value, string format)
        {
            var result = string.Empty;

            if (value != null)
            {
                result = string.Format(format, value);
            }

            return result;
        }
    }
}