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
    public static class ExtensionsPrivate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        public static SKPoint[] ToSkia(this PointD[] points)
        {
            var length = points.Length;
            SKPoint[] result = new SKPoint[length];
            for (int i = 0; i < length; i++)
                result[i] = points[i];
            return result;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsChar(this string s, char ch)
        {
#if NET5_0_OR_GREATER
            return s.Contains(ch);
#else
            return s.Contains(ch);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsSpace(this string s)
        {
            return ContainsChar(s, ' ');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsSemicolon(this string s)
        {
            return ContainsChar(s, ';');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsDot(this string s)
        {
            return ContainsChar(s, '.');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsComma(this string s)
        {
            return ContainsChar(s, ',');
        }
    }
}
