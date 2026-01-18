using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="StdListBox"/> descendant for selecting <see cref="Color"/> values.
    /// </summary>
    /// <remarks>
    /// Items in this control have <see cref="ListControlItem"/> type where
    /// <see cref="ListControlItem.Value"/> is <see cref="Color"/> and
    /// <see cref="ListControlItem.Text"/> is label of the color.
    /// </remarks>
    public partial class ColorListBox : VirtualListBox
    {
        /// <summary>
        /// Gets or sets default disabled image color.
        /// </summary>
        /// <remarks>
        /// This color is used when control is disabled
        /// for painting color image when color of the disabled image is not specified.
        /// If this property is null, color image will be painted in the same way like it is done
        /// when control is enabled.
        /// </remarks>
        public static Color? DefaultDisabledImageColor = Color.LightGray;

        /// <summary>
        /// Gets or sets default painter for the <see cref="ColorListBox"/> items.
        /// </summary>
        public static IListBoxItemPainter Painter = new DefaultItemPainter();

        /// <summary>
        /// Gets or sets method that paints color image in the item. Borders around
        /// color image are also painted by this method.
        /// </summary>
        public static Action<Graphics, RectD, Color> PaintColorImage
            = ColorListBox.PaintDefaultColorImage;

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
        public ColorListBox(AbstractControl parent)
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
        /// <see cref="DefaultDisabledImageColor"/>.
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
        /// Paints color image in the item with the default style. Borders around
        /// color image are also painted by this method.
        /// This is default value of the <see cref="PaintColorImage"/> field.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="rect"><see cref="RectD"/> where drawing is performed.</param>
        /// <param name="color">Color value.</param>
        public static void PaintDefaultColorImage(Graphics canvas, RectD rect, Color color)
        {
            RectD colorRect = DrawingUtils.DrawDoubleBorder(
                canvas,
                rect,
                Color.Empty,
                ComboBox.DefaultImageBorderColor);

            canvas.FillRectangle(color.AsBrush, colorRect);
        }

        /// <summary>
        /// Finds item with the specified color in the collection of the color items.
        /// </summary>
        /// <param name="value">Color value.</param>
        /// <param name="items">Collection of the color items.</param>
        /// <returns></returns>
        public static ListControlItem? Find(Color? value, IEnumerable items)
        {
            if (value is null)
                return null;

            foreach (var item in items)
            {
                if (item is not ListControlItem item2)
                    continue;
                if (item2.Value is not Color color)
                    continue;
                if (color.AsStruct != value.AsStruct)
                    continue;
                return item2;
            }

            return null;
        }

        /// <summary>
        /// Default method of the item creation for the specified color and title.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public static ListControlItem DefaultCreateItem(Color? value, string? title = null)
        {
            title ??= value?.NameLocalized ?? string.Empty;
            ListControlItem controlItem = new(title, value);
            return controlItem;
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
        /// Retrieves the value of the specified item as a <see cref="Color"/> object.
        /// </summary>
        /// <remarks>This method attempts to cast the value of the
        /// provided <see cref="ListControlItem"/>
        /// to a <see cref="Color"/>. If the cast is unsuccessful,
        /// <see langword="null"/> is returned.</remarks>
        /// <param name="item">The <see cref="ListControlItem"/> whose value
        /// is to be retrieved. Can be <see langword="null"/>.</param>
        /// <returns>A <see cref="Color"/> object representing the value of the
        /// specified item, or <see langword="null"/> if the
        /// item is <see langword="null"/> or its value is not a <see cref="Color"/>.</returns>
        public virtual Color? GetItemValue(ListControlItem? item)
        {
            if (item is null)
                return null;
            var color = item.Value as Color;
            return color;
        }

        /// <summary>
        /// Finds item with the specified color.
        /// </summary>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public virtual ListControlItem? Find(Color? value)
        {
            return Find(value, Items);
        }

        /// <summary>
        /// Finds item with the specified color or adds it.
        /// </summary>
        /// <param name="value">Color value.</param>
        /// <param name="title">Color title. Optional.</param>
        /// <returns></returns>
        public virtual ListControlItem FindOrAdd(Color? value, string? title = null)
        {
            var result = Find(value);
            result ??= AddColor(value, title);
            return result;
        }

        /// <summary>
        /// Finds an existing color that matches the specified value or adds
        /// a new color if no match is found.
        /// </summary>
        /// <remarks>This method attempts to locate a color that matches the specified <paramref
        /// name="value"/>. If no match is found, a new color is added to the collection.</remarks>
        /// <param name="value">The color to search for or add. Cannot be null.</param>
        /// <param name="title">An optional title associated with the color. If provided,
        /// it may be used to label the color.</param>
        /// <returns>The matching or newly added <see cref="Color"/> instance,
        /// or <see langword="null"/> if the operation fails.</returns>
        public virtual Color? FindOrAddColor(Color? value, string? title = null)
        {
            var item = FindOrAdd(value, title);
            var result = GetItemValue(item);
            return result;
        }

        /// <summary>
        /// Creates item for the specified color and title.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public virtual ListControlItem CreateItem(Color? value, string? title = null)
        {
            return DefaultCreateItem(value, title);
        }

        /// <summary>
        /// Adds color to the list of colors.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        public virtual ListControlItem AddColor(Color? value, string? title = null)
        {
            var item = CreateItem(value, title);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds colors from the specified color categories.
        /// </summary>
        /// <param name="categories">Array of categories to add colors from.</param>
        /// <param name="onlyVisible">Whether to process only
        /// colors which are visible to the end-user. Optional. Default is True.</param>
        public virtual void AddColors(
            KnownColorCategory[]? categories,
            bool onlyVisible = true)
        {
            ListControlUtils.AddColors(this, false, null, categories, onlyVisible);
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
                if (sender is not ColorListBox listBox)
                    return SizeD.MinusOne;

                return ListControlItem.DefaultMeasureItemSize(listBox, listBox.MeasureCanvas, index);
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
                        ?? DefaultDisabledImageColor;
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
                    var (colorRect, itemRect) = ListControlItem.GetItemImageRect(e.ClientRectangle);
                    e.ClientRectangle = itemRect;
                    colorListBox.DefaultDrawItemForeground(e);
                    PaintColorImage(e.Graphics, colorRect, itemColor);
                }
                else
                {
                    PaintColorImage(e.Graphics, e.ClientRectangle, itemColor);
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