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
    public class ListControlTextValuePair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlTextValuePair"/> class.
        /// </summary>
        /// <param name="text">Text to display in the <see cref="ListControl"/></param>
        /// <param name="value">User data.</param>
        public ListControlTextValuePair(string text, object value)
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// Gets or sets text which is displayed in the <see cref="ListControl"/>.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets user data.
        /// </summary>
        public object Value { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Text;
        }
    }
}
