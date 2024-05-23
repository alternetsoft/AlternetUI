using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="IListBoxItemPainter.Paint"/> event.
    /// </summary>
    public class ListBoxItemPaintEventArgs : PaintEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxItemPaintEventArgs"/> class.
        /// </summary>
        /// <param name="control">Control which owns the item.</param>
        /// <param name="graphics"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="bounds">Bounds of the item.</param>
        /// <param name="itemIndex">Index of the tem.</param>
        public ListBoxItemPaintEventArgs(
            VListBox control,
            Graphics graphics,
            RectD bounds,
            int itemIndex)
            : base(graphics, bounds)
        {
            ListBox = control;
            this.ItemIndex = itemIndex;
        }

        /// <summary>
        /// Gets index of the item.
        /// </summary>
        public int ItemIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether item is selected.
        /// </summary>
        public bool IsSelected => ListBox.IsSelected(ItemIndex);

        /// <summary>
        /// Gets whether item is current.
        /// </summary>
        public bool IsCurrent => ListBox.Handler.IsCurrent(ItemIndex);

        /// <summary>
        /// Gets font of the item.
        /// </summary>
        public Font ItemFont
        {
            get
            {
                var itemFont = ListBox.GetItemFont(ItemIndex);
                if (ListBox.SelectedItemIsBold && IsCurrent)
                    itemFont = itemFont.AsBold;
                return itemFont;
            }
        }

        /// <summary>
        /// Gets foreground color of the item.
        /// </summary>
        public Color TextColor
        {
            get
            {
                Color textColor;
                if (IsSelected)
                {
                    textColor = ListBox.GetSelectedItemTextColor(ItemIndex);
                }
                else
                {
                    textColor = ListBox.GetItemTextColor(ItemIndex);
                }

                return textColor;
            }
        }

        /// <summary>
        /// Gets text of the item.
        /// </summary>
        public string ItemText => ListBox.GetItemText(ItemIndex);

        /// <summary>
        /// Gets minimal height of the item.
        /// </summary>
        public double ItemMinHeight => ListBox.GetItemMinHeight(ItemIndex);

        /// <summary>
        /// Gets alignment of the item.
        /// </summary>
        public GenericAlignment ItemAlignment => ListBox.GetItemAlignment(ItemIndex);

        /// <summary>
        /// Gets normal and disabled images of the item.
        /// </summary>
        public (Image? Normal, Image? Disabled, Image? Selected) ItemImages
            => ListBox.GetItemImages(ItemIndex, ListBox.GetSelectedItemTextColor(ItemIndex));

        /// <summary>
        /// Gets control which item is painted.
        /// </summary>
        public VListBox ListBox { get; }
    }
}
