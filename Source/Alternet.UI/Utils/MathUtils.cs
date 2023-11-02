using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Math"/> related static methods.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="x1">X coordinate of the first point.</param>
        /// <param name="y1">Y coordinate of the first point.</param>
        /// <param name="x2">X coordinate of the second point.</param>
        /// <param name="y2">Y coordinate of the second point.</param>
        /// <returns><see cref="double"/> value with distance between point (x1, y1) and
        /// point (x2, y2).</returns>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns><see cref="double"/> value with distance between point <paramref name="p1"/>
        /// and point <paramref name="p2"/>.</returns>
        public static double GetDistance(Point p1, Point p2)
        {
            return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        /// <summary>
        /// Compares distance between two points and specified value.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="value">Value to compare distance with.</param>
        /// <returns><c>true</c> if distance between two points is less
        /// than <paramref name="value"/>; <c>false</c> otherwise.</returns>
        public static bool DistanceIsLess(Point p1, Point p2, double value)
        {
            return (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)) < Math.Pow(value, 2);
        }

        /// <summary>
        /// Returns the larger of two objects.
        /// Objects must support <see cref="IComparable"/> interface.
        /// </summary>
        /// <param name="a">The first of two objects to compare.</param>
        /// <param name="b">The second of two objects to compare.</param>
        /// <returns>Parameter <paramref name="a"/> or <paramref name="b"/>,
        /// whichever is larger.</returns>
        /// <remarks>Any of the objects can be <c>null</c>.</remarks>
        public static object? Max(object? a, object? b)
        {
            if (a is not IComparable)
                return b;
            if (b is null)
                return a;
            var result = SafeCompareTo(a, b);
            if (result < 0) // "a" precedes "b" in the sort order.
                return b;
            return a;
        }

        /// <summary>
        /// Compares <paramref name="value"/> with the default value of the
        /// specified type.
        /// </summary>
        /// <param name="code">Type code.</param>
        /// <param name="value">Value to compare with the default value.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is less than a default value;
        /// <c>false</c> otherwise.</returns>
        public static bool? LessThanDefault(TypeCode code, object? value)
        {
            if (value is null)
                return null;
            var defaultValue = AssemblyUtils.GetDefaultValue(code);
            if (defaultValue is null)
                return null;
            var result = MathUtils.SafeCompareTo(value, defaultValue);
            return result < 0;
        }

        /// <summary>
        /// Checks whether <paramref name="value"/> is in range specified with
        /// <paramref name="minValue"/> and <paramref name="maxValue"/> parameters.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="minValue">Minimal possible value.</param>
        /// <param name="maxValue">Maximal possible value.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is in range;
        /// <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If range parameter is <c>null</c> it is not checked. If <paramref name="value"/> is null,
        /// <c>null</c> is returned.
        /// </remarks>
        public static ValueInRangeResult ValueInRange(object? value, object? minValue, object? maxValue)
        {
            if (value is null)
                return ValueInRangeResult.Unknown;

            if(minValue is not null)
            {
                var result = SafeCompareTo(value, minValue);
                if (result < 0)
                    return ValueInRangeResult.Less;
            }

            if (maxValue is not null)
            {
                var result = SafeCompareTo(value, maxValue);
                if (result > 0)
                    return ValueInRangeResult.Greater;
            }

            return ValueInRangeResult.Ok;
        }

        /// <summary>
        /// Same as <see cref="IComparable.CompareTo"/> but additionally allows
        /// to compare different integer types without exceptions.
        /// Objects must support <see cref="IComparable"/> interface.
        /// </summary>
        /// <param name="a">The first of two objects to compare.</param>
        /// <param name="b">The second of two objects to compare.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// Meanings: Result is less than zero – <paramref name="a"/> precedes <paramref name="b"/>
        /// in the sort order. Result is zero – <paramref name="a"/> occurs in the same position
        /// in the sort order as <paramref name="b"/>. Result is greater than zero – <paramref name="a"/>
        /// follows <paramref name="b"/> in the sort order.
        /// </returns>
        public static int SafeCompareTo(object a, object b)
        {
            var aUpgraded = AssemblyUtils.UpgradeNumberType(a);
            var bUpgraded = AssemblyUtils.UpgradeNumberType(b);
            var aTypeCode = AssemblyUtils.GetRealTypeCode(aUpgraded.GetType());
            var bTypeCode = AssemblyUtils.GetRealTypeCode(bUpgraded.GetType());
            if (aTypeCode == bTypeCode)
                return (aUpgraded as IComparable)!.CompareTo(bUpgraded);
            if (aTypeCode == TypeCode.Int64 && bTypeCode == TypeCode.UInt64)
                return -1;
            if (aTypeCode == TypeCode.UInt64 && bTypeCode == TypeCode.Int64)
                return 1;
            return (aUpgraded as IComparable)!.CompareTo(bUpgraded);
        }

        /// <summary>
        /// Returns the smaller of two objects.
        /// Objects must support <see cref="IComparable"/> interface.
        /// </summary>
        /// <param name="a">The first of two objects to compare.</param>
        /// <param name="b">The second of two objects to compare.</param>
        /// <returns>Parameter <paramref name="a"/> or <paramref name="b"/>,
        /// whichever is smaller.</returns>
        /// <remarks>Any of the objects can be <c>null</c>.</remarks>
        public static object? Min(object? a, object? b)
        {
            if (a is null)
                return b;
            if (b is null)
                return a;
            var result = SafeCompareTo(a, b);
            if (result < 0) // "a" precedes "b" in the sort order.
                return a;
            return b;
        }

        /// <summary>
        /// Returns the larger of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="double"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static double Max(params double[] values)
        {
            var length = values.Length;

            if (length < 2)
            {
                if(length == 0)
                    throw new ArgumentOutOfRangeException(nameof(values));
                return values[0];
            }

            var result = values[0];

            for(int i = 1; i < values.Length; i++)
            {
                var item = values[i];
                if (item > result)
                    result = item;
            }

            return result;
        }

        /// <summary>
        /// Returns the larger of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="int"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static int Max(params int[] values)
        {
            var length = values.Length;

            if (length < 2)
            {
                if (length == 0)
                    throw new ArgumentOutOfRangeException(nameof(values));
                return values[0];
            }

            var result = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                var item = values[i];
                if (item > result)
                    result = item;
            }

            return result;
        }

        internal static double ApplyMinMax(double value, double? min = null, double? max = null)
        {
            if (min is not null && value < min)
                value = min.Value;
            if (max is not null && value > max)
                value = max.Value;
            return value;
        }
    }
}
