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
        /// Initializes a new instance of the <see cref="ListBoxItemPaintEventArgs"/> class.
        /// </summary>
        /// <param name="control">Control which owns the item.</param>
        /// <param name="graphics"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="bounds">Bounds of the item.</param>
        /// <param name="itemIndex">Index of the tem.</param>
        public ListBoxItemPaintEventArgs(
            IListControlItemContainer control,
            Graphics graphics,
            RectD bounds,
            int itemIndex)
            : base(() => graphics, bounds)
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
        public virtual bool IsSelected { get; set; }

        /// <summary>
        /// Gets whether item is current.
        /// </summary>
        public virtual bool IsCurrent { get; set; }

        /// <summary>
        /// Gets font of the item.
        /// </summary>
        public virtual Font ItemFont
        {
            get
            {
                var itemFont = ListControlItem.GetFont(Item, ListBox, IsSelected);
                return itemFont;
            }
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        public virtual ListControlItem? Item
        {
            get
            {
                return ListBox.SafeItem(ItemIndex);
            }
        }

        /// <summary>
        /// Gets foreground color of the item.
        /// </summary>
        public virtual Color? TextColor
        {
            get
            {
                return GetTextColor(IsSelected);
            }
        }

        /// <summary>
        /// Gets text of the item.
        /// </summary>
        public virtual string ItemText
        {
            get
            {
                return ListBox.GetItemText(ItemIndex, false);
            }
        }

        /// <summary>
        /// Gets text of the item for display.
        /// </summary>
        public virtual string ItemTextForDisplay
        {
            get
            {
                return ListBox.GetItemText(ItemIndex, true);
            }
        }

        /// <summary>
        /// Gets minimal height of the item.
        /// </summary>
        public virtual Coord ItemMinHeight
        {
            get
            {
                var result = ListControlItem.GetMinHeight(Item, ListBox);
                return result;
            }
        }

        /// <summary>
        /// Gets alignment of the item.
        /// </summary>
        public virtual HVAlignment ItemAlignment
        {
            get
            {
                var result = ListControlItem.GetAlignment(Item, ListBox);
                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether real painting need to be performed. When <see cref="Visible"/>
        /// is False, caller need only <see cref="LabelMetrics"/> without any actual painting.
        /// </summary>
        public virtual bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to use columns for item painting.
        /// </summary>
        public virtual bool UseColumns { get; set; } = false;

        /// <summary>
        /// Gets draw label result returned after painting of the label is performed.
        /// </summary>
        public virtual Graphics.DrawLabelParams LabelMetrics { get; set; }

        /// <summary>
        /// Gets normal and disabled images of the item.
        /// </summary>
        public virtual EnumArrayStateImages ItemImages
        {
            get
            {
                var color = ListControlItem.GetSelectedTextColor(Item, ListBox);
                return ListControlItem.GetItemImages(Item, ListBox, color);
            }
        }

        /// <summary>
        /// Gets control which item is painted.
        /// </summary>
        public virtual IListControlItemContainer ListBox { get; set; }

        /// <summary>
        /// Gets text color.
        /// </summary>
        /// <param name="isSelected">Whether to get text color for the selected state.</param>
        /// <returns></returns>
        public virtual Color? GetTextColor(bool isSelected)
        {
            Color? textColor;
            if (isSelected)
            {
                textColor = ListControlItem.GetSelectedTextColor(Item, ListBox);
            }
            else
            {
                textColor = ListControlItem.GetItemTextColor(Item, ListBox);
            }

            return textColor;
        }
    }
}
