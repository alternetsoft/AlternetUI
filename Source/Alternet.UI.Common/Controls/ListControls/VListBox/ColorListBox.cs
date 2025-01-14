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
    public class ColorListBox : VirtualListBox
    {
        /// <summary>
        /// Gets or sets default painter for the <see cref="ColorListBox"/> items.
        /// </summary>
        public static IListBoxItemPainter Painter = new DefaultItemPainter();

        /// <summary>
        /// Gets or sets method that initializes items in <see cref="ColorListBox"/>.
        /// </summary>
        public static Action<ColorListBox>? InitColors = InitDefaultColors;

        private Color? disabledImageColor;
        private bool useDisabledImageColor = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ColorListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

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
        /// Gets or sets whether to use <see cref="DisabledImageColor"/>
        /// for painting of the color image
        /// when control is disabled.
        /// </summary>
        public virtual bool UseDisabledImageColor
        {
            get
            {
                return useDisabledImageColor;
            }

            set
            {
                if (useDisabledImageColor == value)
                    return;
                useDisabledImageColor = value;
                if (Enabled)
                    return;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets disabled image color.
        /// </summary>
        /// <remarks>
        /// This color is used for painting color image when control is disabled.
        /// If this property is null, color image will be painted using
        /// <see cref="ColorComboBox.DefaultDisabledImageColor"/>.
        /// </remarks>
        public virtual Color? DisabledImageColor
        {
            get
            {
                return disabledImageColor;
            }

            set
            {
                if (disabledImageColor == value)
                    return;
                disabledImageColor = value;
                if (Enabled)
                    return;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// Color value must be added to the list of colors
        /// before selecting it.
        /// </summary>
        public virtual Color? Value
        {
            get
            {
                if (SelectedItem is ListControlItem item)
                    return item.Value as Color;
                return null;
            }

            set
            {
                if (Value == value)
                    return;
                if (value is null)
                {
                    SelectedIndex = null;
                }
                else
                {
                    var item = FindOrAdd(value);
                    SelectedItem = item;
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
            IListControl control,
            int itemIndex,
            Color defaultValue)
        {
            object? item = control.GetItemAsObject(itemIndex);

            if (item is ListControlItem item1)
                item = item1.Value;

            var itemColor = (item as Color) ?? defaultValue;

            if (!itemColor.IsOk)
                itemColor = defaultValue;

            return itemColor;
        }

        /// <summary>
        /// Finds item with the specified color.
        /// </summary>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public virtual ListControlItem? Find(Color? value)
        {
            return ColorComboBox.Find(value, Items);
        }

        /// <summary>
        /// Finds item with the specified color or adds it.
        /// </summary>
        /// <param name="value">Color value.</param>
        /// <param name="title">Color title. Optional.</param>
        /// <returns></returns>
        public virtual ListControlItem FindOrAdd(Color value, string? title = null)
        {
            var result = Find(value);
            result ??= AddColor(value, title);
            return result;
        }

        /// <summary>
        /// Creates item for the specified color and title.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public virtual ListControlItem CreateItem(Color value, string? title = null)
        {
            return ColorComboBox.DefaultCreateItem(value, title);
        }

        /// <summary>
        /// Adds color to the list of colors.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        public virtual ListControlItem AddColor(Color value, string? title = null)
        {
            var item = CreateItem(value, title);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds colors from the specified color categories.
        /// </summary>
        /// <param name="categories">Array of categories to add colors from.</param>
        public virtual void AddColors(KnownColorCategory[]? categories)
        {
            ListControlUtils.AddColors(this, false, null, categories);
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
            public virtual SizeD GetSize(object sender, int index)
            {
                if (sender is not ColorListBox listbox)
                    return SizeD.MinusOne;

                return ListControlItem.DefaultMeasureItemSize(listbox, listbox.MeasureCanvas, index);
            }

            /// <summary>
            /// Gets image color for the item.
            /// </summary>
            /// <param name="sender">Control.</param>
            /// <param name="e">Parameters.</param>
            /// <returns></returns>
            public virtual Color GetImageColor(
                ColorListBox sender,
                ListBoxItemPaintEventArgs e)
            {
                var itemColor = GetItemValueOrDefault(sender, e.ItemIndex, Color.White);
                var colorListBox = sender as ColorListBox;
                var useDisabledImageColor = colorListBox?.UseDisabledImageColor ?? false;

                if (!sender.Enabled && useDisabledImageColor)
                {
                    var disabledColor = colorListBox?.DisabledImageColor
                        ?? ColorComboBox.DefaultDisabledImageColor;
                    if (disabledColor is not null)
                        itemColor = disabledColor;
                }

                return itemColor;
            }

            /// <inheritdoc/>
            public virtual void Paint(object sender, ListBoxItemPaintEventArgs e)
            {
                if (sender is not ColorListBox colorListBox)
                {
                    if (sender is VirtualListControl listControl)
                        listControl.DefaultDrawItemForeground(e);
                    return;
                }

                var itemColor = GetImageColor(colorListBox, e);
                if (colorListBox.TextVisible)
                {
                    var (colorRect, itemRect) = ListControlItem.GetItemImageRect(e.ClipRectangle);
                    e.ClipRectangle = itemRect;
                    colorListBox.DefaultDrawItemForeground(e);
                    ColorComboBox.PaintColorImage(e.Graphics, colorRect, itemColor);
                }
                else
                {
                    ColorComboBox.PaintColorImage(e.Graphics, e.ClipRectangle, itemColor);
                }
            }

            /// <inheritdoc/>
            public virtual bool PaintBackground(object sender, ListBoxItemPaintEventArgs e)
            {
                return false;
            }
        }
    }
}