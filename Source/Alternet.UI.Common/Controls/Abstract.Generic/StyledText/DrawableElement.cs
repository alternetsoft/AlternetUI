using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Base.Collections;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base drawable element.
    /// </summary>
    public class DrawableElement : ImmutableObject, IDrawableElement
    {
        private DrawableElementStyle? style;
        private DrawableElement? parent;
        private RectD bounds;
        private bool isClipped = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableElement"/> class.
        /// </summary>
        public DrawableElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableElement"/> class.
        /// </summary>
        /// <param name="style">The style of this object.</param>
        public DrawableElement(DrawableElementStyle? style)
        {
            this.style = style;
        }

        /// <summary>
        /// Gets real attributes of this object.
        /// </summary>
        [Browsable(false)]
        public virtual DrawableElementStyle RealStyle
        {
            get
            {
                return Style ?? Parent?.RealStyle ?? DrawableElementStyle.Default;
            }
        }

        /// <summary>
        /// Gets bounds.
        /// </summary>
        public RectD Bounds
        {
            get
            {
                return bounds;
            }

            private set
            {
                SetProperty(ref bounds, value, nameof(Bounds));
            }
        }

        /// <summary>
        /// Gets or sets whether element is clipped when painted.
        /// </summary>
        public virtual bool IsClipped
        {
            get
            {
                return isClipped;
            }

            set
            {
                SetProperty(ref isClipped, value, nameof(IsClipped));
            }
        }

        /// <summary>
        /// Gets or sets style attributes of this objecy.
        /// </summary>
        public virtual DrawableElementStyle? Style
        {
            get
            {
                return style;
            }

            set
            {
                SetProperty(ref style, value, nameof(Style));
            }
        }

        /// <summary>
        /// Gets or sets attributes of the styled text.
        /// </summary>
        public virtual DrawableElement? Parent
        {
            get
            {
                return parent;
            }

            set
            {
                SetProperty(ref parent, value, nameof(Parent));
            }
        }

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
            var result = CreateStack(null, alignment, distance, isVertical);
            var items = CreateCollection(strings, result);
            result.Items = items;
            return result;
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
            var result = CreateStack(null, alignment, distance, isVertical);
            var items = CreateCollection(strings, result);
            result.Items = items;
            return result;
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
            IEnumerable<IDrawableElement>? items,
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
        /// <param name="parent">Parent of the elements.</param>
        /// <returns></returns>
        public static IEnumerable<IDrawableElement> CreateCollection(
            IEnumerable strings,
            DrawableElement? parent)
        {
            List<IDrawableElement> items = new();

            foreach (var s in strings)
            {
                var item = new DrawableTextElement(s);
                item.Parent = parent;
                items.Add(item);
            }

            return items;
        }

        /// <inheritdoc/>
        public virtual void Draw(Graphics dc, RectD container)
        {
        }

        /// <inheritdoc/>
        public virtual SizeD Measure(Graphics dc, SizeD availableSize)
        {
            return SizeD.Empty;
        }
    }
}
