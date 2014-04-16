﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sugar
{
    /// <summary>
    /// To CSV extension
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs the given action on each of the elements in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="each">The each.</param>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> each)
        {
            foreach (var item in enumerable)
                each(item);
        }

        /// <summary>
        /// Returns a comma seprated value representation of this list of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <param name="separator">The seperator.</param>
        /// <param name="lastSeparator">The last separator (is set to separator when null or empty).</param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> values, string separator = ",", string lastSeparator = null)
        {
            var result = new StringBuilder();

            if (values != null)
            {
                if (String.IsNullOrEmpty(lastSeparator))
                {
                    lastSeparator = separator;
                }

                var lastIndex = values.Count() - 1;
                var index = 0;

                foreach (var value in values)
                {
                    if (result.Length > 0)
                    {
                        result.Append(index == lastIndex ? lastSeparator : separator);
                    }

                    result.Append(value);
                    index++;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts a CSV string to list of <see cref="T"/> objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<T> FromCsv<T>(this string value)
        {
            var results = new List<T>();

            if (value != null)
            {
                var current = string.Empty;
                var inQuotes = false;

                foreach (var @char in value.ToCharArray())
                {
                    if (@char == ',' && !inQuotes)
                    {
                        var newValue = (T)Convert.ChangeType(current, typeof(T));

                        results.Add(newValue);

                        current = string.Empty;
                    }
                    else if (@char == '"')
                    {
                        inQuotes = !inQuotes;
                    }
                    else
                    {
                        current += @char;
                    }
                }

                if (!string.IsNullOrEmpty(current))
                {
                    var newValue = (T)Convert.ChangeType(current, typeof(T));

                    results.Add(newValue);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns a collection of strings from the given CSV formatted string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Handles quoted fields, not sure how quotes are escaped within though.
        /// </remarks>
        public static IList<string> FromCsv(this string value)
        {
            return value.FromCsv<string>();
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, long> selector)
        {
            return collection.Distinct<T, long>(selector);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, ulong> selector)
        {
            return collection.Distinct<T, ulong>(selector);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, double> selector)
        {
            return collection.Distinct<T, double>(selector);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, string> selector)
        {
            return collection.Distinct<T, string>(selector);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, bool> selector)
        {
            return collection.Distinct<T, bool>(selector);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, char> selector)
        {
            return collection.Distinct<T, char>(selector);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by comparing the given values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrim">The type of the prim.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        private static IEnumerable<T> Distinct<T, TPrim>(this IEnumerable<T> collection, Func<T, TPrim> selector) where TPrim : IComparable
        {
            var results = new List<T>();

            foreach (var element in collection
                .Where(element => results
                    .Select(selector)
                    .All(r => !r.Equals(selector(element)))))
            {
                results.Add(element);
            }

            return results;
        }

        /// <summary>
        /// Reindexes the specified collection, using the given index as the first element.  Items before the index are
        /// appended to the end.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static IEnumerable<T> Reindex<T>(this IEnumerable<T> collection, int index)
        {
            var results = new List<T>();
            var count = collection.Count();

            if (index >= count)
            {
                throw new ArgumentException("Can't reindex a list at a position greater than the contents of the list.");
            }

            for (var i = index; i < count; i++)
            {
                results.Add(collection.ElementAt(i));
            }

            for (var i = 0; i < index; i++)
            {
                results.Add(collection.ElementAt(i));
            }

            return results;
        }

        /// <summary>
        /// Strips the specified collection of the given value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="toStrip">To strip.</param>
        /// <returns></returns>
        public static IEnumerable<T> Strip<T>(this IEnumerable<T> collection, T toStrip)
        {
            var results = new List<T>();

            foreach (var candidate in collection)
            {
                if (!Equals(toStrip, candidate))
                {
                    results.Add(candidate);
                }
            }

            return results;
        }

        /// <summary>
        /// Strips the specified string collection of the empty values.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<string> StripNullOrWhitespace(this IEnumerable<string> collection)
        {
            var results = new List<string>();

            foreach (var candidate in collection)
            {
                if (!string.IsNullOrWhiteSpace(candidate))
                {
                    results.Add(candidate);
                }
            }

            return results;
        }

        /// <summary>
        /// Trims all the values in the given collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<string> Trim(this IEnumerable<string> collection)
        {
            var results = new List<string>();

            foreach (var item in collection)
            {
                if (item != null)
                {
                    results.Add(item.Trim());
                }
            }

            return results;
        }

        /// <summary>
        /// Matcheses the wildcard.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="pattern">The pattern.</param>
        /// <remarks>
        /// From: http://stackoverflow.com/questions/3102250/linq-search-using-wildcards-character-like
        /// </remarks>
        public static IEnumerable<T> WildcardSearch<T>(this IEnumerable<T> sequence, Func<T, string> expression, string pattern)
        {
            var regEx = WildcardToRegex(pattern);

            return sequence.Where(item => Regex.IsMatch(expression(item), regEx));
        }

        /// <summary>
        /// Wildcards to regex.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <remarks>
        /// From: http://stackoverflow.com/questions/3102250/linq-search-using-wildcards-character-like
        /// </remarks>
        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex
                .Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$";
        }
    }
}
