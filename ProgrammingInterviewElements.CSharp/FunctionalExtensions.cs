using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgrammingInterviewElements.CSharp
{
    public static class FunctionalExtensions
    {
        #region IEnumerable Extensions

        //reverse is not efficient in general
        public static void ForEach<T>(this IEnumerable<T> collection,
                                      Action<T> action,
                                      bool reverse = false)
        {
            foreach (T item in reverse ? collection.Reverse() : collection)
            {
                action(item);
            }
        }

        public static T FoldL<T>(this IEnumerable<T> collection,
                                 T intialValue,
                                 Func<T, T, T> f) where T : struct
        {
            T acc = intialValue;
            collection.ForEach(val => acc = f(val, acc));
            return acc;
        }

        //not efficient in general, as it reverses the sequence
        public static T FoldR<T>(this IEnumerable<T> collection,
                                 T intialValue,
                                 Func<T, T, T> f) where T : struct
        {
            T acc = intialValue;
            collection.ForEach(val => acc = f(val, acc), true);
            return acc;
        }

        public static T Fold<T>(this IEnumerable<T> collection,
                                 T intialValue,
                                 Func<T, T, T> f) where T : struct
        {
            return FoldL(collection, intialValue, f);
        }

        #endregion
    }
}
