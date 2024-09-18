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
            VirtualListBox control,
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
        public virtual int ItemIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether item is selected.
        /// </summary>
        public virtual bool IsSelected => ListBox.IsSelected(ItemIndex);

        /// <summary>
        /// Gets whether item is current.
        /// </summary>
        public virtual bool IsCurrent => ListBox.Handler.IsCurrent(ItemIndex);

        /// <summary>
        /// Gets font of the item.
        /// </summary>
        public virtual Font ItemFont
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
        public virtual Color TextColor
        {
            get
            {
                return GetTextColor(IsSelected);
            }
        }

        /// <summary>
        /// Gets text of the item.
        /// </summary>
        public virtual string ItemText => ListBox.GetItemText(ItemIndex);

        /// <summary>
        /// Gets minimal height of the item.
        /// </summary>
        public virtual Coord ItemMinHeight => ListBox.GetItemMinHeight(ItemIndex);

        /// <summary>
        /// Gets alignment of the item.
        /// </summary>
        public GenericAlignment ItemAlignment => ListBox.GetItemAlignment(ItemIndex);

        /// <summary>
        /// Gets normal and disabled images of the item.
        /// </summary>
        public virtual (Image? Normal, Image? Disabled, Image? Selected) ItemImages
            => ListBox.GetItemImages(ItemIndex, ListBox.GetSelectedItemTextColor(ItemIndex));

        /// <summary>
        /// Gets control which item is painted.
        /// </summary>
        public virtual VirtualListBox ListBox { get; set; }

        /// <summary>
        /// Gets text color.
        /// </summary>
        /// <param name="isSelected">Whether to get text color for the selected state.</param>
        /// <returns></returns>
        public virtual Color GetTextColor(bool isSelected)
        {
            Color textColor;
            if (isSelected)
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
}
