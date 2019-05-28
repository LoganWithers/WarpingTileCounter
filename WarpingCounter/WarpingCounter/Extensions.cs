namespace WarpingCounter
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Numerics;

    using Common.Models;

    public static class Extensions
    {

        public static List<string> ToBase(this string source, int radix)
        {
            Dictionary<BigInteger, string> digits = NumberUtils.Digits.ToDictionary(BigInteger.Parse, d => d);

            if (radix < 2 || radix > digits.Count)
            {
                throw new ArgumentException($"The radix must be >= 2 and <= {digits.Count}");
            }

            var decimalNumber = BigInteger.Parse(source);

            if (decimalNumber.IsZero)
            {
                return new List<string> {"0"};
            }

            var currentNumber = BigInteger.Abs(decimalNumber);
            var digitList     = new LinkedList<string>();
            var bigRadix      = new BigInteger(radix);

            while (currentNumber != 0)
            {
                var remainder = currentNumber % bigRadix;
                digitList.AddFirst(digits[remainder]);
                currentNumber /= bigRadix;
            }

            return new List<string>(digitList);
        }


        public static bool None<T>(this IEnumerable<T> elements) => !elements.Any();

        public static bool None<T>(this IEnumerable<T> elements, Func<T, bool> predicate) => !elements.Any(predicate);
        
        /// <summary>
        ///   Retrieves the last n characters from this instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public static string GetLast(this string source, int n) => n >= source.Length ? source : source.Substring(source.Length - n);


        /// <summary>
        ///   Converts a <see cref="BigInteger" /> value to the specified radix.
        /// </summary>
        /// <param name="decimalValue">The value to convert</param>
        /// <param name="radix">The radix of the new value.</param>
        /// <returns></returns>
        public static List<string> ToBase(this BigInteger decimalValue, int radix) => decimalValue.ToString()
                                                                                                  .ToBase(radix);


        /// <summary>
        ///   Splits the elements into chunks of size n.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements">The elements to split.</param>
        /// <param name="chunkSize">The number of elements in each chunk.</param>
        /// <returns></returns>
        [Pure]
        public static IEnumerable<IEnumerable<T>> SplitEvery<T>(this IReadOnlyCollection<T> elements, int chunkSize)
        {
            List<T> head = elements.Take(chunkSize)
                                   .ToList();

            List<T> tail = elements.Skip(chunkSize)
                                   .ToList();

            return head.Any() ? Cons(head, SplitEvery(tail, chunkSize)) : Enumerable.Empty<IEnumerable<T>>();
        }


        /// <summary>
        ///   Appends the head in front of the other elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="head">The head.</param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public static IEnumerable<T> Cons<T>(T head, IEnumerable<T> elements)
        {
            yield return head;

            foreach (var element in elements)
            {
                yield return element;
            }
        }


        public static void PrependNamesWith(this IEnumerable<Tile> tiles, string name)
        {
            IEnumerable<Tile> enumerable = tiles.ToList();
            var               first      = enumerable.First();
            var               last       = enumerable.Last();

            foreach (var tile in enumerable)
            {
                tile.Prepend(name);
            }

            first.Prepend("First");
            last.Prepend("Last");
        }


        public static void AppendRange<T>(this LinkedList<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.AddLast(item);
            }
        }


        /// <summary>
        ///   Removes all duplicates based on the key each element returns.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    }

}
