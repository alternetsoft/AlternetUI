using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a method to retrieve a tooltip representation of the current object for display in rich UI contexts.
    /// </summary>
    /// <remarks>Use this interface to obtain a formatted tooltip suitable for visual presentation. If the
    /// object does not support tooltip generation, the method returns <see langword="null"/>.</remarks>
    public interface IGetAsToolTip
    {
        /// <summary>
        /// Gets a tooltip representation of the current object, suitable for display in rich UI contexts.
        /// </summary>
        /// <remarks>Use this method to obtain a formatted tooltip for visual presentation. If the object
        /// does not support tooltip generation, the method returns <see langword="null"/>.</remarks>
        /// <returns>A <see cref="RichToolTipParams"/> instance containing tooltip data if available; otherwise, <see
        /// langword="null"/>.</returns>
        RichToolTipParams? GetAsToolTip();
    }
}
