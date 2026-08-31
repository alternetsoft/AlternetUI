using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a style for <see cref="ListControlItem"/>.
    /// </summary>
    internal partial class ListControlItemStyle : DisposableObject
    {
        /// <summary>
        /// Gets the default style for <see cref="ListControlItem"/>.
        /// </summary>
        public static readonly ListControlItemStyle Default = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItemStyle"/> class.
        /// </summary>
        public ListControlItemStyle()
        {
        }
    }
}
