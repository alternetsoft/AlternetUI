using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a contract for a property that can be read-only.
    /// </summary>
    public interface IReadOnlyProperty
    {
        /// <summary>
        /// Gets or sets a value indicating whether the property is read-only.
        /// </summary>
        bool ReadOnly { get; set; }
    }
}
