﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic toolbar control.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public partial class ToolBar : HiddenBorder
    {
        private Coord itemSize;
        private bool textVisible = false;
        private bool imageVisible = true;
        private ImageToText imageToText = ImageToText.Horizontal;

        static ToolBar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ToolBar(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBar"/> class.
        /// </summary>
        public ToolBar()
        {
            ParentBackColor = true;
            ParentForeColor = true;
            Layout = LayoutStyle.Horizontal;
            itemSize = Math.Max(DefaultSize, DefaultMinItemSize);
            IsGraphicControl = true;
        }

        /// <summary>
        /// Enumerates all toolbar item kinds.
        /// </summary>
        public enum ItemKind
        {
            /// <summary>
            /// Item is button.
            /// </summary>
            Button,

            /// <summary>
            /// Item is button.
            /// </summary>
            ButtonSticky,

            /// <summary>
            /// Item is text only button.
            /// </summary>
            ButtonText,

            /// <summary>
            /// Item is separator.
            /// </summary>
            Separator,

            /// <summary>
            /// Item is spacer.
            /// </summary>
            Spacer,

            /// <summary>
            /// Item is control.
            /// </summary>
            Control,

            /// <summary>
            /// Item is static text.
            /// </summary>
            Text,

            /// <summary>
            /// Item is picture.
            /// </summary>
            Picture,
        }

        /// <summary>
        /// Gets or sets default item size in dips.
        /// </summary>
        public static Coord DefaultSize { get; set; } = 24;

        /// <summary>
        /// Gets or sets default minimal item size in dips. You should not normally
        /// set this value to lower than 24.
        /// </summary>
        public static Coord DefaultMinItemSize { get; set; } = 24;

        /// <summary>
        /// Gets or sets default spacer item size.
        /// </summary>
        public static Coord DefaultSpacerSize { get; set; } = 4;

        /// <summary>
        /// Gets or sets default margin of the sticky button item.
        /// </summary>
        public static Thickness DefaultSpeedBtnMargin { get; set; } = (1, 0, 1, 0);

        /// <summary>
        /// Gets or sets default margin of the sticky button item.
        /// </summary>
        public static Thickness DefaultStickyBtnMargin { get; set; } = (1, 0, 1, 0);

        /// <summary>
        /// Gets or sets default margin of the text button item.
        /// </summary>
        public static Thickness DefaultTextBtnMargin { get; set; } = (1, 0, 1, 0);

        /// <summary>
        /// Gets or sets default margin of the static text item.
        /// </summary>
        public static Thickness DefaultTextMargin { get; set; } = (4, 0, 4, 0);

        /// <summary>
        /// Gets or sets default item padding.
        /// </summary>
        public static Coord DefaultItemPadding { get; set; } = 4;

        /// <summary>
        /// Gets or sets default color of the separator item. If Null, default border color is used.
        /// </summary>
        public static Color? DefaultSeparatorColor { get; set; }

        /// <summary>
        /// Gets or sets default width of the separator item.
        /// </summary>
        public static Coord DefaultSeparatorWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets default margin of the separator item.
        /// </summary>
        public static Thickness DefaultSeparatorMargin { get; set; } = (4, 4, 4, 4);

        /// <summary>
        /// Gets or sets default toolbar distance to other content of the control.
        /// </summary>
        /// <remarks>
        /// This property is not used in the <see cref="ToolBar"/> directly and added here
        /// for the convenience.
        /// You can assign one of the sides of <see cref="AbstractControl.Margin"/>
        /// property with it. For example, for toolbars with top
        /// <see cref="AbstractControl.VerticalAlignment"/>, you can set bottom margin.
        /// </remarks>
        public static Coord DefaultDistanceToContent { get; set; } = 4;

        /// <summary>
        /// Gets or sets default image size.
        /// </summary>
        /// <remarks>
        /// If this property is null,
        /// <see cref="ToolBarUtils.GetDefaultImageSize(AbstractControl)"/> is used.
        /// </remarks>
        public static int? DefaultImageSize { get; set; }

        /// <summary>
        /// Gets or sets default color of the SVG images in the normal state.
        /// </summary>
        /// <remarks>
        /// If this property is null,
        /// <see cref="AbstractControl.GetSvgColor(KnownSvgColor)"/> is used with
        /// <see cref="KnownSvgColor.Normal"/> parameter.
        /// </remarks>
        public static Color? DefaultNormalImageColor { get; set; }

        /// <summary>
        /// Gets or sets default color of the SVG images in the disabled state.
        /// </summary>
        /// <remarks>
        /// If this property is null,
        /// <see cref="AbstractControl.GetSvgColor(KnownSvgColor)"/> is used with
        /// <see cref="KnownSvgColor.Disabled"/> parameter.
        /// </remarks>
        public static Color? DefaultDisabledImageColor { get; set; }

        /// <summary>
        /// Gets or sets color of the SVG images in the normal state.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="DefaultNormalImageColor"/> is used.
        /// </remarks>
        [Browsable(false)]
        public virtual Color? NormalImageColor { get; set; }

        /// <summary>
        /// Gets or sets color of the SVG images in the disabled state.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="DefaultDisabledImageColor"/> is used.
        /// </remarks>
        [Browsable(false)]
        public virtual Color? DisabledImageColor { get; set; }

        /// <summary>
        /// Gets or sets image size.
        /// </summary>
        /// <remarks>
        /// This property specifies size of the new images in the toolbar.
        /// Existing items size is not changed. If this property is null,
        /// <see cref="DefaultImageSize"/> is used.
        /// </remarks>
        [Browsable(false)]
        public virtual int? ImageSize { get; set; }

        /// <summary>
        /// Gets or sets a value which specifies display modes for
        /// item image and text.
        /// </summary>
        [Browsable(true)]
        public virtual ImageToText ImageToText
        {
            get => imageToText;
            set
            {
                if (imageToText == value)
                    return;
                imageToText = value;
                DoInsideLayout(() =>
                {
                    foreach (var item in Children)
                    {
                        if (item is SpeedButton speedButton)
                            speedButton.ImageToText = value;
                    }
                });
            }
        }

        /// <summary>
        /// Gets toolbar item size.
        /// </summary>
        public virtual Coord ItemSize
        {
            get
            {
                return itemSize;
            }

            set
            {
                if (value < DefaultMinItemSize)
                    value = DefaultMinItemSize;
                if (itemSize == value)
                    return;
                itemSize = value;

                DoInsideLayout(() =>
                {
                    foreach (var item in Children)
                    {
                        if (item is SpeedButton || item is PictureBox)
                            item.SuggestedSize = GetItemSuggestedSize(item);
                    }
                });
            }
        }

        /// <inheritdoc/>
        public override SizeD MinimumSize
        {
            get
            {
                var result = SizeD.Max(base.MinimumSize, itemSize);
                return result;
            }

            set
            {
                base.MinimumSize = value;
            }
        }

        /// <inheritdoc/>
        public override Font? Font
        {
            get
            {
                return base.Font;
            }

            set
            {
                base.Font = value;
                SetChildrenFont(RealFont, true);
            }
        }

        /// <inheritdoc/>
        public override bool IsBold
        {
            get
            {
                return base.IsBold;
            }

            set
            {
                if (IsBold == value)
                    return;
                base.IsBold = value;
                GetChildren(true).IsBold(value);
            }
        }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get
            {
                return base.BackgroundColor;
            }

            set
            {
                base.BackgroundColor = value;
                foreach(var item in Children)
                {
                    if (NeedUpdateBackColor(item))
                        item.BackgroundColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets 'IsClickRepeated' property for all the tools.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsToolClickRepeated
        {
            get
            {
                var hasTrue = false;
                var hasFalse = false;
                foreach (var btn in ChildrenOfType<SpeedButton>())
                {
                    if (btn.IsClickRepeated)
                        hasTrue = true;
                    else
                        hasFalse = true;
                }

                if (hasTrue && !hasFalse)
                    return true;
                return false;
            }

            set
            {
                foreach (var btn in ChildrenOfType<SpeedButton>())
                {
                    btn.IsClickRepeated = value;
                }
            }
        }

        /// <inheritdoc/>
        public override Brush? Background
        {
            get
            {
                return base.Background;
            }

            set
            {
                base.Background = value;
                var items = GetChildren(true).Items;
                foreach (var item in items)
                {
                    if (NeedUpdateBackColor(item))
                        item.Background = value;
                }
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get
            {
                return base.ForegroundColor;
            }

            set
            {
                base.ForegroundColor = value;
                var items = GetChildren(true).Items;
                foreach (var item in items)
                {
                    if (NeedUpdateForeColor(item))
                        item.ForegroundColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to display text in the buttons.
        /// </summary>
        public virtual bool TextVisible
        {
            get
            {
                return textVisible;
            }

            set
            {
                if (textVisible == value)
                    return;
                textVisible = value;
                DoInsideLayout(() =>
                {
                    foreach (var item in Children)
                    {
                        if (item is SpeedButton speedButton)
                        {
                            speedButton.TextVisible = value;
                            item.SuggestedSize = GetItemSuggestedSize(item);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets whether to display images in the buttons.
        /// </summary>
        public virtual bool ImageVisible
        {
            get
            {
                return imageVisible;
            }

            set
            {
                if (imageVisible == value)
                    return;
                imageVisible = value;
                DoInsideLayout(() =>
                {
                    foreach (var item in Children)
                    {
                        if (item is SpeedButton speedButton)
                            speedButton.ImageVisible = value;
                    }
                });
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <summary>
        /// Adds an empty disabled <see cref="SpeedButton"/> to the control.
        /// </summary>
        public virtual ObjectUniqueId AddSpeedBtn()
        {
            var result = AddSpeedBtnCore();
            return result.UniqueId;
        }

        /// <summary>
        /// Adds an empty disabled <see cref="SpeedButton"/> aligned to the right.
        /// </summary>
        public virtual ObjectUniqueId AddRightSpeedBtn()
        {
            var result = AddSpeedBtnCore();
            result.HorizontalAlignment = HorizontalAlignment.Right;
            return result.UniqueId;
        }

        /// <summary>
        /// Adds <see cref="SpeedButton"/> with svg image.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="image">Item image.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpeedBtn(
            string? text,
            SvgImage? image,
            EventHandler? action)
        {
            var result = AddSpeedBtnCore(
                text,
                image,
                null,
                action);
            return result.UniqueId;
        }

        /// <summary>
        /// Adds <see cref="SpeedButton"/> aligned to the right.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="image">Item image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddRightSpeedBtn(
            string? text,
            SvgImage? image,
            string? toolTip = null,
            EventHandler? action = null)
        {
            var result = AddSpeedBtnCore(
                text,
                image,
                toolTip,
                action);
            result.HorizontalAlignment = HorizontalAlignment.Right;
            return result.UniqueId;
        }

        /// <summary>
        /// Adds <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="image">Item image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpeedBtn(
            string? text,
            SvgImage? image,
            string? toolTip = null,
            EventHandler? action = null)
        {
            var result = AddSpeedBtnCore(
                text,
                image,
                toolTip,
                action);
            return result.UniqueId;
        }

        /// <summary>
        /// Adds <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="image">Item image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual SpeedButton AddSpeedBtnCore(
            string? text,
            SvgImage? image = null,
            string? toolTip = null,
            EventHandler? action = null)
        {
            var result = AddSpeedBtnCore(
                ItemKind.Button,
                text,
                ToNormal(image),
                ToDisabled(image),
                toolTip,
                action);
            return result;
        }

        /// <summary>
        /// Adds <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="imageSet">Item image.</param>
        /// <param name="imageSetDisabled">Item disabled image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpeedBtn(
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled = null,
            string? toolTip = null,
            EventHandler? action = null)
        {
            var result = AddSpeedBtnCore(
                ItemKind.Button,
                text,
                imageSet,
                imageSetDisabled,
                toolTip,
                action);
            return result.UniqueId;
        }

        /// <summary>
        /// Adds sticky <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="image">Item image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddStickyBtn(
            string? text,
            SvgImage? image,
            string? toolTip = null,
            EventHandler? action = null)
        {
            return AddStickyBtn(
                text,
                ToNormal(image),
                ToDisabled(image),
                toolTip,
                action);
        }

        /// <summary>
        /// Adds sticky <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="imageSet">Item image.</param>
        /// <param name="imageSetDisabled">Item disabled image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddStickyBtn(
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled,
            string? toolTip = null,
            EventHandler? action = null)
        {
            var result = AddSpeedBtnCore(
                ItemKind.ButtonSticky,
                text,
                imageSet,
                imageSetDisabled,
                toolTip,
                action);
            result.Margin = DefaultStickyBtnMargin;
            result.Click += StickyButton_Click;
            return result.UniqueId;

            static void StickyButton_Click(object? sender, EventArgs e)
            {
                if (sender is not SpeedButton button)
                    return;
                button.Borders ??= new();
                if(button.Sticky)
                {
                    button.Borders.Pressed?.SetWidth(2);
                    button.Borders.Hovered?.SetWidth(2);
                }
                else
                {
                    button.Borders.Pressed?.SetWidth(1);
                    button.Borders.Hovered?.SetWidth(1);
                }
            }
        }

        /// <summary>
        /// Sets image of the item for the normal state.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">Image identifier.</param>
        public virtual void SetToolImage(ObjectUniqueId id, KnownButton value)
        {
            var info = KnownButtons.GetInfo(value);
            var svg = info?.SvgImage;
            SetToolSvg(id, svg);
        }

        /// <summary>
        /// Sets image of the item for the normal state.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value"><see cref="ImageSet"/> to use as item image.</param>
        public virtual void SetToolSvg(ObjectUniqueId id, SvgImage? value)
        {
            var image = value?.AsImageSet(GetImageSize());
            SetToolImage(id, image);
        }

        /// <summary>
        /// Sets image of the item for the normal state.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value"><see cref="ImageSet"/> to use as item image.</param>
        public virtual void SetToolImage(ObjectUniqueId id, ImageSet? value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.ImageSet = value;
        }

        /// <summary>
        /// Sets command and command parameters for the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="command">A command that will be executed when tool is clicked.</param>
        /// <param name="commandParameter">A parameter that will be passed
        /// to the command when executing it.</param>
        public virtual void SetToolCommand(
            ObjectUniqueId id,
            ICommand? command = null,
            object? commandParameter = null)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.CommandParameter = commandParameter;
            item.Command = command;
        }

        /// <summary>
        /// Gets 'IsClickRepeated' property of the tool.
        /// </summary>
        /// <param name="id">Item id.</param>
        public virtual bool GetToolIsClickRepeated(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return false;
            return item.IsClickRepeated;
        }

        /// <summary>
        /// Sets 'IsClickRepeated' property of the tool.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">Value for the 'IsClickRepeated' property of the tool.</param>
        public virtual void SetToolIsClickRepeated(ObjectUniqueId id, bool value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.IsClickRepeated = value;
        }

        /// <summary>
        /// Sets image of the item for the disabled state.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value"><see cref="ImageSet"/> to use as item image.</param>
        public virtual void SetToolDisabledImage(ObjectUniqueId id, ImageSet? value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.DisabledImageSet = value;
        }

        /// <summary>
        /// Gets image of the item for the normal state.
        /// </summary>
        /// <param name="id">Item id.</param>
        public virtual ImageSet? GetToolImage(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return null;
            return item.ImageSet;
        }

        /// <summary>
        /// Gets image of the item for the disabled state.
        /// </summary>
        /// <param name="id">Item id.</param>
        public virtual ImageSet? GetToolDisabledImage(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return null;
            return item.DisabledImageSet;
        }

        /// <summary>
        /// Gets <see cref="ImageSet"/> from <see cref="SvgImage"/>
        /// for the normal state.
        /// </summary>
        /// <param name="image">Svg image.</param>
        /// <returns></returns>
        public ImageSet? ToNormal(SvgImage? image)
        {
            var imageSize = GetImageSize();
            var result = image?.AsNormal(imageSize, IsDarkBackground);
            return result;
        }

        /// <summary>
        /// Gets <see cref="ImageSet"/> from <see cref="SvgImage"/>
        /// for the disabled state.
        /// </summary>
        /// <param name="image">Svg image.</param>
        /// <returns></returns>
        public ImageSet? ToDisabled(SvgImage? image)
        {
            var result = image?.AsDisabled(GetImageSize(), IsDarkBackground);
            return result;
        }

        /// <summary>
        /// Adds known <see cref="SpeedButton"/> to the control.
        /// </summary>
        public virtual ObjectUniqueId AddSpeedBtn(KnownButton button, EventHandler? action = null)
        {
            var info = KnownButtons.GetInfo(button);

            var result = AddSpeedBtn(info?.Text?.SafeToString(), info?.SvgImage, action);
            return result;
        }

        /// <summary>
        /// Adds array of known <see cref="SpeedButton"/> to the control.
        /// </summary>
        public virtual ObjectUniqueId[] AddSpeedBtn(params KnownButton[] buttons)
        {
            ObjectUniqueId[] result = new ObjectUniqueId[buttons.Length];
            for(int i = 0; i < buttons.Length; i++)
                result[i] = AddSpeedBtn(buttons[i]);
            return result;
        }

        /// <summary>
        /// Adds known <see cref="SpeedButton"/> to the control.
        /// </summary>
        public virtual ObjectUniqueId[] AddSpeedBtn(MessageBoxButtons button)
        {
            switch (button)
            {
                case MessageBoxButtons.OK:
                    return AddSpeedBtn(new KnownButton[] { KnownButton.OK });
                case MessageBoxButtons.OKCancel:
                    return AddSpeedBtn(KnownButton.OK, KnownButton.Cancel);
                case MessageBoxButtons.YesNoCancel:
                    return AddSpeedBtn(KnownButton.Yes, KnownButton.No, KnownButton.Cancel);
                case MessageBoxButtons.YesNo:
                    return AddSpeedBtn(KnownButton.Yes, KnownButton.No);
                case MessageBoxButtons.AbortRetryIgnore:
                    return AddSpeedBtn(KnownButton.Abort, KnownButton.Retry, KnownButton.Ignore);
                case MessageBoxButtons.RetryCancel:
                    return AddSpeedBtn(KnownButton.Retry, KnownButton.Cancel);
            }

            return Array.Empty<ObjectUniqueId>();
        }

        /// <summary>
        /// Adds <see cref="PictureBox"/> to the control.
        /// </summary>
        /// <param name="image">Svg image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddPicture(
            SvgImage? image = null,
            string? toolTip = default)
        {
            return AddPicture(ToNormal(image), ToDisabled(image), toolTip);
        }

        /// <summary>
        /// Adds <see cref="PictureBox"/> to the control with contents created
        /// from the the specified template.
        /// </summary>
        /// <param name="template">Template which is used to get picture pixels.</param>
        /// <param name="needDisabled">Whether to get pixels
        /// for disabled state of the template.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <returns></returns>
        public virtual ObjectUniqueId AddPicture(
            TemplateControl template,
            bool needDisabled = false,
            string? toolTip = default)
        {
            template.Enabled = true;
            ImageSet? image = TemplateUtils.GetTemplateAsImageSet(template);
            ImageSet? imageDisabled = null;

            if (needDisabled)
            {
                template.Enabled = false;

                try
                {
                    imageDisabled = TemplateUtils.GetTemplateAsImageSet(template);
                }
                finally
                {
                    template.Enabled = true;
                }
            }

            var result = AddPicture(image, imageDisabled, toolTip, true);
            return result;
        }

        /// <summary>
        /// Adds <see cref="PictureBox"/> to the control.
        /// </summary>
        /// <param name="image">Normal image.</param>
        /// <param name="imageDisabled">Disable image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="ignoreSuggestedSize">Whether to ignore suggested
        /// size of the item's control.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddPicture(
            ImageSet? image = null,
            ImageSet? imageDisabled = null,
            string? toolTip = default,
            bool ignoreSuggestedSize = false)
        {
            PictureBox picture = new()
            {
                IsGraphicControl = true,
                ImageStretch = false,
                ImageSet = image,
                ToolTip = toolTip ?? string.Empty,
                VerticalAlignment = UI.VerticalAlignment.Center,
            };

            if (ignoreSuggestedSize)
            {
                picture.IgnoreSuggestedSize = true;
            }
            else
            {
                picture.SuggestedSize = GetItemSuggestedSize(picture);
            }

            if (imageDisabled is not null)
            {
                picture.DisabledImageSet = imageDisabled;
            }

            UpdateItemProps(picture, ItemKind.Picture);

            picture.Parent = this;

            return picture.UniqueId;
        }

        /// <summary>
        /// Adds existing control to the toolbar.
        /// </summary>
        /// <param name="control">Control to add.</param>
        /// <remarks>
        /// You can use this method to add <see cref="TextBox"/> or <see cref="ComboBox"/>
        /// to the toolbar.
        /// </remarks>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddControl(AbstractControl control)
        {
            control.Parent = this;
            UpdateItemProps(control, ItemKind.Control);
            return control.UniqueId;
        }

        /// <summary>
        /// Adds static text to the control.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddText(string text)
        {
            var label = AddTextCore(text);
            return label.UniqueId;
        }

        /// <summary>
        /// Adds static text to the control.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <returns><see cref="AbstractControl"/> which is used to show text
        /// for the added item.</returns>
        public virtual AbstractControl AddTextCore(string text)
        {
            var label = AddSpeedBtnCore(text);
            label.ToolTip = string.Empty;
            label.UseTheme = SpeedButton.KnownTheme.NoBorder;

            return label;
        }

        /// <summary>
        /// Adds an empty space with the specified or default size.
        /// </summary>
        /// <param name="size">Optional spacer size.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpacer(Coord? size = null)
        {
            Panel control = new()
            {
                SuggestedSize = size ?? DefaultSpacerSize,
            };

            UpdateItemProps(control, ItemKind.Spacer);

            control.Parent = this;
            return control.UniqueId;
        }

        /// <summary>
        /// Adds separator item (vertical line).
        /// </summary>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSeparator()
        {
            Border border = new()
            {
                BorderWidth = (DefaultSeparatorWidth, 0, 0, 0),
                SuggestedWidth = DefaultSeparatorWidth,
                Margin = DefaultSeparatorMargin,
            };

            if(DefaultSeparatorColor is not null)
                border.BorderColor = DefaultSeparatorColor;

            UpdateItemProps(border, ItemKind.Separator);

            border.Parent = this;
            return border.UniqueId;
        }

        /// <summary>
        /// Sets 'Visible' property of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="visible">New property value.</param>
        public virtual void SetToolVisible(ObjectUniqueId id, bool visible = true)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Visible = visible;
        }

        /// <summary>
        /// Sets 'Enabled' property of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="enabled">New property value.</param>
        public virtual void SetToolEnabled(ObjectUniqueId? id, bool enabled = true)
        {
            if (id is null)
                return;
            var item = GetToolControl(id.Value);
            if (item is null)
                return;
            item.Enabled = enabled;
        }

        /// <summary>
        /// Sets whether toolbar item is right aligned.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="isRight"><c>true</c> if item must be right aligned.</param>
        public virtual void SetToolAlignRight(ObjectUniqueId id, bool isRight = true)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            if (isRight)
                item.HorizontalAlignment = UI.HorizontalAlignment.Right;
            else
                item.HorizontalAlignment = UI.HorizontalAlignment.Left;
        }

        /// <summary>
        /// Sets whether toolbar item is aligned to the center.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="isCenter"><c>true</c> if item must be aligned to the center.</param>
        public virtual void SetToolAlignCenter(ObjectUniqueId id, bool isCenter = true)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            if (isCenter)
                item.HorizontalAlignment = UI.HorizontalAlignment.Center;
            else
                item.HorizontalAlignment = UI.HorizontalAlignment.Left;
        }

        /// <summary>
        /// Gets whether item is right aligned.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual bool GetToolAlignRight(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return false;
            return item.HorizontalAlignment == UI.HorizontalAlignment.Right;
        }

        /// <summary>
        /// Gets drop down menu of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual ContextMenu? GetToolDropDownMenu(ObjectUniqueId id)
        {
            var item = FindTool(id);
            if (item is null)
                return null;
            return item.DropDownMenu;
        }

        /// <summary>
        /// Gets location of the popup window for the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Popup window location in screen coordinates.</returns>
        public PointD? GetToolPopupLocation(ObjectUniqueId toolId)
        {
            RectD toolRect = GetToolRect(toolId);
            PointD pt = ClientToScreen(toolRect.BottomLeft);
            return pt;
        }

        /// <summary>
        /// Returns the specified tool rectangle in the toolbar.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Position and size of the tool in the toolbar in device-independent units.</returns>
        public RectD GetToolRect(ObjectUniqueId toolId)
        {
            var item = GetToolControl(toolId);
            if (item is null)
                return RectD.Empty;
            return item.Bounds;
        }

        /// <summary>
        /// Sets the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="value">new Sticky property value.</param>
        public void SetToolSticky(ObjectUniqueId toolId, bool value = true)
        {
            var item = FindTool(toolId);
            if (item is null)
                return;
            item.Sticky = value;
        }

        /// <summary>
        /// Sets the specified toolbar items 'Sticky' property value.
        /// </summary>
        /// <param name="toolIds">IDs of a previously added tool.</param>
        /// <param name="value">new Sticky property value.</param>
        public void SetToolSticky(IReadOnlyList<ObjectUniqueId> toolIds, bool value)
        {
            foreach (var id in toolIds)
                SetToolSticky(id, value);
        }

        /// <summary>
        /// Toggles the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public void ToggleToolSticky(ObjectUniqueId toolId)
        {
            var item = FindTool(toolId);
            if (item is null)
                return;
            item.Sticky = !item.Sticky;
        }

        /// <summary>
        /// Sets 'Tag' property of the tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="tag">Value of the 'Tag' property.</param>
        public void SetToolTag(ObjectUniqueId toolId, object? tag)
        {
            var item = GetToolControl(toolId);
            if (item is null)
                return;
            item.Tag = tag;
        }

        /// <summary>
        /// Gets 'Tag' property of the tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public object? GetToolTag(ObjectUniqueId toolId)
        {
            var item = GetToolControl(toolId);
            return item?.Tag;
        }

        /// <summary>
        /// Gets all tools which are derived from the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the tool</typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> GetToolsAs<T>()
        {
            foreach (var item in Children)
            {
                if (item is not T btn)
                    continue;
                yield return btn;
            }
        }

        /// <summary>
        /// Gets all tools which are derived from the <see cref="SpeedButton"/>.
        /// </summary>
        public IEnumerable<SpeedButton> GetTools() => GetToolsAs<SpeedButton>();

        /// <summary>
        /// Gets collection of the tools with 'Sticky' property equal to the specified value.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SpeedButton> GetStickyTools(bool value = true)
        {
            foreach (var item in Children)
            {
                if (item is not SpeedButton btn)
                    continue;
                if(btn.Sticky == value)
                    yield return btn;
            }
        }

        /// <summary>
        /// Gets the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public bool GetToolSticky(ObjectUniqueId toolId)
        {
            var item = FindTool(toolId);
            if (item is null)
                return false;
            return item.Sticky;
        }

        /// <summary>
        /// Sets drop down menu of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="menu"><see cref="ContextMenu"/> to use as drop down menu.</param>
        public virtual void SetToolDropDownMenu(ObjectUniqueId id, ContextMenu? menu)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.DropDownMenu = menu;
        }

        /// <summary>
        /// Gets item 'Enabled' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual bool GetToolEnabled(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return false;
            return item.Enabled;
        }

        /// <summary>
        /// Gets whether item is 'Enabled' and 'Visible'.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual bool GetToolEnabledAndVisible(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return false;
            return item.Enabled && item.Visible;
        }

        /// <summary>
        /// Gets item 'Visible' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual bool GetToolVisible(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return false;
            return item.Visible;
        }

        /// <summary>
        /// Deletes items with the specified id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="dispose">Whether to dispose tool controls.</param>
        /// <remarks>
        /// This method disposes tool control if <paramref name="dispose"/> is <c>true</c>.
        /// </remarks>
        public virtual void DeleteTool(ObjectUniqueId id, bool dispose = false)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Parent = null;
            if(dispose)
                item.Dispose();
        }

        /// <summary>
        /// Deletes all items from the control.
        /// </summary>
        /// <remarks>
        /// This method disposes tool controls if <paramref name="dispose"/> is <c>true</c>.
        /// </remarks>
        public virtual void DeleteAll(bool dispose = false)
        {
            if (!HasChildren)
                return;

            Stack<AbstractControl> controls = new(Children);
            DoInsideLayout(() =>
            {
                foreach (var control in controls)
                {
                    control.Parent = null;
                    if (dispose)
                        control.Dispose();
                }
            });
        }

        /// <summary>
        /// Gets item 'Text' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual string? GetToolText(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return null;
            return item.Text;
        }

        /// <summary>
        /// Gets item 'ToolTip' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual string? GetToolShortHelp(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return null;
            return item.ToolTip;
        }

        /// <summary>
        /// Sets item 'Text' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="text">New property value.</param>
        public virtual void SetToolText(ObjectUniqueId id, object? text)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Text = text.SafeToString();
        }

        /// <summary>
        /// Sets item 'ToolTip' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetToolShortHelp(ObjectUniqueId id, object? value)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.ToolTip = value.SafeToString();
        }

        /// <summary>
        /// Gets custom attributes of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual ICustomAttributes<string, object>? GetToolCustomAttr(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return null;
            return item.CustomAttr;
        }

        /// <summary>
        /// Gets custom flags of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual ICustomFlags<string>? GetToolCustomFlags(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return null;
            return item.CustomFlags;
        }

        /// <summary>
        /// Gets tools which has custom flag with the specified name.
        /// </summary>
        /// <param name="name">Name of the custom flag.</param>
        /// <returns></returns>
        public virtual IReadOnlyList<ObjectUniqueId> GetToolsWithCustomFlag(string name)
        {
            List<ObjectUniqueId> result = new();

            foreach(var item in Children)
            {
                var hasFlag = item.CustomFlags[name];
                if (hasFlag)
                    result.Add(item.UniqueId);
            }

            return result;
        }

        /// <summary>
        /// Gets total count of the items.
        /// </summary>
        /// <returns></returns>
        public virtual int GetToolCount()
        {
            return Children.Count;
        }

        /// <summary>
        /// Gets an id of the item with the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        public virtual ObjectUniqueId GetToolId(int index)
        {
            return Children[index].UniqueId;
        }

        /// <summary>
        /// Gets item control.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual AbstractControl? GetToolControl(ObjectUniqueId id)
        {
            var result = FindChild(id);
            return result;
        }

        /// <summary>
        /// Gets item control at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual AbstractControl? GetToolControlAt(int index)
        {
            if(index < GetToolCount())
                return Children[index];
            return null;
        }

        /// <summary>
        /// Sets item 'Shortcut' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetToolShortcut(ObjectUniqueId id, Keys value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.ShortcutKeys = value;
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Moves toolbar to the bottom of the container. This is done
        /// by setting it's <see cref="AbstractControl.VerticalAlignment"/> to
        /// <see cref="VerticalAlignment.Bottom"/>. This moves toolbar to the bottom if
        /// container's <see cref="AbstractControl.Layout"/> is <see cref="LayoutStyle.Vertical"/>.
        /// Also this method optionally updates margin and border of the toolbar.
        /// </summary>
        /// <param name="onlyTopBorder">If True, only top border will be visible.
        /// If false, doesn't change the border.</param>
        /// <param name="updateMargin">If True, margin will be updated to have distance to other
        /// content of the container. If False, doesn't change the margin.</param>
        public virtual void MakeBottomAligned(bool onlyTopBorder = true, bool updateMargin = true)
        {
            VerticalAlignment = VerticalAlignment.Bottom;
            if(onlyTopBorder)
                OnlyTopBorder();
            if(updateMargin)
                Margin = (0, ToolBar.DefaultDistanceToContent, 0, 0);
        }

        /// <summary>
        /// Moves toolbar to the top of the container. This is done
        /// by setting it's <see cref="AbstractControl.VerticalAlignment"/> to
        /// <see cref="VerticalAlignment.Top"/>. This moves toolbar to the top if
        /// container's <see cref="AbstractControl.Layout"/> is <see cref="LayoutStyle.Vertical"/>.
        /// Also this method optionally updates margin and border of the toolbar.
        /// </summary>
        /// <param name="onlyBottomBorder">If True, only bottom border will be visible.
        /// If false, doesn't change the border.</param>
        /// <param name="updateMargin">If True, margin will be updated to have distance to other
        /// content of the container. If False, doesn't change the margin.</param>
        public virtual void MakeTopAligned(bool onlyBottomBorder = true, bool updateMargin = true)
        {
            VerticalAlignment = VerticalAlignment.Top;
            if (onlyBottomBorder)
                OnlyBottomBorder();
            if (updateMargin)
                Margin = (0, 0, 0, ToolBar.DefaultDistanceToContent);
        }

        /// <summary>
        /// Sets item 'Shortcut' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetToolShortcut(ObjectUniqueId id, KeyInfo[]? value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.ShortcutKeyInfo = value;
        }

        /// <summary>
        /// Sets item 'Shortcut' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetToolShortcut(ObjectUniqueId id, KeyGesture? value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.Shortcut = value;
        }

        /// <summary>
        /// Sets item 'Shortcut' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="key">Key code.</param>
        /// <param name="modifiers">Key modifiers.</param>
        public virtual void SetToolShortcut(
            ObjectUniqueId id,
            Key key,
            ModifierKeys modifiers = UI.ModifierKeys.None)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.ShortcutKeyInfo = [new KeyInfo(key, modifiers)];
        }

        /// <summary>
        /// Set click action for the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">Action.</param>
        public virtual void SetToolAction(ObjectUniqueId id, Action? value)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.ClickAction = value;
        }

        /// <summary>
        /// Gets last tool casted to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the required result.</typeparam>
        /// <returns></returns>
        public virtual T? LastTool<T>()
            where T : AbstractControl
        {
            return Children[Children.Count - 1] as T;
        }

        /// <summary>
        /// Adds click event handler to the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">Event handler.</param>
        public virtual void AddToolAction(ObjectUniqueId id, EventHandler? value)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Click += value;
        }

        /// <summary>
        /// Removes click event handler from the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">Event handler.</param>
        public virtual void RemoveToolAction(ObjectUniqueId id, EventHandler? value)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Click -= value;
        }

        /// <summary>
        /// Gets image size taking into account <see cref="ImageSize"/>
        /// and <see cref="DefaultImageSize"/> properties.
        /// </summary>
        /// <returns></returns>
        public virtual int GetImageSize()
        {
            return ImageSize ?? DefaultImageSize ?? Internal();

            int Internal()
            {
                var result = ToolBarUtils.GetDefaultImageSize(this).Width;
                return result;
            }
        }

        /// <summary>
        /// Sets border and margin using the specified parameters.
        /// </summary>
        /// <param name="borders">The flags that specify visible borders of the toolbar.
        /// If Null, border will not be changed.</param>
        /// <param name="margins">The flags that specify borders for which default margin
        /// is assigned. If Null, margins will not be set.</param>
        /// <param name="setDefaultPadding">Whether to set default padding for the toolbar.</param>
        public virtual void SetBorderAndMargin(
            AnchorStyles? borders = null,
            AnchorStyles? margins = null,
            bool setDefaultPadding = true)
        {
            if (borders is not null)
            {
                bool left = borders.Value.HasFlag(AnchorStyles.Left);
                bool top = borders.Value.HasFlag(AnchorStyles.Top);
                bool right = borders.Value.HasFlag(AnchorStyles.Right);
                bool bottom = borders.Value.HasFlag(AnchorStyles.Bottom);

                SetVisibleBorders(left, top, right, bottom);
            }

            if (margins is not null)
            {
                Margin = (
                    Distance(AnchorStyles.Left),
                    Distance(AnchorStyles.Top),
                    Distance(AnchorStyles.Right),
                    Distance(AnchorStyles.Bottom));

                Coord Distance(AnchorStyles flag)
                {
                    if (margins is null)
                        return 0;
                    return margins.Value.HasFlag(flag) ? ToolBar.DefaultDistanceToContent : 0;
                }
            }

            if (setDefaultPadding)
                Padding = 1;
        }

        /// <summary>
        /// Gets image color in the normal state taking into account <see cref="NormalImageColor"/>
        /// and <see cref="DefaultNormalImageColor"/> properties.
        /// </summary>
        public virtual Color GetNormalImageColor()
        {
            return NormalImageColor ?? DefaultNormalImageColor ?? GetSvgColor(KnownSvgColor.Normal);
        }

        /// <summary>
        /// Gets image color in the disabled state  taking into account
        /// <see cref="DisabledImageColor"/> and <see cref="DefaultDisabledImageColor"/> properties.
        /// </summary>
        public virtual Color GetDisabledImageColor()
        {
            return DisabledImageColor ?? DefaultDisabledImageColor
                ?? GetSvgColor(KnownSvgColor.Disabled);
        }

        /// <summary>
        /// Gets item control as <see cref="SpeedButton"/>. If tool doesn't use
        /// <see cref="SpeedButton"/> as a control, returns Null.
        /// </summary>
        /// <param name="id">Item id.</param>
        public SpeedButton? FindTool(ObjectUniqueId id)
        {
            var result = GetToolControl(id) as SpeedButton;
            return result;
        }

        /// <summary>
        /// Adds text only <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="action">Click action.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddTextBtn(
            string? text,
            string? toolTip = null,
            EventHandler? action = null)
        {
            var speedButton = AddTextBtnCore(text, toolTip, action);
            return speedButton.UniqueId;
        }

        /// <summary>
        /// Adds text only <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="action">Click action.</param>
        /// <param name="toolTip">Item tooltip.</param>
        public virtual SpeedTextButton AddTextBtnCore(
            string? text,
            string? toolTip = null,
            EventHandler? action = null)
        {
            text ??= string.Empty;

            var speedButton = CreateToolSpeedTextButton();

            speedButton.Padding = DefaultItemPadding;
            speedButton.ToolTip = toolTip ?? string.Empty;
            speedButton.Text = text;
            speedButton.VerticalAlignment = UI.VerticalAlignment.Center;
            speedButton.Margin = DefaultTextBtnMargin;

            UpdateItemProps(speedButton, ItemKind.ButtonText);

            if (action is not null)
                speedButton.Click += action;
            speedButton.Parent = this;

            return speedButton;
        }

        /// <summary>
        /// Adds an empty disabled <see cref="SpeedButton"/> to the control.
        /// </summary>
        public virtual SpeedButton AddSpeedBtnCore()
        {
            var result = AddSpeedBtnCore(
                ItemKind.Button,
                null,
                KnownSvgImages.ImgEmpty.AsImageSet(GetImageSize()),
                null);
            result.Enabled = false;
            return result;
        }

        /// <summary>
        /// Adds <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="itemKind">Item kind.</param>
        /// <param name="imageSet">Item image.</param>
        /// <param name="imageSetDisabled">Item disabled image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        public virtual SpeedButton AddSpeedBtnCore(
            ItemKind itemKind,
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled,
            string? toolTip = null,
            EventHandler? action = null)
        {
            text ??= string.Empty;

            var speedButton = CreateToolSpeedButton();

            speedButton.Padding = DefaultItemPadding;
            speedButton.ImageVisible = imageVisible;
            speedButton.TextVisible = textVisible;
            speedButton.ImageToText = imageToText;
            speedButton.Text = text;
            speedButton.ImageSet = imageSet;
            speedButton.ToolTip = toolTip ?? text;
            speedButton.VerticalAlignment = UI.VerticalAlignment.Center;
            speedButton.Margin = DefaultSpeedBtnMargin;

            speedButton.SuggestedSize = GetItemSuggestedSize(speedButton);

            if (imageSetDisabled is not null)
            {
                speedButton.DisabledImageSet = imageSetDisabled;
            }

            UpdateItemProps(speedButton, itemKind);

            if (action is not null)
                speedButton.Click += action;
            speedButton.Parent = this;

            return speedButton;
        }

        /// <summary>
        /// Creates control for use in the toolbar as a label.
        /// Override to create customized label controls.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateToolLabel()
        {
            return new GenericLabel();
        }

        /// <summary>
        /// Creates <see cref="SpeedButton"/> for use in the toolbar.
        /// Override to create customized speed buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual SpeedButton CreateToolSpeedButton()
        {
            return new SpeedButton();
        }

        /// <summary>
        /// Creates <see cref="SpeedTextButton"/> for use in the toolbar.
        /// Override to create customized speed text buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual SpeedTextButton CreateToolSpeedTextButton()
        {
            return new SpeedTextButton();
        }

        /// <summary>
        /// Updates common properties of the item control.
        /// </summary>
        /// <param name="control">Control which properties to update.</param>
        /// <param name="itemKind">Item kind.</param>
        /// <remarks>
        /// This method is called when new item is added, it updates
        /// <see cref="AbstractControl.BackgroundColor"/> and other properties.
        /// </remarks>
        protected virtual void UpdateItemProps(AbstractControl control, ItemKind itemKind)
        {
            if (itemKind == ItemKind.Control)
                return;
            if (BackgroundColor is not null)
                control.BackgroundColor = BackgroundColor;
        }

        /// <summary>
        /// Gets whether child control background color need to be updated when
        /// toolbar background color is changed.
        /// </summary>
        /// <param name="control">Control to check</param>
        /// <returns></returns>
        protected virtual bool NeedUpdateBackColor(AbstractControl control)
            => NeedUpdateForeColor(control);

        /// <summary>
        /// Gets whether child control foreground color need to be updated when
        /// toolbar foreground color is changed.
        /// </summary>
        /// <param name="control">Control to check</param>
        /// <returns></returns>
        protected virtual bool NeedUpdateForeColor(AbstractControl control)
        {
            Type[] types =
            {
                typeof(SpeedButton),
                typeof(GenericLabel),
                typeof(PictureBox),
                typeof(StackPanel),
                typeof(Panel),
                typeof(Grid),
                typeof(Border),
            };

            var controlType = control.GetType();

            if (Array.IndexOf(types, controlType) >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// Gets item's suggested size.
        /// </summary>
        /// <param name="control">Child control for which to get the suggested size.</param>
        /// <returns></returns>
        protected virtual SizeD GetItemSuggestedSize(AbstractControl control)
        {
            if (control is PictureBox)
            {
                return itemSize;
            }

            if (TextVisible)
                return (Coord.NaN, itemSize);
            return itemSize;
        }
    }
}
