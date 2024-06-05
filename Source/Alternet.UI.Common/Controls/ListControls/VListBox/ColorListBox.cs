using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ListBox"/> descendant for selecting <see cref="Color"/> values.
    /// </summary>
    /// <remarks>
    /// Items in this control have <see cref="ListControlItem"/> type where
    /// <see cref="ListControlItem.Value"/> is <see cref="Color"/> and
    /// <see cref="ListControlItem.Text"/> is label of the color.
    /// </remarks>
    public class ColorListBox : VListBox
    {
        /// <summary>
        /// Gets or sets default painter for the <see cref="ColorListBox"/> items.
        /// </summary>
        public static IListBoxItemPainter Painter = new DefaultItemPainter();

        /// <summary>
        /// Gets or sets method that initializes items in <see cref="ColorListBox"/>.
        /// </summary>
        public static Action<ColorListBox>? InitColors = InitDefaultColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorListBox"/> class.
        /// </summary>
        public ColorListBox()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorListBox"/> class.
        /// </summary>
        /// <param name="defaultColors">Specifies whether to add default color items
        /// to the control.</param>
        public ColorListBox(bool defaultColors)
        {
            Initialize(defaultColors);
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// Color value must be added to the list of colors
        /// before selecting it.
        /// </summary>
        public Color? Value
        {
            get
            {
                if (SelectedItem is ListControlItem item)
                    return item.Value as Color;
                return SelectedItem as Color;
            }

            set
            {
                if (Value == value)
                    return;
                foreach (var item in Items)
                {
                    if (item is not ListControlItem item2)
                        continue;
                    if (item2.Value is not Color color)
                        continue;
                    if (color != value)
                        continue;
                    SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Adds color items to the <see cref="ColorListBox"/>. This is default
        /// implementation of the initialization method. It is assigned to
        /// <see cref="InitColors"/> property by default.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        public static void InitDefaultColors(ColorListBox control)
        {
            ListControlUtils.AddColors(control);
        }

        /// <summary>
        /// Gets color value of the specified item or default color.
        /// </summary>
        /// <param name="control">Control with items.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public static Color GetItemValueOrDefault(
            ListControl control,
            int itemIndex,
            Color defaultValue)
        {
            object? item = control.GetItem(itemIndex);

            if (item is ListControlItem item1)
                item = item1.Value;

            var itemColor = (item as Color) ?? defaultValue;

            if (!itemColor.IsOk)
                itemColor = defaultValue;

            return itemColor;
        }

        /// <summary>
        /// Initializes control with default colors and assigns item painter.
        /// This method is called from constructor.
        /// </summary>
        /// <param name="defaultColors">Whether to add default colors.</param>
        protected virtual void Initialize(bool defaultColors = true)
        {
            if (defaultColors)
            {
                if (InitColors is not null)
                    InitColors(this);
            }

            ItemPainter = Painter;
        }

        /// <summary>
        /// Default item painter for the <see cref="ColorListBox"/> items.
        /// </summary>
        public class DefaultItemPainter : IListBoxItemPainter
        {
            /// <inheritdoc/>
            public virtual SizeD GetSize(VListBox sender, int index)
            {
                return sender.DefaultMeasureItemSize(index);
            }

            /// <inheritdoc/>
            public virtual void Paint(VListBox sender, ListBoxItemPaintEventArgs e)
            {
                var itemColor = GetItemValueOrDefault(sender, e.ItemIndex, Color.White);

                if (sender.TextVisible)
                {
                    var (colorRect, itemRect) = sender.GetItemImageRect(e.ClipRectangle);
                    e.ClipRectangle = itemRect;
                    sender.DefaultDrawItem(e);
                    ColorComboBox.PaintColorImage(e.Graphics, colorRect, itemColor);
                }
                else
                {
                    ColorComboBox.PaintColorImage(e.Graphics, e.ClipRectangle, itemColor);
                }
            }

            /// <inheritdoc/>
            public virtual bool PaintBackground(VListBox sender, ListBoxItemPaintEventArgs e)
            {
                return false;
            }
        }
    }
}
