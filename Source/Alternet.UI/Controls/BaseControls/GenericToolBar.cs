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

            if (BackgroundColor is not null)
                speedButton.BackgroundColor = BackgroundColor;

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

            if (BackgroundColor is not null)
                label.BackgroundColor = BackgroundColor;
            label.Parent = panel;
            return label.UniqueId;
        }

        /// <summary>
        /// Adds an empty space with the specified or default size.
        /// </summary>
        /// <param name="size">Optional spacer size.</param>
        /// <returns></returns>
        public virtual ObjectUniqueId AddSpacer(double? size = null)
        {
            Panel control = new()
            {
                SuggestedSize = size ?? DefaultSpacerSize,
            };

            if (BackgroundColor is not null)
                control.BackgroundColor = BackgroundColor;

            control.Parent = panel;
            return control.UniqueId;
        }

        /*public virtual ObjectUniqueId AddSeparator()
        {
        }*/

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

        private SpeedButton? FindTool(ObjectUniqueId id)
        {
            var result = panel.FindChild(id) as SpeedButton;
            return result;
        }
    }
}
