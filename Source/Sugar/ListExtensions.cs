using System;
using System.Collections.Generic;
using System.Linq;

namespace Sugar
{
    /// <summary>
    /// IList entension methods
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Gets the maximum index of the item in a list that matches the given filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static int GetMaximumIndexUsingFilter<T>(this IList<T> collection, Func<T, bool> filter)
        {
            var index = -1;

            for (var i = 0; i < collection.Count; i++)
            {
                if (filter.Invoke(collection[i]))
                {
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// Gets the index of the first item in the list that matches the given filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static int GetIndexUsingFilter<T>(this IList<T> collection, Func<T, bool> filter)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (filter.Invoke(collection[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Swaps the items at the specified indices.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The columns.</param>
        /// <param name="removeIndex">Index of the remove.</param>
        /// <param name="insertIndex">Index of the insert.</param>
        /// <returns></returns>
        public static IList<T> RemoveAndInsertAt<T>(this IList<T> collection, int removeIndex, int insertIndex)
        {
            var temp = collection[removeIndex];

            collection.RemoveAt(removeIndex);
            collection.Insert(insertIndex, temp);

            return collection;
        }

        /// <summary>
        /// Removes an item from the collection if a predicate evaluates to true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveIf<T>(this IList<T> collection, Func<T, bool> predicate)
        {
            for (var counter = collection.Count - 1; counter >= 0; counter--)
            {
                var thisItem = collection[counter];

                if (predicate(thisItem))
                {
                    collection.RemoveAt(counter);
                }
            }
        }

        /// <summary>
        /// Split the given collection into chunks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Chunkify<T>(this IList<T> collection, int size)
        {
            for (var i = 0; i < collection.Count(); i += size)
            {
                yield return collection.Skip(i).Take(size).ToList();
            }
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
    }
}