using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    public static partial class MathUtils
    {
        // DoubleEpsilon and FloatEpsilon come from sdk\inc\crt\double.h

        /// <summary>
        /// Represents the smallest positive <see cref="double"/> value such
        /// that 1.0 + <see cref="DoubleEpsilon"/> is not equal to 1.0.
        /// </summary>
        /// <remarks>This constant is useful for comparing floating-point numbers with a tolerance for
        /// precision errors.</remarks>
        public const double DoubleEpsilon = 2.2204460492503131e-016;

        /// <summary>
        /// Optional override for double-precision floating-point equality comparison.
        /// If set, this delegate will be used by <see cref="AreClose(double, double)"/>
        /// instead of the default implementation.
        /// </summary>
        public static Func<double, double, bool>? AreCloseOverrideD;

        /// <summary>
        /// Determines whether two double-precision floating-point numbers are approximately equal
        /// within a specified relative and absolute tolerance.
        /// </summary>
        /// <remarks>The comparison uses a relative tolerance of 1e-12 and an absolute tolerance of 1e-15.
        /// This ensures that the method accounts for both small differences in large numbers
        /// and significant differences in numbers close to zero.</remarks>
        /// <param name="a">The first double-precision floating-point number to compare.</param>
        /// <param name="b">The second double-precision floating-point number to compare.</param>
        /// <returns><see langword="true"/> if the two numbers are approximately equal
        /// within the defined tolerances; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="AreClose(double,double)"/>
        public static bool AreCloseWithTolerance(double a, double b)
        {
            if (a == b)
                return true;

            const double relTol = 1e-12;
            const double absTol = 1e-15;

            double diff = Math.Abs(a - b);
            double scale = Math.Max(Math.Abs(a), Math.Abs(b));
            double eps = Math.Max(relTol * scale, absTol);

            return diff <= eps;
        }

        /// <summary>
        /// Determines whether the first value is less than the second value
        /// and not approximately equal to it.
        /// </summary>
        /// <remarks>The method uses a predefined tolerance to determine whether the two values are
        /// approximately equal.</remarks>
        /// <param name="value1">The first double-precision floating-point value to compare.</param>
        /// <param name="value2">The second double-precision floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/>
        /// is less than <paramref name="value2"/>  and the two
        /// values are not considered approximately equal;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanAndNotClose(double value1, double value2)
        {
            return (value1 < value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// Determines whether the first value is greater than the
        /// second value and not approximately equal to it.
        /// </summary>
        /// <remarks>The method uses a predefined tolerance to determine whether the two values are
        /// approximately equal.</remarks>
        /// <param name="value1">The first double-precision floating-point value to compare.</param>
        /// <param name="value2">The second double-precision floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is
        /// greater than <paramref name="value2"/>  and the two
        /// values are not approximately equal; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanAndNotClose(double value1, double value2)
        {
            return (value1 > value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// Determines whether the first double-precision floating-point
        /// value is less than  or approximately equal to
        /// the second value.
        /// </summary>
        /// <remarks>The method uses a predefined tolerance to determine whether
        /// the two values are
        /// approximately equal. This is useful for comparisons where floating-point
        /// precision errors might otherwise
        /// lead to unexpected results.</remarks>
        /// <param name="value1">The first double-precision floating-point value to compare.</param>
        /// <param name="value2">The second double-precision floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is less than
        /// <paramref name="value2"/>, or if the two
        /// values are approximately equal; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrClose(double value1, double value2)
        {
            return (value1 < value2) || AreClose(value1, value2);
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
        /// Rounds a double-precision floating-point number to the nearest integer,
        /// rounding away from zero for halfway cases.
        /// </summary>
        /// <param name="val">The value to round.</param>
        /// <returns>The rounded integer value.</returns>
        /// <example>
        /// <code>
        /// DoubleToInt(2.5)   // returns 3
        /// DoubleToInt(-2.5)  // returns -3
        /// DoubleToInt(1.4)   // returns 1
        /// DoubleToInt(-1.4)  // returns -1
        /// DoubleToInt(0.0)   // returns 0
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(double val)
        {
            return (val > 0) ? (int)(val + 0.5) : (int)(val - 0.5);
        }

        /// <summary>
        /// Determines whether the first double-precision floating-point
        /// value is greater than  or approximately equal
        /// to the second value.
        /// </summary>
        /// <remarks>Two values are considered approximately equal if the difference
        /// between them is
        /// within  a small tolerance. The tolerance is determined by the implementation
        /// of the <c>AreClose(double,double)</c> method.</remarks>
        /// <param name="value1">The first double-precision floating-point value to compare.</param>
        /// <param name="value2">The second double-precision floating-point value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is greater
        /// than or approximately equal to  <paramref
        /// name="value2"/>; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrClose(double value1, double value2)
        {
            return (value1 > value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// Determines whether the specified value is equal to 1.0 within a small tolerance.
        /// </summary>
        /// <remarks>This method uses a tolerance-based comparison to account for potential floating-point
        /// precision errors.</remarks>
        /// <param name="value">The double-precision floating-point number to compare to 1.0.</param>
        /// <returns><see langword="true"/> if the specified value is approximately equal to 1.0;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(double value)
        {
            return AreClose(value, 1);
        }

        /// <summary>
        /// Determines whether the specified double-precision floating-point value is equal to zero.
        /// </summary>
        /// <remarks>The comparison accounts for potential floating-point
        /// precision issues and considers
        /// values close to zero as equal to zero.</remarks>
        /// <param name="value">The double-precision floating-point value to compare to zero.</param>
        /// <returns><see langword="true"/> if the specified value is considered equal to zero;
        /// otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(double value)
        {
            return AreClose(value, 0);
        }

        /// <summary>
        /// Determines whether two double-precision floating-point numbers are approximately
        /// equal within a specified relative and absolute tolerance.
        /// </summary>
        /// <remarks>This method is useful for comparing floating-point numbers
        /// where precision errors may
        /// occur due to the limitations of binary representation.
        /// The comparison accounts for both the scale of the
        /// numbers and a fixed minimum tolerance.</remarks>
        /// <param name="a">The first double-precision floating-point number to compare.</param>
        /// <param name="b">The second double-precision floating-point number to compare.</param>
        /// <param name="relTol">The relative tolerance, which defines
        /// the allowable difference relative to the larger of the two values.
        /// Must be a non-negative value. Defaults to <c>1e-12</c>.</param>
        /// <param name="absTol">The absolute tolerance, which defines the minimum
        /// allowable difference regardless of scale. Must be a non-negative value.
        /// Defaults to <c>1e-15</c>.</param>
        /// <returns><see langword="true"/> if the difference
        /// between <paramref name="a"/> and <paramref name="b"/> is less than
        /// or equal to the greater of the relative or absolute tolerance;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool AreCloseWithToleranceEx(
            double a,
            double b,
            double relTol = 1e-12,
            double absTol = 1e-15)
        {
            /*
            When to Specify relTol (Relative Tolerance)
            ===========================================

            Use a larger relTol when:
            - Comparing large magnitude values (e.g., 1e6, 1e9)
            - Working with low-precision inputs (e.g., sensor data, financial rounding)
            - You expect acceptable drift due to accumulation or conversion
            Example:
            AreClose(1e9, 1e9 + 0.01, relTol: 1e-8); // Accepts small relative error

            Use a smaller relTol when:
            - Comparing high-precision results (e.g., scientific computation)
            - Validating numerical algorithms or unit tests
            - Ensuring bitwise reproducibility

            ===========================================
            When to Specify absTol (Absolute Tolerance)
            ===========================================

            Use a larger absTol when:
            - Comparing values near zero
            - Avoiding false negatives due to denormal or rounding
            - Working with physical measurements where zero is noisy

            Example:
            AreClose(1e-16, 0.0, absTol: 1e-14); // Accepts near-zero drift

            Use a smaller absTol when:
            - You need strict zero comparison
            - You're validating mathematical identities or symbolic results
            */

            if (a == b)
                return true;

            double diff = Math.Abs(a - b);
            double scale = Math.Max(Math.Abs(a), Math.Abs(b));
            double eps = Math.Max(relTol * scale, absTol);

            return diff <= eps;
        }

        /// <summary>
        /// Determines whether a double-precision floating-point value represents an even integer.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is an integer and divisible by 2;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// IsEven(4.0)    // returns true
        /// IsEven(3.0)    // returns false
        /// IsEven(2.5)    // returns false
        /// IsEven(-6.0)   // returns true
        /// IsEven(0.0)    // returns true
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEvenInteger(double value)
        {
            // Check whether the value is an integer
            if (value % 1 != 0)
                return false;

            return ((int)value % 2) == 0;
        }

        /// <summary>
        /// Determines whether two <see cref="double"/> values are equal using
        /// a layered comparison strategy.
        /// This method combines magnitude-scaled epsilon comparison,
        /// bitwise proximity (with default step threshold),
        /// and relative/absolute tolerance checks.
        /// It is suitable for robust floating-point validation across domains such
        /// as diagnostics, serialization, and numerical correctness.
        /// </summary>
        /// <param name="a">The first double value to compare.</param>
        /// <param name="b">The second double value to compare.</param>
        /// <returns>
        /// <c>true</c> if any of the following conditions are met:
        /// <list type="bullet">
        /// <item><description><see cref="AreCloseScaled(double,double)"/>
        /// returns true</description></item>
        /// <item><description><see cref="AreCloseBitwise(double,double)"/>
        /// returns true using
        /// its default threshold</description></item>
        /// <item><description><see cref="AreCloseWithTolerance(double,double)"/>
        /// returns true</description></item>
        /// </list>
        /// Otherwise, <c>false</c>.
        /// </returns>
        public static bool AreClose(double a, double b)
        {
            if(AreCloseOverrideD != null)
                return AreCloseOverrideD(a, b);

            return AreCloseScaled(a, b)
                || AreCloseBitwise(a, b)
                || AreCloseWithTolerance(a, b);
        }

        /// <summary>
        /// Returns true if the two doubles are close enough to be considered equal.
        /// Handles infinities and near-zero values.
        /// </summary>
        /// <seealso cref="AreClose(double,double)"/>
        public static bool AreCloseScaled(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }

            double delta = value1 - value2;
            double eps = (Math.Abs(value1) + Math.Abs(value2)) * DoubleEpsilon;

            if (eps == 0.0)
            {
                eps = DoubleEpsilon;
            }

            return Math.Abs(delta) < eps;
        }

        /// <summary>
        /// Determines whether two <see cref="double"/> values are equal within
        /// a specified number of representable steps.
        /// This comparison is based on the binary distance between the two values
        /// in IEEE 754 format,
        /// making it suitable for bitwise proximity checks, serialization validation,
        /// and low-level diagnostics.
        /// </summary>
        /// <param name="a">The first double value to compare.</param>
        /// <param name="b">The second double value to compare.</param>
        /// <param name="maxSteps">
        /// The maximum number of representable floating-point steps (units in the last place)
        /// allowed between <paramref name="a"/> and <paramref name="b"/>.
        /// A value of 4 allows for minor rounding differences while still
        /// ensuring tight binary proximity.
        /// Increase this value to tolerate more drift; decrease
        /// it for stricter equivalence.
        /// </param>
        /// <returns>
        /// <c>true</c> if the binary representations of <paramref name="a"/>
        /// and <paramref name="b"/> differ by no more than <paramref name="maxSteps"/> steps;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <seealso cref="AreCloseBitwise(double,double)"/>
        /// <seealso cref="AreClose(double,double)"/>
        public static bool AreCloseBitwiseEx(double a, double b, int maxSteps = 4)
        {
            long aBits = BitConverter.DoubleToInt64Bits(a);
            long bBits = BitConverter.DoubleToInt64Bits(b);

            // Handle sign differences explicitly
            if ((aBits < 0) != (bBits < 0))
                return a == b;

            long distance = Math.Abs(aBits - bBits);
            return distance <= maxSteps;
        }

        /// <summary>
        /// Determines whether two double-precision floating-point numbers are
        /// equal within a small bitwise difference.
        /// </summary>
        /// <remarks>This method compares the bitwise representations
        /// of the two numbers and allows for a
        /// maximum difference of 4 in their bitwise values. It accounts for sign
        /// differences explicitly and falls back
        /// to a direct comparison when the signs differ.</remarks>
        /// <param name="a">The first double-precision floating-point number to compare.</param>
        /// <param name="b">The second double-precision floating-point number to compare.</param>
        /// <returns><see langword="true"/> if the bitwise difference between
        /// <paramref name="a"/> and <paramref name="b"/> is
        /// within a small threshold; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="AreCloseBitwiseEx(double,double, int)"/>
        /// <seealso cref="AreClose(double, double)"/>
        public static bool AreCloseBitwise(double a, double b)
        {
            const int maxSteps = 4;

            long aBits = BitConverter.DoubleToInt64Bits(a);
            long bBits = BitConverter.DoubleToInt64Bits(b);

            // Handle sign differences explicitly
            if ((aBits < 0) != (bBits < 0))
                return a == b;

            long distance = Math.Abs(aBits - bBits);
            return distance <= maxSteps;
        }

        /// <summary>
        /// Determines whether the specified double-precision floating-point value is NaN (Not a Number).
        /// This method uses bit-level inspection for performance-critical scenarios.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the value is NaN; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// IEEE 754: NaN values have all exponent bits set (0x7FF) and a non-zero mantissa.
        /// This avoids the overhead of <see cref="double.IsNaN"/> and is suitable for tight loops.
        /// </remarks>
        /// <example>
        /// <code>
        /// IsNaNFast(double.NaN)              // returns true
        /// IsNaNFast(0.0)                     // returns false
        /// IsNaNFast(double.PositiveInfinity) // returns false
        /// IsNaNFast(double.NegativeInfinity) // returns false
        /// IsNaNFast(1.0 / 0.0)               // returns false (infinity)
        /// IsNaNFast(0.0 / 0.0)               // returns true (NaN)
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(double value)
        {
            // PS item that tracks the CLR improvement is DevDiv Schedule : 26916.
            DoubleUnion t = new() { DoubleValue = value };
            ulong exp = t.UintValue & 0xfff0000000000000;
            ulong man = t.UintValue & 0x000fffffffffffff;
            return (exp == 0x7ff0000000000000 || exp == 0xfff0000000000000) && (man != 0);
        }

        /// <summary>
        /// Provides a union for accessing the bitwise representation of a <see cref="double"/> value.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct DoubleUnion
        {
            /// <summary>
            /// The double-precision floating-point value.
            /// </summary>
            [FieldOffset(0)]
            public double DoubleValue;

            /// <summary>
            /// The unsigned 64-bit integer representation of the double value.
            /// </summary>
            [FieldOffset(0)]
            public ulong UintValue;
        }
    }
}
