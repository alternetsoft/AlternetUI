using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Custom item for <see cref="ListBox"/>, <see cref="ComboBox"/> or other
    /// <see cref="ListControl"/> descendants. This class has both <see cref="Text"/>
    /// and <see cref="Value"/>.
    /// </summary>
    public class ListControlItem : BaseControlItem
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
        /// <param name="text">Text to display.</param>
        /// <param name="action">Action associated with the item.</param>
        public ListControlItem(string text, Action? action)
        {
            Text = text;
            Action = action;
        }

        /// <summary>
        /// Gets or sets text which is displayed in the <see cref="ListControl"/>.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets user data.
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Action"/> associated with this
        /// <see cref="ListControlItem"/> instance.
        /// </summary>
        public Action? Action { get; set; }

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