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
    /// Represents list control for selecting <see cref="Color"/> values.
    /// </summary>
    /// <remarks>
    /// Items in this control have <see cref="ListControlItem"/> type where
    /// <see cref="ListControlItem.Value"/> is <see cref="Color"/> and
    /// <see cref="ListControlItem.Text"/> is label of the color.
    /// </remarks>
    public partial class ColorListBox : VirtualListBox
    {
        /// <summary>
        /// Gets or sets default shape of the item image.
        /// </summary>
        public static DrawingShapeType? DefaultItemImageShape = DrawingShapeType.Circle;

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
        /// Gets or sets method that initializes items in <see cref="ColorListBox"/>.
        /// </summary>
        public static Action<ColorListBox>? InitColors = InitDefaultColors;

        private Color? disabledImageColor;
        private bool useDisabledImageColor = true;
        private bool isColorRightAligned;
        private ItemImageSizeKind colorImageSizeKind = ItemImageSizeKind.Ratio;
        private SizeD? colorImageSize;
        private SizeD colorImageRatio = (3, 2);
        private DrawingShapeType? itemImageShape = DefaultItemImageShape;
        private Color? itemImageBorder;
        private ShapeDrawable? shapeDrawable;

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
        /// Enumerates the ways in which the size of the color image can be determined.
        /// </summary>
        public enum ItemImageSizeKind
        {
            /// <summary>
            /// Color image size is calculated using height of the item.
            /// </summary>
            Auto,

            /// <summary>
            /// Color image size is specified by <see cref="ColorImageSize"/> property.
            /// </summary>
            Custom,

            /// <summary>
            /// Color image size is calculated using ratio of the height of the item.
            /// </summary>
            Ratio,
        }

        /// <summary>
        /// Gets or sets the border color of the item image.
        /// </summary>
        [Browsable(false)]
        public virtual Color? ItemImageBorder
        {
            get => itemImageBorder;
            set
            {
                if (value == itemImageBorder)
                    return;
                itemImageBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the shape of the item image.
        /// </summary>
        public virtual DrawingShapeType? ItemImageShape
        {
            get => itemImageShape;
            set
            {
                if (value == itemImageShape)
                    return;
                itemImageShape = value;
                Invalidate();
            }
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
        /// Gets or sets how to determine size of the color image in the item.
        /// </summary>
        [Browsable(false)]
        public virtual ItemImageSizeKind ColorImageSizeKind
        {
            get => colorImageSizeKind;

            set
            {
                if (colorImageSizeKind == value)
                    return;
                colorImageSizeKind = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets size of the color image in the item.
        /// It is used when <see cref="ColorImageSizeKind"/> is <see cref="ItemImageSizeKind.Custom"/>
        /// </summary>
        [Browsable(false)]
        public virtual SizeD? ColorImageSize
        {
            get => colorImageSize;
            set
            {
                if (colorImageSize == value) return;
                colorImageSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the ratio of the color image size.
        /// It is used when <see cref="ColorImageSizeKind"/> is <see cref="ItemImageSizeKind.Ratio"/>.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD ColorImageRatio
        {
            get => colorImageRatio;
            set
            {
                if (colorImageRatio == value) return;
                colorImageRatio = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether color image is right aligned in the item.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsColorRightAligned
        {
            get => isColorRightAligned;
            set
            {
                if (isColorRightAligned == value)
                    return;
                isColorRightAligned = value;
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
        /// <see cref="Color.ToDisplayString"/> will be used.</param>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public static ListControlItem DefaultCreateItem(Color? value, string? title = null)
        {
            title ??= value?.ToDisplayString() ?? string.Empty;
            ListControlItem controlItem = new(title, value);
            return controlItem;
        }

        /// <summary>
        /// Default method of the item creation for the specified drawing resource and title.
        /// </summary>
        /// <param name="value">Drawing resource value.</param>
        /// <returns></returns>
        public static ListControlItem DefaultCreateItem(DrawingResource value)
        {
            ListControlItem controlItem = new(value.Title ?? string.Empty, value);
            return controlItem;
        }

        /// <summary>
        /// Default method of the item creation for the specified brush and title.
        /// </summary>
        /// <param name="title">Brush title.</param>
        /// <param name="value">Brush value.</param>
        /// <returns></returns>
        public static ListControlItem DefaultCreateItem(Brush value, string title)
        {
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
        /// Gets value of the specified item as a <see cref="Brush"/> object.
        /// </summary>
        /// <param name="control">The control containing the item.</param>
        /// <param name="itemIndex">The index of the item.</param>
        /// <returns>The brush value of the item, or <see langword="null"/> if the item is not a brush.</returns>
        public static Brush? GetItemValueAsBrush(IListControl control, int itemIndex)
        {
            object? item = control.GetItemAsObject(itemIndex);

            if (item is ListControlItem item1)
                item = item1.Value;

            if (item is Brush brush)
                return brush;

            if (item is DrawingResource drawingResource)
                return drawingResource.Brush;

            return null;
        }

        /// <summary>
        /// Gets value of the specified item as a <see cref="DrawingResource"/> object.
        /// </summary>
        /// <param name="control">The control containing the item.</param>
        /// <param name="itemIndex">The index of the item.</param>
        /// <returns>The drawing resource value of the item, or <see langword="null"/> if the item is not a drawing resource.</returns>
        public static DrawingResource? GetItemValueAsDrawingResource(IListControl control, int itemIndex)
        {
            if (control.GetItemAsObject(itemIndex) is not ListControlItem item)
                return null;

            var value = item.Value;
            var valueText = item.DisplayText ?? item.Text;

            if (value is DrawingResource drawingResource)
                return drawingResource;

            if (value is Color color)
            {
                var result = new DrawingResource(color);
                result.Title = valueText;
                return result;
            }

            if (value is Brush brush)
            {
                var result = new DrawingResource(brush, null);
                result.Title = valueText;
                return result;
            }

            if (value is Pen pen)
            {
                var result = new DrawingResource(null, pen);
                result.Title = valueText;
                return result;
            }

            return null;
        }

        /// <summary>
        /// Paints color image in the item with the default style. Borders around
        /// color image are also painted by this method.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="rect"><see cref="RectD"/> where drawing is performed.</param>
        /// <param name="brush">Color value.</param>
        public virtual void PaintItemImage(Graphics canvas, RectD rect, Brush brush)
        {
            var borderColor = ItemImageBorder ?? ListControlItem.DefaultImageBorderColor;

            if (ItemImageShape is null)
            {
                RectD colorRect = DrawingUtils.DrawDoubleBorder(
                    canvas,
                    rect,
                    Color.Empty,
                    borderColor);

                canvas.FillRectangle(brush, colorRect);
            }
            else
            {
                shapeDrawable ??= new ShapeDrawable();

                shapeDrawable.Bounds = rect;
                shapeDrawable.Brush = brush;
                shapeDrawable.Pen = borderColor?.AsPen;
                shapeDrawable.ShapeType = ItemImageShape.Value;
                shapeDrawable.Draw(this, canvas);
            }
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
        /// Attempts to resolve the specified color value to a corresponding item,
        /// returning a coerced color if available.
        /// </summary>
        /// <remarks>This method searches for an item matching the provided color value. If no match is
        /// found, the input value is returned unchanged.</remarks>
        /// <param name="value">The color value to coerce. If the value does not correspond to an item,
        /// the original value is used.</param>
        /// <returns>The coerced color value if a corresponding item is found;
        /// otherwise, the original value.</returns>
        public virtual Color? CoerceColor(Color? value)
        {
            var item = Find(value);
            var result = GetItemValue(item) ?? value;
            return result;
        }

        /// <summary>
        /// Creates item for the specified color and title.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.ToDisplayString"/> will be used.</param>
        /// <param name="value">Color value.</param>
        /// <returns></returns>
        public virtual ListControlItem CreateItem(Color? value, string? title = null)
        {
            return DefaultCreateItem(value, title);
        }

        /// <summary>
        /// Creates item for the specified brush and title.
        /// </summary>
        /// <param name="title">Brush title.</param>
        /// <param name="value">Brush value.</param>
        /// <returns></returns>
        public virtual ListControlItem CreateItem(Brush value, string title)
        {
            return DefaultCreateItem(value, title);
        }

        /// <summary>
        /// Creates item for the specified drawing resource and title.
        /// </summary>
        /// <param name="value">Drawing resource value.</param>
        /// <returns></returns>
        public virtual ListControlItem CreateItem(DrawingResource value)
        {
            return DefaultCreateItem(value);
        }

        /// <summary>
        /// Adds color to the list of colors.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.ToDisplayString"/> will be used.</param>
        /// <param name="value">Color value.</param>
        public virtual ListControlItem AddColor(Color? value, string? title = null)
        {
            var item = CreateItem(value, title);
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds brush item to the list of items.
        /// </summary>
        /// <param name="title">Brush title.</param>
        /// <param name="value">Brush value.</param>
        public virtual ListControlItem AddBrushItem(Brush value, string title)
        {
            var item = CreateItem(value, title);
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds a transparent color to the list of colors.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.ToDisplayString"/> will be used.</param>
        /// <returns>The newly added <see cref="ListControlItem"/> instance.</returns>
        public virtual ListControlItem AddTransparentColor(string? title = null)
        {
            title ??= Localization.CommonStrings.Default.TransparentColorDisplayName;
            var item = CreateItem(Color.Transparent, title);
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds an empty color to the list of colors.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.ToDisplayString"/> will be used.</param>
        /// <returns>The newly added <see cref="ListControlItem"/> instance.</returns>
        public virtual ListControlItem AddEmptyColor(string? title = null)
        {
            title ??= Localization.CommonStrings.Default.EmptyColorDisplayName;
            var item = CreateItem(Color.Empty, title);
            Add(item);
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
        /// Coerces the size of the color image based on the specified size.
        /// </summary>
        /// <param name="size">The size to coerce.</param>
        /// <param name="prm">The parameters for coercing the image size.</param>
        /// <returns>The coerced size.</returns>
        protected virtual SizeD CoerceColorImageSize(SizeD size, ListControlItem.CoerceItemImageSizeParams prm)
        {
            switch (ColorImageSizeKind)
            {
                case ItemImageSizeKind.Custom:
                    return ColorImageSize ?? size;
                case ItemImageSizeKind.Ratio:

                    var hratio = colorImageRatio.Height;
                    if(hratio <= 0)
                        hratio = 1;
                    var wratio = colorImageRatio.Width;
                    if (wratio <= 0)
                        wratio  = 1;

                    var height = size.Height;
                    var v = height / hratio;
                    var width = v * wratio;
                    size = new SizeD(width, height);
                    return size;
            }

            return size;
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
            public virtual Color GetImageColor(ColorListBox sender, ListBoxItemPaintEventArgs e)
            {
                var itemColor = GetItemValueOrDefault(sender, e.ItemIndex, Color.White);
                var useDisabledImageColor = sender.UseDisabledImageColor;

                if (!sender.Enabled && useDisabledImageColor)
                {
                    var disabledColor = sender.DisabledImageColor ?? DefaultDisabledImageColor;
                    if (disabledColor is not null)
                        itemColor = disabledColor;
                }

                return itemColor;
            }

            /// <summary>
            /// Gets image brush for the item.
            /// </summary>
            /// <param name="sender">Control.</param>
            /// <param name="e">Parameters.</param>
            /// <returns></returns>
            public virtual Brush GetImageBrush(ColorListBox sender, ListBoxItemPaintEventArgs e)
            {
                var result = GetItemValueAsBrush(sender, e.ItemIndex);

                if(result is null)
                {
                    result = GetImageColor(sender, e).AsBrush;
                    return result;
                }

                var useDisabledImageColor = sender.UseDisabledImageColor;

                if (!sender.Enabled && useDisabledImageColor)
                {
                    var disabledColor = sender.DisabledImageColor ?? DefaultDisabledImageColor;
                    if (disabledColor is not null)
                        result = disabledColor.AsBrush;
                }

                return result;
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

                var isRight = colorListBox.IsColorRightAligned;
                var itemBrush = GetImageBrush(colorListBox, e);

                if (colorListBox.TextVisible)
                {
                    var (colorRect, itemRect) = ListControlItem.GetItemImageRect(
                        e.ClientRectangle,
                        colorListBox.CoerceColorImageSize,
                        isRight);
                    e.ClientRectangle = itemRect;
                    colorListBox.DefaultDrawItemForeground(e);
                    colorListBox.PaintItemImage(e.Graphics, colorRect, itemBrush);
                }
                else
                {
                    colorListBox.PaintItemImage(e.Graphics, e.ClientRectangle, itemBrush);
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