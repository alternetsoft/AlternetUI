using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ComboBox"/> descendant for editing <see cref="Color"/> values.
    /// </summary>
    /// <remarks>
    /// Items in this control have <see cref="ListControlItem"/> type where
    /// <see cref="ListControlItem.Value"/> is <see cref="Color"/> and
    /// <see cref="ListControlItem.Text"/> is label of the color.
    /// </remarks>
    public class ColorComboBox : ComboBox
    {
        /// <summary>
        /// Gets or sets default disabled image color.
        /// </summary>
        /// <remarks>
        /// This color is used when <see cref="ColorComboBox"/> is disabled
        /// for painting color image when <see cref="ColorComboBox.DisabledImageColor"/> is null.
        /// If this property is null, color image will be painted in the same way like it is done
        /// when control is enabled.
        /// </remarks>
        public static Color? DefaultDisabledImageColor = Color.LightGray;

        /// <summary>
        /// Gets or sets default painter for the <see cref="ColorComboBox"/> items.
        /// </summary>
        public static IComboBoxItemPainter Painter = new DefaultColorItemPainter();

        /// <summary>
        /// Gets or sets default method that initializes items in <see cref="ColorComboBox"/>.
        /// </summary>
        public static Action<ColorComboBox>? InitColors = InitDefaultColors;

        /// <summary>
        /// Gets or sets method that paints color image in the item. Borders around
        /// color image are also painted by this method.
        /// </summary>
        public static Action<Graphics, RectD, Color> PaintColorImage = PaintDefaultColorImage;

        private Color? disabledImageColor;
        private bool useDisabledImageColor = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComboBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ColorComboBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComboBox"/> class.
        /// </summary>
        public ColorComboBox()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComboBox"/> class.
        /// </summary>
        /// <param name="defaultColors">Specifies whether to add default color items
        /// to the control.</param>
        public ColorComboBox(bool defaultColors)
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

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsEditable
        {
            get => false;
            set
            {
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
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
                return SelectedItem as Color;
            }

            set
            {
                if (Value == value)
                    return;
                if(value is null)
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
        /// Adds color items to the <see cref="ColorComboBox"/>. This is default
        /// implementation of the initialization method. It is assigned to
        /// <see cref="InitColors"/> property by default.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        public static void InitDefaultColors(ColorComboBox control)
        {
            ListControlUtils.AddColors(control);
        }

        /// <summary>
        /// Default method of the item creation for the specified color and title.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public static ListControlItem DefaultCreateItem(Color value, string? title = null)
        {
            title ??= value.NameLocalized;
            ListControlItem controlItem = new(title, value);
            return controlItem;
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
            return DefaultCreateItem(value, title);
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
        public virtual ListControlItem FindOrAdd(Color value, string? title = null)
        {
            var result = Find(value);
            result ??= AddColor(value, title);
            return result;
        }

        /// <summary>
        /// Performs default initialization of the colors list and assigns item painter.
        /// </summary>
        /// <param name="defaultColors">Whether to add default colors.</param>
        protected virtual void Initialize(bool defaultColors = true)
        {
            if (defaultColors)
            {
                if (InitColors is not null)
                    InitColors(this);
            }

            OwnerDrawItem = true;
            ItemPainter = Painter;
        }

        /// <summary>
        /// Default item painter for the <see cref="ColorComboBox"/> items.
        /// </summary>
        public class DefaultColorItemPainter : DefaultItemPainter, IComboBoxItemPainter
        {
            /// <inheritdoc/>
            public override void Paint(ComboBox sender, ComboBoxItemPaintEventArgs e)
            {
                if (e.IsPaintingBackground || sender.ShouldPaintHintText())
                {
                    e.DefaultPaint();
                    return;
                }

                object? item = e.Item;

                if (item is ListControlItem item1)
                    item = item1.Value;

                var itemColor = (item as Color) ?? Color.White;

                if (!itemColor.IsOk)
                    itemColor = Color.White;

                var colorComboBox = sender as ColorComboBox;

                var useDisabledImageColor = colorComboBox?.UseDisabledImageColor ?? false;

                if (e.IsPaintingControl && !sender.Enabled && useDisabledImageColor)
                {
                    var disabledColor = colorComboBox?.DisabledImageColor
                        ?? DefaultDisabledImageColor;
                    if(disabledColor is not null)
                        itemColor = disabledColor;
                }

                var (colorRect, itemRect) = sender.GetItemImageRect(e);
                PaintColorImage(e.Graphics, colorRect, itemColor);
                e.ClipRectangle = itemRect;
                sender.DefaultItemPaint(e);
            }
        }
    }
}
