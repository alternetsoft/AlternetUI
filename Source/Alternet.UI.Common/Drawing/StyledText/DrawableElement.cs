using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base drawable element.
    /// </summary>
    public class DrawableElement : ImmutableObject, IDrawableElement
    {
        /// <summary>
        /// Conversion from <see cref="string"/> to <see cref="DrawableElement"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator DrawableElement(string value)
        {
            return new DrawableTextElement(value);
        }

        /// <summary>
        /// Conversion from array of <see cref="string"/> to <see cref="DrawableElement"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator DrawableElement(string[] value)
        {
            return CreateStringsStack(value);
        }

        /// <summary>
        /// Creates styled text with style.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DrawableTextElement CreateText(object? s, DrawableElementStyle? style)
        {
            return new DrawableTextElement(s, style);
        }

        /// <summary>
        /// Create drawable stack element with the strings.
        /// </summary>
        /// <param name="strings">Strings collection.</param>
        /// <param name="distance">Distance between items.</param>
        /// <param name="alignment">Other side alignment.</param>
        /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
        /// <returns></returns>
        public static DrawableStackElement CreateStringsStack(
            IEnumerable strings,
            Coord distance = 0,
            CoordAlignment alignment = CoordAlignment.Near,
            bool isVertical = true)
        {
            var items = CreateCollection(strings);
            return CreateStack(items, alignment, distance, isVertical);
        }

        /// <summary>
        /// Create drawable stack element with the strings.
        /// </summary>
        /// <param name="strings">Strings collection.</param>
        /// <param name="distance">Distance between items.</param>
        /// <param name="alignment">Other side alignment.</param>
        /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
        /// <returns></returns>
        public static DrawableStackElement CreateStringsStack(
            IEnumerable<string> strings,
            Coord distance = 0,
            CoordAlignment alignment = CoordAlignment.Near,
            bool isVertical = true)
        {
            var items = CreateCollection(strings);
            return CreateStack(items, alignment, distance, isVertical);
        }

        /// <summary>
        /// Creates an empty drawable element.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DrawableElement CreateEmpty()
        {
            return new DrawableElement();
        }

        /// <summary>
        /// Creates drawable text element.
        /// </summary>
        /// <param name="s">Text string without line separators.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DrawableTextElement CreateText(object? s)
        {
            return new DrawableTextElement(s);
        }

        /// <summary>
        /// Create drawable stack element with the items.
        /// </summary>
        /// <param name="items">Items collection.</param>
        /// <param name="distance">Distance between items.</param>
        /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
        /// <param name="alignment">Other side alignment.</param>
        /// <returns></returns>
        public static DrawableStackElement CreateStack(
            IEnumerable<IDrawableElement> items,
            CoordAlignment alignment = CoordAlignment.Near,
            Coord distance = 0,
            bool isVertical = true)
        {
            return new DrawableStackElement(items, alignment, distance, isVertical);
        }

        /// <summary>
        /// Creates collection of drawable elements from the collection of strings.
        /// </summary>
        /// <param name="strings">The collection of strings.</param>
        /// <returns></returns>
        public static IEnumerable<IDrawableElement> CreateCollection(IEnumerable strings)
        {
            List<IDrawableElement> items = new();

            foreach (var s in strings)
            {
                items.Add(new DrawableTextElement(s));
            }

            return items;
        }

        /// <inheritdoc/>
        public virtual void Draw(Graphics dc, PointD location)
        {
        }

        /// <inheritdoc/>
        public virtual SizeD Measure(Graphics dc)
        {
            return SizeD.Empty;
        }
    }
}
