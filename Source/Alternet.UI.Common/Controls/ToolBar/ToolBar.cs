using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private Type? customButtonType;
        private MenuChangeRouter? menuChangeRouter;

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
        /// Occurs when a tool is clicked.
        /// </summary>
        /// <remarks>This event is triggered whenever a tool is clicked,
        /// allowing subscribers to handle the click action.</remarks>
        public event EventHandler? ToolClick;

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
        /// Gets or sets default margin of the button item.
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
        /// Gets or sets a value indicating whether the toolbar is vertical.
        /// </summary>
        public virtual bool IsVertical
        {
            get
            {
                return Layout == LayoutStyle.Vertical;
            }

            set
            {
                if (IsVertical == value)
                    return;

                DoInsideLayout(() =>
                {
                    Layout = value ? LayoutStyle.Vertical : LayoutStyle.Horizontal;

                    foreach (var item in Children)
                    {
                        if (item is ToolBarSeparatorItem separator)
                            separator.IsVertical = value;
                    }
                });
            }
        }

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
                    foreach (var speedButton in ToolsAsButton)
                    {
                        speedButton.ImageToText = value;
                    }

                    OnItemSizeChanged();
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
                OnItemSizeChanged();
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

        /// <summary>
        /// Gets an enumerable collection of tools that are of type <see cref="SpeedButton"/>.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<SpeedButton> ToolsAsButton
        {
            get
            {
                if(!HasChildren)
                    yield break;
                foreach (var item in Children)
                {
                    if (item is SpeedButton speedButton)
                    {
                        yield return speedButton;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a set of speed buttons contained within the toolbar.
        /// </summary>
        /// <returns>A <see cref="ControlSet"/> containing all <see cref="SpeedButton"/>
        /// instances that are children of the current control.</returns>
        [Browsable(false)]
        public virtual ControlSet SpeedButtons
        {
            get
            {
                return new ControlSet(ToolsAsButton);
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
                foreach (var btn in ToolsAsButton)
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
                foreach (var btn in ToolsAsButton)
                {
                    btn.IsClickRepeated = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether double click on the button is treated as a single click.
        /// Default is false.
        /// </summary>
        public virtual bool DoubleClickAsClick { get; set; }

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
                    foreach (var speedButton in ToolsAsButton)
                    {
                        speedButton.TextVisible = value;
                        speedButton.SuggestedSize = GetItemSuggestedSize(speedButton);
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
                    foreach (var speedButton in ToolsAsButton)
                    {
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
        /// Gets the <see cref="MenuChangeRouter"/> instance associated with this object.
        /// </summary>
        protected virtual MenuChangeRouter MenuChangeHandler
        {
            get
            {
                return menuChangeRouter ??= new MenuChangeRouter(this);
            }
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
        /// Adds a speed button with the specified text and click event handler.
        /// </summary>
        /// <remarks>This method creates a speed button with the specified text and
        /// associates it with the
        /// provided click event handler. If <paramref name="action"/> is null,
        /// the button will not perform any action
        /// when clicked.</remarks>
        /// <param name="text">The text to display on the button. Cannot be null or empty.</param>
        /// <param name="action">The event handler to invoke when the button is clicked.
        /// Can be null if no action is required.</param>
        /// <returns>The unique identifier of the created button,
        /// which can be used to reference the button later.</returns>
        public virtual ObjectUniqueId AddSpeedBtn(string text, EventHandler? action)
        {
            var result = AddSpeedBtnCore(
                text,
                (SvgImage?)null,
                null,
                action);
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
                if (button.Sticky)
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
            var item = FindTool(id);
            if (item is null)
                return;
            item.ImageSet = ToNormal(value);
            item.DisabledImageSet = ToDisabled(value);
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
        public virtual ImageSet? ToNormal(SvgImage? image)
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
        public virtual ImageSet? ToDisabled(SvgImage? image)
        {
            var result = image?.AsDisabled(GetImageSize(), IsDarkBackground);
            return result;
        }

        /// <summary>
        /// Adds a default combo box button with optional SVG override.
        /// </summary>
        /// <param name="svg">The SVG image associated with the button.
        /// This parameter can be used to specify optional image override.</param>
        /// <param name="action">The action to perform when the button is clicked.</param>
        /// <returns>The unique ID of the added button.</returns>
        public virtual ObjectUniqueId AddDefaultComboBoxBtn(
            SvgImage? svg,
            EventHandler? action = null)
        {
            return AddSpeedBtn(
                ControlAndButton.DefaultBtnComboBoxImage,
                svg ?? ControlAndButton.DefaultBtnComboBoxSvg,
                action);
        }

        /// <summary>
        /// Adds a known speed button to the toolbar with optional svg override.
        /// </summary>
        /// <param name="btn">The known button to add.</param>
        /// <param name="svg">The SVG image associated with the button.
        /// This parameter can be used to specify optional image override.</param>
        /// <param name="action">The action to perform when the button is clicked.</param>
        /// <returns>The unique ID of the added button.</returns>
        public virtual ObjectUniqueId AddSpeedBtn(
            KnownButton btn,
            SvgImage? svg,
            EventHandler? action = null)
        {
            ObjectUniqueId id;

            if (svg is null)
            {
                id = AddSpeedBtn(btn, action);
            }
            else
            {
                id = AddSpeedBtn(null, svg, action);
            }

            return id;
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
            for (int i = 0; i < buttons.Length; i++)
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
                picture.MinimumSize = itemSize;
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
        public virtual ObjectUniqueId AddText(string? text = null)
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
        public virtual AbstractControl AddTextCore(string? text = null)
        {
            var label = CreateToolLabel();
            label.VerticalAlignment = UI.VerticalAlignment.Center;
            label.Text = text ?? string.Empty;
            label.Margin = DefaultTextMargin;
            label.Parent = this;
            label.Click += RaiseToolClick;

            return label;
        }

        /// <summary>
        /// Adds an empty space with the specified or default size.
        /// </summary>
        /// <param name="size">Optional spacer size.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpacer(Coord? size = null)
        {
            var control = AddSpacerCore(size);
            return control.UniqueId;
        }

        /// <summary>
        /// Adds an empty space with the specified or default size.
        /// </summary>
        /// <param name="size">The optional spacer size.</param>
        /// <returns><see cref="AbstractControl"/> which represents the spacer added.</returns>
        public virtual AbstractControl AddSpacerCore(Coord? size = null)
        {
            Spacer control = new()
            {
                SuggestedSize = size ?? DefaultSpacerSize,
            };

            UpdateItemProps(control, ItemKind.Spacer);

            control.Parent = this;
            return control;
        }

        /// <summary>
        /// Adds separator item (vertical line).
        /// </summary>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSeparator()
        {
            ToolBarSeparatorItem border = AddSeparatorCore();
            return border.UniqueId;
        }

        /// <summary>
        /// Creates and adds a new separator item to the toolbar.
        /// </summary>
        /// <remarks>The separator item is initialized with the default separator color,
        /// if specified, and its properties are updated to match the toolbar's configuration.
        /// The parent of the separator item is set to the current toolbar instance.</remarks>
        /// <returns>A <see cref="ToolBarSeparatorItem"/> representing
        /// the newly created separator.</returns>
        public virtual ToolBarSeparatorItem AddSeparatorCore()
        {
            var result = InsertSeparatorCore(Children.Count);
            return result;
        }

        /// <summary>
        /// Inserts a separator item at the specified index within the toolbar.
        /// </summary>
        /// <remarks>The separator item is created using the <see cref="CreateToolSeparator"/> method.
        /// If a default separator color is specified via <see cref="DefaultSeparatorColor"/>,
        /// it is applied to the separator's border. The item properties are updated to reflect
        /// its kind as a separator before being added to
        /// the toolbar's children.</remarks>
        /// <param name="index">The zero-based index at which the separator
        /// item should be inserted.</param>
        /// <returns>The <see cref="ToolBarSeparatorItem"/> that was inserted.</returns>
        public virtual ToolBarSeparatorItem InsertSeparatorCore(int index)
        {
            ToolBarSeparatorItem result = CreateToolSeparator();
            if (DefaultSeparatorColor is not null)
                result.BorderColor = DefaultSeparatorColor;
            UpdateItemProps(result, ItemKind.Separator);
            Children.Insert(index, result);
            return result;
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
        /// Gets collection of the tools with 'Sticky' property equal to the specified value.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SpeedButton> GetStickyTools(bool value = true)
        {
            foreach (var btn in ToolsAsButton)
            {
                if (btn.Sticky == value)
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
        /// <param name="image"></param>
        public virtual void SetToolDropDownMenu(
            ObjectUniqueId id,
            ContextMenu? menu,
            KnownButtonImage? image = null)
        {
            var item = FindTool(id);
            if (item is null)
                return;
            item.DropDownMenu = menu;

            if (image is null)
            {
                item.SetLabelImage(null);
            }
            else
            {
                item.SetLabelImage(image.Value);
            }
        }

        /// <summary>
        /// Sets the drop-down menu position for all child controls that are
        /// of type <see cref="UserControl"/>.
        /// </summary>
        /// <remarks>This method iterates through the child controls and updates the
        /// <c>DropDownMenuPosition</c> property  for each child that is
        /// a <see cref="UserControl"/>. Controls that are
        /// not of type <see cref="UserControl"/> are ignored.</remarks>
        /// <param name="position">The horizontal and vertical alignment to apply to the drop-down menu.
        /// A value of <see langword="null"/> clears the alignment setting.</param>
        public virtual void SetDropDownMenuPosition(HVDropDownAlignment? position)
        {
            foreach (var item in Children)
            {
                if (item is not UserControl userControl)
                    continue;
                userControl.DropDownMenuPosition = position;
            }
        }

        /// <summary>
        /// Configures the toolbar to look and behave as a context menu.
        /// </summary>
        /// <remarks>This method sets the layout to vertical and adjusts child elements,
        /// such as <see cref="SpeedButton"/> instances, to behave as menu items.
        /// It should be called when the object is intended to function as a context menu.</remarks>
        public virtual void ConfigureAsContextMenu()
        {
            DoInsideLayout(() =>
            {
                IsVertical = true;

                foreach (var item in Children)
                {
                    HorizontalAlignment = HorizontalAlignment.Fill;

                    if (item is SpeedButton speedButton)
                    {
                        speedButton.ConfigureAsMenuItem();
                    }
                }
            });

            SetDropDownMenuPosition(new (DropDownAlignment.AfterEnd, DropDownAlignment.AfterStart));
        }

        /// <summary>
        /// Determines whether any child item is a <see cref="SpeedButton"/> with an image.
        /// </summary>
        /// <remarks>This method iterates through the collection of child items and
        /// checks if any of them
        /// are of type <see cref="SpeedButton"/> and have an image.</remarks>
        /// <returns><see langword="true"/> if at least one child item is
        /// a <see cref="SpeedButton"/> with an image;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool HasToolsWithImages()
        {
            foreach (var speedButton in ToolsAsButton)
            {
                if (speedButton.HasImage)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether any child item is a <see cref="SpeedButton"/> with a label image.
        /// </summary>
        /// <returns><see langword="true"/> if at least one child item is a <see cref="SpeedButton"/>
        /// and its <see cref="SpeedButton.HasLabelImage"/> property is <see langword="true"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool HasToolsWithLabelImages()
        {
            foreach (var speedButton in ToolsAsButton)
            {
                if (speedButton.HasLabelImage)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Ensures that all child <see cref="SpeedButton"/> instances have a label image assigned.
        /// </summary>
        /// <remarks>This method iterates through the child elements and assigns
        /// a custom or default transparent label image to any <see cref="SpeedButton"/>
        /// that does not already have one. If no child elements require
        /// a label image, the method performs no action.</remarks>
        /// <returns><see langword="true"/> if at least one <see cref="SpeedButton"/>
        /// was updated with a label image; otherwise, <see langword="false"/>.</returns>
        /// <param name="img">The custom label image to use;
        /// if <see langword="null"/>, a default transparent image will be used.</param>
        public virtual bool AddRemainingLabelImages(KnownButtonImage? img = null)
        {
            var hasLabelImages = HasToolsWithLabelImages();

            if (!hasLabelImages)
                return false;

            bool result = false;

            foreach (var speedButton in ToolsAsButton)
            {
                if (speedButton.HasLabelImage)
                    continue;
                speedButton.SetLabelImage(img ?? KnownButtonImage.Transparent);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Adds a default transparent image to all child speed buttons that
        /// do not already have an image.
        /// </summary>
        /// <remarks>This method iterates through the child elements and assigns
        /// a custom or default transparent image to any <see cref="SpeedButton"/>
        /// that does not already
        /// have an image. If no child speed buttons require an image, the method returns
        /// <see langword="false"/>.</remarks>
        /// <returns><see langword="true"/> if at least one child speed button
        /// was updated with a transparent image; otherwise, <see langword="false"/>.</returns>
        /// <param name="img">The custom label image to use;
        /// if <see langword="null"/>, a default transparent image will be used.</param>
        public virtual bool AddRemainingImages(KnownButtonImage? img = null)
        {
            var hasImages = HasToolsWithImages();

            if (!hasImages)
                return false;

            bool result = false;

            foreach (var speedButton in ToolsAsButton)
            {
                if (speedButton.HasImage)
                    continue;
                speedButton.SetImage(img ?? KnownButtonImage.Transparent);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Sets the minimum tool label width to the maximum width of all tool label texts.
        /// </summary>
        /// <remarks>This method calculates the maximum width of all tool label texts
        /// and updates the minimum tool label width to match it.
        /// It ensures consistent alignment or layout for tool labels based on
        /// their text widths.</remarks>
        public virtual void MaximizeLabelTextWidth()
        {
            var maxWidth = GetMaxToolLabelTextWidth();
            SetMinToolLabelTextWidth(maxWidth);
        }

        /// <summary>
        /// Sets the minimum text width for the labels of all child speed buttons.
        /// </summary>
        /// <remarks>This method iterates through the child elements and applies
        /// the specified minimum
        /// text width to the labels of all child elements that are
        /// of type <see cref="SpeedButton"/>.</remarks>
        /// <param name="width">The minimum width, in device-independent units,
        /// to set for the labels' text.</param>
        public virtual void SetMinToolLabelTextWidth(double width)
        {
            foreach (var speedButton in ToolsAsButton)
            {
                speedButton.Label.MinTextWidth = width;
            }
        }

        /// <summary>
        /// Calculates the maximum width of the labels for all child speed buttons.
        /// </summary>
        /// <remarks>This method iterates through the child elements and calculates
        /// the width of the labels for any child that is a <see cref="SpeedButton"/>.
        /// The width is determined based on the specified
        /// font or the control's font if none is provided.</remarks>
        /// <param name="font">An optional font to use when measuring the label widths.
        /// If null, the control's font is used.</param>
        /// <returns>The maximum width, in device-independent units,
        /// of the labels for all child speed buttons. Returns 0 if
        /// there are no child speed buttons.</returns>
        public virtual double GetMaxToolLabelTextWidth(Font? font = null)
        {
            font ??= RealFont;

            double maxWidth = 0;
            foreach (var speedButton in ToolsAsButton)
            {
                var labelWidth = speedButton.Label.GetFormattedTextSize(font).Width;
                if (labelWidth > maxWidth)
                    maxWidth = labelWidth;
            }

            return maxWidth;
        }

        /// <summary>
        /// Maximizes the width of the tool's right-side element to its maximum allowable value.
        /// </summary>
        /// <remarks>This method adjusts the minimum width of the tool's right-side element to match its
        /// maximum width, ensuring the element occupies the largest possible space.
        /// The specific maximum width is
        /// determined by the implementation of <see cref="GetMaxToolRightSideElementWidth"/>.</remarks>
        /// <param name="additionalInc">
        /// An optional additional increment to add to the maximum width.
        /// </param>
        public virtual void MaximizeToolRightSideElementWidth(Coord additionalInc = 0)
        {
            var maxWidth = GetMaxToolRightSideElementWidth();
            SetToolRightSideElementMinWidth(maxWidth + additionalInc);
        }

        /// <summary>
        /// Sets the minimum width of the right side element for all child
        /// elements that are speed buttons.
        /// </summary>
        /// <remarks>This method iterates through the child elements and applies the specified
        /// width to the <see cref="SpeedButton.MinRightSideWidth"/> property of each child
        /// that is a <see cref="SpeedButton"/>.</remarks>
        /// <param name="width">The minimum width, in device-independent units,
        /// to set for the right side of each speed button.</param>
        public virtual void SetToolRightSideElementMinWidth(double width)
        {
            foreach (var speedButton in ToolsAsButton)
            {
                speedButton.MinRightSideWidth = width;
            }
        }

        /// <summary>
        /// Calculates the maximum width of the right side of all tool labels
        /// within the collection of child elements.
        /// </summary>
        /// <remarks>This method iterates through the child elements and evaluates
        /// the right-side width of labels associated with any child that is
        /// a <see cref="SpeedButton"/>. Only the maximum width encountered
        /// is returned. If no <see cref="SpeedButton"/> elements are present,
        /// the method returns 0.</remarks>
        /// <returns>The maximum right-side width of the labels for
        /// all <see cref="SpeedButton"/> elements in the collection.
        /// Returns 0 if no such elements are found.</returns>
        public virtual double GetMaxToolRightSideElementWidth()
        {
            double maxWidth = 0;
            foreach (var speedButton in ToolsAsButton)
            {
                var rightSideWidth = speedButton.Label.GetRightSideWidth(true);
                if (rightSideWidth > maxWidth)
                    maxWidth = rightSideWidth;
            }

            return maxWidth;
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
            if (dispose)
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
                    control.DataContext = null;
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

            foreach (var item in Children)
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
        public virtual AbstractControl? GetToolControl(ObjectUniqueId? id)
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
            if (index < GetToolCount())
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
            if (onlyTopBorder)
                OnlyTopBorder();
            if (updateMargin)
                Margin = (0, ToolBar.DefaultDistanceToContent, 0, 0);
        }

        /// <summary>
        /// Sets the margins of the toolbar.
        /// </summary>
        /// <param name="left">Whether to set the left margin.</param>
        /// <param name="top">Whether to set the top margin.</param>
        /// <param name="right">Whether to set the right margin.</param>
        /// <param name="bottom">Whether to set the bottom margin.</param>
        /// <param name="value">The value to set for the margins.
        /// If null, <see cref="DefaultDistanceToContent"/> is used.</param>
        public virtual void SetMargins(
            bool left,
            bool top = false,
            bool right = false,
            bool bottom = false,
            Coord? value = null)
        {
            value ??= ToolBar.DefaultDistanceToContent;

            Coord GetWidth(bool visible)
            {
                return visible ? value.Value : 0;
            }

            Margin = (GetWidth(left), GetWidth(top), GetWidth(right), GetWidth(bottom));
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
        public SpeedButton? FindTool(ObjectUniqueId? id)
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
            string? text = null,
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
        public virtual SpeedButton AddTextBtnCore(
            string? text = null,
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
            speedButton.Click += RaiseToolClick;

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
        /// Sets the alignment for the content of all toolbar items.
        /// </summary>
        /// <param name="alignment">The alignment to set for the toolbar items.</param>
        public virtual void SetToolContentAlignment(HVAlignment alignment)
        {
            DoInsideLayout(() =>
            {
                SetToolSpacerAlignment(alignment);
                SetToolImageAlignment(alignment);
                SetToolTextAlignment(alignment);
            });
        }

        /// <summary>
        /// Sets the alignment for the spacer of all toolbar items.
        /// </summary>
        /// <param name="alignment">The alignment to set for the toolbar items.</param>
        public virtual void SetToolSpacerAlignment(HVAlignment alignment)
        {
            foreach (var control in ToolsAsButton)
            {
                control.SpacerHorizontalAlignment = alignment.Horizontal;
                control.SpacerVerticalAlignment = alignment.Vertical;
            }
        }

        /// <summary>
        /// Sets the alignment for the image of all toolbar items.
        /// </summary>
        /// <param name="alignment">The alignment to set for the toolbar items.</param>
        public virtual void SetToolImageAlignment(HVAlignment alignment)
        {
            foreach (var control in ToolsAsButton)
            {
                control.ImageHorizontalAlignment = alignment.Horizontal;
                control.ImageVerticalAlignment = alignment.Vertical;
            }
        }

        /// <summary>
        /// Sets the alignment for the text of all toolbar items.
        /// </summary>
        /// <param name="alignment">The alignment to set for the toolbar items.</param>
        public virtual void SetToolTextAlignment(HVAlignment alignment)
        {
            foreach (var control in ToolsAsButton)
            {
                control.LabelHorizontalAlignment = alignment.Horizontal;
                control.LabelVerticalAlignment = alignment.Vertical;
            }
        }

        /// <summary>
        /// Same as <see cref="ToolsAsButton"/>.
        /// </summary>
        public IEnumerable<SpeedButton> GetTools() => ToolsAsButton;

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
            var speedButton = InsertSpeedBtnCore(
                Children.Count,
                itemKind,
                text,
                imageSet,
                imageSetDisabled,
                toolTip,
                action);
            return speedButton;
        }

        /// <summary>
        /// Inserts a menu item at the specified index with the provided properties.
        /// </summary>
        /// <remarks>The <paramref name="menuItem"/> parameter defines the appearance
        /// and behavior of the menu item, including its text, images, and the action
        /// to perform when clicked.</remarks>
        /// <param name="index">The zero-based index at which the menu item should be inserted.</param>
        /// <param name="menuItem">An object containing the properties of the menu item,
        /// such as text, images, and click behavior.</param>
        /// <returns>A <see cref="SpeedButton"/> representing the inserted menu item.</returns>
        public virtual AbstractControl InsertMenuItemCore(
            int index,
            IMenuItemProperties menuItem)
        {
            AbstractControl result;

            if(index < 0 || index > Children.Count)
                index = Children.Count;

            if (menuItem.Text == "-")
            {
                result = InsertSeparatorCore(index);
            }
            else
            {
                var speedButton = InsertSpeedBtnCore(
                    index,
                    ItemKind.Button,
                    menuItem.Text,
                    imageSet: null,
                    imageSetDisabled: null,
                    toolTip: null,
                    (s, e) =>
                    {
                        Post(menuItem.RaiseClick);
                    });
                speedButton.Label.MnemonicMarker = '_';
                speedButton.Label.MnemonicMarkerEnabled = true;
                result = speedButton;
            }

            result.DataContext = menuItem;
            return result;
        }

        /// <summary>
        /// Inserts a new <see cref="SpeedButton"/> into the collection at the specified index.
        /// </summary>
        /// <remarks>The <see cref="SpeedButton"/> is configured with default padding,
        /// alignment, and margins. If <paramref name="imageSetDisabled"/> is provided,
        /// it is used as the image set for the disabled state.
        /// If <paramref name="action"/> is provided, it is attached to both the click
        /// and double-click events of the <see cref="SpeedButton"/>.</remarks>
        /// <param name="index">The zero-based index at which the <see cref="SpeedButton"/>
        /// should be inserted.</param>
        /// <param name="itemKind">The kind of item represented by the <see cref="SpeedButton"/>.
        /// This determines its behavior and appearance.</param>
        /// <param name="text">The text to display on the <see cref="SpeedButton"/>.
        /// If null, an empty string is used.</param>
        /// <param name="imageSet">The image set to display on the <see cref="SpeedButton"/>.
        /// Can be null if no image is required.</param>
        /// <param name="imageSetDisabled">The image set to display when the <see cref="SpeedButton"/>
        /// is disabled. Can be null.</param>
        /// <param name="toolTip">The tooltip text to display when the user hovers
        /// over the <see cref="SpeedButton"/>. If null, the value of
        /// <paramref name="text"/> is used.</param>
        /// <param name="action">An optional event handler to invoke when
        /// the <see cref="SpeedButton"/> is clicked or double-clicked. Can be null.</param>
        /// <returns>The newly created <see cref="SpeedButton"/> instance that
        /// was inserted into the collection.</returns>
        public virtual SpeedButton InsertSpeedBtnCore(
            int index,
            ItemKind itemKind,
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled,
            string? toolTip = null,
            EventHandler? action = null)
        {
            text ??= string.Empty;

            var speedButton = CreateToolSpeedButton();

            speedButton.Click += (s, e) =>
            {
                RaiseToolClick(s, e);
            };

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
            speedButton.MinimumSize = itemSize;

            if (imageSetDisabled is not null)
            {
                speedButton.DisabledImageSet = imageSetDisabled;
            }

            UpdateItemProps(speedButton, itemKind);

            if (action is not null)
            {
                speedButton.DoubleClick += (s, e) =>
                {
                    if (DoubleClickAsClick)
                    {
                        action(s, e);
                    }
                };

                speedButton.Click += action;
            }

            speedButton.ContextMenuShowing += (s, e) =>
            {
                if (e.Value is not null)
                    return;
                e.Value = ContextMenuStrip;
            };

            Children.Insert(index, speedButton);

            return speedButton;
        }

        /// <summary>
        /// Overrides button type used when items are created and calls the specified action.
        /// After action is called an override is cleared.
        /// </summary>
        /// <param name="type">The type of the button to use as override.</param>
        /// <param name="action">Action to call.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="type"/>
        /// is not derived from <see cref="SpeedButton"/>.</exception>
        public virtual void OverrideButtonType(Type type, Action action)
        {
            if (!typeof(SpeedButton).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{type.Name} must inherit from {nameof(SpeedButton)}");
            }

            try
            {
                customButtonType = type;
                action();
            }
            finally
            {
                customButtonType = null;
            }
        }

        /// <summary>
        /// Adds custom <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="type">The type of the button to add. This value must inherit from
        /// <see cref="SpeedButton"/>.</param>
        /// <param name="factory">Factory method to create the button.</param>
        /// <returns>The created <see cref="SpeedButton"/>.</returns>
        public virtual SpeedButton AddCustomBtn(Type type, Func<SpeedButton> factory)
        {
            SpeedButton? result = null;

            OverrideButtonType(type, () =>
            {
                result = factory();
            });

            return result!;
        }

        /// <summary>
        /// Raises the <see cref="OnToolClick"/> method
        /// and invokes the <see cref="ToolClick"/> event handlers.
        /// </summary>
        /// <param name="s">The source of the event, typically
        /// the object that triggered the event. Can be <see langword="null"/>.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        public void RaiseToolClick(object? s, EventArgs e)
        {
            ToolClick?.Invoke(s, e);
            OnToolClick(s, e);
        }

        /// <summary>
        /// Creates control for use in the toolbar as a label.
        /// Override to create customized label controls.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateToolLabel()
        {
            return new Label();
        }

        /// <summary>
        /// Creates <see cref="SpeedButton"/> for use in the toolbar.
        /// Override to create customized speed buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual SpeedButton CreateToolSpeedButton()
        {
            if (customButtonType is null)
                return new SpeedButton();
            return (SpeedButton)Activator.CreateInstance(customButtonType);
        }

        /// <summary>
        /// Creates <see cref="SpeedTextButton"/> for use in the toolbar.
        /// Override to create customized speed text buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual SpeedButton CreateToolSpeedTextButton()
        {
            if (customButtonType is null)
                return new SpeedTextButton();
            return (SpeedButton)Activator.CreateInstance(customButtonType);
        }

        /// <summary>
        /// Updates common properties of the item control.
        /// </summary>
        /// <param name="control">Control which properties to update.</param>
        /// <param name="itemKind">Item kind.</param>
        /// <remarks>
        /// This method is called when new item is added, it updates
        /// some of its properties.
        /// </remarks>
        protected virtual void UpdateItemProps(AbstractControl control, ItemKind itemKind)
        {
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
                typeof(Label),
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
        /// Called when <see cref="ItemSize"/> property is changed.
        /// </summary>
        protected virtual void OnItemSizeChanged()
        {
            DoInsideLayout(() =>
            {
                foreach (var item in Children)
                {
                    if (item is SpeedButton || item is PictureBox)
                    {
                        var size = GetItemSuggestedSize(item);
                        item.SuggestedSize = size;
                        item.MinimumSize = itemSize;
                    }
                }
            });
        }

        /// <summary>
        /// Handles the event triggered when a tool is clicked.
        /// </summary>
        /// <remarks>This method is intended to be overridden in a derived class to provide custom
        /// handling for tool click events. The base implementation does nothing.</remarks>
        /// <param name="sender">The source of the event. This may be <see langword="null"/>.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnToolClick(object? sender, EventArgs e)
        {
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
            {
                return SizeD.NaN;
            }

            return itemSize;
        }

        /// <inheritdoc/>
        protected override void OnDataContextChanged(object? oldValue, object? newValue)
        {
            if (oldValue is IMenuProperties oldProperties)
            {
                oldProperties.Notification.CollectionChanged -= OnMenuItemsCollectionChanged;
            }

            if (newValue is IMenuProperties newProperties)
            {
                newProperties.Notification.CollectionChanged += OnMenuItemsCollectionChanged;
                MenuChangeHandler.OnCollectionReset(newProperties);
            }
        }

        /// <summary>
        /// Handles changes to the attached menu items collection.
        /// </summary>
        /// <remarks>This method is called whenever the menu items collection is modified.
        /// Subclasses can override this method to provide custom handling
        /// for collection changes.</remarks>
        /// <param name="sender">The source of the collection change event.
        /// This may be <see langword="null"/>.</param>
        /// <param name="e">The event data containing details about the collection change.</param>
        protected virtual void OnMenuItemsCollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            DoInsideLayout(() =>
            {
                ListUtils.RouteCollectionChange(
                    sender,
                    e,
                    MenuChangeHandler);
            });
        }

        /// <summary>
        /// Creates a new instance of a tool separator item for use in a toolbar.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes
        /// to customize the creation of the tool separator item.
        /// By default, it returns a new instance of <see cref="ToolBarSeparatorItem"/>.</remarks>
        /// <returns>A new instance of <see cref="ToolBarSeparatorItem"/> representing
        /// a separator in the toolbar.</returns>
        protected virtual ToolBarSeparatorItem CreateToolSeparator()
        {
            ToolBarSeparatorItem border = new();
            return border;
        }

        /// <summary>
        /// Provides functionality to manage and route changes in a menu collection
        /// to a <see cref="ToolBar"/>.
        /// </summary>
        /// <remarks>This class is responsible for handling collection change events,
        /// such as adding, removing, or moving menu items, and updating the associated
        /// <see cref="ToolBar"/> accordingly. It ensures
        /// that the toolbar reflects the current state of the menu collection.</remarks>
        protected class MenuChangeRouter : ICollectionChangeRouter
        {
            private readonly ToolBar owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="MenuChangeRouter"/> class.
            /// </summary>
            /// <param name="owner">The <see cref="ToolBar"/> instance that owns this router.
            /// Cannot be <see langword="null"/>.</param>
            public MenuChangeRouter(ToolBar owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// Inserts a menu item at the specified index to the toolbar.
            /// </summary>
            /// <param name="index">The zero-based index at which the menu item
            /// should be inserted.</param>
            /// <param name="menuItem">The menu item to insert.</param>
            public void InsertItem(int index, IMenuItemProperties menuItem)
            {
                owner.InsertMenuItemCore(index, menuItem);
            }

            /// <inheritdoc/>
            public virtual void OnCollectionAdd(object? sender, IList newItems, int newIndex)
            {
                foreach (var newItem in newItems)
                {
                    if (newItem is not IMenuItemProperties menuItem)
                        continue;
                    InsertItem(newIndex, menuItem);
                    newIndex++;
                }
            }

            /// <inheritdoc/>
            public virtual void OnCollectionMove(
                object? sender,
                IList movedItems,
                int oldIndex,
                int newIndex)
            {
                if (oldIndex == newIndex)
                    return;
                OnCollectionRemove(sender, movedItems, oldIndex);
                OnCollectionAdd(sender, movedItems, newIndex);
            }

            /// <inheritdoc/>
            public virtual void OnCollectionRemove(object? sender, IList oldItems, int oldIndex)
            {
                foreach (var oldItem in oldItems)
                {
                    if (oldItem is not IMenuItemProperties menuItem)
                        continue;
                    var child = owner.FindChildWithDataContextId(menuItem.UniqueId);
                    if (child is null)
                        continue;
                    child.Parent = null;
                    child.Dispose();
                }
            }

            /// <inheritdoc/>
            public virtual void OnCollectionReplace(
                object? sender,
                IList oldItems,
                IList newItems,
                int index)
            {
                OnCollectionRemove(sender, oldItems, index);
                OnCollectionAdd(sender, newItems, index);
            }

            /// <inheritdoc/>
            public virtual void OnCollectionReset(object? sender)
            {
                owner.DeleteAll(true);

                if (sender is not IMenuProperties menuProperties)
                    return;

                owner.DoInsideLayout(() =>
                {
                    for (int i = 0; i < menuProperties.Count; i++)
                    {
                        var menuItem = menuProperties.GetItem(i);
                        if (menuItem is not IMenuItemProperties item)
                            continue;
                        InsertItem(-1, item);
                    }
                });
            }
        }
    }
}