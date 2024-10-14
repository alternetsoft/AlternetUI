using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Draws multiple styled text items at the location with the specified
        /// font, horizontal alignment, foreground and background colors.
        /// </summary>
        /// <param name="maxWidth">
        /// Maximal width which is used when alignment is applied.
        /// When -1 is specified, it is calculated.
        /// </param>
        /// <param name="alignment">Horizontal alignment.</param>
        /// <param name="text">Strings to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="lineDistance">Distance between lines of text. Optional. Default is 0.</param>
        public virtual void DrawStyledText(
            IEnumerable<StyledText> text,
            PointD origin,
            Font font,
            Color foreColor,
            Color backColor,
            HorizontalAlignment alignment,
            Coord maxWidth = -1,
            Coord lineDistance = 0)
        {
            if (maxWidth < 0)
            {
                maxWidth = 0;

                foreach (var obj in text)
                {
                    var measure = MeasureStyledText(obj, font);
                    maxWidth = Math.Max(maxWidth, measure.Width);
                }
            }

            foreach (var obj in text)
            {
                var measure = MeasureStyledText(obj, font);
                var alignedOrigin = origin;
                if (alignment != HorizontalAlignment.Left && maxWidth > 0)
                {
                    RectD rect = (origin, measure);
                    RectD maxRect = (origin.X, origin.Y, maxWidth, measure.Height);
                    var alignedRect = AlignUtils.AlignRectInRect(rect, maxRect, alignment, null);
                    alignedOrigin.X = alignedRect.X;
                }

                DrawStyledText(obj, alignedOrigin, font, foreColor, backColor);
                origin.Y += measure.Height + lineDistance;
            }
        }

        /// <summary>
        /// Draws multiple styled text items at the location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">Items to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="brush">Default <see cref="Brush"/> that determines the default
        /// color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the text.</param>
        /// <param name="distance">Distance between items. Optional. Default is 0.</param>
        /// <param name="isVertical">Whether to draw items vertically or horizontally.</param>
        public virtual void DrawStyledText(
            IEnumerable<StyledText> text,
            Font font,
            Brush brush,
            PointD origin,
            Coord distance = 0,
            bool isVertical = true)
        {
            foreach (var obj in text)
            {
                DrawStyledText(obj, font, brush, origin);
                var textSize = MeasureStyledText(obj, font).GetSize(isVertical);
                origin.IncLocation(isVertical, textSize + distance);
            }
        }

        /// <summary>
        /// Draws multiple styled text items at the location with the specified
        /// font, foreground and background colors.
        /// </summary>
        /// <param name="text">Items to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="distance">Distance between items. Optional. Default is 0.</param>
        /// <param name="isVertical">Whether to draw items vertically or horizontally.</param>
        public virtual void DrawStyledText(
            IEnumerable<StyledText> text,
            PointD origin,
            Font font,
            Color foreColor,
            Color backColor,
            Coord distance = 0,
            bool isVertical = true)
        {
            foreach (var obj in text)
            {
                DrawStyledText(obj, origin, font, foreColor, backColor);
                var textSize = MeasureStyledText(obj, font).GetSize(isVertical);
                origin.IncLocation(isVertical, textSize + distance);
            }
        }

        /// <summary>
        /// Draws styled text object.
        /// </summary>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="brush">Default <see cref="Brush"/> that determines the default
        /// color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawStyledText(StyledText styledText, Font font, Brush brush, PointD origin)
        {
            ((IStyledText)styledText).Draw(this, origin, font, brush);
        }

        /// <summary>
        /// Measures styled text object.
        /// </summary>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureStyledText(StyledText styledText, Font font)
        {
            var measure = ((IStyledText)styledText).Measure(this, font);
            return measure;
        }

        /// <summary>
        /// Draws styled text object.
        /// </summary>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="location"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawStyledText(
            StyledText styledText,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            ((IStyledText)styledText).Draw(this, location, font, foreColor, backColor);
        }
    }
}
