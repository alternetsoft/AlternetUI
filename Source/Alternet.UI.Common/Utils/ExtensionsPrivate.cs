using System;
using System.Collections.Generic;
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
    public static class ExtensionsPrivate
    {
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
        /// Converts array of <see cref="PointD"/> to array of <see cref="SKPoint"/>.
        /// </summary>
        /// <param name="points">Array of points.</param>
        /// <returns></returns>
        public static SKPoint[] ToSkia(this PointD[] points)
        {
            var length = points.Length;
            SKPoint[] result = new SKPoint[length];
            for (int i = 0; i < length; i++)
                result[i] = points[i];
            return result;
        }

        /// <summary>
        /// Converts array of <see cref="PointD"/> to array of <see cref="SKPointI"/>
        /// using the specified <paramref name="scaleFactor"/>.
        /// </summary>
        /// <param name="points">Array of points.</param>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified,
        /// the default scale factor is used.</param>
        /// <returns></returns>
        public static SKPointI[] PixelFromDipI(this PointD[] points, Coord? scaleFactor = null)
        {
            var length = points.Length;
            SKPointI[] result = new SKPointI[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GraphicsFactory.PixelFromDip(points[i], scaleFactor);
            }

            return result;
        }

        /// <summary>
        /// Converts array of <see cref="PointD"/> to array of <see cref="SKPoint"/>
        /// using the specified <paramref name="scaleFactor"/>.
        /// </summary>
        /// <param name="points">Array of points.</param>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified,
        /// the default scale factor is used.</param>
        /// <returns></returns>
        public static SKPoint[] PixelFromDipD(this PointD[] points, Coord? scaleFactor = null)
        {
            var length = points.Length;
            SKPoint[] result = new SKPoint[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GraphicsFactory.PixelFromDip(points[i], scaleFactor).SkiaPoint;
            }

            return result;
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
        /// Converts <see cref="LineJoin"/> to <see cref="SKStrokeJoin"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static SKStrokeJoin ToSkia(this LineJoin value)
        {
            switch (value)
            {
                case LineJoin.Miter:
                default:
                    return SKStrokeJoin.Miter;
                case LineJoin.Bevel:
                    return SKStrokeJoin.Bevel;
                case LineJoin.Round:
                    return SKStrokeJoin.Round;
            }
        }

        /// <summary>
        /// Converts <see cref="LineCap"/> to <see cref="SKStrokeCap"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static SKStrokeCap ToSkia(this LineCap value)
        {
            switch (value)
            {
                case LineCap.Flat:
                default:
                    return SKStrokeCap.Butt;
                case LineCap.Square:
                    return SKStrokeCap.Square;
                case LineCap.Round:
                    return SKStrokeCap.Round;
            }
        }

        /// <summary>
        /// Converts <see cref="BitmapType"/> to <see cref="SKEncodedImageFormat"/>.
        /// </summary>
        /// <param name="type">Bitmap type</param>
        /// <returns>
        /// <see cref="SKEncodedImageFormat"/> if <see cref="BitmapType"/> can be converted,
        /// <c>null</c> otherwise.
        /// </returns>
        public static SKEncodedImageFormat? ToSKEncodedImageFormat(this BitmapType type)
        {
            switch (type)
            {
                default:
                    return null;
                case BitmapType.Bmp:
                    return SKEncodedImageFormat.Bmp;
                case BitmapType.Ico:
                    return SKEncodedImageFormat.Ico;
                case BitmapType.Gif:
                    return SKEncodedImageFormat.Gif;
                case BitmapType.Png:
                    return SKEncodedImageFormat.Png;
                case BitmapType.Jpeg:
                    return SKEncodedImageFormat.Jpeg;
                case BitmapType.Icon:
                    return SKEncodedImageFormat.Ico;
            }
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
        /// Removes underscore characters ('_') from string.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveUnderscore(this string s)
        {
            return s.Replace("_", string.Empty);
        }

        /// <summary>
        /// Trims end of line characters from the string.
        /// </summary>
        /// <param name="s">String.</param>
        /// <returns>String without all end of line characters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimEndEol(this string s)
        {
            return s.TrimEnd('\r', '\n');
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

        /// <summary>
        /// Reports whether the specified Unicode character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <param name="ch">A Unicode character to seek.</param>
        /// <returns>
        /// <c>true</c> if that character is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsChar(this string s, char ch)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return s.IndexOf(ch) >= 0;
        }

        /// <summary>
        /// Reports whether space character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if space is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsSpace(this string s)
        {
            return ContainsChar(s, ' ');
        }

        /// <summary>
        /// Reports whether semicolon character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if semicolon is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsSemicolon(this string s)
        {
            return ContainsChar(s, ';');
        }

        /// <summary>
        /// Reports whether dot character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if dot is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsDot(this string s)
        {
            return ContainsChar(s, '.');
        }

        /// <summary>
        /// Reports whether comma character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if comma is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsComma(this string s)
        {
            return ContainsChar(s, ',');
        }
    }
}
