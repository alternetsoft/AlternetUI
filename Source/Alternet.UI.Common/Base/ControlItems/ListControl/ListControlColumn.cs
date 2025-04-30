using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a column within a list control, defining its layout properties.
    /// </summary>
    /// <remarks>
    /// This class provides configuration settings such as suggested width for column rendering.
    /// It can be extended to accommodate additional visual or functional enhancements.
    /// </remarks>
    public class ListControlColumn : BaseControlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlColumn"/> class.
        /// </summary>
        public ListControlColumn()
        {
        }

        /// <summary>
        /// Gets or sets the suggested width for the list control column.
        /// </summary>
        /// <remarks>
        /// The width recommendation is based on layout constraints and user preferences.
        /// </remarks>
        public virtual Coord SuggestedWidth { get; set; }
    }
}
