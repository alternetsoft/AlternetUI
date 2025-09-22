using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    public static partial class MathUtils
    {
        /// <summary>
        /// Represents the smallest positive value that can be added to 1.0f
        /// to produce a distinct value of type <see cref="float"/>.
        /// </summary>
        /// <remarks>This constant is useful for comparisons and calculations that require a very small
        /// tolerance value to account for floating-point precision errors.</remarks>
        public const float FloatEpsilon = 1.192092896e-07F;

        /// <summary>
        /// Optional override for single-precision floating-point equality comparison.
        /// If set, this delegate will be used by <c>AreClose(float, float)</c>
        /// instead of the default implementation.
        /// </summary>
        public static Func<float, float, bool>? AreCloseOverrideF;

        /// <summary>
        /// Determines whether the first value is less than the second value
        /// and not approximately equal to it.
        /// </summary>
        /// <remarks>The method uses a predefined tolerance to determine whether the two values are
        /// approximately equal.</remarks>
        /// <param name="value1">The first floating-point value to compare.</param>
        /// <param name="value2">The second floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/>
        /// is less than <paramref name="value2"/>  and the two
        /// values are not considered approximately equal;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanAndNotClose(float value1, float value2)
        {
            return (value1 < value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// Determines whether the first value is greater than the
        /// second value and not approximately equal to it.
        /// </summary>
        /// <remarks>The method uses a predefined tolerance to determine whether the two values are
        /// approximately equal.</remarks>
        /// <param name="value1">The first floating-point value to compare.</param>
        /// <param name="value2">The second floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is
        /// greater than <paramref name="value2"/>  and the two
        /// values are not approximately equal; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanAndNotClose(float value1, float value2)
        {
            return (value1 > value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// Determines whether the first floating-point
        /// value is less than or approximately equal to the second value.
        /// </summary>
        /// <remarks>The method uses a predefined tolerance to determine whether
        /// the two values are
        /// approximately equal. This is useful for comparisons where floating-point
        /// precision errors might otherwise
        /// lead to unexpected results.</remarks>
        /// <param name="value1">The first floating-point value to compare.</param>
        /// <param name="value2">The second floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is less than
        /// <paramref name="value2"/>, or if the two
        /// values are approximately equal; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrClose(float value1, float value2)
        {
            return (value1 < value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// Returns true if the two single-precision floats are close enough to be considered equal.
        /// Handles infinities and near-zero values using scaled epsilon.
        /// </summary>
        /// <param name="value1">First value to compare.</param>
        /// <param name="value2">Second value to compare.</param>
        /// <returns><c>true</c> if the values are close; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method scales the epsilon based on the magnitude of the inputs,
        /// ensuring robustness near zero and across large ranges.
        /// </remarks>
        /// <example>
        /// <code>
        /// AreCloseScaled(1.0f, 1.000001f)     // true
        /// AreCloseScaled(0.0f, 1e-8f)         // false
        /// AreCloseScaled(float.NaN, float.NaN) // false
        /// AreCloseScaled(float.PositiveInfinity, float.PositiveInfinity) // true
        /// </code>
        /// </example>
        public static bool AreCloseScaled(float value1, float value2)
        {
            if (value1 == value2)
            {
                return true;
            }

            float delta = value1 - value2;
            float eps = (Math.Abs(value1) + Math.Abs(value2)) * FloatEpsilon;

            if (eps == 0.0f)
            {
                eps = FloatEpsilon;
            }

            return Math.Abs(delta) < eps;
        }

        /// <summary>
        /// Returns true if the two single-precision floats are close enough to be considered equal,
        /// using both relative and absolute tolerance.
        /// </summary>
        /// <param name="a">First value to compare.</param>
        /// <param name="b">Second value to compare.</param>
        /// <returns><c>true</c> if the values are close; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method handles near-zero comparisons and large magnitudes
        /// by combining relative and absolute tolerances.
        /// It is symmetric with <see cref="AreCloseWithTolerance(double, double)"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// AreCloseWithTolerance(1.0f, 1.000001f);     // true
        /// AreCloseWithTolerance(0.0f, 1e-8f);         // false
        /// AreCloseWithTolerance(float.NaN, float.NaN); // false
        /// AreCloseWithTolerance(float.PositiveInfinity, float.PositiveInfinity); // true
        /// </code>
        /// </example>
        public static bool AreCloseWithTolerance(float a, float b)
        {
            if (a == b)
                return true;

            const float relTol = 1e-6f;
            const float absTol = 1e-8f;

            float diff = Math.Abs(a - b);
            float scale = Math.Max(Math.Abs(a), Math.Abs(b));
            float eps = Math.Max(relTol * scale, absTol);

            return diff <= eps;
        }

        /// <summary>
        /// Returns true if the two single-precision floats
        /// are close enough to be considered equal,
        /// using both relative and absolute tolerance thresholds.
        /// </summary>
        /// <param name="a">First value to compare.</param>
        /// <param name="b">Second value to compare.</param>
        /// <param name="relTol">Relative tolerance, scaled by the magnitude of the inputs.
        /// Default is 1e-6f.</param>
        /// <param name="absTol">Absolute tolerance, used for near-zero comparisons.
        /// Default is 1e-8f.</param>
        /// <returns><c>true</c> if the values are close within the specified tolerances;
        /// otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method is robust across magnitudes and avoids false positives near zero.
        /// It is symmetric with <see cref="AreCloseWithTolerance(double, double)"/>
        /// and suitable for diagnostic-grade comparisons.
        /// </remarks>
        /// <example>
        /// <code>
        /// AreCloseWithToleranceEx(1.0f, 1.000001f);     // true
        /// AreCloseWithToleranceEx(0.0f, 1e-8f);         // false
        /// AreCloseWithToleranceEx(1e-7f, 2e-7f, 1e-5f, 1e-7f); // true
        /// AreCloseWithToleranceEx(float.NaN, float.NaN); // false
        /// AreCloseWithToleranceEx(float.PositiveInfinity, float.PositiveInfinity); // true
        /// </code>
        /// </example>
        public static bool AreCloseWithToleranceEx(
            float a,
            float b,
            float relTol = 1e-6f,
            float absTol = 1e-8f)
        {
            if (a == b)
                return true;

            float diff = Math.Abs(a - b);
            float scale = Math.Max(Math.Abs(a), Math.Abs(b));
            float eps = Math.Max(relTol * scale, absTol);

            return diff <= eps;
        }

        /// <summary>
        /// Determines whether a single-precision floating-point value represents an even integer.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is an integer and divisible by 2;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// IsEven(4f);     // returns true
        /// IsEven(3f);     // returns false
        /// IsEven(2.5f);   // returns false
        /// IsEven(-6f);    // returns true
        /// IsEven(0f);     // returns true
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEvenInteger(float value)
        {
            if (value % 1 != 0)
                return false;

            return ((int)value % 2) == 0;
        }

        /// <summary>
        /// Determines whether the specified floating-point value represents an integer.
        /// </summary>
        /// <remarks>A value is considered an integer if it has no fractional component.
        /// For example, 5.0 and -3.0 are integers, but 5.5 and -3.1 are not.</remarks>
        /// <param name="value">The floating-point value to evaluate.</param>
        /// <returns><see langword="true"/> if the specified value is an integer;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool IsInteger(float value)
        {
            return value % 1 == 0;
        }

        /// <summary>
        /// Determines whether the specified single-precision floating-point value is NaN (Not a Number).
        /// This method uses bit-level inspection for performance-critical scenarios.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the value is NaN; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// IEEE 754: NaN values have all exponent bits set (0x7F8) and a non-zero mantissa.
        /// This avoids the overhead of <see cref="float.IsNaN"/> and is suitable for tight loops.
        /// </remarks>
        /// <example>
        /// <code>
        /// IsNaN(float.NaN);               // returns true
        /// IsNaN(0f);                      // returns false
        /// IsNaN(float.PositiveInfinity);  // returns false
        /// IsNaN(float.NegativeInfinity);  // returns false
        /// IsNaN(1f / 0f);                 // returns false (infinity)
        /// IsNaN(0f / 0f);                 // returns true (NaN)
        /// </code>
        /// </example>
        public static bool IsNaN(float value)
        {
            FloatUnion u = new() { FloatValue = value };
            uint exp = u.UIntValue & 0x7F800000;
            uint man = u.UIntValue & 0x007FFFFF;
            return exp == 0x7F800000 && man != 0;
        }

        /// <summary>
        /// Determines whether the specified single-precision floating-point value is NaN (Not a Number).
        /// This method uses bit-level inspection.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the value is NaN; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// IsNaNFast(float.NaN);               // returns true
        /// IsNaNFast(0f);                      // returns false
        /// IsNaNFast(float.PositiveInfinity);  // returns false
        /// IsNaNFast(float.NegativeInfinity);  // returns false
        /// IsNaNFast(1f / 0f);                 // returns false (infinity)
        /// IsNaNFast(0f / 0f);                 // returns true (NaN)
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaNFast(float value)
        {
            return IsNaN(value);
        }

        /// <summary>
        /// Returns true if the two single-precision floats are close enough to be considered equal,
        /// based on bitwise proximity. Handles sign differences explicitly.
        /// </summary>
        /// <param name="a">First value to compare.</param>
        /// <param name="b">Second value to compare.</param>
        /// <returns><c>true</c> if the values are within a few representable steps;
        /// otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method compares the bitwise distance between two floats, allowing up
        /// to <c>maxSteps</c> ULPs (Units in the Last Place).
        /// It is symmetric with <see cref="AreCloseBitwise(double, double)"/>
        /// and suitable for low-level numeric diagnostics.
        /// </remarks>
        /// <example>
        /// <code>
        /// AreCloseBitwise(1.0f, 1.0000001f)     // true
        /// AreCloseBitwise(0.0f, -0.0f)          // true
        /// AreCloseBitwise(float.NaN, float.NaN) // false
        /// AreCloseBitwise(float.PositiveInfinity, float.PositiveInfinity) // true
        /// </code>
        /// </example>
        public static bool AreCloseBitwise(float a, float b)
        {
            const int maxSteps = 4;

            int aBits = FloatUnion.AsInt(a);
            int bBits = FloatUnion.AsInt(b);

            // Handle sign differences explicitly
            if ((aBits < 0) != (bBits < 0))
                return a == b;

            int distance = Math.Abs(aBits - bBits);
            return distance <= maxSteps;
        }

        /// <summary>
        /// Determines whether two <see cref="float"/> values are equal using
        /// a layered comparison strategy.
        /// This method combines magnitude-scaled epsilon comparison,
        /// bitwise proximity (with default step threshold),
        /// and relative/absolute tolerance checks.
        /// It is suitable for robust floating-point validation across domains such
        /// as diagnostics, serialization, and numerical correctness.
        /// </summary>
        /// <param name="a">The first float value to compare.</param>
        /// <param name="b">The second float value to compare.</param>
        /// <returns>
        /// <c>true</c> if any of the following conditions are met:
        /// <list type="bullet">
        /// <item><description><see cref="AreCloseScaled(float,float)"/>
        /// returns true</description></item>
        /// <item><description><see cref="AreCloseBitwise(float,float)"/>
        /// returns true using
        /// its default threshold</description></item>
        /// <item><description><see cref="AreCloseWithTolerance(float,float)"/>
        /// returns true</description></item>
        /// </list>
        /// Otherwise, <c>false</c>.
        /// </returns>
        public static bool AreClose(float a, float b)
        {
            if (AreCloseOverrideF != null)
                return AreCloseOverrideF(a, b);

            return AreCloseScaled(a, b)
                || AreCloseBitwise(a, b)
                || AreCloseWithTolerance(a, b);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is either
        /// infinite (positive or negative) or not a number (NaN).
        /// </summary>
        /// <param name="value">The floating-point number to evaluate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is <see cref="float.NaN"/>,
        /// <see cref="float.PositiveInfinity"/>, or <see cref="float.NegativeInfinity"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinityOrNan(float value)
        {
            return IsNaN(value) || float.IsInfinity(value);
        }

        /// <summary>
        /// Determines whether the specified floating-point value is NaN,
        /// infinite, or equal to the
        /// maximum representable value.
        /// </summary>
        /// <remarks>This method can be used to validate or filter floating-point values that are
        /// not finite or exceed typical numeric ranges.</remarks>
        /// <param name="value">The floating-point value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is NaN, infinite,
        /// or equal to <see
        /// cref="float.MaxValue"/>;  otherwise, <see langword="false"/>. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinityOrNanOrMax(float value)
        {
            return value == float.MaxValue || IsNaN(value) || float.IsInfinity(value);
        }

        /// <summary>
        /// Clamps the specified value to zero if it is less than zero.
        /// </summary>
        /// <param name="value">The value to clamp. If the value is negative,
        /// it will be clamped to zero; otherwise, the original value is
        /// returned.</param>
        /// <returns>The original value if it is zero or positive; otherwise, zero.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampToZero(float value) => value < 0 ? 0 : value;

        /// <summary>
        /// Determines whether the first floating-point
        /// value is greater than or approximately equal
        /// to the second value.
        /// </summary>
        /// <remarks>Two values are considered approximately equal if the difference
        /// between them is
        /// within  a small tolerance. The tolerance is determined by the implementation
        /// of the <c>AreClose(float,float)</c> method.</remarks>
        /// <param name="value1">The first floating-point value to compare.</param>
        /// <param name="value2">The second floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is greater
        /// than or approximately equal to  <paramref
        /// name="value2"/>; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrClose(float value1, float value2)
        {
            return (value1 > value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// Determines whether the specified value is equal to 1.0 within a small tolerance.
        /// </summary>
        /// <remarks>This method uses a tolerance-based comparison to account for potential floating-point
        /// precision errors.</remarks>
        /// <param name="value">The floating-point number to compare to 1.0.</param>
        /// <returns><see langword="true"/> if the specified value is approximately equal to 1.0;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(float value)
        {
            return AreClose(value, 1);
        }

        /// <summary>
        /// Determines whether the specified floating-point value is equal to zero.
        /// </summary>
        /// <remarks>The comparison accounts for potential floating-point
        /// precision issues and considers
        /// values close to zero as equal to zero.</remarks>
        /// <param name="value">The floating-point value to compare to zero.</param>
        /// <returns><see langword="true"/> if the specified value is considered equal to zero;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(float value)
        {
            return AreClose(value, 0);
        }

        /// <summary>
        /// Returns the smallest integral value greater than or equal
        /// to the specified single-precision float.
        /// Behaves like <see cref="Math.Ceiling(double)"/> including handling of NaN and infinities.
        /// </summary>
        /// <param name="value">The value to round up.</param>
        /// <returns>The smallest integral float greater than or equal
        /// to <paramref name="value"/>.</returns>
        /// <remarks>
        /// This method mimics <see cref="Math.Ceiling(double)"/> behavior for <c>float</c> values,
        /// including correct handling of special IEEE 754 values.
        /// </remarks>
        /// <example>
        /// <code>
        /// Ceiling(3.14f);       // returns 4.0f
        /// Ceiling(-2.7f);       // returns -2.0f
        /// Ceiling(float.NaN);   // returns float.NaN
        /// Ceiling(float.PositiveInfinity); // returns float.PositiveInfinity
        /// Ceiling(float.NegativeInfinity); // returns float.NegativeInfinity
        /// </code>
        /// </example>
        public static float Ceiling(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                return value;

            int i = (int)value;
            return (value > i) ? i + 1 : i;
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
        /// Gets percentage of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PercentOf(float value, float percent)
        {
            float result = (percent / 100) * value;
            return result;
        }

        /// <summary>
        /// Returns the larger of the specified numbers.
        /// </summary>
        /// <param name="values">Array of <see cref="float"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static float Max(params float[] values)
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
        /// <param name="values">Array of <see cref="float"/> numbers.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="values"/> is
        /// an empty array.</exception>
        public static float Min(params float[] values)
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
    }
}
