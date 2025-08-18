using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to <see cref="double"/> handling.
    /// </summary>
    public static class DoubleUtils
    {
        // Const values come from sdk\inc\crt\double.h

        /// <summary>
        /// Smallest such that 1.0+DBL_EPSILON != 1.0
        /// </summary>
        internal const double DoubleEpsilon = 2.2204460492503131e-016;

        /// <summary>
        /// Number close to zero, where double.MinValue is -double.MaxValue
        /// </summary>
        internal const double FltMin = 1.175494351e-38F;

        /// <summary>
        /// AreClose - Returns whether or not two doubles are "close".  That is, whether or
        /// not they are within epsilon of each other.  Note that this epsilon is proportional
        /// to the numbers themselves to that AreClose survives scalar multiplication.
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// </summary>
        /// <returns>
        /// bool - the result of the AreClose comparison.
        /// </returns>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        public static bool AreClose(double value1, double value2)
        {
            // in case they are Infinities (then epsilon check does not work)
            if (value1 == value2)
            {
                return true;
            }

            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DBL_EPSILON
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DoubleEpsilon;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        /// <summary>
        /// Compares two rectangles for fuzzy equality.  This function
        /// helps compensate for the fact that double values can
        /// acquire error when operated upon
        /// </summary>
        /// <param name='rect1'>The first rectangle to compare</param>
        /// <param name='rect2'>The second rectangle to compare</param>
        /// <returns>Whether or not the two rectangles are equal</returns>
        public static bool AreClose(RectD rect1, RectD rect2)
        {
            // If they're both empty, don't bother with the double logic.
            if (rect1.SizeIsEmpty)
            {
                return rect2.SizeIsEmpty;
            }

            // At this point, rect1 isn't empty, so the first thing we can test is
            // rect2.IsEmpty, followed by property-wise compares.
            return (!rect2.SizeIsEmpty) &&
                AreClose(rect1.X, rect2.X) &&
                AreClose(rect1.Y, rect2.Y) &&
                AreClose(rect1.Height, rect2.Height) &&
                AreClose(rect1.Width, rect2.Width);
        }

        /// <summary>
        /// Determines whether the specified double value is an even number.
        /// </summary>
        /// <param name="value">The double value to check.</param>
        /// <returns>True if the value is an even number; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(double value)
        {
            return value % 1 == 0 && value % 2 == 0;
        }

        /// <summary>
        /// LessThan - Returns whether or not the first double is less than the second double.
        /// That is, whether or not the first is strictly less than *and* not within epsilon of
        /// the other number.  Note that this epsilon is proportional to the numbers themselves
        /// to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// </summary>
        /// <returns>
        /// bool - the result of the LessThan comparison.
        /// </returns>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanAndNotClose(double value1, double value2)
        {
            return (value1 < value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// GreaterThan - Returns whether or not the first double is greater than the second double.
        /// That is, whether or not the first is strictly greater than *and* not within epsilon of
        /// the other number.  Note that this epsilon is proportional to the numbers themselves
        /// to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// </summary>
        /// <returns>
        /// bool - the result of the GreaterThan comparison.
        /// </returns>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanAndNotClose(double value1, double value2)
        {
            return (value1 > value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// LessThanOrClose - Returns whether or not the first double is less than or close to
        /// the second double.  That is, whether or not the first is strictly less than or within
        /// epsilon of the other number.  Note that this epsilon is proportional to the numbers
        /// themselves to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// </summary>
        /// <returns>
        /// bool - the result of the LessThanOrClose comparison.
        /// </returns>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrClose(double value1, double value2)
        {
            return (value1 < value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// GreaterThanOrClose - Returns whether or not the first double is greater than or close to
        /// the second double.  That is, whether or not the first is strictly greater than or within
        /// epsilon of the other number.  Note that this epsilon is proportional to the numbers
        /// themselves to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// </summary>
        /// <returns>
        /// bool - the result of the GreaterThanOrClose comparison.
        /// </returns>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrClose(double value1, double value2)
        {
            return (value1 > value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// IsOne - Returns whether or not the double is "close" to 1.  Same as AreClose(double, 1),
        /// but this is faster.
        /// </summary>
        /// <returns>
        /// bool - the result of the AreClose comparison.
        /// </returns>
        /// <param name="value"> The double to compare to 1. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(double value)
        {
            return Math.Abs(value - 1.0) < 10.0 * DoubleEpsilon;
        }

        /// <summary>
        /// IsZero - Returns whether or not the double is "close" to 0. Same as AreClose(double, 0),
        /// but this is faster.
        /// </summary>
        /// <returns>
        /// bool - the result of the AreClose comparison.
        /// </returns>
        /// <param name="value"> The double to compare to 0. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(double value)
        {
            return Math.Abs(value) < 10.0 * DoubleEpsilon;
        }

        /// <summary>
        /// Compares two Size instances for fuzzy equality.  This function
        /// helps compensate for the fact that double values can
        /// acquire error when operated upon
        /// </summary>
        /// <param name='size1'>The first size to compare</param>
        /// <param name='size2'>The second size to compare</param>
        /// <returns>Whether or not the two Size instances are equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreClose(SizeD size1, SizeD size2)
        {
            return DoubleUtils.AreClose(size1.Width, size2.Width) &&
                   DoubleUtils.AreClose(size1.Height, size2.Height);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="val">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DoubleToInt(double val)
        {
            return (val > 0) ? (int)(val + 0.5) : (int)(val - 0.5);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is either
        /// infinite (positive or negative) or not a number (NaN).
        /// </summary>
        /// <param name="value">The double-precision floating-point number to evaluate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is <see cref="double.NaN"/>,
        /// <see cref="double.PositiveInfinity"/>, or <see cref="double.NegativeInfinity"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinityOrNan(double value)
        {
            return IsNaN(value) || double.IsInfinity(value);
        }

        /// <summary>
        /// Determines whether the specified double-precision floating-point value is NaN,
        /// infinite, or equal to the
        /// maximum representable value.
        /// </summary>
        /// <remarks>This method can be used to validate or filter double values that are
        /// not finite or exceed typical numeric ranges.</remarks>
        /// <param name="value">The double-precision floating-point value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is NaN, infinite,
        /// or equal to <see
        /// cref="double.MaxValue"/>;  otherwise, <see langword="false"/>. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinityOrNanOrMax(double value)
        {
            return value == double.MaxValue || IsNaN(value) || double.IsInfinity(value);
        }

        /// <summary>
        /// Clamps the specified value to zero if it is less than zero.
        /// </summary>
        /// <param name="value">The value to clamp. If the value is negative,
        /// it will be clamped to zero; otherwise, the original value is
        /// returned.</param>
        /// <returns>The original value if it is zero or positive; otherwise, zero.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ClampToZero(double value) => value < 0 ? 0 : value;

        /// <summary>
        /// Fast implementation of the <see cref="double.IsNaN"/> method.
        /// </summary>
        /// <param name="value">The double-precision floating-point number to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is <see cref="double.NaN"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool IsNaN(double value)
        {
            // The standard CLR double.IsNaN() function is approximately 100 times slower
            // than our own wrapper,
            // so please make sure to use DoubleUtil.IsNaN() in performance sensitive code.
            // PS item that tracks the CLR improvement is DevDiv Schedule : 26916.
            // IEEE 754 : If the argument is any value in
            // the range 0x7ff0000000000001L through 0x7fffffffffffffffL
            // or in the range 0xfff0000000000001L through 0xffffffffffffffffL, the result will be NaN.
            NanUnion t = new();
            t.DoubleValue = value;

            ulong exp = t.UintValue & 0xfff0000000000000;
            ulong man = t.UintValue & 0x000fffffffffffff;

            return (exp == 0x7ff0000000000000 || exp == 0xfff0000000000000) && (man != 0);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)]
            internal double DoubleValue;
            [FieldOffset(0)]
            internal ulong UintValue;
        }
    }
}
