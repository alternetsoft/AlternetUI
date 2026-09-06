using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Gives access to the 'Checked' property of the object.
    /// </summary>
    public interface ICheckedProperty
    {
        /// <summary>
        /// Gets or sets the 'Checked' property of the object.
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Occurs when the 'Checked' property of the object changes.
        /// </summary>
        event EventHandler? CheckedChanged;
    }
}
