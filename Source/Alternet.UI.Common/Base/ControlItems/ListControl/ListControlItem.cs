﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    using ILockedItem
        = IndexedValues<ObjectUniqueId, ListControlItem.ContainerRelatedData>.ILockedItem;

    /// <summary>
    /// Custom item for <see cref="ListBox"/>, <see cref="ComboBox"/> and other
    /// list controls. This class has <see cref="Text"/>,
    /// <see cref="Value"/> and other properties which allow to customize look of the item.
    /// </summary>
    public partial class ListControlItem : BaseControlItem, IComparable<ListControlItem>
    {
        /// <summary>
        /// Gets id of the null container.
        /// </summary>
        public static readonly ObjectUniqueId NullContainerId = new();

        /// <summary>
        /// Gets default item alignment.
        /// </summary>
        public static readonly HVAlignment DefaultItemAlignment
            = new(HorizontalAlignment.Left, VerticalAlignment.Center);

        /// <summary>
        /// Indicates whether images with different sizes are allowed
        /// for disabled and selected states.
        /// Default is false.
        /// </summary>
        public static bool AllowDifferentSizeForDisabledImage;

        /// <summary>
        /// Gets or sets the size of the checkbox override. Default is Null.
        /// It is used when checkbox size is determined if specified.
        /// </summary>
        public static Coord? CheckBoxSizeOverride;

        /// <summary>
        /// Gets or sets whether to draw debug corners around item elements (image, text, etc.).
        /// </summary>
        public static bool DrawDebugCornersOnElements = false;

        private CachedSvgImage<Image> cachedSvg = new();
        private string? text;
        private string? displayText;
        private HVAlignment alignment = DefaultItemAlignment;
        private IndexedValues<ObjectUniqueId, ContainerRelatedData>? containerRelated;
        private TextHorizontalAlignment? textLineAlignment;
        private Coord? textLineDistance;
        private Coord minHeight;
        private int? imageIndex;
        private CheckState checkState;
        private FontStyle? fontStyle;
        private Font? font;

        private bool? checkBoxThreeState;
        private bool? checkBoxAllowAllStatesForUser;
        private bool? checkBoxVisible;

        private bool canRemove = true;
        private bool hideSelection;
        private bool hideFocusRect;
        private bool isRadioButton;

        private Color? foregroundColor;
        private Color? backgroundColor;
        private BorderSettings? border;
        private Thickness foregroundMargin;
        private DrawLabelFlags labelFlags;
        private object? value;
        private Action? action;
        private Action? doubleClickAction;
        private Action<IListControlItemContainer?, ListBoxItemPaintEventArgs>? drawBackgroundAction;
        private Action<IListControlItemContainer?, ListBoxItemPaintEventArgs>? drawForegroundAction;

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
        /// <param name="text">The default value of the <see cref="Text"/> property.</param>
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItem"/> class
        /// with the default value for the <see cref="Text"/> property and an action associated
        /// with the item.
        /// </summary>
        /// <param name="text">The default value of the <see cref="Text"/> property.</param>
        /// <param name="action">Action associated with the item.</param>
        public ListControlItem(string text, Action? action)
        {
            Text = text;
            Action = action;
        }

        /// <summary>
        /// Gets or sets horizontal alignment of the text line within text block.
        /// </summary>
        public virtual TextHorizontalAlignment? TextLineAlignment
        {
            get => textLineAlignment;
            set => textLineAlignment = value;
        }

        /// <summary>
        /// Gets or sets group id of the item.
        /// </summary>
        public virtual ObjectUniqueId? Group { get; set; }

        /// <summary>
        /// Gets or sets distance between lines of the text.
        /// </summary>
        public virtual Coord? TextLineDistance
        {
            get => textLineDistance;
            set => textLineDistance = value;
        }

        /// <summary>
        /// Gets whether <see cref="Image"/> or <see cref="SvgImage"/> is assigned.
        /// </summary>
        [Browsable(false)]
        public bool HasImageOrSvg => Image is not null || SvgImage is not null || HasValidImageIndex;

        /// <summary>
        /// Gets or sets text for the display purposes.
        /// </summary>
        public virtual string? DisplayText
        {
            get => displayText;
            set => displayText = value;
        }

        /// <summary>
        /// Gets a value indicating whether the item has a valid image index assigned.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasValidImageIndex
        {
            get
            {
                return ImageIndex is not null && ImageIndex >= 0;
            }
        }

        /// <summary>
        /// Gets or sets image index used when item is painted.
        /// </summary>
        /// <remarks>
        /// Currently this property is not used by the default item painter.
        /// </remarks>
        public virtual int? ImageIndex
        {
            get => imageIndex;
            set => imageIndex = value;
        }

        /// <summary>
        /// Gets or sets state of the check box associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual CheckState CheckState
        {
            get => checkState;
            set
            {
                checkState = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// Uses <see cref="CheckState"/> internally.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                if (CheckState == CheckState.Checked)
                    return true;
                return false;
            }

            set
            {
                if (value)
                    CheckState = CheckState.Checked;
                else
                    CheckState = CheckState.Unchecked;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the font style of the item is bold.
        /// This property is based on the <see cref="FontStyle"/> value.
        /// </summary>
        /// <value>
        /// <c>true</c> if the font style is bold; otherwise, <c>false</c>.
        /// </value>
        public bool IsBold
        {
            get
            {
                return FontStyle?.HasFlag(Alternet.Drawing.FontStyle.Bold) ?? false;
            }

            set
            {
                if (value == IsBold)
                    return;
                FontStyle = Drawing.Font.ChangeFontStyle(FontStyle ?? 0, Drawing.FontStyle.Bold, value);
            }
        }

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
        public virtual bool? CheckBoxThreeState
        {
            get => checkBoxThreeState;
            set => checkBoxThreeState = value;
        }

        /// <summary>
        /// Gets or sets whether text may contain html bold tags.
        /// </summary>
        public bool TextHasBold
        {
            get
            {
                return LabelFlags.HasFlag(DrawLabelFlags.TextHasBold);
            }

            set
            {
                SetLabelFlag(DrawLabelFlags.TextHasBold, value);
            }
        }

        /// <summary>
        /// Gets or sets whether user can set the checkbox to
        /// the third state by clicking. If property is null (default),
        /// control's setting is used.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// </remarks>
        [DefaultValue(false)]
        public virtual bool? CheckBoxAllowAllStatesForUser
        {
            get => checkBoxAllowAllStatesForUser;
            set => checkBoxAllowAllStatesForUser = value;
        }

        /// <summary>
        /// Gets or sets whether to show check box inside the item. This property (if specified)
        /// overrides global checkboxes visibility setting in the control.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual bool? CheckBoxVisible
        {
            get => checkBoxVisible;
            set => checkBoxVisible = value;
        }

        /// <summary>
        /// Gets or sets whether to paint check box as radio button.
        /// </summary>
        public virtual bool IsRadioButton
        {
            get
            {
                return isRadioButton;
            }

            set
            {
                isRadioButton = value;
            }
        }

        /// <summary>
        /// Gets or sets whether item can be removed.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// </remarks>
        public virtual bool CanRemove
        {
            get => canRemove;
            set => canRemove = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Image"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual Image? Image
        {
            get
            {
                return GetImage(VisualControlState.Normal);
            }

            set
            {
                SetImage(VisualControlState.Normal, value);
            }
        }

        /// <summary>
        /// Gets or sets disabled <see cref="Image"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual Image? DisabledImage
        {
            get
            {
                return GetImage(VisualControlState.Disabled);
            }

            set
            {
                SetImage(VisualControlState.Disabled, value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Image"/> associated with the item when it is selected.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual Image? SelectedImage
        {
            get
            {
                return GetImage(VisualControlState.Selected);
            }

            set
            {
                SetImage(VisualControlState.Selected, value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="SvgImage"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
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
        /// Gets or sets width of the svg image.
        /// </summary>
        public virtual int? SvgImageWidth
        {
            get => SvgImageSize?.Width;

            set
            {
                if(value is null)
                {
                    SvgImageSize = null;
                }
                else
                {
                    if (SvgImageSize is null)
                        SvgImageSize = value;
                    else
                    {
                        SvgImageSize = SvgImageSize.Value.WithWidth(value.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets height of the svg image.
        /// </summary>
        public virtual int? SvgImageHeight
        {
            get => SvgImageSize?.Height;

            set
            {
                if (value is null)
                {
                    SvgImageSize = null;
                }
                else
                {
                    if (SvgImageSize is null)
                        SvgImageSize = value;
                    else
                    {
                        SvgImageSize = SvgImageSize.Value.WithHeight(value.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets minimal item height.
        /// </summary>
        public virtual Coord MinHeight
        {
            get => minHeight;
            set => minHeight = value;
        }

        /// <summary>
        /// Gets or sets <see cref="FontStyle"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual FontStyle? FontStyle
        {
            get => fontStyle;
            set => fontStyle = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Font"/> associated with the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual Font? Font
        {
            get => font;
            set => font = value;
        }

        /// <summary>
        /// Gets or sets whether to hide selection for this item.
        /// </summary>
        public virtual bool HideSelection
        {
            get => hideSelection;
            set => hideSelection = value;
        }

        /// <summary>
        /// Gets or sets whether to hide focus rectangle for this item.
        /// </summary>
        public virtual bool HideFocusRect
        {
            get => hideFocusRect;
            set => hideFocusRect = value;
        }

        /// <summary>
        /// Gets or sets foreground color of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual Color? ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Gets or sets background color of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        public virtual Color? BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Gets or sets border of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual BorderSettings? Border
        {
            get => border;
            set => border = value;
        }

        /// <summary>
        /// Gets or sets alignment of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual HVAlignment Alignment
        {
            get => alignment;
            set => alignment = value;
        }

        /// <summary>
        /// Gets or sets horizontal alignment of the item.
        /// </summary>
        /// <remarks>
        /// This property just changes horizontal part of the <see cref="Alignment"/>.
        /// </remarks>
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return alignment.Horizontal;
            }

            set
            {
                alignment = alignment.WithHorizontal(value);
            }
        }

        /// <summary>
        /// Gets or sets vertical alignment of the item.
        /// </summary>
        /// <remarks>
        /// This property just changes vertical part of the <see cref="Alignment"/>.
        /// </remarks>
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return alignment.Vertical;
            }

            set
            {
                alignment = alignment.WithVertical(value);
            }
        }

        /// <summary>
        /// Gets or sets margin of the item when foreground is painted.
        /// </summary>
        [Browsable(false)]
        public virtual Thickness ForegroundMargin
        {
            get => foregroundMargin;
            set => foregroundMargin = value;
        }

        /// <summary>
        /// Gets or sets left margin of the item when foreground is painted.
        /// </summary>
        [Browsable(false)]
        public Coord ForegroundMarginLeft
        {
            get => ForegroundMargin.Left;

            set
            {
                ForegroundMargin = ForegroundMargin.WithLeft(value);
            }
        }

        /// <summary>
        /// Gets or sets draw label flags.
        /// </summary>
        public virtual DrawLabelFlags LabelFlags
        {
            get => labelFlags;
            set => labelFlags = value;
        }

        /// <summary>
        /// Gets or sets text which is displayed when item is painted.
        /// </summary>
        public virtual string Text
        {
            get
            {
                if (string.IsNullOrEmpty(text))
                    return Value?.ToString() ?? string.Empty;

                return text ?? string.Empty;
            }

            set => text = value;
        }

        /// <summary>
        /// Gets or sets user data. This is different from <see cref="BaseObjectWithAttr.Tag"/>.
        /// </summary>
        public virtual object? Value
        {
            get => value;
            set => this.value = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> associated with this
        /// <see cref="ListControlItem"/> instance.
        /// </summary>
        [Browsable(false)]
        public virtual Action? Action
        {
            get => action;
            set => action = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which is executed on mouse double click.
        /// </summary>
        [Browsable(false)]
        public virtual Action? DoubleClickAction
        {
            get => doubleClickAction;
            set => doubleClickAction = value;
        }

        /// <summary>
        /// Gets or sets an action which is called when background is painted for the item.
        /// When this action is specified, default background painting is not performed.
        /// </summary>
        [Browsable(false)]
        public virtual Action<IListControlItemContainer?, ListBoxItemPaintEventArgs>?
            DrawBackgroundAction
        {
            get => drawBackgroundAction;
            set => drawBackgroundAction = value;
        }

        /// <summary>
        /// Gets or sets an action which is called when foreground is painted for the item.
        /// When this action is specified, default foreground painting is not performed.
        /// </summary>
        [Browsable(false)]
        public virtual Action<IListControlItemContainer?, ListBoxItemPaintEventArgs>?
            DrawForegroundAction
        {
            get => drawForegroundAction;
            set => drawForegroundAction = value;
        }

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
        /// <param name="onlyNormal">Specifies whether to get image only for the normal state.</param>
        /// <returns></returns>
        public static EnumArrayStateImages GetItemImages(
            ListControlItem? item,
            IListControlItemContainer? container,
            Color? svgColor,
            bool onlyNormal = false)
        {
            EnumArrayStateImages result = new();

            if (item is null)
                return result;

            if (item.HasValidImageIndex && container is not null)
            {
                var img = container.ImageList?.GetImage(item.ImageIndex);
                result[VisualControlState.Normal] = img;

                if (!onlyNormal)
                {
                    result[VisualControlState.Selected] = img;
                    result[VisualControlState.Disabled] = img?.ToGrayScaleCached();
                }

                return result;
            }

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

                if (!onlyNormal)
                {
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
                }

                /* ============================= */
            }

            Image? image = null;
            Image? disabledImage = null;
            Image? selectedImage = null;

            SetResult(isDark);

            if (isDark)
                SetResult(false);

            void SetResult(bool isDark)
            {
                image ??= item.GetImage(VisualControlState.Normal, isDark);

                if (!onlyNormal)
                {
                    disabledImage ??= item.GetImage(VisualControlState.Disabled, isDark);
                    selectedImage ??= item.GetImage(VisualControlState.Selected, isDark);
                }
            }

            result[VisualControlState.Normal] = image;

            if (!onlyNormal)
            {
                disabledImage ??= image;
                selectedImage ??= image;
                result[VisualControlState.Disabled] = disabledImage;
                result[VisualControlState.Selected] = selectedImage;
            }

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

            var itemImages = ListControlItem.GetItemImages(
                item,
                container,
                null,
                !AllowDifferentSizeForDisabledImage);

            var normal = itemImages[VisualControlState.Normal];
            var maxHeightI = normal?.Size.Height ?? 0;

            if (AllowDifferentSizeForDisabledImage)
            {
                var disabled = itemImages[VisualControlState.Disabled];
                var selected = itemImages[VisualControlState.Selected];
                maxHeightI = MathUtils.Max(maxHeightI, disabled?.Size.Height, selected?.Size.Height);
            }

            var maxHeightD = GraphicsFactory.PixelToDip(maxHeightI, dc.ScaleFactor);

            var checkBoxSize = GetCheckBoxSize(container).Height + GetAdditionalImageMargin()
                + GetAdditionalTextMargin().Vertical;

            maxHeightD = Math.Max(checkBoxSize, maxHeightD);

            var font = ListControlItem.GetFont(item, container, true).AsBold;

            var hasNewLineChars = item?.LabelFlags.HasFlag(DrawLabelFlags.TextHasNewLineChars) ?? false;

            SizeD size;

            if (hasNewLineChars && StringUtils.ContainsNewLineChars(s))
            {
                var splitText = StringUtils.Split(s, false);
                size = dc.DrawStrings(
                    RectD.Empty,
                    font,
                    splitText,
                    item?.TextLineAlignment ?? TextHorizontalAlignment.Left,
                    item?.TextLineDistance ?? 0);
            }
            else
            {
                size = dc.GetTextExtent(s, font);
            }

            size.Height = Math.Max(size.Height, maxHeightD);

            var itemMargin = container.Defaults.ItemMargin;

            size.Width += itemMargin.Horizontal;
            size.Height += itemMargin.Vertical;

            var minItemHeight = ListControlItem.GetMinHeight(item, container);

            size.Height = MathUtils.RoundUpAndIncrementIfOdd(Math.Max(size.Height, minItemHeight));
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

            var itemMinHeight = item.MinHeight;

            return Math.Max(itemMinHeight, containerMinHeight);
        }

        /// <summary>
        /// Gets font when item is inside the specified container. Result must not be <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public static Font GetFont(
            ListControlItem? item,
            IListControlItemContainer? container,
            bool isSelectedItem)
        {
            Font result;

            if (item is null)
            {
                result = GetContainerFont(container);
            }
            else
            {
                result = item.GetFont(container);
            }

            if (isSelectedItem && (container?.Defaults.SelectedItemIsBold ?? false))
            {
                result = result.AsBold;
            }

            return result;
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
        /// Gets item text color when item is inside the container.
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
                if (container.Focused)
                {
                    return defaults.SelectedItemBackColor
                        ?? VirtualListBox.DefaultSelectedItemBackColor;
                }
                else
                {
                    return defaults.UnfocusedSelectedItemBackColor
                        ?? VirtualListBox.DefaultUnfocusedSelectedItemBackColor;
                }
            }
            else
            {
                return control.RealBackgroundColor;
            }
        }

        /// <summary>
        /// Adds a range of values to the specified collection of items.
        /// For each value, a new <see cref="ListControlItem"/> is created
        /// and added to the collection.
        /// </summary>
        /// <param name="items">The collection of list control items to which
        /// values will be added.</param>
        /// <param name="values">The values to add to the collection.</param>
        public static void AddRangeOfValues(
            BaseCollection<ListControlItem>? items,
            IEnumerable? values)
        {
            if (items is null)
                return;
            if (values is null)
                return;
            foreach (var value in values)
            {
                var item = new ListControlItem();
                item.Value = value;
                items.Add(item);
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
                            CalcForegroundMetrics(container, e);
                            var drawParams = e.LabelMetrics;
                            var rectangles = drawParams.ResultRects;
                            if (rectangles is not null && rectangles.Length > 1)
                            {
                                var distance = (drawParams.ImageLabelDistance ?? 0) / 2;
                                var imageRect = rectangles[1];
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

            DefaultDrawBackgroundDebug(container, e);
        }

        /// <summary>
        /// Default method which calls <see cref="DefaultDrawForeground"/> for the
        /// <see cref="ListBoxItemPaintEventArgs.LabelMetrics"/> calculation.
        /// </summary>
        public static void CalcForegroundMetrics(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e)
        {
            var savedVisible = e.Visible;
            e.Visible = false;
            DefaultDrawForeground(container, e);
            e.Visible = savedVisible;
        }

        /// <summary>
        /// Gets the size of the checkbox.
        /// </summary>
        /// <param name="container">The container of the item.</param>
        /// <param name="checkState">The state of the checkbox.</param>
        /// <param name="partState">The visual state of the checkbox.</param>
        /// <returns>The size of the checkbox.</returns>
        public static SizeD GetCheckBoxSize(
            IListControlItemContainer? container,
            CheckState? checkState = null,
            VisualControlState? partState = null)
        {
            if (CheckBoxSizeOverride is not null)
                return CheckBoxSizeOverride.Value;

            var result = DrawingUtils.GetCheckBoxSize(
                            container?.Control ?? App.SafeWindow ?? ControlUtils.Empty,
                            checkState ?? CheckState.Unchecked,
                            partState ?? VisualControlState.Normal);
            return result;
        }

        /// <summary>
        /// Draws the default checkbox for the item.
        /// </summary>
        /// <param name="canvas">The graphics context used to draw the checkbox.</param>
        /// <param name="control">The control associated with the checkbox.</param>
        /// <param name="info">Information about the checkbox to be drawn.</param>
        public static void DefaultDrawCheckBox(
            Graphics canvas,
            AbstractControl control,
            ItemCheckBoxInfo info)
        {
            var useSvg = info.ImageChecked is not null && info.ImageUnchecked is not null;

            if (useSvg)
            {
                SvgImage? svgImage = null;

                switch (info.CheckState)
                {
                    case CheckState.Unchecked:
                        svgImage = info.ImageUnchecked;
                        break;
                    case CheckState.Checked:
                        svgImage = info.ImageChecked;
                        break;
                    case CheckState.Indeterminate:
                        svgImage = info.ImageIndeterminate;
                        break;
                }

                svgImage ??= KnownSvgImages.ImgEmpty;

                var size = control.PixelFromDip(info.CheckSize.Width);

                Image? image;

                switch (info.SvgState)
                {
                    default:
                    case VisualControlState.Normal:
                        image = svgImage.AsNormalImage(size, control.IsDarkBackground);
                        break;
                    case VisualControlState.Disabled:
                        image = svgImage.AsDisabledImage(size, control.IsDarkBackground);
                        break;
                    case VisualControlState.Selected:
                        image = svgImage.ImageWithColor(size, info.SvgImageColor);
                        break;
                }

                if (image is not null)
                {
                    canvas.DrawImage(image, info.CheckRect.Location);
                    return;
                }
            }

            if (info.IsRadioButton)
            {
                canvas.DrawRadioButton(
                    control,
                    info.CheckRect,
                    info.CheckState == CheckState.Checked,
                    info.SvgState);
            }
            else
            {
                canvas.DrawCheckBox(
                    control,
                    info.CheckRect,
                    info.CheckState,
                    info.SvgState);
            }
        }

        /// <summary>
        /// Finds the index of the item with <see cref="ListControlItem.Value"/> property which is
        /// equal to the specified value.
        /// </summary>
        /// <param name="collection">The collection of <c>ListControlItem</c>.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns></returns>
        public static int? FindItemIndexWithValue(IList<ListControlItem> collection, object? value)
        {
            if (value is null)
                return null;

            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i];

                if (value.Equals(item.Value))
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Gets the members of a specific group within the collection.
        /// Uses the <see cref="ListControlItem.Group"/> property to filter items.
        /// </summary>
        /// <param name="collection">The collection of <see cref="ListControlItem"/>.</param>
        /// <param name="group">The unique identifier for the group.</param>
        /// <returns>Enumerable collection of <see cref="ListControlItem"/> belonging
        /// to the specified group.</returns>
        /// <remarks>
        /// If <paramref name="group"/> is <c>null</c>, an empty collection is returned.
        /// </remarks>
        public static IEnumerable<ListControlItem> GetMembersOfGroup(
            IEnumerable<ListControlItem> collection,
            ObjectUniqueId? group)
        {
            if (group is null)
                return Enumerable.Empty<ListControlItem>();
            return collection.Where(item => item.Group == group);
        }

        /// <summary>
        /// Gets the members of a specific group within the collection except the specified item.
        /// Uses the <see cref="ListControlItem.Group"/> property to filter items.
        /// </summary>
        /// <param name="collection">The collection of <see cref="ListControlItem"/>.</param>
        /// <param name="group">The unique identifier for the group.</param>
        /// <param name="excludeItem">The item to exclude from the results.</param>
        /// <returns>Enumerable collection of <see cref="ListControlItem"/> belonging
        /// to the specified group except for the excluded item.</returns>
        /// <remarks>
        /// If <paramref name="group"/> is <c>null</c>, an empty collection is returned.
        /// </remarks>
        public static IEnumerable<ListControlItem> GetOtherMembersOfGroup(
            IEnumerable<ListControlItem> collection,
            ObjectUniqueId? group,
            ListControlItem excludeItem)
        {
            if (group is null)
                return Enumerable.Empty<ListControlItem>();
            return collection.Where(item => item.Group == group && item != excludeItem);
        }

        /// <summary>
        /// Unchecks all items in the specified collection that belong to the same group
        /// which is defined by the <see cref="ListControlItem.Group"/> property.
        /// <paramref name="item"/> is used to determine the group and is not unchecked.
        /// </summary>
        /// <param name="collection">The collection of <see cref="ListControlItem"/>.</param>
        /// <param name="item">The item to exclude from being unchecked.</param>
        public static void UncheckOtherItemsInGroup(
            IEnumerable<ListControlItem> collection,
            ListControlItem item)
        {
            var members = ListControlItem.GetOtherMembersOfGroup(collection, item.Group, item);
            foreach (var member in members)
            {
                member.IsChecked = false;
            }
        }

        /// <summary>
        /// Default method which draws item foreground.
        /// </summary>
        public static void DefaultDrawForeground(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e)
        {
            var item = e.Item;
            Thickness padding;

            if (item is null)
            {
                padding = Thickness.Empty;
            }
            else
            {
                padding = item.ForegroundMargin;
            }

            var paintRectangle = e.ClipRectangle;
            paintRectangle.ApplyMargin(padding);

            var isSelected = e.IsSelected;
            var hideSelection = item?.HideSelection ?? false;
            if (hideSelection)
                isSelected = false;

            if (item is not null)
            {
                var info = item.GetCheckBoxInfo(container, paintRectangle);
                if (info.IsCheckBoxVisible)
                {
                    info.SvgState = IsContainerEnabled(container)
                        ? (isSelected ? VisualControlState.Selected : VisualControlState.Normal)
                        : VisualControlState.Disabled;
                    if(info.SvgState == VisualControlState.Selected)
                        info.SvgImageColor = ListControlItem.GetSelectedTextColor(item, container);
                    info.IsRadioButton = item.IsRadioButton;
                    DefaultDrawCheckBox(e.Graphics, ControlUtils.SafeControl(container), info);
                    paintRectangle = info.TextRect;
                }
                else
                {
                    if(info.KeepTextPaddingWithoutCheckBox)
                        paintRectangle = info.TextRect;
                }
            }

            var itemImages = e.ItemImages;
            var normalImage = itemImages[VisualControlState.Normal];
            var disabledImage = itemImages[VisualControlState.Disabled];
            var selectedImage = itemImages[VisualControlState.Selected];

            var image = IsContainerEnabled(container)
                ? (isSelected ? selectedImage : normalImage) : disabledImage;

            var textVisible = container?.Defaults.TextVisible ?? true;

            var s = textVisible ? e.ItemTextForDisplay : string.Empty;

            var itemColor = e.GetTextColor(isSelected) ?? SystemColors.WindowText;

            Graphics.DrawLabelParams prm = new(
                s,
                e.ItemFont,
                itemColor,
                Color.Empty,
                image,
                paintRectangle,
                e.ItemAlignment);

            prm.Visible = e.Visible;

            if(item is not null)
            {
                prm.Flags = item.LabelFlags;
                prm.TextHorizontalAlignment = item.TextLineAlignment ?? TextHorizontalAlignment.Left;
                prm.LineDistance = item.TextLineDistance ?? 0;

#if DEBUG
                prm.DebugId = item.UniqueId;
#endif
            }

#if DEBUG
            prm.DrawDebugCorners = DrawDebugCornersOnElements;

            if (DrawDebugCornersOnElements)
            {
                BorderSettings.DrawDesignCorners(e.Graphics, prm.Rect, BorderSettings.DebugBorderGreen);
            }
#endif
            e.Graphics.DrawLabel(ref prm);
            e.LabelMetrics = prm;
        }

        /// <summary>
        /// Gets selected item text color when item is inside the container.
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
                    if (container?.Focused ?? true)
                    {
                        return container?.Defaults.SelectedItemTextColor
                                    ?? VirtualListBox.DefaultSelectedItemTextColor;
                    }
                    else
                    {
                        return container?.Defaults.UnfocusedSelectedItemTextColor
                                    ?? VirtualListBox.DefaultUnfocusedSelectedItemTextColor;
                    }
                }
                else
                    return GetDisabledTextColor(item, container);
            }
            else
                return GetItemTextColor(item, container);
        }

        /// <summary>
        /// Gets an additional text margin.
        /// </summary>
        /// <returns></returns>
        public static Thickness GetAdditionalTextMargin()
        {
            return Thickness.Empty;
        }

        /// <summary>
        /// Gets an additional image margin.
        /// </summary>
        /// <returns></returns>
        public static Coord GetAdditionalImageMargin()
        {
            var result = ComboBox.DefaultImageVerticalOffset;

            return result;
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
            Thickness textMargin = GetAdditionalTextMargin();

            var imageMargin = GetAdditionalImageMargin();

            var size = rect.Height - textMargin.Vertical - (imageMargin * 2);

            if (imageSize is null || imageSize.Value.Height > size)
                imageSize = (size, size);

            PointD imageLocation = (
                rect.X + textMargin.Left,
                rect.Y + textMargin.Top + imageMargin);

            var imageRect = new RectD(imageLocation, imageSize.Value);
            var centeredImageRect = imageRect.CenterIn(rect, centerHorz: false, centerVert: true);

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
        /// Gets item text color when item is inside the container.
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
            if(DrawBackgroundAction is null)
                DefaultDrawBackground(container, e);
            else
                DrawBackgroundAction(container, e);
        }

        /// <summary>
        /// Draws item foreground.
        /// </summary>
        public virtual void DrawForeground(
            IListControlItemContainer? container,
            ListBoxItemPaintEventArgs e)
        {
            if (DrawForegroundAction is null)
            {
                DefaultDrawForeground(container, e);
            }
            else
            {
                DrawForegroundAction(container, e);
            }
        }

        /// <summary>
        /// Turns on or off <see cref="LabelFlags"/> element(s).
        /// </summary>
        /// <param name="element">Element to add or remove.</param>
        /// <param name="add"><c>true</c> to add, <c>false</c> to remove.</param>
        public void SetLabelFlag(DrawLabelFlags element, bool add)
        {
            if (add)
                LabelFlags |= element;
            else
                LabelFlags &= ~element;
        }

        /// <summary>
        /// Gets image for the specified item state and light/dark theme flag.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag</param>
        /// <returns></returns>
        public virtual Image? GetImage(VisualControlState state, bool? isDark = null)
        {
            var result = cachedSvg.GetImage(state, isDark);
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
            cachedSvg.SetImage(state, image, isDark);
        }

        /// <summary>
        /// Sets <see cref="Value"/> property.
        /// </summary>
        public void SetValue(object? value)
        {
            Value = value;
        }

        /// <summary>
        /// Copies the properties of the specified <see cref="ListControlItem"/> instance
        /// to the current instance. This method may not copy all the properties.
        /// </summary>
        /// <param name="assignFrom">The <see cref="ListControlItem"/> instance whose properties
        /// will be copied.</param>
        public virtual void Assign(ListControlItem assignFrom)
        {
            DisplayText = assignFrom.DisplayText;
            CheckState = assignFrom.CheckState;
            CheckBoxThreeState = assignFrom.CheckBoxThreeState;
            CheckBoxAllowAllStatesForUser = assignFrom.CheckBoxAllowAllStatesForUser;
            CheckBoxVisible = assignFrom.CheckBoxVisible;
            CanRemove = assignFrom.CanRemove;
            Image = assignFrom.Image;
            DisabledImage = assignFrom.DisabledImage;
            SelectedImage = assignFrom.SelectedImage;
            SvgImage = assignFrom.SvgImage;
            SvgImageSize = assignFrom.SvgImageSize;
            MinHeight = assignFrom.MinHeight;
            FontStyle = assignFrom.FontStyle;
            Font = assignFrom.Font;
            HideSelection = assignFrom.HideSelection;
            HideFocusRect = assignFrom.HideFocusRect;
            ForegroundColor = assignFrom.ForegroundColor;
            BackgroundColor = assignFrom.BackgroundColor;
            Border = assignFrom.Border;
            Alignment = assignFrom.Alignment;
            LabelFlags = assignFrom.LabelFlags;
            Text = assignFrom.Text;
            Value = assignFrom.Value;
        }

        /// <summary>
        /// Creates a copy of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ListControlItem Clone()
        {
            ListControlItem result = new();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Gets whether item is selected.
        /// </summary>
        public virtual bool IsSelected(IListControlItemContainer? container)
        {
            var item = GetContainerRelated(container);
            return item.Value.IsSelected;
        }

        /// <summary>
        /// Sets whether item is selected.
        /// </summary>
        /// <returns>True if selected state was changed; False otherwise.</returns>
        public virtual bool SetSelected(IListControlItemContainer? container, bool value)
        {
            var item = GetContainerRelated(container);
            var data = item.Value;

            if(data.IsSelected != value)
            {
                data.IsSelected = value;
                item.Value = data;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Toggles selected state of the item.
        /// </summary>
        /// <param name="container">Item container.</param>
        /// <returns>New value for the selected state.</returns>
        public virtual bool ToggleSelected(IListControlItemContainer? container)
        {
            var item = GetContainerRelated(container);
            var data = item.Value;
            var newSelected = !data.IsSelected;
            data.IsSelected = newSelected;
            item.Value = data;
            return newSelected;
        }

        /// <summary>
        /// Gets font used when item is painted. Result is not <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public virtual Font GetFont(IListControlItemContainer? container)
        {
            var result = Font ?? container?.Control?.RealFont ?? AbstractControl.DefaultFont;

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

        /// <summary>
        /// Retrieves information about the checkbox associated with the item, including
        /// its visibility, state, size, and position.
        /// </summary>
        /// <param name="container">The container that holds the item. Can be null.</param>
        /// <param name="rect">The bounding rectangle of the item.</param>
        /// <returns>An <see cref="ItemCheckBoxInfo"/> object containing
        /// details about the checkbox.</returns>
        public virtual ItemCheckBoxInfo GetCheckBoxInfo(
            IListControlItemContainer? container,
            RectD rect)
        {
            ListControlItem.ItemCheckBoxInfo result = new();
            result.IsCheckBoxVisible = GetShowCheckBox(container);
            result.PartState = IsContainerEnabled(container)
                ? VisualControlState.Normal : VisualControlState.Disabled;
            result.CheckState = GetCheckState(container);
            result.CheckSize = GetCheckBoxSize(container, result.CheckState, result.PartState);

            if(container is not null)
            {
                result.ImageChecked = container.CheckImageChecked;
                result.ImageUnchecked = container.CheckImageUnchecked;
                result.ImageIndeterminate = container.CheckImageIndeterminate;
            }

#if DEBUG
            if(Graphics.DebugElementId is not null)
            {
            }
#endif

            var (checkRect, textRect) = ListControlItem.GetItemImageRect(rect, result.CheckSize);
            result.CheckRect = checkRect;
            result.CheckSize = checkRect.Size;
            result.TextRect = textRect;
            result.Bounds = rect;
            return result;
        }

        /// <inheritdoc/>
        public virtual int CompareTo(ListControlItem other)
        {
            var thisItemText = DisplayText ?? Text ?? Value?.ToString() ?? string.Empty;
            var otherItemText = other.DisplayText ?? other.Text
                ?? other.Value?.ToString() ?? string.Empty;

            return string.Compare(thisItemText, otherItemText, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Retrieves an item containing data related to the specified container.
        /// If the container is null, a default container-related data object is returned.
        /// </summary>
        /// <param name="container">The container for which related data is retrieved.</param>
        /// <returns>An item containing container-related data.</returns>
        public virtual ILockedItem GetContainerRelated(IListControlItemContainer? container)
        {
            var id = container?.UniqueId ?? NullContainerId;
            containerRelated ??= new();
            var result = containerRelated.GetLockedItemCached(id, () => new());
            return result;
        }

        [Conditional("DEBUG")]
        private static void DefaultDrawBackgroundDebug(
                   IListControlItemContainer? container,
                   ListBoxItemPaintEventArgs e)
        {
            if (ContainerControl.ShowDebugFocusRect && e.IsCurrent)
            {
                if (Control.FocusedControl == container?.Control)
                {
                    e.Graphics.FillBorderRectangle(
                        e.ClipRectangle,
                        null,
                        BorderSettings.DebugBorder);
                }
            }
        }

        /// <summary>
        /// Represents data related to a container for a <see cref="ListControlItem"/>.
        /// </summary>
        public struct ContainerRelatedData
        {
            /// <summary>
            /// Gets or sets a value indicating whether the item is selected.
            /// </summary>
            public bool IsSelected;

            private FlagsAndAttributesStruct attr = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="ContainerRelatedData"/> struct.
            /// </summary>
            public ContainerRelatedData()
            {
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.Tag"/>
            /// </summary>
            [Browsable(false)]
            public object? Tag { get; set; }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.IntFlags"/>
            /// </summary>
            [Browsable(false)]
            public ICustomIntFlags IntFlags
            {
                get
                {
                    return IntFlagsAndAttributes;
                }
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.IntFlagsAndAttributes"/>
            /// </summary>
            [Browsable(false)]
            public IIntFlagsAndAttributes IntFlagsAndAttributes
            {
                get
                {
                    return attr.IntFlagsAndAttributes;
                }
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.FlagsAndAttributes"/>
            /// </summary>
            [Browsable(false)]
            public IFlagsAndAttributes FlagsAndAttributes
            {
                get
                {
                    return attr.FlagsAndAttributes;
                }
            }

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.CustomFlags"/>
            /// </summary>
            [Browsable(false)]
            public ICustomFlags<string> CustomFlags => FlagsAndAttributes.Flags;

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.CustomAttr"/>
            /// </summary>
            [Browsable(false)]
            public ICustomAttributes<string, object> CustomAttr => FlagsAndAttributes.Attr;

            /// <summary>
            /// <inheritdoc cref="BaseObjectWithAttr.IntAttr"/>
            /// </summary>
            [Browsable(false)]
            public ICustomAttributes<int, object> IntAttr
            {
                get
                {
                    return IntFlagsAndAttributes.Attr;
                }
            }
        }

        /// <summary>
        /// Contains information about the checkbox associated with a <see cref="ListControlItem"/>.
        /// </summary>
        public class ItemCheckBoxInfo : BaseObject
        {
            /// <summary>
            /// Gets or sets the rectangle that defines the bounds of the item.
            /// </summary>
            public RectD Bounds;

            /// <summary>
            /// Gets or sets the visual state of the checkbox.
            /// </summary>
            public VisualControlState PartState;

            /// <summary>
            /// Gets or sets the size of the checkbox.
            /// </summary>
            public SizeD CheckSize;

            /// <summary>
            /// Gets or sets the rectangle that defines the position and size of the checkbox.
            /// </summary>
            public RectD CheckRect;

            /// <summary>
            /// Gets or sets the rectangle that defines the position and size of the text.
            /// </summary>
            public RectD TextRect;

            /// <summary>
            /// Gets or sets the state of the checkbox.
            /// </summary>
            public CheckState CheckState;

            /// <summary>
            /// Gets or sets the SVG image used when the checkbox is unchecked.
            /// </summary>
            public SvgImage? ImageUnchecked;

            /// <summary>
            /// Gets or sets the SVG image used when the checkbox is checked.
            /// </summary>
            public SvgImage? ImageChecked;

            /// <summary>
            /// Gets or sets the SVG image used when the checkbox is in an indeterminate state.
            /// </summary>
            public SvgImage? ImageIndeterminate;

            /// <summary>
            /// Gets or sets the color of the SVG image.
            /// </summary>
            public Color? SvgImageColor;

            /// <summary>
            /// Gets or sets the visual state of the SVG image.
            /// </summary>
            public VisualControlState SvgState = VisualControlState.Normal;

            /// <summary>
            /// Gets or sets a value indicating whether the checkbox is visible.
            /// </summary>
            public bool IsCheckBoxVisible;

            /// <summary>
            /// Gets or sets whether to draw the checkbox as a radio button.
            /// </summary>
            public bool IsRadioButton;

            /// <summary>
            /// Gets or sets a value indicating whether to keep text padding
            /// even when the checkbox is not visible.
            /// </summary>
            public bool KeepTextPaddingWithoutCheckBox;
        }
    }
}