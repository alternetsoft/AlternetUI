using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="ListControlItem"/> with horizontal separator painting.
    /// </summary>
    public class ListControlSeparatorItem : ListControlItem
    {
        /// <summary>
        /// Gets or sets default separator color. Default is Null and
        /// <see cref="DefaultColors.BorderColor"/> is used.
        /// </summary>
        public static LightDarkColor? DefaultSeparatorColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlSeparatorItem"/> class.
        /// </summary>
        public ListControlSeparatorItem()
        {
            this.HideSelection = true;
            ForegroundColor = DefaultSeparatorColor ?? DefaultColors.BorderColor;
            DrawForegroundAction = DefaultDrawSeparator;
            DrawBackgroundAction = (s, e) => { };
        }

        /// <summary>
        /// Gets color of the separator line for the specified item.
        /// </summary>
        /// <param name="container">Container of the item. Optional.</param>
        /// <param name="item">Item for which to get the separator color. Optional.</param>
        /// <returns></returns>
        public static Color GetSeparatorColor(
            IListControlItemContainer? container = null,
            ListControlItem? item = null)
        {
            var control = container?.Control;

            if (control is null)
                return SystemColors.GrayText;

            var result = item?.GetTextColor(container);

            result ??= DefaultSeparatorColor ?? SystemColors.GrayText;

            if (result is LightDarkColor lightDark)
            {
                result = lightDark.LightOrDark(control.IsDarkBackground);
            }

            return result;
        }

        /// <summary>
        /// Implements default painting of the <see cref="ListControlSeparatorItem"/>
        /// </summary>
        /// <param name="container">Item container. Optional.</param>
        /// <param name="e">Paint arguments.</param>
        public static void DefaultDrawSeparator(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e)
        {
            if (!e.Visible)
                return;

            var rect = e.ClientRectangle;
            var lineOffset = 0;
            RectD lineRect = (
                rect.Left + lineOffset,
                rect.Top + (rect.Height / 2),
                rect.Width - (lineOffset * 2),
                1);

            var color = GetSeparatorColor(container, e.Item);
            e.Graphics.FillRectangle(color.AsBrush, lineRect);
        }
    }
}
