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

        private readonly double itemSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericToolBar"/> class.
        /// </summary>
        public GenericToolBar()
        {
            itemSize = DefaultSize;
            Height = DefaultSize;
            SuggestedHeight = DefaultSize;
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
        /// Adds toolbar item.
        /// </summary>
        /// <param name="text">Item text.</param>
        /// <param name="imageSet">Item image.</param>
        /// <param name="imageSetDisabled">Item disabled image.</param>
        /// <param name="toolTip">Item tooltip.</param>
        /// <param name="action">Click action.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId Add(
            string? text,
            ImageSet? imageSet,
            ImageSet? imageSetDisabled,
            string? toolTip,
            EventHandler? action = null)
        {
            SizeI imageSize = Toolbar.GetDefaultImageSize(this);

            var image = imageSet?.AsImage(imageSize);
            var imageDisabled = imageSetDisabled?.AsImage(imageSize);

            SpeedButton speedButton = new()
            {
                Text = text ?? string.Empty,
                Image = image,
                ToolTip = toolTip,
                SuggestedSize = itemSize,
            };

            if (imageDisabled is not null)
            {
                speedButton.DisabledImage = imageDisabled;
            }

            UpdateItemProps(speedButton, ItemKind.Button);

            speedButton.Click += action;
            panel.Children.Add(speedButton);

            return speedButton.UniqueId;
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
        /// Adds static text to the toolbar.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <returns><see cref="ObjectUniqueId"/> of the added item.</returns>
        public virtual ObjectUniqueId AddText(string text)
        {
            GenericLabel label = new(text)
            {
                Margin = DefaultTextMargin,
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
            var item = FindTool(id);
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
            var item = FindTool(id);
            if (item is null)
                return;
            item.Enabled = enabled;
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
            var item = FindTool(toolId);
            if (item is null)
                return RectD.Empty;
            return item.Bounds;
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
            var item = FindTool(id);
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
            var item = FindTool(id);
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
            var item = FindTool(id);
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
            var item = FindTool(id);
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
            var item = FindTool(id);
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
            var item = FindTool(id);
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
            var item = FindTool(id);
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
            return panel.FindChild(id);
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
            if(itemKind == ItemKind.Text)
                control.VerticalAlignment = VerticalAlignment.Center;
        }

        private SpeedButton? FindTool(ObjectUniqueId id)
        {
            var result = panel.FindChild(id) as SpeedButton;
            return result;
        }
    }
}
