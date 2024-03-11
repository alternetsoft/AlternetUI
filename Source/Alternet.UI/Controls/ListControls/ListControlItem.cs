using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Custom item for <see cref="ListBox"/>, <see cref="ComboBox"/> or other
    /// <see cref="ListControl"/> descendants. This class has <see cref="Text"/>,
    /// <see cref="Value"/> and other item properties.
    /// </summary>
    public partial class ListControlItem : BaseControlItem
    {
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
        /// It is up to control to decide whether and how checkboxes are painted.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual CheckState CheckState { get; set; }

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
        /// Gets or sets foreground color of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets alignment of the item.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// When this property is changed, you need to repaint the item.
        /// </remarks>
        [Browsable(false)]
        public virtual GenericAlignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets text which is displayed in the <see cref="ListControl"/>.
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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Text;
        }
    }
}