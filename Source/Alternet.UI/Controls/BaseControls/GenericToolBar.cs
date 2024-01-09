using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic toolbar control.
    /// </summary>
    public class GenericToolBar : Control
    {
        private readonly StackPanel panel = new()
        {
            Orientation = StackPanelOrientation.Horizontal,
        };

        private double itemSize;
        private bool textVisible = false;
        private bool imageVisible = true;
        private ImageToText imageToText = ImageToText.Horizontal;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericToolBar"/> class.
        /// </summary>
        public GenericToolBar()
        {
            itemSize = Math.Max(DefaultSize, 24);
            panel.Parent = this;
            TabStop = false;
            AcceptsFocusAll = false;
        }

        /// <summary>
        /// Enumerates all toolbar item kinds.
        /// </summary>
        protected enum ItemKind
        {
            /// <summary>
            /// Item is button.
            /// </summary>
            Button,

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
        public static double DefaultSize { get; set; } = 24;

        /// <summary>
        /// Gets or sets default spacer item size.
        /// </summary>
        public static double DefaultSpacerSize { get; set; } = 4;

        /// <summary>
        /// Gets or sets default margin of the static text item.
        /// </summary>
        public static Thickness DefaultTextMargin { get; set; } = (4, 0, 4, 0);

        /// <summary>
        /// Gets or sets default color of the separator item.
        /// </summary>
        public static Color DefaultSeparatorColor { get; set; } = SystemColors.GrayText;

        /// <summary>
        /// Gets or sets default width of the separator item.
        /// </summary>
        public static double DefaultSeparatorWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets default margin of the separator item.
        /// </summary>
        public static Thickness DefaultSeparatorMargin { get; set; } = (4, 4, 4, 4);

        /// <summary>
        /// Gets or sets default image size.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="Toolbar.GetDefaultImageSize(Control)"/> is used.
        /// </remarks>
        public static int? DefaultImageSize { get; set; }

        /// <summary>
        /// Gets or sets default color of the images in the normal state.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="Control.GetSvgColor(KnownSvgColor)"/> is used with
        /// <see cref="KnownSvgColor.Normal"/> parameter.
        /// </remarks>
        public static Color? DefaultNormalImageColor { get; set; }

        /// <summary>
        /// Gets or sets default color of the images in the disabled state.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="Control.GetSvgColor(KnownSvgColor)"/> is used with
        /// <see cref="KnownSvgColor.Disabled"/> parameter.
        /// </remarks>
        public static Color? DefaultDisabledImageColor { get; set; }

        /// <summary>
        /// Gets or sets color of the images in the normal state.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="DefaultNormalImageColor"/> is used.
        /// </remarks>
        [Browsable(false)]
        public Color? NormalImageColor { get; set; }

        /// <summary>
        /// Gets or sets color of the images in the disabled state.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="DefaultDisabledImageColor"/> is used.
        /// </remarks>
        [Browsable(false)]
        public Color? DisabledImageColor { get; set; }

        /// <summary>
        /// Gets or sets image size.
        /// </summary>
        /// <remarks>
        /// This property specifies size of the new images in the toolbar.
        /// Existing items size is not changed. If this property is null,
        /// <see cref="DefaultImageSize"/> is used.
        /// </remarks>
        [Browsable(false)]
        public int? ImageSize { get; set; }

        /// <summary>
        /// Gets or sets <see cref="KnownSvgImages"/> for the normal state.
        /// </summary>
        [Browsable(false)]
        public KnownSvgImages? NormalSvgImages { get; set; }

        /// <summary>
        /// Gets or sets <see cref="KnownSvgImages"/> for the disabled state.
        /// </summary>
        [Browsable(false)]
        public KnownSvgImages? DisabledSvgImages { get; set; }

        /// <summary>
        /// Gets or sets a value which specifies display modes for
        /// item image and text.
        /// </summary>
        public ImageToText ImageToText
        {
            get => imageToText;
            set
            {
                if (imageToText == value)
                    return;
                imageToText = value;
                if (!ImageVisible || !TextVisible)
                    return;
                foreach (var item in Items)
                {
                    if (item is SpeedButton speedButton)
                        speedButton.ImageToText = value;
                }
            }
        }

        /// <summary>
        /// Gets items added to the control.
        /// </summary>
        public IReadOnlyList<Control> Items => panel.Children;

        /// <summary>
        /// Gets toolbar item size.
        /// </summary>
        public double ItemSize
        {
            get
            {
                return itemSize;
            }

            set
            {
                if (itemSize < 24)
                    itemSize = 24;
                if (itemSize == value)
                    return;
                itemSize = value;

                SuspendLayout();
                foreach (var item in Items)
                {
                    if (item is SpeedButton || item is PictureBox)
                        item.SuggestedSize = value;
                }

                ResumeLayout();
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
                SetChildrenFont(value, true);
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
                foreach(var item in Items)
                {
                    if (NeedUpdateBackColor(item))
                        item.BackgroundColor = value;
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
        internal bool TextVisible
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
                foreach (var item in Items)
                {
                    if (item is SpeedButton speedButton)
                        speedButton.TextVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to display images in the buttons.
        /// </summary>
        internal bool ImageVisible
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
                foreach (var item in Items)
                {
                    if (item is SpeedButton speedButton)
                        speedButton.ImageVisible = value;
                }
            }
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
            ImageSet? imageSetDisabled,
            string? toolTip = null,
            EventHandler? action = null)
        {
            text ??= string.Empty;

            SpeedButton speedButton = new()
            {
                ImageVisible = imageVisible,
                TextVisible = textVisible,
                ImageToText = imageToText,
                Text = text,
                ImageSet = imageSet,
                ToolTip = toolTip ?? text,
                SuggestedSize = itemSize,
                VerticalAlignment = VerticalAlignment.Center,
            };

            if (imageSetDisabled is not null)
            {
                speedButton.DisabledImageSet = imageSetDisabled;
            }

            UpdateItemProps(speedButton, ItemKind.Button);

            if(action is not null)
                speedButton.Click += action;
            speedButton.Parent = panel;

            return speedButton.UniqueId;
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
        /// Adds known <see cref="SpeedButton"/> to the control.
        /// </summary>
        public virtual ObjectUniqueId AddSpeedBtn(KnownButton button)
        {
            var strings = CommonStrings.Default;
            var images = GetNormalSvgImages();
            var disabled = GetDisabledSvgImages();

            switch (button)
            {
                case KnownButton.OK:
                default:
                    return AddSpeedBtn(strings.ButtonOk, images.ImgOk, disabled.ImgOk);
                case KnownButton.Cancel:
                    return AddSpeedBtn(strings.ButtonCancel, images.ImgCancel, disabled.ImgCancel);
                case KnownButton.Yes:
                    return AddSpeedBtn(strings.ButtonYes, images.ImgYes, disabled.ImgYes);
                case KnownButton.No:
                    return AddSpeedBtn(strings.ButtonNo, images.ImgNo, disabled.ImgNo);
                case KnownButton.Abort:
                    return AddSpeedBtn(strings.ButtonAbort, images.ImgAbort, disabled.ImgAbort);
                case KnownButton.Retry:
                    return AddSpeedBtn(strings.ButtonRetry, images.ImgRetry, disabled.ImgRetry);
                case KnownButton.Ignore:
                    return AddSpeedBtn(strings.ButtonIgnore, images.ImgIgnore, disabled.ImgIgnore);
                case KnownButton.Help:
                    return AddSpeedBtn(strings.ButtonHelp, images.ImgHelp, disabled.ImgHelp);
                case KnownButton.Add:
                    return AddSpeedBtn(strings.ButtonAdd, images.ImgAdd, disabled.ImgAdd);
                case KnownButton.Remove:
                    return AddSpeedBtn(strings.ButtonRemove, images.ImgRemove, disabled.ImgRemove);
                case KnownButton.Clear:
                    return AddSpeedBtn(strings.ButtonClear, images.ImgRemoveAll, disabled.ImgRemoveAll);
                case KnownButton.AddChild:
                    return AddSpeedBtn(strings.ButtonAddChild, images.ImgAddChild, disabled.ImgAddChild);
                case KnownButton.MoreItems:
                    return AddSpeedBtn(
                        strings.ToolbarSeeMore,
                        images.ImgMoreActionsHorz,
                        disabled.ImgMoreActionsHorz);
                case KnownButton.New:
                    return AddSpeedBtn(strings.ButtonNew, images.ImgFileNew, disabled.ImgFileNew);
                case KnownButton.Open:
                    return AddSpeedBtn(strings.ButtonOpen, images.ImgFileOpen, disabled.ImgFileOpen);
                case KnownButton.Save:
                    return AddSpeedBtn(strings.ButtonSave, images.ImgFileSave, disabled.ImgFileSave);
                case KnownButton.Undo:
                    return AddSpeedBtn(strings.ButtonUndo, images.ImgUndo, disabled.ImgUndo);
                case KnownButton.Redo:
                    return AddSpeedBtn(strings.ButtonRedo, images.ImgRedo, disabled.ImgRedo);
                case KnownButton.Bold:
                    return AddSpeedBtn(strings.ButtonBold, images.ImgBold, disabled.ImgBold);
                case KnownButton.Italic:
                    return AddSpeedBtn(strings.ButtonItalic, images.ImgItalic, disabled.ImgItalic);
                case KnownButton.Underline:
                    return AddSpeedBtn(strings.ButtonUnderline, images.ImgUnderline, disabled.ImgUnderline);
            }
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
                    return [AddSpeedBtn(KnownButton.OK)];
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

            return [];
        }

        /// <summary>
        /// Adds <see cref="PictureBox"/> to the control.
        /// </summary>
        /// <param name="image">Normal image.</param>
        /// <param name="imageDisabled">Disable image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddPicture(
            ImageSet? image,
            ImageSet? imageDisabled,
            string? toolTip = default)
        {
            PictureBox picture = new()
            {
                AcceptsFocusAll = false,
                ImageStretch = false,
                ImageSet = image,
                ToolTip = toolTip ?? string.Empty,
                SuggestedSize = itemSize,
                VerticalAlignment = VerticalAlignment.Center,
            };

            if (imageDisabled is not null)
            {
                picture.DisabledImageSet = imageDisabled;
            }

            UpdateItemProps(picture, ItemKind.Picture);

            picture.Parent = panel;

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
        public virtual ObjectUniqueId AddControl(Control control)
        {
            control.Parent = panel;
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
            GenericLabel label = new(text)
            {
                Margin = DefaultTextMargin,
                VerticalAlignment = VerticalAlignment.Center,
            };

            UpdateItemProps(label, ItemKind.Text);
            label.Parent = panel;
            return label.UniqueId;
        }

        /// <summary>
        /// Adds an empty space with the specified or default size.
        /// </summary>
        /// <param name="size">Optional spacer size.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpacer(double? size = null)
        {
            Panel control = new()
            {
                SuggestedSize = size ?? DefaultSpacerSize,
            };

            UpdateItemProps(control, ItemKind.Spacer);

            control.Parent = panel;
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
                BorderColor = DefaultSeparatorColor,
                BorderWidth = (DefaultSeparatorWidth, 0, 0, 0),
                SuggestedWidth = DefaultSeparatorWidth,
                Margin = DefaultSeparatorMargin,
            };

            UpdateItemProps(border, ItemKind.Separator);

            border.Parent = panel;
            return border.UniqueId;
        }

        /// <summary>
        /// Sets 'Visible' property of the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="visible">New property value.</param>
        public virtual void SetToolVisible(ObjectUniqueId id, bool visible)
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
        public virtual void SetToolEnabled(ObjectUniqueId id, bool enabled)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Enabled = enabled;
        }

        /// <summary>
        /// Sets whether toolbar item is right aligned.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="isRight"><c>true</c> if item must be right aligned.</param>
        public virtual void SetToolAlignRight(ObjectUniqueId id, bool isRight)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            if (isRight)
                item.HorizontalAlignment = HorizontalAlignment.Right;
            else
                item.HorizontalAlignment = HorizontalAlignment.Left;
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
            return item.HorizontalAlignment == HorizontalAlignment.Right;
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
        /// <returns>Position and size of the tool in the toolbar in device-independent units
        /// (1/96 inch).</returns>
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
        public void SetToolSticky(ObjectUniqueId toolId, bool value)
        {
            var item = FindTool(toolId);
            if (item is null)
                return;
            item.Sticky = value;
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
        public virtual void DeleteTool(ObjectUniqueId id)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Parent = null;
            item.Dispose();
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
        public virtual void SetToolText(ObjectUniqueId id, string? text)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.Text = text ?? string.Empty;
        }

        /// <summary>
        /// Sets item 'ToolTip' property value.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetToolShortHelp(ObjectUniqueId id, string? value)
        {
            var item = GetToolControl(id);
            if (item is null)
                return;
            item.ToolTip = value;
        }

        /// <summary>
        /// Gets total count of the items.
        /// </summary>
        /// <returns></returns>
        public virtual int GetToolCount()
        {
            return panel.Children.Count;
        }

        /// <summary>
        /// Gets an id of the item with the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        public virtual ObjectUniqueId GetToolId(int index)
        {
            return panel.Children[index].UniqueId;
        }

        /// <summary>
        /// Gets item control.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns></returns>
        public virtual Control? GetToolControl(ObjectUniqueId id)
        {
            var result = panel.FindChild(id);
            return result;
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

        /// <summary>
        /// Gets image size taking into account <see cref="ImageSize"/>
        /// and <see cref="DefaultImageSize"/> properties.
        /// </summary>
        /// <returns></returns>
        public virtual int GetImageSize() =>
            ImageSize ?? DefaultImageSize ?? Toolbar.GetDefaultImageSize(this).Width;

        /// <summary>
        /// Gets image color in the normal state taking into account <see cref="NormalImageColor"/>
        /// and <see cref="DefaultNormalImageColor"/> properties.
        /// </summary>
        public virtual Color GetNormalImageColor() =>
            NormalImageColor ?? DefaultNormalImageColor ?? GetSvgColor(KnownSvgColor.Normal);

        /// <summary>
        /// Gets image color in the disabled state  taking into account
        /// <see cref="DisabledImageColor"/> and <see cref="DefaultDisabledImageColor"/> properties.
        /// </summary>
        public virtual Color GetDisabledImageColor() =>
            DisabledImageColor ?? DefaultDisabledImageColor ?? GetSvgColor(KnownSvgColor.Disabled);

        /// <summary>
        /// Gets <see cref="KnownSvgImages"/> for the normal state.
        /// </summary>
        public virtual KnownSvgImages GetNormalSvgImages() =>
            NormalSvgImages ??= KnownSvgImages.GetForSize(GetNormalImageColor(), GetImageSize());

        /// <summary>
        /// Gets <see cref="KnownSvgImages"/> for the disabled state.
        /// </summary>
        public virtual KnownSvgImages GetDisabledSvgImages() =>
            DisabledSvgImages ??= KnownSvgImages.GetForSize(GetDisabledImageColor(), GetImageSize());

        /// <summary>
        /// Updates common properties of the item control.
        /// </summary>
        /// <param name="control">Control which properties to update.</param>
        /// <param name="itemKind">Item kind.</param>
        /// <remarks>
        /// This method is called when new item is added, it updates
        /// <see cref="Control.BackgroundColor"/> and other properties.
        /// </remarks>
        protected virtual void UpdateItemProps(Control control, ItemKind itemKind)
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
        protected virtual bool NeedUpdateBackColor(Control control) => NeedUpdateForeColor(control);

        /// <summary>
        /// Gets whether child control foreground color need to be updated when
        /// toolbar foreground color is changed.
        /// </summary>
        /// <param name="control">Control to check</param>
        /// <returns></returns>
        protected virtual bool NeedUpdateForeColor(Control control)
        {
            Type[] types = [
                typeof(SpeedButton),
                typeof(GenericLabel),
                typeof(PictureBox),
                typeof(Label),
                typeof(StackPanel),
                typeof(Panel),
                typeof(Grid),
                typeof(Border),
            ];

            if (Array.IndexOf(types, control.GetType()) >= 0)
                return true;
            return false;
        }

        private SpeedButton? FindTool(ObjectUniqueId id)
        {
            var result = GetToolControl(id) as SpeedButton;
            return result;
        }
    }
}
