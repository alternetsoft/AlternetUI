using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a method which is raised when the system colors are changed.
    /// </summary>
    public interface IRaiseSystemColorsChanged
    {
        /// <summary>
        /// Updates visual style of the control when the system colors are changed.
        /// </summary>
        void RaiseSystemColorsChanged();
    }
}
