using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the layout and align.
    /// </summary>
    public static class AlignUtils
    {
        /// <summary>
        /// Calculates the position of a dropdown relative to a specified <see cref="SizeD"/>.
        /// </summary>
        /// <remarks>The <paramref name="position"/> parameter allows fine-grained
        /// control over the dropdown's alignment. For example, setting the horizontal alignment
        /// to <see cref="HorizontalAlignment.Center"/> and the vertical alignment to
        /// <see cref="VerticalAlignment.Top"/> will position the dropdown at the center of
        /// the rectangle's top edge.</remarks>
        /// <param name="ownerSize">The <see cref="SizeD"/> used as the reference for positioning
        /// the dropdown.</param>
        /// <param name="popupSize">Size of the drop-down control.</param>
        /// <param name="position">An optional alignment specifying the horizontal and vertical
        /// position of the dropdown relative to the <see cref="SizeD"/>.
        /// If <see langword="null"/>, the dropdown is positioned at the bottom-left corner.</param>
        /// <returns>A <see cref="PointD"/> representing the calculated
        /// position of the dropdown.</returns>
        public static PointD GetDropDownPosition(
            SizeD ownerSize,
            SizeD popupSize,
            HVDropDownAlignment? position = null)
        {
            position ??= new HVDropDownAlignment(
                DropDownAlignment.AfterStart,
                DropDownAlignment.AfterEnd);

            Coord x = 0;
            Coord y = ownerSize.Height;

            switch(position.Value.Horizontal)
            {
                case DropDownAlignment.BeforeStart:
                    x = -popupSize.Width;
                    break;
                case DropDownAlignment.AfterStart:
                    x = 0;
                    break;
                case DropDownAlignment.BeforeEnd:
                    x = ownerSize.Width - popupSize.Width;
                    break;
                case DropDownAlignment.AfterEnd:
                    x = ownerSize.Width;
                    break;
                case DropDownAlignment.Center:
                    x = (ownerSize.Width - popupSize.Width) / 2;
                    break;
            }

            switch (position.Value.Vertical)
            {
                case DropDownAlignment.BeforeStart:
                    y = -popupSize.Height;
                    break;
                case DropDownAlignment.AfterStart:
                    y = 0;
                    break;
                case DropDownAlignment.BeforeEnd:
                    y = ownerSize.Height - popupSize.Height;
                    break;
                case DropDownAlignment.AfterEnd:
                    y = ownerSize.Height;
                    break;
                case DropDownAlignment.Center:
                    y = (ownerSize.Height - popupSize.Height) / 2;
                    break;
            }

            return (x, y);
        }

        /// <summary>
        /// Converts <see cref="GenericAlignment"/> to <see cref="VerticalAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static VerticalAlignment GetVertical(GenericAlignment value)
        {
            if ((value & GenericAlignment.CenterVertical) != 0)
                return VerticalAlignment.Center;
            if ((value & GenericAlignment.Bottom) != 0)
                return VerticalAlignment.Bottom;
            return VerticalAlignment.Top;
        }

        /// <summary>
        /// Converts <see cref="GenericAlignment"/> to <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static HorizontalAlignment GetHorizontal(GenericAlignment value)
        {
            if ((value & GenericAlignment.CenterHorizontal) != 0)
                return HorizontalAlignment.Center;
            if ((value & GenericAlignment.Right) != 0)
                return HorizontalAlignment.Right;
            return HorizontalAlignment.Left;
        }

        /// <summary>
        /// Converts <see cref="TextHorizontalAlignment"/> to <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HorizontalAlignment Convert(TextHorizontalAlignment value)
        {
            switch (value)
            {
                case TextHorizontalAlignment.Center:
                    return HorizontalAlignment.Center;
                case TextHorizontalAlignment.Right:
                    return HorizontalAlignment.Right;
                default:
                    return HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Converts <see cref="TextVerticalAlignment"/> to <see cref="VerticalAlignment"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VerticalAlignment Convert(TextVerticalAlignment value)
        {
            switch (value)
            {
                case TextVerticalAlignment.Center:
                    return VerticalAlignment.Center;
                case TextVerticalAlignment.Bottom:
                    return VerticalAlignment.Bottom;
                default:
                    return VerticalAlignment.Top;
            }
        }

        /// <summary>
        /// Aligns vertical or horizontal coordinates of the specified rectangle
        /// in the container using the alignment options.
        /// </summary>
        /// <param name="rect">Rectangle to align.</param>
        /// <param name="alignment">Alignment to apply.</param>
        /// <param name="container">Container rectangle.</param>
        /// <param name="vert">Whether to align vertical or horizontal coordinate.</param>
        /// <param name="shrinkSize">Whether to reduce size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.
        /// Only width (when <paramref name="vert"/> is false) or height
        /// (when <paramref name="vert"/> is true) is reduced.
        /// </param>
        /// <returns></returns>
        public static RectD AlignRectInRect(
            bool vert,
            RectD rect,
            RectD container,
            CoordAlignment alignment,
            bool shrinkSize = true)
        {
            if (shrinkSize)
                rect.Size = rect.Size.Shrink(vert, container.GetSize(vert));

            switch (alignment)
            {
                case CoordAlignment.Near:
                    rect.SetLocation(vert, container.GetLocation(vert));
                    break;
                case CoordAlignment.Center:
                    rect.SetCenter(vert, container.GetCenter(vert));
                    break;
                case CoordAlignment.Far:
                    rect.SetFarLocation(vert, container.GetFarLocation(vert));
                    break;
                case CoordAlignment.Stretch:
                case CoordAlignment.Fill:
                    rect.SetLocation(vert, container.GetLocation(vert));
                    rect.SetSize(vert, container.GetSize(vert));
                    break;
            }

            return rect;
        }

        /// <summary>
        /// Aligns the specified rectangle in the container using horizontal and vertical
        /// alignment options.
        /// </summary>
        /// <param name="rect">Rectangle to align.</param>
        /// <param name="container">Container rectangle.</param>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="vert">Vertical alignment.</param>
        /// <param name="shrinkSize">Whether to shrink size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.</param>
        /// <returns></returns>
        public static RectD AlignRectInRect(
            RectD rect,
            RectD container,
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            bool shrinkSize = true)
        {
            if (shrinkSize)
                rect.Size = rect.Size.Shrink(container.Width, container.Height);

            if(horz is not null)
            {
                rect = AlignRectInRect(false, rect, container, (CoordAlignment)horz, false);
            }

            if (vert is not null)
            {
                rect = AlignRectInRect(true, rect, container, (CoordAlignment)vert, false);
            }

            return rect;
        }
    }
}
