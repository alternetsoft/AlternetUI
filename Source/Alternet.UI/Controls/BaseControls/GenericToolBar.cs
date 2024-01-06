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
        /// Gets or sets whether to display text in the buttons.
        /// </summary>
        public bool TextVisible
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
        public bool ImageVisible
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
                var items = GetChildren(true).Items;
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
        /// Adds <see cref="SpeedButton"/> to the control.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="imageSet">Item image.</param>
        /// <param name="imageSetDisabled">Item disabled image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddSpeedButton(
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled,
            string? toolTip,
            EventHandler? action = null)
        {
            SpeedButton speedButton = new()
            {
                ImageVisible = imageVisible,
                TextVisible = textVisible,
                ImageToText = imageToText,
                Text = text ?? string.Empty,
                ImageSet = imageSet,
                ToolTip = toolTip ?? string.Empty,
                SuggestedSize = itemSize,
                VerticalAlignment = VerticalAlignment.Center,
            };

            if (imageSetDisabled is not null)
            {
                speedButton.DisabledImageSet = imageSetDisabled;
            }

            UpdateItemProps(speedButton, ItemKind.Button);

            speedButton.Click += action;
            speedButton.Parent = panel;

            return speedButton.UniqueId;
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
