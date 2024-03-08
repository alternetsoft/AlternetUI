using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements virtual ListBox control. This is experimental and will be changed at any time.
    /// </summary>
    public class VListBox : ListBox
    {
        /// <summary>
        /// Gets or sets default item margin.
        /// </summary>
        public static Thickness DefaultItemMargin = 2;

        /// <summary>
        /// Gets or sets default selected item text color.
        /// </summary>
        public static Color DefaultSelectedItemTextColor = SystemColors.HighlightText;

        /// <summary>
        /// Gets or sets default selected item background color.
        /// </summary>
        public static Color DefaultSelectedItemBackColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets default item text color.
        /// </summary>
        public static Color DefaultItemTextColor = SystemColors.WindowText;

        private Thickness itemMargin = DefaultItemMargin;
        private Color? selectedItemTextColor;
        private Color? itemTextColor;
        private Color? selectedItemBackColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="VListBox"/> class.
        /// </summary>
        public VListBox()
        {
            Handler.NativeControl.SetSelectionBackground(DefaultSelectedItemBackColor);
            SuggestedSize = 200;
        }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        public Color? SelectedItemTextColor
        {
            get
            {
                return selectedItemTextColor;
            }

            set
            {
                if (selectedItemTextColor == value)
                    return;
                selectedItemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        public Color? SelectedItemBackColor
        {
            get
            {
                return selectedItemBackColor;
            }

            set
            {
                if (selectedItemBackColor == value)
                    return;
                selectedItemBackColor = value;
                Handler.NativeControl.SetSelectionBackground(value ?? DefaultSelectedItemBackColor);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item text color.
        /// </summary>
        public Color? ItemTextColor
        {
            get
            {
                return itemTextColor;
            }

            set
            {
                if (itemTextColor == value)
                    return;
                itemTextColor = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override int? SelectedIndex
        {
            get
            {
                var result = Handler.NativeControl.GetSelection();
                if (result < 0)
                    return null;
                return result;
            }

            set
            {
                if (SelectedIndex == value)
                    return;
                Handler.NativeControl.SetSelection(value ?? -1);
            }
        }

        /// <summary>
        /// Gets or sets item margin.
        /// </summary>
        public virtual Thickness ItemMargin
        {
            get
            {
                return itemMargin;
            }

            set
            {
                if (itemMargin == value)
                    return;
                itemMargin = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override bool CanUserPaint
        {
            get => false;
        }

        /// <summary>
        /// Gets or sets number of items in the control.
        /// </summary>
        public new int Count
        {
            get
            {
                return base.Count;
            }

            set
            {
                ((VListBoxHandler)Handler).NativeControl.ItemsCount = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="VListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new VListBoxHandler Handler
        {
            get
            {
                return (VListBoxHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets item font. It must not be <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public virtual Font GetItemFont()
        {
            return Font ?? UI.Control.DefaultFont;
        }

        /// <summary>
        /// Measures item height.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual double MeasureItemHeight(int itemIndex)
        {
            string s;

            if (itemIndex < Items.Count)
                s = GetItemText(itemIndex);
            else
                s = "Wy";

            var font = GetItemFont();
            var size = MeasureCanvas.MeasureText(s, font);
            return size.Height + ItemMargin.Vertical;
        }

        /// <summary>
        /// Draws item with the specified index.
        /// </summary>
        /// <param name="dc">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="rect">Rectangle in which to draw the item.</param>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual void DrawItem(Graphics dc, RectD rect, int itemIndex)
        {
            var s = GetItemText(itemIndex);
            var font = GetItemFont();

            Color textColor;
            if (SelectedIndex == itemIndex)
                textColor = GetSelectedItemTextColor();
            else
                textColor = GetItemTextColor();

            rect.ApplyMargin(ItemMargin);

            dc.DrawText(
                s,
                font,
                textColor.AsBrush,
                rect);
        }

        /// <summary>
        /// Gets selected item text color. Default is <see cref="SystemColors.HighlightText"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemTextColor()
        {
            return SelectedItemTextColor ?? DefaultSelectedItemTextColor;
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="SystemColors.WindowText"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetItemTextColor()
        {
            return ItemTextColor ?? DefaultItemTextColor;
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new VListBoxHandler();
        }
    }
}
