﻿using System;
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
        public static readonly HVAlignment DefaultItemAlignment
            = new(HorizontalAlignment.Left, VerticalAlignment.Center);

        /// <summary>
        /// Gets or sets whether to draw debug corners around item elements (image, text, etc.).
        /// </summary>
        public static bool DrawDebugCornersOnElements = false;

        private CachedSvgImage cachedSvg = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItem"/> class
        /// with the default value for the <see cref="Text"/> property.
        /// </summary>
        /// <param name="text">The initial value of the <see cref="Text"/> property.</param>
        public ListControlItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItem"/> class
        /// with the default values for the <see cref="Text"/> and <see cref="Value"/> properties.
        /// </summary>
        /// <param name="text">The defult value of the <see cref="Text"/> property.</param>
        /// <param name="value">User data.</param>
        public ListControlItem(string text, object? value)
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
        /// with the default value for the <see cref="Text"/> property and an action associated
        /// with the item.
        /// </summary>
        /// <param name="text">The defult value of the <see cref="Text"/> property.</param>
        /// <param name="action">Action associated with the item.</param>
        public ListControlItem(string text, Action? action)
        {
            Text = text;
            Action = action;
        }

        /// <summary>
        /// Gets or sets text for the display purposes.
        /// </summary>
        public virtual string? DisplayText { get; set; }

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
        public virtual Image? Image
        {
            get => GetImage(VisualControlState.Normal);
            set => SetImage(VisualControlState.Normal, value);
        }

        /// <summary>
        /// Gets or sets disabled <see cref="Image"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Image? DisabledImage
        {
            get => GetImage(VisualControlState.Disabled);
            set => SetImage(VisualControlState.Disabled, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Image"/> associated with the item when it is selected.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Image? SelectedImage
        {
            get => GetImage(VisualControlState.Selected);
            set => SetImage(VisualControlState.Selected, value);
        }

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
            get => cachedSvg.SvgImage;
            set
            {
                cachedSvg.SvgImage = value;
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
        public virtual SizeI? SvgImageSize
        {
            get => cachedSvg.SvgSize;
            set => cachedSvg.SvgSize = value;
        }

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
        public virtual HVAlignment Alignment { get; set; } = DefaultItemAlignment;

        /// <summary>
        /// Gets or sets draw label flags.
        /// </summary>
        public virtual DrawLabelFlags LabelFlags { get; set; }

        /// <summary>
        /// Gets or sets text which is displayed when item is painted.
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets user data. This is different from <see cref="BaseObjectWithAttr.Tag"/>.
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

            var control = container?.Control;

            if (control is not null)
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
        public static EnumArrayStateImages GetItemImages(
            ListControlItem? item,
            IListControlItemContainer? container,
            Color? svgColor)
        {
            if (item is null)
                return new();

            var svgImage = item.SvgImage;
            var isDark = IsContainerDark(container);

            if (svgImage is not null)
            {
                var imageSize = item.SvgImageSize ?? container?.Defaults.SvgImageSize
                    ?? ToolBarUtils.GetDefaultImageSize(container?.Control);
                var imageHeight = imageSize.Height;

                /* ============================= */

                if(!item.HasImage(VisualControlState.Normal, isDark))
                {
                    item.SetImage(
                        VisualControlState.Normal,
                        svgImage.AsNormalImage(imageHeight, isDark),
                        isDark);
                }

                if (!item.HasImage(VisualControlState.Disabled, isDark))
                {
                    item.SetImage(
                        VisualControlState.Disabled,
                        svgImage.AsDisabledImage(imageHeight, isDark),
                        isDark);
                }

                if (svgColor is not null)
                {
                    if (!item.HasImage(VisualControlState.Selected, isDark))
                    {
                        item.SetImage(
                            VisualControlState.Selected,
                            svgImage.ImageWithColor(imageHeight, svgColor),
                            isDark);
                    }
                }

                /* ============================= */
            }

            EnumArrayStateImages result = new();

            Image? image = null;
            Image? disabledImage = null;
            Image? selectedImage = null;

            SetResult(isDark);

            if (isDark)
                SetResult(false);

            void SetResult(bool isDark)
            {
                image ??= item.GetImage(VisualControlState.Normal, isDark);
                disabledImage ??= item.GetImage(VisualControlState.Disabled, isDark);
                selectedImage ??= item.GetImage(VisualControlState.Selected, isDark);
            }

            disabledImage ??= image;
            selectedImage ??= image;

            result[VisualControlState.Normal] = image;
            result[VisualControlState.Disabled] = disabledImage;
            result[VisualControlState.Selected] = selectedImage;

            return result;
        }

        /// <summary>
        /// Default method which measures item size.
        /// </summary>
        /// <param name="container">Item container.</param>
        /// <param name="dc">Graphics context used to measure item's text.</param>
        /// <param name="itemIndex">Index of the item.</param>
        public static SizeD DefaultMeasureItemSize(
            IListControlItemContainer container,
            Graphics dc,
            int itemIndex)
        {
            var item = container.SafeItem(itemIndex);

            var s = container.GetItemText(itemIndex, true);
            if (string.IsNullOrEmpty(s))
                s = "Wy";

            var itemImages = ListControlItem.GetItemImages(item, container, null);

            var normal = itemImages[VisualControlState.Normal];
            var disabled = itemImages[VisualControlState.Disabled];
            var selected = itemImages[VisualControlState.Selected];

            var maxHeightI =
                MathUtils.Max(normal?.Size.Height, disabled?.Size.Height, selected?.Size.Height);
            var maxHeightD = GraphicsFactory.PixelToDip(maxHeightI, dc.ScaleFactor);

            var font = ListControlItem.GetFont(item, container).AsBold;
            var size = dc.GetTextExtent(s, font);
            size.Height = Math.Max(size.Height, maxHeightD);

            var itemMargin = container.Defaults.ItemMargin;

            size.Width += itemMargin.Horizontal;
            size.Height += itemMargin.Vertical;

            var minItemHeight = ListControlItem.GetMinHeight(item, container);

            size.Height = Math.Max(size.Height, minItemHeight);
            return size;
        }

        /// <summary>
        /// Gets item minimal height.
        /// </summary>
        public static Coord GetMinHeight(ListControlItem? item, IListControlItemContainer? container)
        {
            var containerMinHeight
                = container?.Defaults.MinItemHeight ?? VirtualListBox.DefaultMinItemHeight;
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
        /// Gets whether container is using dark color theme.
        /// </summary>
        public static bool IsContainerDark(IListControlItemContainer? container)
        {
            if(LightDarkColor.IsDarkOverride is null)
            {
                var control = container?.Control;

                if (control is not null)
                    return control.IsDarkBackground;
                else
                    return SystemSettings.AppearanceIsDark;
            }

            return LightDarkColor.IsDarkOverride.Value;
        }

        /// <summary>
        /// Gets foreground color of the container.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Color? GetContainerForegroundColor(IListControlItemContainer? container)
        {
            var control = container?.Control;
            if (control is not null)
                return control.ForegroundColor;
            return null;
        }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        /// <returns></returns>
        public static Color? GetDisabledTextColor(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            return container?.Defaults.DisabledItemTextColor
                ?? VirtualListBox.DefaultDisabledItemTextColor;
        }

        /// <summary>
        /// Gets item text color when item is inside the spedified container.
        /// </summary>
        public static Color? GetItemTextColor(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            if (IsContainerEnabled(container))
            {
                Color? itemColor
                    = item?.ForegroundColor ?? GetContainerForegroundColor(container)
                    ?? container?.Defaults.ItemTextColor
                    ?? VirtualListBox.DefaultItemTextColor;
                return itemColor;
            }
            else
                return GetDisabledTextColor(item, container);
        }

        /// <summary>
        /// Gets item alignment.
        /// </summary>
        public static HVAlignment GetAlignment(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            if (item is null)
            {
                return container?.Defaults.ItemAlignment
                    ?? new HVAlignment(HorizontalAlignment.Left, VerticalAlignment.Center);
            }
            else
                return item.Alignment;
        }

        /// <summary>
        /// Gets selected item back color.
        /// </summary>
        /// <returns></returns>
        public static Color? GetSelectedItemBackColor(
            ListControlItem? item,
            IListControlItemContainer? container)
        {
            var control = container?.Control;
            if (control is null)
                return VirtualListBox.DefaultSelectedItemBackColor;

            var defaults = container!.Defaults;

            if (control.Enabled && defaults.SelectionVisible)
            {
                return defaults.SelectedItemBackColor
                    ?? VirtualListBox.DefaultSelectedItemBackColor;
            }
            else
            {
                return control.RealBackgroundColor;
            }
        }

        /// <summary>
        /// Draws default background for the item.
        /// </summary>
        public static void DefaultDrawBackground(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e)
        {
            var item = e.Item;

            var control = container?.Control;
            var focused = control?.Focused ?? false;
            var selectionUnderImage = container?.Defaults.SelectionUnderImage ?? true;
            var rect = e.ClipRectangle;
            var dc = e.Graphics;

            var isSelected = e.IsSelected;
            var isCurrent = e.IsCurrent;

            var hideSelection = item?.HideSelection ?? false;
            var hideFocusRect = item?.HideFocusRect ?? false;

            if (IsContainerEnabled(container))
            {
                dc.FillBorderRectangle(
                    rect,
                    item?.BackgroundColor?.AsBrush,
                    item?.Border,
                    true,
                    control);

                var selectionVisible = container?.Defaults.SelectionVisible ?? true;

                if (isSelected && selectionVisible)
                {
                    if (!hideSelection)
                    {
                        var selectionBorder = container?.Defaults.SelectionBorder;

                        if (!selectionUnderImage)
                        {
                            DefaultDrawForeground(container, e, out var drawParams, false);
                            var rects = drawParams.ResultRects;
                            if (rects is not null && rects.Length > 1)
                            {
                                var distance = (drawParams.ImageLabelDistance ?? 0) / 2;
                                var imageRect = rects[1];
                                var delta = imageRect.Left - rect.Left - distance;
                                rect = rect.DeflatedWithPadding((delta, 0, 0, 0));
                            }
                        }

                        dc.FillBorderRectangle(
                            rect,
                            GetSelectedItemBackColor(item, container)?.AsBrush,
                            selectionBorder,
                            true,
                            control);
                    }
                }

                var currentItemBorderVisible = container?.Defaults.CurrentItemBorderVisible ?? true;

                if (isCurrent && focused && currentItemBorderVisible && !hideFocusRect)
                {
                    var border = container?.Defaults.CurrentItemBorder
                        ?? VirtualListBox.DefaultCurrentItemBorder;
                    DrawingUtils.DrawBorder(control, e.Graphics, rect, border);
                }
            }
            else
            {
                var border = item?.Border?.ToGrayScale();
                DrawingUtils.DrawBorder(control, e.Graphics, rect, border);
            }
        }

        /// <summary>
        /// Default method which draws item foreground.
        /// </summary>
        public static void DefaultDrawForeground(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e,
            out Graphics.DrawLabelParams drawParams,
            bool visible = true)
        {
            var item = e.Item;

            if (item is not null)
            {
                var info = item.GetCheckBoxInfo(container, e.ClipRectangle);
                if (info is not null)
                {
                    e.Graphics.DrawCheckBox(
                        ControlUtils.SafeControl(container),
                        info.CheckRect,
                        info.CheckState,
                        info.PartState);
                    e.ClipRectangle = info.TextRect;
                }
            }

            var isSelected = e.IsSelected;
            var hideSelection = item?.HideSelection ?? false;
            if (hideSelection)
                isSelected = false;

            var itemImages = e.ItemImages;
            var normalImage = itemImages[VisualControlState.Normal];
            var disabledImage = itemImages[VisualControlState.Disabled];
            var selectedImage = itemImages[VisualControlState.Selected];

            var image = IsContainerEnabled(container)
                ? (isSelected ? selectedImage : normalImage) : disabledImage;

            var textVisible = container?.Defaults.TextVisible ?? true;

            var s = textVisible ? e.ItemTextForDisplay.Trim() : string.Empty;

            var itemColor = e.GetTextColor(isSelected) ?? SystemColors.WindowText;

            Graphics.DrawLabelParams prm = new(
                s,
                e.ItemFont,
                itemColor,
                Color.Empty,
                image,
                e.ClipRectangle,
                e.ItemAlignment);

            prm.Visible = visible;

            if(item is not null)
            {
                prm.Flags = item.LabelFlags;
            }

            if (DebugUtils.IsDebugDefined)
            {
                prm.DrawDebugCorners = DrawDebugCornersOnElements;
            }

            e.Graphics.DrawLabel(ref prm);
            drawParams = prm;
        }

        /// <summary>
        /// Gets selected item text color when item is inside the spedified container.
        /// </summary>
        /// <returns></returns>
        public static Color? GetSelectedTextColor(
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

            var offsetWidth = centeredImageRect.Width + ComboBox.DefaultImageTextDistance;

            itemRect.X += offsetWidth;
            itemRect.Width -= offsetWidth;

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
        public virtual Color? GetTextColor(IListControlItemContainer? container)
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
        /// Draws item background;
        /// </summary>
        public virtual void DrawBackground(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e)
        {
            DefaultDrawBackground(container, e);
        }

        /// <summary>
        /// Draws item foreground.
        /// </summary>
        public virtual void DrawForeground(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e,
            out Graphics.DrawLabelParams drawParams,
            bool visible = true)
        {
            ListControlItem.DefaultDrawForeground(container, e, out drawParams, visible);
        }

        /// <summary>
        /// Gets image for the specified item state and light/dark theme flag.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag</param>
        /// <returns></returns>
        public virtual Image? GetImage(VisualControlState state, bool? isDark = null)
        {
            isDark ??= LightDarkColor.IsUsingDarkColor;
            var result = cachedSvg.GetImage(state, isDark.Value);
            return result;
        }

        /// <summary>
        /// Gets whether item has image for the specified item state and light/dark theme flag.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag</param>
        /// <returns></returns>
        public virtual bool HasImage(VisualControlState state, bool? isDark = null)
        {
            return GetImage(state, isDark) != null;
        }

        /// <summary>
        /// Sets image for the specified color theme light/dark theme flag.
        /// </summary>
        /// <param name="state">Visual state (normal, disabled, selected)
        /// for which image is set.</param>
        /// <param name="image">New image value.</param>
        /// <param name="isDark">Whether theme is dark.</param>
        public virtual void SetImage(VisualControlState state, Image? image, bool? isDark = null)
        {
            isDark ??= LightDarkColor.IsUsingDarkColor;
            cachedSvg.SetImage(state, image, isDark.Value);
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
                var control = container?.Control;

                if (control is not null)
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
                container?.Control ?? App.SafeWindow ?? ControlUtils.Empty,
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