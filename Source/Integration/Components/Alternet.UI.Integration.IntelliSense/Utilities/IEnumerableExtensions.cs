using System;
using System.Collections.Generic;

namespace Alternet.UI.Integration.IntelliSense
{
    public static class IEnumerableExtensions
    {
        public static bool SequenceEqual<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> comparer)
        {
            if (first == null)
                throw new NullReferenceException("first");

            if (second == null)
                throw new NullReferenceException("second");

            using (IEnumerator<T1> e1 = first.GetEnumerator())
            using (IEnumerator<T2> e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && comparer(e1.Current, e2.Current)))
                        return false;
                }

                if (e2.MoveNext())
                    return false;
            }

            return true;
        }
    }
}