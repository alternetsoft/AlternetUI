using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Gives access to the 'ValueHelper' property of the object.
    /// </summary>
    public interface IValueHelperProperty
    {
        /// <summary>
        /// Gets or sets the 'ValueHelper' property of the object.
        /// </summary>
        TextAsValueHelper ValueHelper { get; }
    }
}
