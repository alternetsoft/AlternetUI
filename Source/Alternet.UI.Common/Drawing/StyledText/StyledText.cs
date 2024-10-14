using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Styled text object.
    /// </summary>
    public class StyledText : BaseObject
    {
        /// <summary>
        /// Conversion from <see cref="string"/> to <see cref="StyledText"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator StyledText(string value)
        {
            return Create(value);
        }

        /// <summary>
        /// Conversion from array of <see cref="string"/> to <see cref="StyledText"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator StyledText(string[] value)
        {
            return Create(value);
        }

        /// <summary>
        /// Creates styled text from the collection of strings.
        /// </summary>
        /// <param name="strings">The collection of strings.</param>
        /// <returns></returns>
        public static IEnumerable<StyledText> CreateCollection(IEnumerable strings)
        {
            List<StyledText> items = new();

            foreach (var s in strings)
            {
                items.Add(StyledText.Create(s));
            }

            return items;
        }

        /// <summary>
        /// Create styled text container with the strings.
        /// </summary>
        /// <param name="strings">Strings collection.</param>
        /// <param name="distance">Distance between items.</param>
        /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
        /// <returns></returns>
        public static StyledText Create(
            IEnumerable strings,
            Coord distance = 0,
            bool isVertical = true)
        {
            var items = CreateCollection(strings);
            return Create(items, distance, isVertical);
        }

        /// <summary>
        /// Create styled text container wiht the items.
        /// </summary>
        /// <param name="items">Items collection.</param>
        /// <param name="distance">Distance between items.</param>
        /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
        /// <returns></returns>
        public static StyledText Create(
                        IEnumerable<StyledText> items,
                        Coord distance = 0,
                        bool isVertical = true)
        {
            return new StyledTextContainer(items, distance, isVertical);
        }

        /// <summary>
        /// Creates an empty styled text.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyledText Create()
        {
            return new SimpleStyledText();
        }

        /// <summary>
        /// Creates styled text without font and color attributes.
        /// </summary>
        /// <param name="s">Text string without line separators.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyledText Create(object s)
        {
            return new SimpleStyledText(s);
        }

        /// <summary>
        /// Creates styled text with font and foreground brush attributes.
        /// </summary>
        /// <param name="s">Text string without line separators.</param>
        /// <param name="fontStyle"><see cref="FontStyle"/> that is used to draw the text.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the
        /// color and texture of the text. If this is null, default brush is used.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyledText Create(object s, FontStyle fontStyle, Brush? brush)
        {
            return new StyledTextWithFontAndColor(s, fontStyle, brush);
        }

        /// <summary>
        /// Creates styled text with font and foreground brush attributes.
        /// </summary>
        /// <param name="s">Text string without line separators.</param>
        /// <param name="font"><see cref="Font"/> that is used to draw the text.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the
        /// color and texture of the text. If this is null, default brush is used.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyledText Create(object s, Font? font, Brush? brush)
        {
            return new StyledTextWithFontAndColor(s, font, brush);
        }

        /// <summary>
        /// Creates styled text with font style, foreground and background color attributes.
        /// </summary>
        /// <param name="s">Text string without line separators.</param>
        /// <param name="font"><see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.
        /// If this is null, default foreground color is used.
        /// </param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background is transparent.
        /// If this is null, default background color is used.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyledText Create(
            object s,
            Font? font,
            Color? foreColor,
            Color? backColor)
        {
            return new StyledTextWithFontAndColor(s, font, foreColor, backColor);
        }

        /// <summary>
        /// Creates styled text with font style, foreground and background color attributes.
        /// </summary>
        /// <param name="s">Text string without line separators.</param>
        /// <param name="fontStyle"><see cref="FontStyle"/> that is used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.
        /// If this is null, default foreground color is used.
        /// </param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background is transparent.
        /// If this is null, default background color is used.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyledText Create(
            object s,
            FontStyle fontStyle,
            Color? foreColor,
            Color? backColor)
        {
            return new StyledTextWithFontAndColor(s, fontStyle, foreColor, backColor);
        }

        internal virtual Font SafeFont(Font? font)
        {
            return font ?? Control.DefaultFont;
        }

        internal virtual Brush SafeForeBrush(Brush? brush)
        {
            return brush ?? Brush.Default;
        }

        internal virtual Color SafeForeColor(Color? foreColor)
        {
            return foreColor ?? Color.Black;
        }

        internal virtual Color SafeBackColor(Color? backColor)
        {
            return backColor ?? Color.Empty;
        }
    }
}
