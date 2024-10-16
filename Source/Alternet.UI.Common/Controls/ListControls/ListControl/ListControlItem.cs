using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Custom item for <see cref="ListBox"/>, <see cref="ComboBox"/> and other
    /// list controls. This class has <see cref="Text"/>,
    /// <see cref="Value"/> and other properties which allow to customize look of the item.
    /// </summary>
    public partial class ListControlItem : BaseControlItem
    {
        /// <summary>
        /// Gets default item alignment
        /// </summary>
        public static readonly GenericAlignment DefaultItemAlignment
            = GenericAlignment.CenterVertical | GenericAlignment.Left;

        private SvgImage? svgImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItem"/> class.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="value">User data.</param>
        public ListControlItem(string text, object? value = null)
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItem"/> class.
        /// </summary>
        public ListControlItem()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItem"/> class.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="action">Action associated with the item.</param>
        public ListControlItem(string text, Action? action)
        {
            Text = text;
            Action = action;
        }

        /// <summary>
        /// Gets or sets state of the check box associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual CheckState CheckState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether checkbox will
        /// allow three check states rather than two. If property is null (default),
        /// control's setting is used.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the checkbox is able to display
        /// three check states; <see langword="false" /> if not; <c>null</c> if control's setting
        /// is used.
        /// </returns>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [DefaultValue(false)]
        public virtual bool? CheckBoxThreeState { get; set; }

        /// <summary>
        /// Gets or sets whether user can set the checkbox to
        /// the third state by clicking. If property is null (default),
        /// control's setting is used.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// </remarks>
        [DefaultValue(false)]
        public virtual bool? CheckBoxAllowAllStatesForUser { get; set; }

        /// <summary>
        /// Gets or sets whether to show check box inside the item. This property (if specified)
        /// overrides global checkboxes visibility setting in the control.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual bool? CheckBoxVisible { get; set; }

        /// <summary>
        /// Gets or sets whether item can be removed.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// </remarks>
        [Browsable(false)]
        public virtual bool CanRemove { get; set; } = true;

        /// <summary>
        /// Gets or sets <see cref="Image"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Image? Image { get; set; }

        /// <summary>
        /// Gets or sets <see cref="SvgImage"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual SvgImage? SvgImage
        {
            get => svgImage;
            set
            {
                if (svgImage == value)
                    return;
                svgImage = value;
                Image = null;
                DisabledImage = null;
                SelectedImage = null;
            }
        }

        /// <summary>
        /// Gets or sets size of the svg image.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// Currently only rectangular svg images are supported.
        /// </remarks>
        [Browsable(false)]
        public virtual SizeI? SvgImageSize { get; set; }

        /// <summary>
        /// Gets or sets disabled <see cref="Image"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Image? DisabledImage { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Image"/> associated with the item when it is selected.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Image? SelectedImage { get; set; }

        /// <summary>
        /// Gets or sets minimal item height.
        /// </summary>
        [Browsable(false)]
        public virtual Coord MinHeight { get; set; }

        /// <summary>
        /// Gets or sets <see cref="FontStyle"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual FontStyle? FontStyle { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Font"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Font? Font { get; set; }

        /// <summary>
        /// Gets or sets whether to hide selection for this item.
        /// </summary>
        [Browsable(false)]
        public virtual bool HideSelection { get; set; }

        /// <summary>
        /// Gets or sets whether to hide focus rectangle for this item.
        /// </summary>
        [Browsable(false)]
        public virtual bool HideFocusRect { get; set; }

        /// <summary>
        /// Gets or sets foreground color of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets background color of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets border of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual BorderSettings? Border { get; set; }

        /// <summary>
        /// Gets or sets alignment of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual GenericAlignment Alignment { get; set; } = DefaultItemAlignment;

        /// <summary>
        /// Gets or sets text which is displayed when item is painted.
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets user data. This is different from <see cref="BaseControlItem.Tag"/>.
        /// </summary>
        public virtual object? Value { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Action"/> associated with this
        /// <see cref="ListControlItem"/> instance.
        /// </summary>
        [Browsable(false)]
        public virtual Action? Action { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which is executed on mouse double click.
        /// </summary>
        [Browsable(false)]
        public virtual Action? DoubleClickAction { get; set; }

        /// <summary>
        /// Gets font of the container.
        /// </summary>
        public static Font GetContainerFont(IListControlItemContainer? container)
        {
            Font result;

            if (container is Control control)
                result = control.RealFont;
            else
                result = Control.DefaultFont;

            return result;
        }

        /// <summary>
        /// Gets item images.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="container">Container of the items.</param>
        /// <param name="svgColor">Color of the svg image when item is selected.</param>
        /// <returns></returns>
        public static (Image? Normal, Image? Disabled, Image? Selected)
            GetItemImages(
            ListControlItem? item,
            IListControlItemContainer? container,
            Color? svgColor)
        {
            if (item is null)
                return (null, null, null);

            var svgImage = item.SvgImage;

            if (svgImage is not null)
            {
                var isDark = IsContainerDarkBackground(container);

                var imageSize = item.SvgImageSize ?? container?.Defaults.SvgImageSize
                    ?? ToolBarUtils.GetDefaultImageSize(container as Control);
                var imageHeight = imageSize.Height;
                item.Image ??= svgImage.AsNormalImage(imageHeight, isDark);
                item.DisabledImage ??= svgImage.AsDisabledImage(imageHeight, isDark);

                if (svgColor is not null)
                    item.SelectedImage ??= svgImage.ImageWithColor(imageHeight, svgColor);
            }

            var image = item.Image;
            var disabledImage = item.DisabledImage ?? item.Image;
            var selectedImage = item.SelectedImage ?? item.Image;

            return (
                image,
                disabledImage,
                selectedImage);
        }

        /// <summary>
        /// Gets item minimal height.
        /// </summary>
        public static Coord GetMinHeight(ListControlItem? item, IListControlItemContainer? container)
        {
            var containerMinHeight = container?.Defaults.MinItemHeight ?? VirtualListBox.DefaultMinItemHeight;
            if (item is null)
                return containerMinHeight;
            return Math.Max(item.MinHeight, containerMinHeight);
        }

        /// <summary>
        /// Gets font when item is inside the specified container. Result must not be <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public static Font GetFont(ListControlItem? item, IListControlItemContainer? container)
        {
            if (item is null)
                return GetContainerFont(container);
            else
                return item.GetFont(container);
        }

        /// <summary>
        /// Gets whether container is enabled.
        /// </summary>
        public static bool IsContainerEnabled(IListControlItemContainer? container)
        {
            var enabled = container is not Control control || control.Enabled;
            return enabled;
        }

        /// <summary>
        /// Gets whether container is enabled.
        /// </summary>
        public static bool IsContainerDarkBackground(IListControlItemContainer? container)
        {
            if (container is Control control)
                return control.IsDarkBackground;
            else
                return SystemSettings.AppearanceIsDark;
        }

        /// <summary>
        /// Gets foreground color of the container.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Color? GetContainerForegroundColor(IListControlItemContainer? container)
        {
            if (container is Control control)
                return control.ForegroundColor;
            return null;
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public static Color GetDisabledTextColor(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            return container?.Defaults.DisabledItemTextColor
                ?? VirtualListBox.DefaultDisabledItemTextColor;
        }

        /// <summary>
        /// Gets item text color when item is inside the spedified container.
        /// </summary>
        public static Color GetItemTextColor(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            if (IsContainerEnabled(container))
            {
                var itemColor = item?.ForegroundColor ?? GetContainerForegroundColor(container)
                    ?? container?.Defaults.ItemTextColor ?? VirtualListBox.DefaultItemTextColor;
                return itemColor;
            }
            else
                return GetDisabledTextColor(item, container);
        }

        /// <summary>
        /// Gets item alignment.
        /// </summary>
        public static GenericAlignment GetAlignment(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            if (item is null)
            {
                return container?.Defaults.ItemAlignment
                    ?? (GenericAlignment.CenterVertical | GenericAlignment.Left);
            }
            else
                return item.Alignment;
        }

        /// <summary>
        /// Gets selected item text color when item is inside the spedified container.
        /// </summary>
        /// <returns></returns>
        public static Color GetSelectedTextColor(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            if (container?.Defaults.SelectionVisible ?? true)
            {
                if (IsContainerEnabled(container))
                {
                    return container?.Defaults.SelectedItemTextColor
                            ?? VirtualListBox.DefaultSelectedItemTextColor;
                }
                else
                    return GetDisabledTextColor(item, container);
            }
            else
                return GetItemTextColor(item, container);
        }

        /// <summary>
        /// Gets suggested rectangles of the item's image and text.
        /// </summary>
        /// <param name="rect">Item rectangle.</param>
        /// <param name="imageSize">Image size. Optional. If not specified, calculated
        /// using height of the item.</param>
        /// <returns></returns>
        public static (RectD ImageRect, RectD TextRect) GetItemImageRect(
            RectD rect, SizeD? imageSize = null)
        {
            Thickness textMargin = Thickness.Empty;

            var offset = ComboBox.DefaultImageVerticalOffset;

            var size = rect.Height - textMargin.Vertical - (offset * 2);

            if (imageSize is null || imageSize.Value.Height > size)
                imageSize = (size, size);

            PointD imageLocation = (
                rect.X + textMargin.Left,
                rect.Y + textMargin.Top + offset);

            var imageRect = new RectD(imageLocation, imageSize.Value);
            var centeredImageRect = imageRect.CenterIn(rect, false, true);

            var itemRect = rect;
            itemRect.X += centeredImageRect.Width + ComboBox.DefaultImageTextDistance;
            itemRect.Width -= centeredImageRect.Width + ComboBox.DefaultImageTextDistance;

            return (centeredImageRect, itemRect);
        }

        /// <summary>
        /// Gets <see cref="UI.CheckState"/> of the item using <see cref="GetAllowThreeState"/>
        /// and <see cref="ListControlItem.CheckState"/>.
        /// </summary>
        public virtual CheckState GetCheckState(IListControlItemContainer? container)
        {
            var allowThreeState = GetAllowThreeState(container);
            var checkState = CheckState;
            if (!allowThreeState && checkState == CheckState.Indeterminate)
                checkState = CheckState.Unchecked;
            return checkState;
        }

        /// <summary>
        /// Gets whether three state checkbox is allowed in the item.
        /// </summary>
        public virtual bool GetAllowThreeState(IListControlItemContainer? container)
        {
            bool allowThreeState = container?.Defaults.CheckBoxThreeState ?? false;
            if (allowThreeState && CheckBoxThreeState is not null)
                allowThreeState = CheckBoxThreeState.Value;
            return allowThreeState;
        }

        /// <summary>
        /// Gets item text color when item is inside the spedified container.
        /// </summary>
        public virtual Color GetTextColor(IListControlItemContainer? container)
        {
            return GetItemTextColor(this, container);
        }

        /// <summary>
        /// Gets whether checkbox is shown in the item.
        /// </summary>
        public virtual bool GetShowCheckBox(IListControlItemContainer? container)
        {
            var showCheckBox = container?.Defaults.CheckBoxVisible ?? true;
            if (showCheckBox)
            {
                if (CheckBoxVisible is not null)
                    showCheckBox = CheckBoxVisible.Value;
            }

            return showCheckBox;
        }

        /// <summary>
        /// Gets font when item is inside the specified container. Result must not be <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public virtual Font GetFont(IListControlItemContainer? container)
        {
            var result = Font;
            if (result is null)
            {
                if (container is Control control)
                {
                    result = control.Font ?? UI.AbstractControl.DefaultFont;
                    if (control.IsBold)
                        result = result.AsBold;
                }
                else
                    result = UI.AbstractControl.DefaultFont;
            }

            if (FontStyle is not null)
                result = result.WithStyle(FontStyle.Value);

            return result;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return Text ?? base.ToString();
        }

        internal virtual ItemCheckBoxInfo? GetCheckBoxInfo(
            IListControlItemContainer? container,
            RectD rect)
        {
            var showCheckBox = GetShowCheckBox(container);
            if (!showCheckBox)
                return null;
            ListControlItem.ItemCheckBoxInfo result = new();
            result.PartState = IsContainerEnabled(container)
                ? VisualControlState.Normal : VisualControlState.Disabled;
            result.CheckState = GetCheckState(container);
            result.CheckSize = DrawingUtils.GetCheckBoxSize(
                container as Control ?? App.SafeWindow ?? ControlUtils.Empty,
                result.CheckState,
                result.PartState);
            var (checkRect, textRect) = ListControlItem.GetItemImageRect(rect, result.CheckSize);
            result.CheckRect = checkRect;
            result.TextRect = textRect;
            return result;
        }

        internal class ItemCheckBoxInfo
        {
            public VisualControlState PartState;
            public SizeD CheckSize;
            public RectD CheckRect;
            public RectD TextRect;
            public CheckState CheckState;
        }
    }
}