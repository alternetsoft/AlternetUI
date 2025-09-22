using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI.Extensions
{
    /// <summary>
    /// Contains extension methods for the different classes.
    /// </summary>
    public static partial class ExtensionsPublic
    {
        /// <summary>
        /// Returns a random single-precision float in the range [0.0f, 1.0f).
        /// </summary>
        public static float NextFloat(this Random rng)
        {
            return (float)rng.NextDouble();
        }

        /// <summary>
        /// Returns a random float in the range [min, max).
        /// </summary>
        public static float NextFloat(this Random rng, float min, float max)
        {
            return min + (float)rng.NextDouble() * (max - min);
        }

        /// <summary>
        /// Converts <see cref="GenericAlignment"/> to <see cref="TextHorizontalAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static TextHorizontalAlignment AsTextHorizontalAlignment(this GenericAlignment value)
        {
            switch (value)
            {
                default:
                case GenericAlignment.Left:
                    return TextHorizontalAlignment.Left;
                case GenericAlignment.Right:
                    return TextHorizontalAlignment.Right;
                case GenericAlignment.CenterHorizontal:
                    return TextHorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// Converts <see cref="TextHorizontalAlignment"/> to <see cref="GenericAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static GenericAlignment AsGenericAlignment(this TextHorizontalAlignment value)
        {
            switch (value)
            {
                default:
                case TextHorizontalAlignment.Left:
                    return GenericAlignment.Left;
                case TextHorizontalAlignment.Center:
                    return GenericAlignment.CenterHorizontal;
                case TextHorizontalAlignment.Right:
                    return GenericAlignment.Right;
            }
        }

        /// <summary>
        /// Gets whether property name is specified in the <see cref="PropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <returns></returns>
        public static bool HasPropertyName(this PropertyChangedEventArgs e)
        {
            return !string.IsNullOrEmpty(e.PropertyName);
        }

        /// <summary>
        /// Copies the contents of this string into the destination span.
        /// </summary>
        /// <param name="destination">The span into which to copy string's contents.</param>
        /// <exception cref="ArgumentException">The destination span is shorter
        /// than the source string.</exception>
        /// <param name="s">String to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyToSpan(this string s, Span<char> destination)
        {
            if ((uint)s.Length <= (uint)destination.Length)
            {
                fixed(char* sourceChar = s)
                {
                    fixed(char* destChar = destination)
                    {
                        BaseMemory.Move((IntPtr)destChar, (IntPtr)sourceChar, sizeof(char));
                    }
                }
            }
            else
            {
                throw new Exception("Destination is too short");
            }
        }

        /// <summary>
        /// Checks whether <paramref name="lockMode"/> is not equal <see cref="ImageLockMode.WriteOnly"/>.
        /// </summary>
        /// <param name="lockMode">Value to check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanRead(this ImageLockMode lockMode)
        {
            return lockMode != ImageLockMode.WriteOnly;
        }

        /// <summary>
        /// Checks whether <paramref name="lockMode"/> is not equal <see cref="ImageLockMode.ReadOnly"/>.
        /// </summary>
        /// <param name="lockMode">Value to check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanWrite(this ImageLockMode lockMode)
        {
            return lockMode != ImageLockMode.ReadOnly;
        }

        /// <summary>
        /// Returns the absolute value of a double-precision floating-point number.
        /// Same as <see cref="Math.Abs(double)"/>.
        /// </summary>
        /// <param name="value">
        /// A number that is greater than or equal to <see cref="double.MinValue"/>, but less than
        /// or equal to <see cref="double.MaxValue"/>.
        /// </param>
        /// <returns>A double-precision floating-point number,
        /// x, such that 0 ≤ x ≤ double.MaxValue.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value of a single-precision floating-point number.
        /// Same as <see cref="Math.Abs(float)"/>.
        /// </summary>
        /// <param name="value">
        /// A number that is greater than or equal to <see cref="float.MinValue"/>, but less than
        /// or equal to <see cref="float.MaxValue"/>.
        /// </param>
        /// <returns>A single-precision floating-point number,
        /// x, such that 0 ≤ x ≤ float.MaxValue.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Converts value in device-independent units to pixels using the specified scale factor.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified,
        /// the default scale factor is used.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PixelFromDip(this Coord value, Coord? scaleFactor = null)
        {
            return GraphicsFactory.PixelFromDip(value, scaleFactor);
        }

        /// <summary>
        /// Gets whether <see cref="DockStyle"/> equals <see cref="DockStyle.Top"/> or
        /// <see cref="DockStyle.Bottom"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTopOrBottom(this DockStyle dock)
        {
            return dock == DockStyle.Bottom || dock == DockStyle.Top;
        }

        /// <summary>
        /// Calls <see cref="Stack{T}.Push"/> for the each item in <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Type of the item</typeparam>
        /// <param name="stack"><see cref="Stack{T}"/> instance.</param>
        /// <param name="items">Items to push.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        /// <summary>
        /// Gets whether <see cref="DockStyle"/> equals <see cref="DockStyle.Left"/> or
        /// <see cref="DockStyle.Right"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeftOrRight(this DockStyle dock)
        {
            return dock == DockStyle.Left || dock == DockStyle.Right;
        }
    }
}
