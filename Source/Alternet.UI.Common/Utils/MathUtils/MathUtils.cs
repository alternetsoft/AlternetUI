using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// Gets <see cref="Math.PI"/> divided by 180d.
        /// </summary>
        public const double DegToRad = Math.PI / 180d;

        /// <summary>
        /// Gets 180d divided by <see cref="Math.PI"/>.
        /// </summary>
        public const double RadToDeg = 180d / Math.PI;

        /// <summary>
        /// Gets <see cref="Math.PI"/> divided by 180f.
        /// </summary>
        public const float DegToRadF = (float)Math.PI / 180f;

        /// <summary>
        /// Gets 180f divided by <see cref="Math.PI"/>.
        /// </summary>
        public const float RadToDegF = 180f / (float)Math.PI;

        /// <summary>
        /// Gets high <see cref="short"/> of the <see cref="int"/> value.
        /// </summary>
        /// <param name="n">Integer value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short HighInt16(int n) =>
            unchecked((short)((n >> 16) & 0xffff));

        /// <summary>
        /// Gets low <see cref="short"/> of the <see cref="int"/> value.
        /// </summary>
        /// <param name="n">Integer value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short LowInt16(int n) => unchecked((short)(n & 0xffff));

        /// <summary>
        /// Returns <paramref name="min"/> if <paramref name="value"/> is null
        /// or less than <paramref name="min"/>; otherwise returns <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ValueOrMin(long? value, long min = default)
        {
            if (value is null || value <= min)
                return min;
            return value.Value;
        }

        /// <summary>
        /// Rounds up the given coordinate value to the nearest integer and
        /// increments it by 1 if the result is odd.
        /// </summary>
        /// <param name="value">The coordinate value to process.</param>
        /// <returns>An integer that is the rounded-up value incremented by 1 if it is odd.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundUpAndIncrementIfOdd(Coord value)
        {
            return IncrementIfOdd((int)Math.Ceiling(value));
        }

        /// <summary>
        /// Increments the given number by 1 if it is odd; otherwise, returns the number unchanged.
        /// </summary>
        /// <param name="number">The integer to check and possibly increment.</param>
        /// <returns>The incremented number if it is odd; otherwise, the original number.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IncrementIfOdd(int number)
        {
            return number % 2 != 0 ? number + 1 : number;
        }

        /// <summary>
        /// Maps value from range specified by
        /// (<paramref name="fromLow"/>, <paramref name="fromHigh"/>)
        /// to range specified by
        /// (<paramref name="toLow"/>, <paramref name="toHigh"/>).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromLow"></param>
        /// <param name="fromHigh"></param>
        /// <param name="toLow"></param>
        /// <param name="toHigh"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MapRanges(
            double value,
            double fromLow,
            double fromHigh,
            double toLow,
            double toHigh) =>
            ((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow)) + toLow;

        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="x1">X coordinate of the first point.</param>
        /// <param name="y1">Y coordinate of the first point.</param>
        /// <param name="x2">X coordinate of the second point.</param>
        /// <param name="y2">Y coordinate of the second point.</param>
        /// <returns><see cref="double"/> value with distance between point (x1, y1) and
        /// point (x2, y2).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        /// <summary>
        /// Gets percentage of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PercentOf(double value, double percent)
        {
            double result = (percent / 100) * value;
            return result;
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
        /// Returns the smaller of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="double"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static double Min(params double[] values)
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
                if (item < result)
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

        /// <summary>
        /// Returns the smaller of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="int"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static int Min(params int[] values)
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
                if (item < result)
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
        public static int Max(params int?[] values)
        {
            var length = values.Length;

            if (length < 2)
            {
                if (length == 0)
                    throw new ArgumentOutOfRangeException(nameof(values));
                return values[0] ?? 0;
            }

            var result = values[0] ?? 0;

            for (int i = 1; i < values.Length; i++)
            {
                var item = values[i] ?? 0;
                if (item > result)
                    result = item;
            }

            return result;
        }

        /// <summary>
        /// Applies minimum and maximum to the value if limitations are not <c>null</c>.
        /// </summary>
        /// <param name="value">Value to which min and max limits are applied.</param>
        /// <param name="min">Minimal value. Optional.</param>
        /// <param name="max">Maximal value. Optional.</param>
        /// <returns>Value with applied min and max limits.</returns>
        public static int ApplyMinMax(int value, int? min = null, int? max = null)
        {
            if (min is not null && value < min)
                value = min.Value;
            if (max is not null && value > max)
                value = max.Value;
            return value;
        }

        /// <inheritdoc cref="ApplyMinMax(int, int?, int?)"/>
        public static double ApplyMinMax(double value, double? min = null, double? max = null)
        {
            if (min is not null && value < min)
                value = min.Value;
            if (max is not null && value > max)
                value = max.Value;
            return value;
        }

        /// <inheritdoc cref="ApplyMinMax(int, int?, int?)"/>
        public static Coord ApplyMinMaxCoord(Coord value, Coord? min = null, Coord? max = null)
        {
            if (min is not null && value < min)
                value = min.Value;
            if (max is not null && value > max)
                value = max.Value;
            return value;
        }

        /// <summary>
        /// Returns <see cref="int"/> value clamped to the inclusive range of min and max.
        /// </summary>
        /// <param name="v">The value to be clamped.</param>
        /// <param name="min">The lower bound of the result.</param>
        /// <param name="max">The upper bound of the result.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int v, int min, int max)
        {
            return Math.Min(Math.Max(v, min), max);
        }

        /// <summary>
        /// Returns <see cref="double"/> value clamped to the inclusive range of min and max.
        /// </summary>
        /// <param name="v">The value to be clamped.</param>
        /// <param name="min">The lower bound of the result.</param>
        /// <param name="max">The upper bound of the result.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ClampD(double v, double min, double max)
        {
            return Math.Min(Math.Max(v, min), max);
        }

        /// <summary>
        /// Returns coordinate value clamped to the inclusive range of min and max.
        /// </summary>
        /// <param name="v">The value to be clamped.</param>
        /// <param name="min">The lower bound of the result.</param>
        /// <param name="max">The upper bound of the result.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coord ClampCoord(Coord v, Coord min, Coord max)
        {
            return Math.Min(Math.Max(v, min), max);
        }

        /// <summary>
        /// Combines two integer hash codes into a single value using high-entropy bit mixing.
        /// </summary>
        /// <param name="newKey">The new hash code to incorporate.</param>
        /// <param name="currentKey">The current accumulated hash code.</param>
        /// <returns>
        /// A 32-bit integer representing the combined hash value.
        /// </returns>
        /// <remarks>
        /// This method multiplies <paramref name="currentKey"/> by <c>0xA5555529</c>,
        /// a high-dispersion constant,
        /// and adds <paramref name="newKey"/> to it. The result achieves better distribution
        /// across bit space
        /// than simpler prime-based mixing strategies, and mirrors techniques used
        /// in internal .NET hashing.
        /// </remarks>
        public static int CombineHashCodes(int newKey, int currentKey)
        {
            return unchecked((currentKey * (int)0xA5555529) + newKey);
        }

        /// <summary>
        /// Computes a sequence-sensitive hash code for a collection
        /// of values using high-entropy mixing.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
        /// <param name="values">
        /// An <see cref="IEnumerable{T}"/> of values to hash. If <c>null</c>, returns <c>0</c>.
        /// </param>
        /// <returns>
        /// A 32-bit hash code representing the ordered contents of the sequence.
        /// Equal sequences produce equal hashes.
        /// </returns>
        /// <remarks>
        /// This method combines the hash codes of each item using a dispersion-aware
        /// mixing strategy defined in <c>CombineHashCodes</c>.
        /// The order of elements influences the result, making the hash sensitive
        /// to positional changes.
        /// </remarks>
        public static int SequentialValuesHash<T>(IEnumerable<T> values)
        {
            if (values is null)
                return 0;

            int hash = 0;
            foreach (var value in values)
            {
                hash = CombineHashCodes(hash, value?.GetHashCode() ?? 0);
            }

            return hash;
        }

        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static float ToRadians(float degrees)
        {
            var angleRadians = degrees * DegToRadF;

            // Normalize to [0, 2*pi) if needed
            if (angleRadians < 0f)
                angleRadians += 2f * (float)Math.PI;

            return angleRadians;
        }

        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static float ToDegrees(float radians)
        {
            var angleDegrees = radians * RadToDegF;

            // Ensure angle is in [0, 360)
            if (angleDegrees < 0f)
                angleDegrees += 360f;

            return angleDegrees;
        }

        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static double ToRadians(double degrees)
        {
            var angleRadians = degrees * DegToRadF;

            // Normalize to [0, 2*pi) if needed
            if (angleRadians < 0d)
                angleRadians += 2d * Math.PI;

            return angleRadians;
        }

        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static double ToDegrees(double radians)
        {
            var angleDegrees = radians * RadToDeg;

            // Ensure angle is in [0, 360)
            if (angleDegrees < 0d)
                angleDegrees += 360d;

            return angleDegrees;
        }
    }
}
