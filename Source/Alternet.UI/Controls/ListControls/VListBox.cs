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
        /// Gets or sets default disabled item text color.
        /// </summary>
        public static Color DefaultDisabledItemTextColor = SystemColors.GrayText;

        /// <summary>
        /// Gets or sets default item text color.
        /// </summary>
        public static Color DefaultItemTextColor = SystemColors.WindowText;

        private Thickness itemMargin = DefaultItemMargin;
        private Color? selectedItemTextColor;
        private Color? itemTextColor;
        private Color? selectedItemBackColor;
        private bool selectedIsBold;
        private Color? disabledItemTextColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="VListBox"/> class.
        /// </summary>
        public VListBox()
        {
            Handler.NativeControl.SetSelectionBackground(DefaultSelectedItemBackColor);
            SuggestedSize = 200;
        }

        /// <summary>
        /// Gets or sets disabled item text color.
        /// </summary>
        public Color? DisabledItemTextColor
        {
            get
            {
                return disabledItemTextColor;
            }

            set
            {
                if (disabledItemTextColor == value)
                    return;
                disabledItemTextColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ListControl.SelectedItem"/> has bold font.
        /// </summary>
        public bool SelectedItemIsBold
        {
            get
            {
                return selectedIsBold;
            }

            set
            {
                if (selectedIsBold == value)
                    return;
                selectedIsBold = value;
                Invalidate();
            }
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
        [Browsable(false)]
        public override bool CanUserPaint
        {
            get => false;
        }

        /// <summary>
        /// Gets or sets number of items in the control.
        /// </summary>
        [Browsable(false)]
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
            var result = Font ?? UI.Control.DefaultFont;
            if (IsBold)
                result = result.AsBold;
            return result;
        }

        /// <summary>
        /// Measures item size.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        public virtual SizeD MeasureItemSize(int itemIndex)
        {
            string s;

            if (itemIndex < Items.Count)
                s = GetItemText(itemIndex);
            else
                s = "Wy";

            var font = GetItemFont().AsBold;
            var size = MeasureCanvas.MeasureText(s, font);
            size.Width += ItemMargin.Horizontal;
            size.Height += ItemMargin.Vertical;
            return size;
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
            {
                if (SelectedItemIsBold)
                    font = font.AsBold;
                textColor = GetSelectedItemTextColor();
            }
            else
            {
                textColor = GetItemTextColor();
            }

            rect.ApplyMargin(ItemMargin);

            dc.DrawText(
                s,
                font,
                textColor.AsBrush,
                rect);
        }

        /// <summary>
        /// Gets selected item text color. Default is <see cref="SelectedItemTextColor"/>
        /// (if it is not <c>null</c>) or <see cref="DefaultSelectedItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetSelectedItemTextColor()
        {
            if (Enabled)
                return SelectedItemTextColor ?? DefaultSelectedItemTextColor;
            else
                return GetDisabledItemTextColor();
        }

        /// <summary>
        /// Gets item text color. Default is <see cref="ItemTextColor"/> (if it is not <c>null</c>)
        /// or <see cref="DefaultItemTextColor"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetItemTextColor()
        {
            if (Enabled)
                return ForegroundColor ?? ItemTextColor ?? DefaultItemTextColor;
            else
                return GetDisabledItemTextColor();
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetDisabledItemTextColor()
        {
            return DisabledItemTextColor ?? DefaultDisabledItemTextColor;
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new VListBoxHandler();
        }
    }
}
